using System;
using System.Runtime.InteropServices;

public static class AddressHelper
{
    private static object mutualObject;
    private static ObjectReinterpreter reinterpreter;

    static AddressHelper()
    {
        AddressHelper.mutualObject = new object();
        AddressHelper.reinterpreter = new ObjectReinterpreter();
        AddressHelper.reinterpreter.AsObject = new ObjectWrapper();
    }

    public static IntPtr GetAddress(object obj)
    {
        lock (AddressHelper.mutualObject)
        {
            AddressHelper.reinterpreter.AsObject.Object = obj;
            IntPtr address = AddressHelper.reinterpreter.AsIntPtr.Value;
            AddressHelper.reinterpreter.AsObject.Object = null;
            return address;
        }
    }

    public static T GetInstance<T>(IntPtr address)
    {
        lock (AddressHelper.mutualObject)
        {
            AddressHelper.reinterpreter.AsIntPtr.Value = address;
            return (T)AddressHelper.reinterpreter.AsObject.Object;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct ObjectReinterpreter
    {
        [FieldOffset(0)] public ObjectWrapper AsObject;
        [FieldOffset(0)] public IntPtrWrapper AsIntPtr;
    }

    private class ObjectWrapper
    {
        public object Object;
    }

    private class IntPtrWrapper
    {
        public IntPtr Value;
    }
}