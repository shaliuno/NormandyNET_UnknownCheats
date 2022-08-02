using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace NDepend.Product.ErrorHandling
{
    public static class StackTraceHelper
    {
        public static string FormatStackTrace(Exception ex)
        {
            Debug.Assert(ex != null);
#if DEBUG
            return FormatUnlocalizedStackTraceFromString(ex.StackTrace);
#else
         return FormatUnlocalizedStackTraceWithILOffset(ex);
#endif
        }

        #region FormatUnlocalizedStackTraceWithILOffset()

        public static string FormatUnlocalizedStackTraceWithILOffset(Exception ex)
        {
            Debug.Assert(ex != null);
            try
            {
                var stackTrace = new StackTrace(ex, true);
                StackFrame[] stackFrames = stackTrace.GetFrames();
                if (stackFrames == null || stackFrames.Length == 0) { return FormatUnlocalizedStackTraceFromString(ex.StackTrace); }

                var sb = new StringBuilder();

                var stackFramesLength = new int[stackFrames.Length];
                for (var i = 0; i < stackFramesLength.Length; i++)
                {
                    var stackFrame = stackFrames[i];
                    var method = stackFrame.GetMethod();
                    var parameters = GetMethodParameters(method);
                    var ilOffset = GetILOffset(stackFrame.GetILOffset());
                    sb.Append($"  {method.ReflectedType.FullName}.{method.Name}({parameters})   {ilOffset} \r\n");
                }
                return sb.ToString();
            }
            catch (Exception)
            {
                return FormatUnlocalizedStackTraceFromString(ex.StackTrace);
            }
        }

        private static string GetILOffset(int ilOffset)
        {
            Debug.Assert(ilOffset >= 0);

            var ilOffsetHexString = ilOffset.ToString("X").ToLower();

            var sb = new StringBuilder("L_");
            if (ilOffsetHexString.Length < 4)
            {
                sb.Append(new string('0', 4 - ilOffsetHexString.Length));
            }
            sb.Append(ilOffsetHexString);
            return sb.ToString();
        }

        private static string GetMethodParameters(MethodBase method)
        {
            Debug.Assert(method != null);

            if (!TryGetParameters(method, out ParameterInfo[] parameters, out string failureReason))
            {
                Debug.Assert(!string.IsNullOrEmpty(failureReason));
                return failureReason;
            }
            Debug.Assert(parameters != null);

            var length = parameters.Length;
            var sb = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var parameter = parameters[i];
                sb.Append(parameter.ParameterType.Name);
                sb.Append(" ");
                sb.Append(parameter.Name);
                if (i < length - 1)
                {
                    sb.Append(", ");
                }
            }
            return sb.ToString();
        }

        private static bool TryGetParameters(MethodBase method, out ParameterInfo[] parameters, out string failureReason)
        {
            Debug.Assert(method != null);
            try
            {
                parameters = method.GetParameters();
                Debug.Assert(parameters != null);
                failureReason = null;
                return true;
            }
            catch (FileLoadException ex)
            {
                failureReason = $@"Exception thrown while calling MethodBase.GetParameters()   Exception Type: {{{ex.GetType().ToString()}}}  Exception Message: {{{ex.Message}}}";
                parameters = null;
                return false;
            }
        }

        #endregion FormatUnlocalizedStackTraceWithILOffset()

        #region FormatUnlocalizedStackTraceFromString()

        public const string EMPTY_STACK_TRACE = "Empty StackTrace";

        public static string FormatUnlocalizedStackTraceFromString(string stackTraceIn)
        {
            if (string.IsNullOrEmpty(stackTraceIn))
            {
                return EMPTY_STACK_TRACE;
            }

            var lines = stackTraceIn.Split(new char[] { '\r' });
            Debug.Assert(lines != null);
            Debug.Assert(lines.Length >= 1);

            var sb = new StringBuilder();

            for (var i = 0; i < lines.Length; i++)
            {
                var unlocalizedLine = UnlocalizeLine(lines[i]);
                if (i > 0) { sb.Append("\r\n"); }
                sb.Append(unlocalizedLine);
            }

            return sb.ToString();
        }

        private static object UnlocalizeLine(string lineIn)
        {
            Debug.Assert(lineIn != null);
            lineIn = lineIn.Replace("\n", "");

            int indexFirstNonWhiteSpace = 0;
            for (indexFirstNonWhiteSpace = 0; indexFirstNonWhiteSpace < lineIn.Length; indexFirstNonWhiteSpace++)
            {
                if (lineIn[indexFirstNonWhiteSpace] != ' ') { break; }
            }

            var indexOfSecondSpace = lineIn.IndexOf(' ', indexFirstNonWhiteSpace);
            if (indexOfSecondSpace == -1) { return lineIn; }

            var lineOut = lineIn.Substring(indexOfSecondSpace, lineIn.Length - indexOfSecondSpace);
            return lineOut;
        }

        #endregion FormatUnlocalizedStackTraceFromString()
    }
}