using Mono.Simd;
using NormandyNET.Core;
using System;
using UnityEngine;
using static NormandyNET.Modules.EFT.EFTHelpers;

namespace NormandyNET.Modules.EFT.Objects.Components
{
    internal struct TransformAccessReadOnly
    {
        internal ulong localTransforms_pTransformData;
        internal int index;
    }

    internal struct TransformData
    {
        internal ulong pTransformArray;
        internal ulong pTransformIndices;
    }

    internal struct Matrix34
    {
        internal Vector4f vec0;
        internal Vector4f vec1;
        internal Vector4f vec2;
    }

    internal class Transform : GameObjectComponent
    {
        private ulong tranformPositionPtr;

        internal TransformData transformData;

        public Transform(object[] args) : base(args)
        {
                        
            tranformPositionPtr = Memory.Read<ulong>(this.component + ModuleEFT.offsetsEFT.Transform_TransformAccess);
        }

        public Transform(ulong address) : base(address)
        {
                        
            tranformPositionPtr = Memory.Read<ulong>(this.component + ModuleEFT.offsetsEFT.Transform_TransformAccess);
        }

        internal Vector3 GetPosition()
        {
            if (!Memory.IsValidPointer(this.component) || !Memory.IsValidPointer(tranformPositionPtr))
            {
                return Vector3.negativeInfinity;
            }

            var databuffer = Memory.ReadBytes(tranformPositionPtr + ModuleEFT.offsetsEFT.TransformPosition, sizeof(float) * 3);
            return new UnityEngine.Vector3(
                   BitConverter.ToSingle(databuffer, 0x0),
                   BitConverter.ToSingle(databuffer, 0x4),
                   BitConverter.ToSingle(databuffer, 0x8));
        }

        internal void SetPosition(Vector3 pos)
        {
            if (!Memory.IsValidPointer(this.component) || !Memory.IsValidPointer(tranformPositionPtr))
            {
                return;
            }

            Memory.Write(tranformPositionPtr + ModuleEFT.offsetsEFT.TransformPosition + 0x0, pos.x);
            Memory.Write(tranformPositionPtr + ModuleEFT.offsetsEFT.TransformPosition + 0x4, pos.y);
            Memory.Write(tranformPositionPtr + ModuleEFT.offsetsEFT.TransformPosition + 0x8, pos.z);
        }

        internal unsafe Vector3 GetPositionViaIndices()
        {
            
            Vector4f mulVec0 = new Vector4f(-2.0f, 2.0f, -2.0f, 0.0f);
            Vector4f mulVec1 = new Vector4f(2.0f, -2.0f, -2.0f, 0.0f);
            Vector4f mulVec2 = new Vector4f(-2.0f, -2.0f, 2.0f, 0.0f);

            var pTransformAccessReadOnly = Memory.Read<TransformAccessReadOnly>(this.component + ModuleEFT.offsetsEFT.Transform_TransformAccess);

            var transformData = Memory.Read<TransformData>(pTransformAccessReadOnly.localTransforms_pTransformData + 0x18);

            var MatriciesBufSize = (int)(sizeof(Matrix34) * (uint)pTransformAccessReadOnly.index + sizeof(Matrix34));
            var IndicesBufSize = (int)(sizeof(int) * (uint)pTransformAccessReadOnly.index + sizeof(int));

            if (MatriciesBufSize < 0 || IndicesBufSize < 0)
            {
                return Vector3.zero;
            }

            var MatriciesBuf = new byte[MatriciesBufSize];
            var IndicesBuf = new byte[IndicesBufSize];

            int chunkCurrent = 0;
            int chunkAmountStatic = SynchronousSocketDriverClient.MTU;
            int chunkAmount = chunkAmountStatic;

            
            if (Memory.IsValidPointer(transformData.pTransformArray) & Memory.IsValidPointer(transformData.pTransformIndices))
            {
                chunkCurrent = 0;
                chunkAmount = chunkAmountStatic;

                if (MatriciesBufSize < chunkAmount)
                {
                    chunkAmount = MatriciesBufSize;
                }

                while (chunkCurrent < MatriciesBufSize)
                {
                    var buff = Memory.ReadBytes(transformData.pTransformArray + (uint)chunkCurrent, chunkAmount);
                    Buffer.BlockCopy(buff, 0, MatriciesBuf, chunkCurrent, chunkAmount);
                    chunkCurrent += chunkAmountStatic;

                    if ((MatriciesBufSize - chunkCurrent) < chunkAmountStatic)
                    {
                        chunkAmount = MatriciesBufSize - chunkCurrent;
                    }
                }

                chunkCurrent = 0;
                chunkAmount = chunkAmountStatic;

                if (IndicesBufSize < chunkAmount)
                {
                    chunkAmount = IndicesBufSize;
                }

                while (chunkCurrent < IndicesBufSize)
                {
                    var buff = Memory.ReadBytes(transformData.pTransformIndices + (uint)chunkCurrent, chunkAmount);
                    Buffer.BlockCopy(buff, 0, IndicesBuf, chunkCurrent, chunkAmount);
                    chunkCurrent += chunkAmountStatic;

                    if ((IndicesBufSize - chunkCurrent) < chunkAmountStatic)
                    {
                        chunkAmount = IndicesBufSize - chunkCurrent;
                    }
                }

                                
                var result = new Vector4f(
                    BitConverter.ToSingle(MatriciesBuf, 0x0 + 0x30 * pTransformAccessReadOnly.index),
                    BitConverter.ToSingle(MatriciesBuf, 0x4 + 0x30 * pTransformAccessReadOnly.index),
                    BitConverter.ToSingle(MatriciesBuf, 0x8 + 0x30 * pTransformAccessReadOnly.index),
                    BitConverter.ToSingle(MatriciesBuf, 0xC + 0x30 * pTransformAccessReadOnly.index)
                    );

                var transformIndex = BitConverter.ToInt32(IndicesBuf, 0x4 * pTransformAccessReadOnly.index);

                
                for (int i = 0; transformIndex >= 0 && i < 200; i++)

                {
                    
                    
                    Matrix34 matrix34 = Memory.Read<Matrix34>(transformData.pTransformArray + 0x30 * (uint)transformIndex);

                    Vector4f tmp7 = _mm_mul_ps(matrix34.vec2, result);

                    var xxxx = (Vector4f)(VectorOperations.Shuffle(matrix34.vec1, (ShuffleSel)(0x00)));
                    var yyyy = (Vector4f)(VectorOperations.Shuffle(matrix34.vec1, (ShuffleSel)(0x55)));
                    var zwxy = (Vector4f)(VectorOperations.Shuffle(matrix34.vec1, (ShuffleSel)(0x8E)));
                    var wzyw = (Vector4f)(VectorOperations.Shuffle(matrix34.vec1, (ShuffleSel)(0xDB)));
                    var zzzz = (Vector4f)(VectorOperations.Shuffle(matrix34.vec1, (ShuffleSel)(0xAA)));
                    var yxwy = (Vector4f)(VectorOperations.Shuffle(matrix34.vec1, (ShuffleSel)(0x71)));

                    result = _mm_add_ps(
                        _mm_add_ps(
                            _mm_add_ps(
                                _mm_mul_ps(
                                    _mm_sub_ps(
                                        _mm_mul_ps(_mm_mul_ps(xxxx, mulVec1), zwxy),
                                        _mm_mul_ps(_mm_mul_ps(yyyy, mulVec2), wzyw)),
                                    _mm_shuffle_epi32(tmp7, 0xAA)),
                                _mm_mul_ps(
                                    _mm_sub_ps(
                                        _mm_mul_ps(_mm_mul_ps(zzzz, mulVec2), wzyw),
                                        _mm_mul_ps(_mm_mul_ps(xxxx, mulVec0), yxwy)),
                                    _mm_shuffle_epi32(tmp7, 0x55))),
                            _mm_add_ps(
                                _mm_mul_ps(
                                    _mm_sub_ps(
                                        _mm_mul_ps(_mm_mul_ps(yyyy, mulVec0), yxwy),
                                        _mm_mul_ps(_mm_mul_ps(zzzz, mulVec1), zwxy)),
                                    _mm_shuffle_epi32(tmp7, 0x00)),
                                tmp7)), matrix34.vec0);

                    transformIndex = Memory.Read<int>(transformData.pTransformIndices + 0x4 * (uint)transformIndex);
                }

                
                                var positionFound = new Vector3(result.X, result.Y, result.Z);

                return positionFound;
            }

            return Vector3.negativeInfinity;
        }

        internal Quaternion GetRotation()
        {
            if (!Memory.IsValidPointer(this.component) || !Memory.IsValidPointer(tranformPositionPtr))
            {
                return Quaternion.identity;
            }

            var databuffer = Memory.ReadBytes(tranformPositionPtr + ModuleEFT.offsetsEFT.TransformRotation, sizeof(float) * 4);

            return new UnityEngine.Quaternion(
                   BitConverter.ToSingle(databuffer, 0x0),
                   BitConverter.ToSingle(databuffer, 0x4),
                   BitConverter.ToSingle(databuffer, 0x8),
                   BitConverter.ToSingle(databuffer, 0xC)
                   );
        }

        internal void ForceRefresh()
        {
        }

        public Vector3 forward
        {
            get
            {
                return this.GetRotation() * Vector3.forward;
            }
        }

        public Vector3 up
        {
            get
            {
                return this.GetRotation() * Vector3.up;
            }
        }

        public Vector3 right
        {
            get
            {
                return this.GetRotation() * Vector3.right;
            }
        }
    }
}