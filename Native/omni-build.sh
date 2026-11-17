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

    bazel build \
        --config="${config}" \
        -c opt "$@" \
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

# ----------------- APPLY VERSION SPECIFIC PATCH(ES) ------------------

LITERTLM_PATCH="../Patches/v0.19.1.patch"
git apply "${LITERTLM_PATCH}"


# Set ANDROID_NDK_HOME before running this
# ------------------------------ Android ------------------------------

build android_arm64 --linkopt=-Wl,-z,max-page-size=16384    \
                    --linkopt=-Wl,-z,common-page-size=16384 || exit 1

copy_libs android arm64 so Android "${PREBUILT_LIBS_ANDROID} libwebgpu_dawn" # Dawn added only for v0.19.x


# ------------------------------ macOS ------------------------------

# Note: Unity needs to load the Metal accelerators on startup for
# liblitert-lm.dylib to register it.

build macos_arm64 --linkopt=-Wl,-rpath,@loader_path \
    --define=litert_runtime_link_mode=dynamic || exit 1

copy_libs macos arm64 dylib macOS "${PREBUILT_LIBS_APPLE} libwebgpu_dawn" # Dawn added only for v0.19.x

echo "LITERT_LM_REV = \"${HEAD_COMMIT}\"" > "${PLUGIN_DIR}/.build_sources.arm64.txt"

git apply -R "${LITERTLM_PATCH}"