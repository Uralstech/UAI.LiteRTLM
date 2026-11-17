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

using System.Diagnostics.CodeAnalysis;
using System.IO;
using UnityEditor.PackageManager;
using UnityEngine;

#nullable enable
namespace Uralstech.UAI.LiteRTLM.Omni.Editor
{
    internal static class PathUtils
    {
        private const string PackageId = "com.uralstech.uai.litertlm.omni";
        private static string? s_packagePathCached;

        public static bool GetPackagePath([NotNullWhen(true)] out string? path)
        {
            path = s_packagePathCached;
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                return true;

            path = s_packagePathCached = PackageInfo.FindForPackageName(PackageId)?.assetPath;
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                return true;
            
            Debug.LogError("Could not resolve the path to the UAI.LiteRTLM.Omni package.");
            return false;
        }
    }
}