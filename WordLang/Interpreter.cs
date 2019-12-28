using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WordLang
{
    public class Interpreter
    {
        private readonly string sourceFile;

        private Dictionary<string, int> backPoints;
        private Dictionary<string, double> variables;

        public Interpreter(string sourceFile)
        {
            this.sourceFile = sourceFile;

            backPoints = new Dictionary<string, int>();
            variables = new Dictionary<string, double>();
        }

        public void Interpret()
        {
            string source = File.ReadAllText(sourceFile);

            double DataValue = 0;

            string ct = string.Empty;       // Current Token

            int line = 1;
            int column = 0;

            int incrementData = 1; // 1 or -1

            bool isComment = false;
            bool isStoreVariable = false;
            bool isReadVariable = false;

            for (int i = 0; i < source.Length; i++)
            {
                char cc = source[i];
                if (cc == '\r') continue;

                column++;

                if (cc == ' ' || cc == '\t' || cc == '\n')
                {
                    if (cc == '\n') { line++; column = 0; }

                    if (isStoreVariable)
                    {
                        variables[ct] = DataValue;
                        DataValue = 0;
                        isStoreVariable = false;
                    }
                    
                    continue;
                }
                else if (cc == '\'')
                {
                    if (isReadVariable)
                    {
                        if (variables.ContainsKey(ct)) DataValue += variables[ct];
                        else
                        {
                            Console.WriteLine($"\nError: The variable {ct} is undefined!");
                        }
                    }

                    isReadVariable = !isReadVariable;

                    ct = string.Empty;
                    continue;
                }
                else if (cc == '"')
                {
                    isComment = !isComment;

                    ct = string.Empty;
                    continue;
                }

                ct += cc;

                if (cc == ',')
                {
                    incrementData *= -1; // Toggle from 1 -> -1 or vise versa.
                    ct = string.Empty;
                }
                else if (cc == '.')
                {
                    Console.Write(getChar(DataValue));
                    ct = string.Empty;
                    DataValue = 0;
                }
                else if (cc == '<')
                {
                    char input = Console.ReadKey().KeyChar;
                    DataValue += incrementData * input;
                    ct = string.Empty;
                }
                else if (cc == '>')
                {
                    isStoreVariable = true;
                    ct = string.Empty;
                }
                else if (cc == '?')
                {
                    // Show debug
                    Console.WriteLine($"\nData value at line {line}, column {column} is '{getChar(DataValue)}'={getAsciiValue(DataValue)} ({DataValue})");
                }
                else
                {
                    // Normal character
                    if (!isStoreVariable && !isReadVariable && !isComment) DataValue += incrementData * cc;
                }
            }
        }

        private static int ASCII_SIZE = 255;
        /// <summary>
        /// Get the corresponding character in ASCII spectrum
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private char getChar(double data)
        {
            return (char)( getAsciiValue(data) % ASCII_SIZE );
        }

        private double getAsciiValue(double data)
        {
            while (data < 0) data += ASCII_SIZE;
            return data % ASCII_SIZE;
        }
    }
}
