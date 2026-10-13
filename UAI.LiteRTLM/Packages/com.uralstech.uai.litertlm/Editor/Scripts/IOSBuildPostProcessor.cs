// Copyright 2026 URAV ADVANCED LEARNING SYSTEMS PRIVATE LIMITED
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#if UNITY_IOS

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;

#nullable enable
namespace Uralstech.UAI.LiteRTLM.Editor
{
    internal sealed class IOSBuildPostProcessor : IPostprocessBuildWithReport
    {
        private const string SigningScript = @"set -eu

SIGN_ID=""${EXPANDED_CODE_SIGN_IDENTITY}""
FW=""${TARGET_BUILD_DIR}/${FRAMEWORKS_FOLDER_PATH}/liblitert-lm.framework""

sign_lib() {
  local file=""$1""

  codesign --force --sign ""${SIGN_ID}"" --timestamp=none ""${file}""
  codesign --verify --strict ""${file}""
}

for dylib in ""$FW""/Libraries/*.dylib; do
  sign_lib ""${dylib}""
done

sign_lib ""${FW}""
";
        
        public int callbackOrder => 999;
        
        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.iOS
                || report.summary.buildType != BuildType.Player
                || report.summary.result is BuildResult.Failed or BuildResult.Cancelled
                || PlayerSettings.iOS.sdkVersion is iOSSdkVersion.SimulatorSDK)
                return;
            
            string projectPath = PBXProject.GetPBXProjectPath(report.summary.outputPath);
            
            PBXProject project = new();
            project.ReadFromFile(projectPath);

            string mainTargetGuid = project.GetUnityMainTargetGuid();
            project.AddShellScriptBuildPhase(mainTargetGuid, "LiteRT-LM Dependency Signing", "/bin/sh", SigningScript);
            
            project.WriteToFile(projectPath);
        }
    }
}

#endif