# BUILD_SCRIPTS_DIR=$(pwd)
cd LiteRT-LM || exit 1

# Fix for LiteRT-LM v0.17.1 issue #3247
# pin_litert_lm_0_17_1() {
#     local repo="https://github.com/Uralstech/LiteRT-LM.git"
#     local commit="dadb79c64229af33dab023e94e7030e872b7a915"
# 
#     # The above fork at the commit ID is equivalent to running:
#     # > git reset --hard v0.17.1
#     # > git cherry-pick a1b8ec1
# 
#     git fetch "${repo}" "${commit}" || return 1
#     git reset --hard "${commit}" || return 1
# }

# pin_litert_lm_0_17_1 || exit 1
HEAD_COMMIT="$(git rev-parse HEAD)"

PLUGIN_DIR="../../UAI.LiteRTLM/Packages/com.uralstech.uai.litertlm/Runtime/Plugins"

BUILT_SYMBOL="liblitert-lm"
BUILD_DIR="./bazel-bin/c"

PREBUILTS_DIR="./prebuilt"

CLEANUP_FILES=""
trap 'for file in ${CLEANUP_FILES}; do rm -rf "${file}"; done' EXIT

# BUILD_FILE="./c/BUILD"
# BUILD_FILE_BACKUP="$(mktemp)"
# CLEANUP_FILES="${CLEANUP_FILES} ${BUILD_FILE_BACKUP}"

PREBUILT_LIBS_COMMON="libLiteRt libGemmaModelConstraintProvider"
PREBUILT_LIBS_ANDROID="libLiteRtTopKOpenClSampler libLiteRtOpenClAccelerator"
PREBUILT_LIBS_APPLE="libLiteRtTopKMetalSampler libLiteRtMetalAccelerator"

build() {
    local config="$1"
    shift

    bazel build                     \
        --config="${config}"        \
        -c opt "$@"                 \
        //c:litert-lm || return 1

}

patch_prebuilt_lib_android() {
    local arch="$1"
    local lib="$2"
    local needed="$3"
    local dst="${PLUGIN_DIR}/Android/${arch}/${lib}.so"

    if ! patchelf --print-needed "${dst}" | grep -q "^${needed}\.so$"; then
        patchelf --add-needed "${needed}.so" "${dst}" || return 1
    fi
}

force_copy_file() {
    local src_file="$1"
    local dst_file="$2"

    rm -f "${dst_file}"
    cp "${src_file}" "${dst_file}"
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
    shift 3

    local temp_dir="$(mktemp -d)" || return 1
    CLEANUP_FILES="${CLEANUP_FILES} ${temp_dir}"

    local frameworks=""
    for arch in ${archs}; do

        local framework_name="${lib_name}.framework"
        local framework_path="${temp_dir}/${arch}/${framework_name}"

        mkdir -p "${framework_path}"
        local framework_lib_path="${framework_path}/${lib_name}"

        lipo -create "${src_dir}/ios_${arch}/${lib_name}.dylib" -output "${framework_lib_path}" || return 1
        install_name_tool -id "@rpath/${framework_name}/${lib_name}"    "${framework_lib_path}" || return 1

        for dependency in $@; do

            install_name_tool -add_rpath "@executable_path/Frameworks/${dependency}.framework" "${framework_lib_path}" || return 1
        done

        cat <<EOF > "${framework_path}/Info.plist"
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>CFBundleName</key>
    <string>${lib_name}</string>
    <key>CFBundleIdentifier</key>
    <string>com.uralstech.uai.litertlm.native.ios.${lib_name}</string>
    <key>CFBundleVersion</key>
    <string>1</string>
    <key>CFBundleShortVersionString</key>
    <string>1.0.0</string>
    <key>CFBundleExecutable</key>
    <string>${lib_name}</string>
    <key>CFBundlePackageType</key>
    <string>FMWK</string>
    <key>MinimumOSVersion</key>
    <string>15.0</string>
</dict>
</plist>
EOF

        frameworks="${frameworks} ${framework_path}"
    done

    local dst="${PLUGIN_DIR}/iOS/${lib_name}.xcframework"
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

        create_xcframework_ios "${lib_name}" "${PREBUILTS_DIR}" "${archs}" || return 1
    done

    create_xcframework_ios "${BUILT_SYMBOL}" "${build_dir}" "${archs}" || return 1
}

# For LiteRT-LM v0.16.0
# patch_capabilities() {
#     cp "${BUILD_FILE}" "${BUILD_FILE_BACKUP}" || return 1
# 
#     extract_trap_cmd() { echo "$3"; }
#     old_trap="$(eval "extract_trap_cmd $(trap -p EXIT)")"
# 
#     trap 'force_copy_file "${BUILD_FILE_BACKUP}" "${BUILD_FILE}"; ${old_trap}' EXIT
#     python3 "${BUILD_SCRIPTS_DIR}/patch_capabilities.py" "$BUILD_FILE" || return 1
# }

copy_libs() {
    local platform="$1"
    local arch="$2"
    local extension="$3"
    local plugin_subdir="$4"
    local platform_libs="$5"

    local src="${PREBUILTS_DIR}/${platform}_${arch}"
    local dst="${PLUGIN_DIR}/${plugin_subdir}/${arch}"

    mkdir -p "$dst"
    for lib in ${PREBUILT_LIBS_COMMON} ${platform_libs}; do

        force_copy_file                     \
            "${src}/${lib}.${extension}"    \
            "${dst}/${lib}.${extension}"
    done

    force_copy_file                                 \
        "${BUILD_DIR}/${BUILT_SYMBOL}.${extension}" \
        "${dst}/${BUILT_SYMBOL}.${extension}"
}

# patch_capabilities || exit 1

# Set ANDROID_NDK_HOME before running this
# ------------------------------ Android ------------------------------

build android_arm64 --linkopt=-Wl,-z,max-page-size=16384 || exit 1
copy_libs android arm64 so Android "${PREBUILT_LIBS_ANDROID}"

patch_prebuilt_lib_android arm64 libLiteRtTopKOpenClSampler "${BUILT_SYMBOL}" || exit 1
# patch_prebuilt_lib_android arm64 libLiteRtOpenClAccelerator "libandroid" || exit 1 # LiteRT-LM v0.17.0-specific

# ------------------------------  macOS  ------------------------------

# Note: Unity needs to load the Metal accelerators on startup for
# liblitert-lm.dylib to register it.

build macos_arm64 --linkopt=-Wl,-rpath,@loader_path \
    --define=litert_runtime_link_mode=dynamic || exit 1

copy_libs macos arm64 dylib macOS "${PREBUILT_LIBS_APPLE}"


# ------------------------------   iOS   ------------------------------

ios_out="$(mktemp -d)" || exit 1
CLEANUP_FILES="${CLEANUP_FILES} ${ios_out}"

build ios_arm64 || exit 1
copy_ios_output arm64 "${ios_out}"

build ios_sim_arm64 || exit 1
copy_ios_output sim_arm64 "${ios_out}"

package_xcframeworks_ios "${ios_out}" "arm64 sim_arm64" || exit 1

echo "LITERT_LM_REV = \"${HEAD_COMMIT}\"" > "${PLUGIN_DIR}/.build_sources.arm64.txt"