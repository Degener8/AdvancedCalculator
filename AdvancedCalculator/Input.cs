using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AdvancedCalculator
{
    class Input
    {
        static bool CheckFileExistence(string path)
        {
            if (File.Exists(path) && File.ReadAllText(path).ToString() != String.Empty)
                return true;
            else
            {
                Console.WriteLine("Файл с входными данными не обнаружен или пуст.");
                
                if(!File.Exists(path))
                File.Create(path);

                return false;
            }
        }

        static string TransformExpression(string original)
        {
            original = original.Replace('.', ',');
            string transformed = string.Empty;
            string current = string.Empty;

            for(int i = 0; i < original.Length - 1; i++)
            {
                current = original[i].ToString();
                
                if(Char.IsDigit(original[i]) && !Char.IsLetterOrDigit(original[i + 1]) && original[i + 1] != ',')
                {
                    transformed += $"{current} ";
                    current = original[i+1].ToString();
                }
            }

            return transformed;
        }
        
        static bool CheckFileFormat(string path)
        {
            int close = 0;
            int open = 0;
            string[] input = File.ReadAllLines(path).ToArray();
            
            foreach(char i in input[0])
            {
                if (i == '(')
                    open++;
                if (i == ')')
                    close++;
            }

            if (close != open)
                return false;

            if (input[0].IndexOf(')') < input[0].IndexOf('('))
                return false;

            char[] chars = input[0].Where(Char.IsLetter).ToArray();
            if (chars.Count() == 0)
                return false;
            else foreach (char letter in chars)
                {
                    if (letter != 'x' && letter != 'X')
                        return false;
                }

            if (input[1].Split(' ').Length != 2
                || input[1].Where(Char.IsLetter).Count() != 0
                || input[2].Where(Char.IsLetter).Count() != 0
                || Regex.Replace(input[0],"[0-9 , . x X + ( ) / * ^]", string.Empty).Replace("-",string.Empty) != string.Empty
                || Regex.Replace(input[0], "[0-9 , . x X ( )]", string.Empty).Split(' ').Length 
                    >= Regex.Replace(input[0], "[/ * + ^ ( )]", string.Empty).Replace("-", string.Empty).Split(' ').Length
                || Double.TryParse(input[2], out double step)
                || (Double.TryParse(input[1].Replace('.', ',').Split(' ')[0], out double min)
                    && Double.TryParse(input[1].Replace('.', ',').Split(' ')[1], out double max))
                || input.Length != 3)
                return false;

            return true;
        }

        static string[] GetInput(string path)
        {
            string[] error = { "0", "0 0", "0" };
            if (CheckFileExistence(path) && CheckFileFormat(path))
            {
                return File.ReadAllLines(path).ToArray();
            }
            else
                return error;
        }


    }
}
