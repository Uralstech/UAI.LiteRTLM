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
HEAD_COMMIT=$(git rev-parse HEAD)

PLUGIN_DIR="../../UAI.LiteRTLM/Packages/com.uralstech.uai.litertlm/Runtime/Plugins"

BUILT_SYMBOL="liblitert-lm"
BUILD_DIR="./bazel-bin/c"

# BUILD_FILE="./c/BUILD"
# BUILD_FILE_BACKUP="$(mktemp)"

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

# https://github.com/dotnet/docs/blob/main/docs/core/deploying/native-aot/ios-like-platforms/creating-and-consuming-custom-frameworks.md
construct_framework_ios() {
    local arch="$1"
    local platform_libs="$2"

    local framework="${BUILT_SYMBOL}.framework"

    local dst="${PLUGIN_DIR}/iOS/${arch}/${framework}"
    local main_lib="${dst}/${BUILT_SYMBOL}"

    local prebuilts_src="./prebuilt/ios_${arch}"
    local prebuilts_dst="${dst}/Libraries"

    rm -rf "${dst}"
    mkdir -p "${dst}"

    lipo -create "${BUILD_DIR}/${BUILT_SYMBOL}.dylib" -output       "${main_lib}" || return 1
    install_name_tool -id "@rpath/${framework}/${BUILT_SYMBOL}"     "${main_lib}" || return 1
    install_name_tool -add_rpath "@loader_path/Libraries"           "${main_lib}" || return 1

    cat <<EOF > "${dst}/Info.plist"
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>CFBundleName</key>
    <string>${BUILT_SYMBOL}</string>
    <key>CFBundleIdentifier</key>
    <string>com.uralstech.uai.litertlm.native.ios</string>
    <key>CFBundleVersion</key>
    <string>1.0</string>
    <key>CFBundleExecutable</key>
    <string>${BUILT_SYMBOL}</string>
    <key>CFBundlePackageType</key>
    <string>FMWK</string>
</dict>
</plist>
EOF

    mkdir -p "${prebuilts_dst}"
    for lib in ${PREBUILT_LIBS_COMMON} ${platform_libs}; do

        force_copy_file             \
            "${prebuilts_src}/${lib}.dylib"   \
            "${prebuilts_dst}/${lib}.dylib"
    done
}

force_copy_file() {
    local src_file="$1"
    local dst_file="$2"

    rm -f "${dst_file}"
    cp "${src_file}" "${dst_file}"
}

# For LiteRT-LM v0.16.0
# patch_capabilities() {
#     cp "${BUILD_FILE}" "${BUILD_FILE_BACKUP}" || return 1
#     trap 'force_copy_file "${BUILD_FILE_BACKUP}" "${BUILD_FILE}"; rm -f "${BUILD_FILE_BACKUP}"' EXIT
#     python3 "${BUILD_SCRIPTS_DIR}/patch_capabilities.py" "$BUILD_FILE" || return 1
# }

copy_libs() {
    local platform="$1"
    local arch="$2"
    local extension="$3"
    local plugin_subdir="$4"
    local platform_libs="$5"

    local src="./prebuilt/${platform}_${arch}"
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

build ios_arm64 || exit 1
construct_framework_ios arm64 "${PREBUILT_LIBS_APPLE}" || exit 1

build ios_sim_arm64 || exit 1
construct_framework_ios sim_arm64 "${PREBUILT_LIBS_APPLE}" || exit 1

echo "LITERT_LM_REV = \"${HEAD_COMMIT}\"" > "${PLUGIN_DIR}/.build_sources.arm64.txt"