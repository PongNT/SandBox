﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubyTouch.SandBox.SimplePerfTester
{
    public static class ConsoleUtils
    {


        #region Indentation
        public static short CurrentIndentLevel { get; private set; } = 1;

        private static short indentLevelSpaceLengh;
        private static short maxIndentDepth = 4;
        #endregion

        private static short maxLineSepLength;

        static ConsoleUtils() => Init();

        public static void ResetIndentLEvel() => CurrentIndentLevel = 0;

        public static void Indent() => CurrentIndentLevel = (short)Math.Min(CurrentIndentLevel + 1, maxIndentDepth);
        public static void UnIndent() => CurrentIndentLevel = (short)Math.Max(CurrentIndentLevel - 1, 0);

        public static void Init(short indentLevelSpaceLengh = 4)
        {
            ConsoleUtils.indentLevelSpaceLengh = indentLevelSpaceLengh;
            maxLineSepLength = (short)Console.BufferWidth;
            ResetIndentLEvel();
        }

        public static void WriteTitle(string title, short? indentLevel = null)
        {
            if (indentLevel == null)
                Indent();
            else
                CurrentIndentLevel = indentLevel.Value;

            char carsep;
            switch (indentLevel)
            {
                case 0:
                    title = title.ToUpper();
                    carsep = '_';
                    break;
                case 1:
                    carsep = '_';
                    break;
                case 2:
                    carsep = '-';
                    break;
                default:
                    carsep = '-';
                    break;
            }

            WriteLine();
            var lineLength = maxLineSepLength - (CurrentIndentLevel * indentLevelSpaceLengh);
            WriteLine(new string(carsep, lineLength), InfoType.title, CurrentIndentLevel, Console.ForegroundColor);
            WriteLine();
            WriteLine(title, InfoType._default, CurrentIndentLevel, GetColorForInfoType(InfoType.title));
            if (indentLevel == 0) Console.WriteLine();
        }

        public static void WriteLine() => WriteLine(null);

        public static void WriteLine(string r, InfoType type = InfoType._default, short? indentLevel = null, ConsoleColor? color = null) =>
            Write_core(r, type, indentLevel ?? (short)(CurrentIndentLevel + 1), true, (color.HasValue) ? color.Value : GetColorForInfoType(type));

        public static void Write(string r, InfoType type = InfoType._default, short? indentLevel = null, ConsoleColor? color = null) => Write_core(r, type, indentLevel ?? (short)(CurrentIndentLevel + 1), false, (color.HasValue) ? color.Value : GetColorForInfoType(type));

        private static void Write_core(string r, InfoType type, short indentLevel, bool withNewLine, ConsoleColor color)
        {
            //if (indentLevel == null) indentLevel = (short)(CurrentIndentLevel + 1);

            Console.ForegroundColor = color; // GetColorForInfoType(type);
            var outPut = GetIndentationSpace(indentLevel) + r;
            if (withNewLine) Console.WriteLine(outPut); else Console.Write(outPut);
            Console.ResetColor();
        }

        private static ConsoleColor GetColorForInfoType(InfoType type)
        {
            switch (type)
            {
                case InfoType.title:
                    return ConsoleColor.Blue;
                case InfoType.result:
                    return ConsoleColor.DarkYellow;
                case InfoType.error:
                    return ConsoleColor.Red;
                case InfoType.warning:
                    return ConsoleColor.Magenta;
                default:
                    return Console.ForegroundColor;
            }
        }

        public static string GetIndentationSpace(short? indentLevel = null) => ' '.Repeat((indentLevel??CurrentIndentLevel) * indentLevelSpaceLengh);

        private static string Repeat(this char c, int count) => new string(c, count);

        public static void WriteProgress(int current, int total)
        {
            Console.WriteLine($"...{current}/{total}...       ");
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
    }
    public enum InfoType { _default, title, result, error, warning }
}
