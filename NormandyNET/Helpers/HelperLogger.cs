using NormandyNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

internal static class HelperLogger
{
    internal static bool saveToFile = false;

    internal class HelperEntry
    {
        internal ulong lineNumber;
        internal DateTime logTime;
        internal string logText;

        internal HelperEntry(ulong _lineNumber, DateTime _logTime, string _logText)
        {
            lineNumber = _lineNumber;
            logTime = _logTime;
            logText = _logText;
            countLine++;
        }
    }

    private static uint keepSeconds = 60;
    private static int keepEntries = 120000;
    private static ulong countLine = 0;
    internal static List<HelperEntry> memoryLog = new List<HelperEntry>();
    internal static string logFile = "_trace.log";
    internal static string logFileZip = "_trace.zip";
    internal static bool saveLog;
    internal static DateTime cleanOldTime = CommonHelpers.dateTimeHolder;
    internal static EventHandler<EventArgs> OnLogSavedEvent;

    internal static void CleanOldEntries()
    {
        try
        {
            SaveLogToFileComplete();
        }
        catch
        {
        }
    }

    internal static void SaveLogToFile(bool running)
    {
        if (running)
        {
            saveLog = true;
        }
        else
        {
            SaveLogToFileComplete();
        }
    }

    internal static void SaveLogToFileComplete()
    {
        try
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logFile, true, Encoding.Unicode))
            {
                for (int i = 0; i < memoryLog.Count; i++)
                {
                    var txt = memoryLog[i].lineNumber + " : " + memoryLog[i].logTime.ToString("yyyy.MM.dd HH:mm:ss:fff") + " : " + memoryLog[i].logText;
                    file.WriteLine(txt);
                }

                file.WriteLine();
            }

            saveLog = false;
            memoryLog.Clear();

            EventHandler<EventArgs> doLogSaveEvent = OnLogSavedEvent;
            if (doLogSaveEvent != null)
            {
                doLogSaveEvent(null, new EventArgs());
            }
        }
        catch
        {
        }
    }

    internal static void Init(string fileName)
    {
        logFile = $"_trace_{fileName}.log";
        countLine = 0;

        if (File.Exists(logFile))
        {
            try
            {
                File.Delete(logFile);
            }
            catch
            {
            }
        }

        if (File.Exists(logFileZip))
        {
            try
            {
                File.Delete(logFileZip);
            }
            catch
            {
            }
        }

            }

    internal static void LogEntry(string entry, bool console = false)
    {
        string myTime = CommonHelpers.dateTimeHolder.ToString("yyyy.MM.dd HH:mm:ss:ffff");

        var logText = new HelperEntry(countLine, CommonHelpers.dateTimeHolder, entry);

        if (saveToFile)
        {
            memoryLog.Add(logText);
        }

        if (memoryLog.Count > keepEntries)
        {
            CleanOldEntries();
        }

        if (console)
        {
            Console.WriteLine(string.Concat(myTime, " : ", entry));
        }

        if (saveLog)
        {
            SaveLogToFileComplete();
        }
    }

    public enum LogLevel
    {
        Trace = 0,

        Debug = 1,

        Information = 2,

        Warning = 3,

        Error = 4,

        Critical = 5,

        None = 6,
    }
}