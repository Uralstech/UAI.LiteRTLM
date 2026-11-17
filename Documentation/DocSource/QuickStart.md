# Quick Start

The example code in this quick start guide is provided for educational and demonstration purposes only.
It may not represent best practices for production use.

## Aim

UAI.LiteRTLM aims to provide a 1:1 interface to the LiteRT-LM cross-platform C API.
As of LiteRT-LM v0.19.0, the C API is now stable. As UAI.LiteRTLM is working to stabilize its own API,
the package is currently in preview.

### Prebuilt Binaries

While UAI.LiteRTLM aims to support all platforms supported by LiteRT-LM, it currently only includes prebuilt native binaries for a
subset of them, depending on the package version. The package aims to provide LiteRT-LM binaries without C patches and builds the default
`c:litert-lm` Bazel target with platform-specific command-line arguments. The build scripts for all platforms are available in
[`Native/`](https://github.com/Uralstech/UAI.LiteRTLM/blob/master/Native). Please note that the scripts may modify or remove files
in the LiteRT-LM submodule's working tree. You can build your own binaries for additional platforms and include them in your
project's `Plugins` directory. UAI.LiteRTLM should use them as-is for that platform.

UAI.LiteRTLM also does not include every LiteRT-LM GPU accelerator. Only one accelerator is included per platform.

Do not include your own binaries for platforms or accelerators that UAI.LiteRTLM already provides binaries for, as this can cause conflicts.
If you want to use your own binaries instead, customize the package and remove the included ones.

Build script contributions are always welcome!

### API

The package API is roughly split into two parts: the [low-level](~/api/Uralstech.UAI.LiteRTLM.Native.yml) API and the [high-level](~/api/Uralstech.UAI.LiteRTLM.yml) API.

#### Low-level API

The low-level API, exposed by various static class, is a simple wrapper around all functions exposed by the LiteRT-LM C API.
It works directly with SafeHandle derivatives and `MonoPInvokeCallback`s, so using this API requires you to manage native C memory yourself.

There's no error handling in the low level API. All native methods return a `StatusCode` indicating success or failure.

#### High-level API

The high-level API is a collection of classes that wrap all native structures and handles defined by the
native API and implement `IDisposable`. References, callbacks, and threading are handled automatically, and some wrappers also
provide additional abstractions to make them easier to use.

Only one feature implemented by the native API is **not** exposed by the managed API: You cannot pass custom additional
data to streamed operations because the wrappers use their own data to track managed `StreamCallback`s.

The high-level API wraps all failures into `LiteRTLMException`s, including messages from the native plugin. Theoretically,
all native functions can fail, so every operation *can* throw a `LiteRTLMException`.

### Compatability Table

| UAI.LiteRTLM     | LiteRT-LM                      | Included Platforms                                                                | Included Accelerators                                                 |
| ---------------- | ------------------------------ | --------------------------------------------------------------------------------- | --------------------------------------------------------------------- |
| 3.0.0-preview.1  | v0.19.1 (`7a03724`)            | Android (arm64)<br/>macOS (arm64)<br/>iOS (arm64, sim_arm64)<br/>Windows (x64)    | CPU<br/>OpenCL (Android)<br/>Metal (macOS, iOS)<br/>WebGPU (Windows)  |

#### LiteRT-LM Notes

# [v0.19.1](#tab/litert-lm-notes-v0191)

- LiteRT-LM's build scripts [force](https://github.com/google-ai-edge/LiteRT-LM/issues/4611) `libwebgpu_dawn` to be linked to the macOS and Android libraries, so it is included.
- GPU-accelerated-sampling is [broken](https://github.com/google-ai-edge/LiteRT-LM/issues/4613) on Windows, so it has been unlinked through a [patch](https://github.com/Uralstech/UAI.LiteRTLM/blob/master/Native/Patches/v0.19.1.patch) to LiteRT-LM's Bazel scripts.

---

> [!IMPORTANT]
> 3.0.0-preview.1 contains many breaking changes from 2.X.X-preview.x

## Android GPU Acceleration

To enable GPU acceleration on Android, add the following to the `<application>` section of your `AndroidManifest.xml` to enable OpenCL:

```xml
<uses-native-library android:name="libvndksupport.so" android:required="false"/>
<uses-native-library android:name="libOpenCL.so" android:required="false"/>
```

## NuGet Package / .NET 6+

Development of the NuGet package has been paused for 3.0.0-preview.1 to focus on stabilizing the Unity C# APIs first.

## Example Script

Documentation for this package is still a WIP.
The following example demonstrates a streamed single-turn conversation using the high-level API, including benchmark collection.

```csharp
using Uralstech.UAI.LiteRTLM;

private async Awaitable RunConversation()
{
    LiteRTLM.SetMinLogLevel(LogSeverity.Info);
    
    string modelPath = Path.Join(Application.persistentDataPath, "model.litertlm");
    string cacheDir = Path.Join(Application.temporaryCachePath, "modelCache");
    Directory.CreateDirectory(cacheDir);
    
    // Speculative decoding performs badly on WebGPU on Windows.
    bool isWindows = Application.platform is RuntimePlatform.WindowsEditor
                                          or RuntimePlatform.WindowsPlayer
                                          or RuntimePlatform.WindowsServer;
    
    bool supportsSpeculativeDecoding;
    using (ModelInfo modelCapabilities = new(modelPath))
        supportsSpeculativeDecoding = modelCapabilities.HasSpeculativeDecodingSupport();

    using SpeculativeDecodingConfig speculativeDecodingConfig = new();
    speculativeDecodingConfig.SetMode(!isWindows && supportsSpeculativeDecoding 
        ? SpeculativeDecodingMode.On : SpeculativeDecodingMode.Off);
    
    using EngineSettings engineSettings = new(modelPath, BackendNames.GPU);
    engineSettings.SetSpeculativeDecodingConfig(speculativeDecodingConfig);
    engineSettings.SetCacheDir(cacheDir);
    engineSettings.EnableBenchmark();
    
    using Engine engine = new(engineSettings);
    using ThinkingConfig thinkingConfig = new();
    thinkingConfig.SetEnableThinking(false);
    
    using ConversationConfig conversationConfig = new();
    conversationConfig.SetThinkingConfig(thinkingConfig);
    
    using Conversation conversation = new(engine, conversationConfig);
    Debug.Log("Engine and conversation created.");

    const string message = "{\"role\":\"user\",\"content\":\"What is the tallest building in the world?\"}";
    TaskCompletionSource<bool> completionSource = new();

    void OnChunk(StreamChunk chunk)
    {
        Debug.Log("Got chunk:"
                  + $"\n\tText: {chunk.GetText()}"
                  + $"\n\tIsFinal: {chunk.IsFinal()}"
                  + $"\n\tError: {chunk.GetError()}"
        );

        if (chunk.IsFinal())
            completionSource.TrySetResult(true);
    }
    
    conversation.SendMessageStream(OnChunk, message);
    await completionSource.Task;
    
    using BenchmarkInfo? benchmarkInfo = conversation.GetBenchmarkInfo();
    if (benchmarkInfo == null)
    {
        Debug.Log("No benchmark info found.");
        return;
    }

    IReadOnlyList<BenchmarkInfo.Turn> prefillTurns = benchmarkInfo.GetPrefillTurns();
    IReadOnlyList<BenchmarkInfo.Turn> decodeTurns = benchmarkInfo.GetDecodeTurns();
    
    Debug.Log("Benchmark info:"
              + $"\n\tInitialization time: {benchmarkInfo.GetTotalInitTime()}"
              + $"\n\tTime to first token: {benchmarkInfo.GetTimeToFirstToken()}"
              + $"\n\tPrefill tokens per second: {prefillTurns[0].TokensPerSecond}, for total: {prefillTurns[0].TokenCount} tokens"
              + $"\n\tDecode tokens per second: {decodeTurns[0].TokensPerSecond}, for total: {decodeTurns[0].TokenCount} tokens"
    );
}
```