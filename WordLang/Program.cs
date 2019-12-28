using ArgumentsUtil;
using System;
using System.IO;

namespace WordLang
{
    class Program
    {
        static void Main(string[] args)
        {
            args = new[] { "test.wl" };

            Arguments a = Arguments.Parse(args);

            if (a.Keyless.Count > 0)
            {
                bool debug = a.ContainsKey("d");

                for (int i = 0; i < a.Keyless.Count; i++)
                {
                    if (File.Exists(a.Keyless[i]))
                    {
                        if (a[i].ToLower().EndsWith(".wl") || a[i].ToLower().EndsWith(".w"))
                        {
                            new Interpreter(a[i]).Interpret();

                            Console.ReadLine(); // Pause
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
