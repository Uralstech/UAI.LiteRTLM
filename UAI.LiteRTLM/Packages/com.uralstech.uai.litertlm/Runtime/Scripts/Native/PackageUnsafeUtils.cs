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

using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

#nullable enable
namespace Uralstech.UAI.LiteRTLM.Native
{
    internal static class PackageUnsafeUtils
    {
        /// <remarks>Use EXCLUSIVELY in <c>using</c> statements</remarks>
        internal readonly struct TempMem : IDisposable
        {
            /// <summary>The pointer.</summary>
            public readonly IntPtr Ptr;
            
            /// <summary>Size in bytes.</summary>
            public readonly UIntPtr Size;
            
            private readonly Allocator _allocator;

            public TempMem(IntPtr ptr, UIntPtr size, Allocator allocator)
            {
                Ptr = ptr;
                Size = size;
                _allocator = allocator;
            }

            public unsafe void Dispose() =>
                UnsafeUtility.Free((void*)Ptr, _allocator);
        }

        /// <remarks>Allocates memory for SHORT-TERM usage.</remarks>
        public static unsafe TempMem Allocate<T>(int count, out Span<T> span)
            where T : unmanaged
        {
            int align = UnsafeUtility.AlignOf<T>();
            int size = UnsafeUtility.SizeOf<T>() * count;
            
            Allocator allocator = ChooseAllocator(size);
            
            void* allocated = UnsafeUtility.Malloc(size, align, allocator);
            span = new Span<T>(allocated, count);
            
            return new TempMem((IntPtr)allocated, (UIntPtr)size, allocator);
        }

        private static Allocator ChooseAllocator(int dataSize)
        {
            return dataSize switch
            {
                <= 5 * 1024 when Awaitable.MainThreadAsync().IsCompleted => Allocator.Temp,
                <= 20 * 1024 * 1024 => Allocator.TempJob,
                _ => Allocator.Persistent,
            };
        }
    }
}