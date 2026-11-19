cd LiteRT-LM || exit 1

HEAD_COMMIT="$(git rev-parse HEAD)"

PLUGIN_DIR="../../UAI.LiteRTLM.Omni/Packages/com.uralstech.uai.litertlm.omni/Runtime/Plugins"

BUILT_SYMBOL="liblitert-lm-omni"
BUILD_DIR="./bazel-bin/c"

PREBUILTS_DIR="./prebuilt"

CLEANUP_FILES=""
trap 'for file in ${CLEANUP_FILES}; do rm -rf "${file}"; done' EXIT

PREBUILT_LIBS_COMMON="libLiteRt libGemmaModelConstraintProvider"
PREBUILT_LIBS_ANDROID="libLiteRtOpenClAccelerator"
PREBUILT_LIBS_APPLE="libLiteRtMetalAccelerator"

build() {
    local config="$1"
    shift

    bazel build                     \
        --config="${config}"        \
        -c opt "$@"                 \
        //c:litert-lm-omni || return 1

}

force_copy_file() {
    local src_file="$1"
    local dst_file="$2"

    rm -f "${dst_file}"
    cp "${src_file}" "${dst_file}"
}

copy_libs() {
    local platform="$1"
    local arch="$2"
    local extension="$3"
    local plugin_subdir="$4"
    local platform_libs="$5"

    local src="${PREBUILTS_DIR}/${platform}_${arch}"
    local dst="${PLUGIN_DIR}/${plugin_subdir}/${arch}"

    mkdir -p "${dst}"
    for lib in ${PREBUILT_LIBS_COMMON} ${platform_libs}; do

        force_copy_file                     \
            "${src}/${lib}.${extension}"    \
            "${dst}/${lib}.${extension}"
    done

    force_copy_file                                 \
        "${BUILD_DIR}/${BUILT_SYMBOL}.${extension}" \
        "${dst}/${BUILT_SYMBOL}.${extension}"
}

copy_ios_output() {
    local arch="$1"
    local dst="$2"

    local bin="${BUILT_SYMBOL}.dylib"
    local bin_dir="${dst}/ios_${arch}"

    mkdir -p "${bin_dir}"
    force_copy_file             \
        "${BUILD_DIR}/${bin}"   \
        "${bin_dir}/${bin}"
}

create_xcframework_ios() {
    local lib_name="$1"
    local src_dir="$2"
    local archs="$3"
    local name="$4"
    shift 4

    lib_dylib="${lib_name}.dylib"

    local temp_dir="$(mktemp -d)" || return 1
    CLEANUP_FILES="${CLEANUP_FILES} ${temp_dir}"

    local frameworks=""
    for arch in ${archs}; do

        local framework_name="${lib_dylib}.framework"
        local framework_path="${temp_dir}/${arch}/${framework_name}"

        mkdir -p "${framework_path}"
        local framework_lib_path="${framework_path}/${lib_dylib}"

        force_copy_file "${src_dir}/ios_${arch}/${lib_dylib}" "${framework_lib_path}"
        
        if [[ "${name}" == "full_name" ]]; then
            install_name_tool -id "@rpath/${lib_dylib}.framework/${lib_dylib}"  "${framework_lib_path}" || return 1
        else
            install_name_tool -id "@rpath/${lib_dylib}"                         "${framework_lib_path}" || return 1
        fi

        for dependency in $@; do

            install_name_tool -add_rpath "@executable_path/Frameworks/${dependency}.dylib.framework" "${framework_lib_path}" || return 1
        done

        cat <<EOF > "${framework_path}/Info.plist"
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>CFBundleName</key>
    <string>${lib_dylib}</string>
    <key>CFBundleIdentifier</key>
    <string>com.uralstech.uai.litertlm.omni.native.ios.${lib_name}</string>
    <key>CFBundleVersion</key>
    <string>1</string>
    <key>CFBundleShortVersionString</key>
    <string>1.0.0</string>
    <key>CFBundleExecutable</key>
    <string>${lib_dylib}</string>
    <key>CFBundlePackageType</key>
    <string>FMWK</string>
    <key>MinimumOSVersion</key>
    <string>15.0</string>
</dict>
</plist>
EOF

        frameworks="${frameworks} ${framework_path}"
    done

    local dst
    if [[ "${name}" == "full_name" ]]; then
        dst="${PLUGIN_DIR}/iOS/${lib_dylib}.xcframework"
    else
        dst="${PLUGIN_DIR}/iOS/Deps~/${lib_dylib}.xcframework"
    fi

    rm -rf "${dst}"

    local command="xcodebuild -create-xcframework"
    for framework in ${frameworks}; do
        
        command="${command} -framework ${framework}"
    done

    command="${command} -output ${dst}"
    eval "${command}"
}

package_xcframeworks_ios() {
    local build_dir="$1"
    local archs="$2"

    local all_prebuilts="${PREBUILT_LIBS_COMMON} ${PREBUILT_LIBS_APPLE}"

    for lib_name in ${all_prebuilts}; do

        create_xcframework_ios "${lib_name}" "${PREBUILTS_DIR}" "${archs}" lib_name || return 1
    done

    create_xcframework_ios "${BUILT_SYMBOL}" "${build_dir}" "${archs}" full_name "${all_prebuilts}" || return 1
}

# ----------------- APPLY VERSION SPECIFIC PATCH(ES) ------------------

LITERTLM_PATCH="../Patches/v0.19.1.patch"
git apply "${LITERTLM_PATCH}"


# Set ANDROID_NDK_HOME before running this
# ------------------------------ Android ------------------------------

build android_arm64 --linkopt=-Wl,-z,max-page-size=16384    \
                    --linkopt=-Wl,-z,common-page-size=16384 || exit 1

copy_libs android arm64 so Android "${PREBUILT_LIBS_ANDROID} libwebgpu_dawn" # Dawn added only for v0.19.x


# ------------------------------  macOS  ------------------------------

# Note: Unity needs to load the Metal accelerators on startup for
# liblitert-lm.dylib to register it.

build macos_arm64 --linkopt=-Wl,-rpath,@loader_path \
    --define=litert_runtime_link_mode=dynamic || exit 1

copy_libs macos arm64 dylib macOS "${PREBUILT_LIBS_APPLE} libwebgpu_dawn" # Dawn added only for v0.19.x


# ------------------------------   iOS   ------------------------------

ios_out="$(mktemp -d)" || exit 1
CLEANUP_FILES="${CLEANUP_FILES} ${ios_out}"

build ios_arm64 || exit 1
copy_ios_output arm64 "${ios_out}"

build ios_sim_arm64 || exit 1
copy_ios_output sim_arm64 "${ios_out}"

package_xcframeworks_ios "${ios_out}" "arm64 sim_arm64" || exit 1

echo "LITERT_LM_REV = \"${HEAD_COMMIT}\"" > "${PLUGIN_DIR}/.build_sources.arm64.txt"

git apply -R "${LITERTLM_PATCH}"