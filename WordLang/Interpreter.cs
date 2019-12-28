using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ColorConsole;

namespace WordLang
{
    public class Interpreter
    {
        private readonly string sourceFile;

        private Dictionary<string, int> backPoints;
        private Dictionary<string, double> variables;

        private ConsoleWriter CConsole;

        public Interpreter(string sourceFile)
        {
            this.sourceFile = sourceFile;

            backPoints = new Dictionary<string, int>();
            variables = new Dictionary<string, double>();

            CConsole = new ConsoleWriter();
        }

        public void Interpret(bool debug = false, bool showComments = false)
        {
            string source = File.ReadAllText(sourceFile);

            RegisterBackPoints(source);

            double DataValue = 0;

            string ct = string.Empty;       // Current Token
            char nc = char.MinValue;        // Next Character

            int operationMode = 1;  // 1 (add) or -1 (sub)

            bool isComment = false;
            bool isStoreVariable = false;
            bool isReadVariable = false;
            bool isBackPointStart = false;

            for (int i = 0; i < source.Length; i++)
            {
                char cc = source[i];

                if (i + 1 < source.Length) nc = source[i + 1];
                else nc = char.MinValue;

                if ((cc == ' ' ||
                    cc == '\t' ||
                    cc == '\n' ||
                    cc == '\r' )
                    && !isComment)
                {
                    if (isStoreVariable && ct != string.Empty)
                    {
                        variables[ct] = DataValue;
                        DataValue = 0;
                        isStoreVariable = false;
                    }
                    else if (isBackPointStart && ct != string.Empty) isBackPointStart = false;
                    

                    ct = string.Empty;
                    continue;
                }
                else if (cc == '\'' && !isComment)
                {
                    if (isReadVariable)
                    {
                        if (variables.ContainsKey(ct)) DataValue += operationMode * variables[ct];
                        else
                        {
                            if (debug) CConsole.WriteLine($"\n[Error at {i}: The variable {ct} is undefined!]", ConsoleColor.Red);
                        }
                    }

                    isReadVariable = !isReadVariable;

                    ct = string.Empty;
                    continue;
                }
                else if (cc == '"')
                {
                    if (isComment && showComments) CConsole.WriteLine($"\n[{i}: {ct}]", ConsoleColor.DarkGreen);

                    isComment = !isComment;

                    ct = string.Empty;
                    continue;
                }
                else if (cc == '?' && !isComment)
                {
                    // Show debug
                    if (debug)
                    {
                        CConsole.WriteLine($"\n[{i}: Data value is {DataValue} ({getAsciiValue(DataValue)}='{getChar(DataValue)}')]", ConsoleColor.Yellow);
                        Console.ReadKey(true);
                    }
                    continue;
                }

                ct += cc;

                if (cc == ',' && !isComment)
                {
                    operationMode *= -1; // Toggle from 1 -> -1 or vise versa.
                    ct = string.Empty;
                }
                else if (cc == '.' && !isComment)
                {
                    Console.Write(getChar(DataValue));
                    ct = string.Empty;
                    DataValue = 0;
                }
                else if (cc == '<' && !isComment)
                {
                    char input = Console.ReadKey().KeyChar;
                    DataValue += operationMode * input;
                    ct = string.Empty;
                }
                else if (cc == '>' && !isComment)
                {
                    isStoreVariable = true;
                    ct = string.Empty;
                }
                else if (cc == '-' && !isComment)
                {
                    isBackPointStart = true;
                    ct = string.Empty;
                }
                else if (cc == '!' && nc != '!' && ct != string.Empty && !isComment)   //  BackpointName!
                {
                    string name = ct.Split('!')[0];

                    // Remove the letters from data value (They should't have been added)
                    for (int l = 0; l < ct.Length - 1; l++)
                    {
                        DataValue -= operationMode * ct[l];
                    }

                    int mode = ct.Count(c => c == '!');
                    if (backPoints.ContainsKey(name))
                    {
                        switch (mode)
                        {
                            default: // 1
                                i = backPoints[name];
                                break;
                            case 2:
                                if (DataValue == 0) i = backPoints[name];
                                break;
                            case 3:
                                if (DataValue != 0) i = backPoints[name];
                                break;
                        }
                    }
                    else
                    {
                        if (debug) CConsole.WriteLine($"\n[Error at {i}: The point '{name}' does not exist!]", ConsoleColor.Red);
                    }
                    ct = string.Empty;
                }
                else
                {
                    // Normal character
                    if (!isStoreVariable && !isReadVariable && !isComment && !isBackPointStart) DataValue += operationMode * cc;
                }
            }
        }

        private void RegisterBackPoints(string source)
        {
            bool isBackPointStart = false;
            string ct = string.Empty;

            for (int i = 0; i < source.Length; i++)
            {
                char cc = source[i];

                if (cc == ' ' ||
                    cc == '?' ||
                    cc == '.' ||
                    cc == ',' ||
                    cc == ';' ||
                    cc == '!' ||
                    cc == '\n' ||
                    cc == '\t' ||
                    cc == '\r' ||
                    i == source.Length -1)
                {
                    if (i == source.Length - 1) ct += cc;

                    if (isBackPointStart && ct.Trim() != string.Empty)
                    {
                        backPoints[ct] = i; // Store the current index as a backpoint under the label ct.
                        isBackPointStart = false;
                    }

                    ct = string.Empty;
                    continue;
                }

                ct += cc;

                if (cc == '-')
                {
                    isBackPointStart = true;
                    ct = string.Empty;
                }
            }
        }

        private static int ASCII_START_OFFSET = 0;
        private static int ASCII_SIZE = 255 - ASCII_START_OFFSET;

        /// <summary>
        /// Get the corresponding character in ASCII spectrum
        /// </summary>
        /// <param name="data">The value to convert to a character</param>
        /// <returns>An ASCII representation of a given char</returns>
        private char getChar(double data)
        {
            return (char)getAsciiValue(data);
        }

        private double getAsciiValue(double data)
        {
            while (data < 0) data += ASCII_SIZE;
            return (data % ASCII_SIZE) + ASCII_START_OFFSET;
        }
    }
}
