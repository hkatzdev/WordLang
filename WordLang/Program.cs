using ArgumentsUtil;
using System;
using System.IO;

namespace WordLang
{
    class Program
    {
        static void Main(string[] args)
        {
            ColorConsole.ConsoleWriter CConsole = new ColorConsole.ConsoleWriter();
            //args = new[] { "test.wl" };

            Arguments a = Arguments.Parse(args, (char)KeySelector.CrossPlatformCompatible);

            if (a.Keyless.Count > 0)
            {
                bool debug = a.ContainsKey("d");
                bool comments = debug; // a.ContainsKey("c");

                for (int i = 0; i < a.Keyless.Count; i++)
                {
                    if (File.Exists(a.Keyless[i]))
                    {
                        if (a[i].ToLower().EndsWith(".wl") || a[i].ToLower().EndsWith(".w"))
                        {
                            if (a.Keyless.Count > 1)
                            {
                                if (i != 0) Console.Write('\n');
                                CConsole.WriteLine($"==== {a[i].Split('\\')[a[i].Split('\\').Length - 1]} ====", ConsoleColor.Yellow);
                            }
                            new Interpreter(a[i]).Interpret(debug, comments);

                            // Console.ReadKey(true); // Pause
                        }
                        else Console.WriteLine($"The file: '{a[i]}' is not a WordLang file!");
                    }
                    else Console.WriteLine($"The file: '{a[i]}' was not found!");
                }
            }
            else
            {
                ArgumentsTemplate at = new ArgumentsTemplate(
                    new System.Collections.Generic.List<ArgumentOption>
                    {
                        new ArgumentOption("d", "debug", "Enable debugging.")
                    },
                    false,
                    new System.Collections.Generic.List<ArgumentCommand>
                    {
                        new ArgumentCommand("[files]", "Files to interpret.")
                    },
                    true,
                    null,
                    null,
                    (char)KeySelector.CrossPlatformCompatible
                );

                at.ShowManual(HelpFormatting.TitleUnderlines);
            }
        }
    }
}
