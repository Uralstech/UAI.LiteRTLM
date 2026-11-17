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

using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
using UnityEngine;

#nullable enable
namespace Uralstech.UAI.LiteRTLM.Omni.Editor
{
    internal sealed class IOSBuildPostProcessor : IPostprocessBuildWithReport
    {
        private const string DependenciesPath = "Runtime/Plugins/iOS/Deps~";
        
        public int callbackOrder => 999;

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.iOS
                || report.summary.buildType != BuildType.Player
                || report.summary.result is BuildResult.Failed or BuildResult.Cancelled
                || (PlayerSettings.iOS.sdkVersion is iOSSdkVersion.SimulatorSDK
                        && PlayerSettings.iOS.simulatorSdkArchitecture != AppleMobileArchitectureSimulator.ARM64)
                || !PathUtils.GetPackagePath(out string? packagePath))
                return;

            string buildOutput = report.summary.outputPath;
            string projectPath = PBXProject.GetPBXProjectPath(buildOutput);
            
            PBXProject project = new();
            project.ReadFromFile(projectPath);
            
            string mainTargetGuid = project.GetUnityMainTargetGuid();
            string dependencies = Path.Combine(packagePath, DependenciesPath);
            foreach (DirectoryInfo directory in new DirectoryInfo(dependencies).EnumerateDirectories())
            {
                string destination = Path.Combine(buildOutput, "Frameworks", directory.Name);
                if (Directory.Exists(destination))
                    Directory.Delete(destination, recursive: true);
                
                CopyDirectory(directory.FullName, destination);
                
                string relative = Path.GetRelativePath(buildOutput, destination);
                string guid = project.AddFile(relative, relative);
                project.AddFileToEmbedFrameworks(mainTargetGuid, guid, linkToTargetGuid: null);
            }
            
            project.WriteToFile(projectPath);
        }

        private static void CopyDirectory(string source, string destination)
        {
            DirectoryInfo sourceDirectory = new(source);
            Directory.CreateDirectory(destination);
            
            foreach (FileInfo file in sourceDirectory.EnumerateFiles())
                File.Copy(file.FullName, Path.Combine(destination, file.Name), overwrite: true);
            
            foreach (DirectoryInfo subDirectory in sourceDirectory.EnumerateDirectories())
                CopyDirectory( subDirectory.FullName, Path.Combine(destination, subDirectory.Name));
        }
    }
}

#endif
