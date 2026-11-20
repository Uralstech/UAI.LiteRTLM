# Quick Start

The example code in this quick start guide is provided for educational and demonstration purposes only.
It may not represent best practices for production use.

> [!IMPORTANT]
> this is vibe coded. i don't provide any guarantees on it working, or the quality of its code.
> i will not be publishing this on any package management services, like OpenUPM.

## What is this?

This is a package that aims to wrap LiteRT-LM's Omni API, along with some tweaks to both the Omni API and the main LiteRT-LM API.

## Download

> [!WARNING]
> uai.litertlm.omni ***IS NOT*** compatible with uai.litertlm!
> ***DO NOT*** use both packages at the same time. This will crash the editor/player due to symbols being present in both packages' native plugins.
> uai.litertlm.omni is a superset of uai.litertlm.

### Unity Package Manager

1. Open the Unity Package Manager window (`Window` -> `Package Manager`)
2. Select the `+` icon and `Add package from git URL...`
3. Paste the package URL and press enter:
    - `https://github.com/Uralstech/UAI.LiteRTLM.git?path=UAI.LiteRTLM.Omni/Packages/com.uralstech.uai.litertlm.omni`

### GitHub Clone

1. Clone or download the repository from the desired branch (master, preview/unstable) or tag
2. Drag the package folder `UAI.LiteRTLM/UAI.LiteRTLM.Omni/Packages/com.uralstech.uai.litertlm.omni` into your Unity project's `Packages` folder

## Usage

The package provides both a low-level and high-level implementation of the Omni API. Please read the Omni API's documentation from LiteRT-LM's repo
for how to use it. You can look up functions wrapped by this package in the reference manual.