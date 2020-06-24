using System;
using System.Diagnostics;

namespace CdsSchemaFileGenerator
{
    static class Log
    {
        internal static void Error(string message)
        {
            Trace.TraceError(message);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        internal static void Information(string message)
        {
            Trace.TraceInformation(message);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        internal static void Progress(string message)
        {
            Trace.TraceInformation(message);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Trace, but don't write to console.
        /// </summary>
        /// <param name="message"></param>
        internal static void Silently(string message)
        {
            Trace.TraceInformation(message);
        }

        internal static void Significant(string message)
        {
            Trace.TraceInformation(message);
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        internal static void Success(string message)
        {
            Trace.TraceInformation(message);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        internal static void Verbose(string message)
        {
            Trace.TraceInformation(message);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        internal static void Warning(string message)
        {
            Trace.TraceWarning(message);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
