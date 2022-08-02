using NormandyNET.Core;
using System;
using System.Numerics;

namespace NormandyNET.Modules.ARMA
{
    internal class VisualState
    {
        internal ulong visualState;

        internal VisualState(ulong _visualState)
        {
            visualState = _visualState;
        }

        internal void SetPosition(Vector3 PositionNew)
        {
            var buff = new byte[sizeof(float) * 3];
            Vector3 DirectionNew = new Vector3(0, 1, 0);

            Buffer.BlockCopy(BitConverter.GetBytes(PositionNew.X), 0, buff, 0 * sizeof(float), sizeof(float));
            Buffer.BlockCopy(BitConverter.GetBytes(PositionNew.Y), 0, buff, 1 * sizeof(float), sizeof(float));
            Buffer.BlockCopy(BitConverter.GetBytes(PositionNew.Z), 0, buff, 2 * sizeof(float), sizeof(float));

            Memory.Write<Vector3>(visualState + ModuleARMA.offsetsARMA.VisualState_Position, PositionNew);
            Console.WriteLine($"visualState {visualState:X2} - {(visualState + ModuleARMA.offsetsARMA.VisualState_Position):x2} - {CommonHelpers.ByteArrayToString(buff, true)}");
        }

        internal Vector3 GetPosition()
        {
            var databuffer = Memory.ReadBytes(visualState + ModuleARMA.offsetsARMA.VisualState_Position, sizeof(float) * 3);

            return new Vector3(
                   BitConverter.ToSingle(databuffer, 0x0),
                   BitConverter.ToSingle(databuffer, 0x4),
                   BitConverter.ToSingle(databuffer, 0x8));
        }

        internal Vector3 GetHeadPosition()
        {
            var databuffer = Memory.ReadBytes(visualState + ModuleARMA.offsetsARMA.VisualState_HeadPos, sizeof(float) * 3);
            return new Vector3(
                   BitConverter.ToSingle(databuffer, 0x0),
                   BitConverter.ToSingle(databuffer, 0x4),
                   BitConverter.ToSingle(databuffer, 0x8));
        }
    }
}