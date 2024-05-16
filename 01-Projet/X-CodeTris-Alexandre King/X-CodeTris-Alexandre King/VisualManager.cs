using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    /// <summary>
    /// Manage some visual part of the program
    /// </summary>
    static class VisualManager
    {
        //Title of the main menu
        static string[] _visualTitle = new string[7]
            {
                " ░▒▓██████▓▒░ ░▒▓██████▓▒░░▒▓███████▓▒░░▒▓████████▓▒░▒▓████████▓▒░▒▓███████▓▒░░▒▓█▓▒░░▒▓███████▓▒░ ",
                "░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░         ░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░▒▓█▓▒░        ",
                "░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░         ░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░▒▓█▓▒░        ",
                "░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓██████▓▒░    ░▒▓█▓▒░   ░▒▓███████▓▒░░▒▓█▓▒░░▒▓██████▓▒░  ",
                "░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░         ░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░ ",
                "░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░         ░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░ ",
                " ░▒▓██████▓▒░ ░▒▓██████▓▒░░▒▓███████▓▒░░▒▓████████▓▒░  ░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░▒▓███████▓▒░  "
            };

        static string[] _giantOBlock = new string[8]
            {
                "████████████████",
                "████████████████",
                "████████████████",
                "████████████████",
                "████████████████",
                "████████████████",
                "████████████████",
                "████████████████",
            };

        static string[] _giantLBlock = new string[12]
            {
                "████████████████",
                "████████████████",
                "████████████████",
                "████████████████",
                "        ████████",
                "        ████████",
                "        ████████",
                "        ████████",
                "        ████████",
                "        ████████",
                "        ████████",
                "        ████████"
            };

        static string[] _giantTBlock1 = new string[12]
            {
                "████████        ",
                "████████        ",
                "████████        ",
                "████████        ",
                "████████████████",
                "████████████████",
                "████████████████",
                "████████████████",
                "████████",
                "████████",
                "████████",
                "████████"
            };

        static string[] _giantTBlock2 = new string[8]
            {
                "        ████████        ",
                "        ████████        ",
                "        ████████        ",
                "        ████████        ",
                "████████████████████████",
                "████████████████████████",
                "████████████████████████",
                "████████████████████████"
            };

        static string[] _giantZBlock = new string[12]
            {
                "        ████████",
                "        ████████",
                "        ████████",
                "        ████████",
                "████████████████",
                "████████████████",
                "████████████████",
                "████████████████",
                "████████",
                "████████",
                "████████",
                "████████"
            };
        static string[] _giantSBlock = new string[8]
            {
                "        ████████████████",
                "        ████████████████",
                "        ████████████████",
                "        ████████████████",
                "████████████████",
                "████████████████",
                "████████████████",
                "████████████████"
            };

        static string[] _giantIBlock = new string[16]
            {
                "████████",
                "████████",
                "████████",
                "████████",
                "████████",
                "████████",
                "████████",
                "████████",
                "████████",
                "████████",
                "████████",
                "████████",
                "████████",
                "████████",
                "████████",
                "████████"
            };

        /// <summary>
        /// Add all the visual to the Main menu (The title and all the giant tetriminos)
        /// </summary>
        static public void AddVisualToMainMenu()
        {
            int topVisualStartPosition = 8;
            int count = 0;
            //Title
            foreach (string titleString in _visualTitle)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - titleString.Length / 2, topVisualStartPosition + count);
                Console.WriteLine(titleString);
                count++;
            }
            //Left bottom corner
            count = 0;
            foreach (string LBlockString in _giantLBlock)
            {
                SetTextColor("dkyellow");
                Console.SetCursorPosition(LBlockString.Length / 2, Console.WindowHeight - _giantLBlock.Length + count);
                Console.WriteLine(LBlockString);
                count++;
            }
            count = 0;
            foreach (string TBlockString in _giantTBlock1)
            {
                SetTextColor("magenta");
                Console.SetCursorPosition(0, Console.WindowHeight - _giantTBlock1.Length - _giantOBlock.Length + count);
                Console.WriteLine(TBlockString);
                count++;
            }
            count = 0;
            foreach (string OBlockString in _giantOBlock)
            {
                SetTextColor("yellow");
                Console.SetCursorPosition(0, Console.WindowHeight - _giantOBlock.Length + count);
                Console.WriteLine(OBlockString);
                count++;
            }
            count = 0;
            foreach (string TBlockString in _giantTBlock2)
            {
                SetTextColor("magenta");
                Console.SetCursorPosition(_giantOBlock[0].Length + _giantLBlock[0].Length / 2, Console.WindowHeight - _giantTBlock2.Length + count);
                Console.WriteLine(TBlockString);
                count++;
            }
            //Right Bottom corner
            int rightMargin = 3; //remove visual bug caused by the scroll bar
            count = 0;
            foreach (string IBlockString in _giantIBlock)
            {
                SetTextColor("cyan");
                Console.SetCursorPosition(Console.WindowWidth - _giantIBlock[0].Length - rightMargin, Console.WindowHeight - _giantIBlock.Length + count);
                Console.WriteLine(IBlockString);
                count++;
            }
            count = 0;
            foreach (string TBlockString in _giantTBlock2)
            {
                SetTextColor("magenta");
                Console.SetCursorPosition(Console.WindowWidth - _giantIBlock[0].Length - _giantTBlock2[0].Length - rightMargin, Console.WindowHeight - _giantTBlock2.Length + count);
                Console.WriteLine(TBlockString);
                count++;
            }
            count = 0;
            foreach (string TBlockString in _giantSBlock)
            {
                SetTextColor("green");
                Console.SetCursorPosition(Console.WindowWidth - _giantIBlock[0].Length - (_giantTBlock2[0].Length / 3) * 2 - _giantSBlock[0].Length - rightMargin, Console.WindowHeight - _giantTBlock2.Length + count);
                Console.WriteLine(TBlockString);
                count++;
            }
            count = 0;
            foreach (string TBlockString in _giantZBlock)
            {
                SetTextColor("red");
                Console.SetCursorPosition(Console.WindowWidth - _giantIBlock[0].Length - (_giantTBlock2[0].Length / 3) * 2 - _giantSBlock[0].Length / 2 - _giantZBlock[0].Length - rightMargin, Console.WindowHeight - _giantZBlock.Length + count);
                Console.WriteLine(TBlockString);
                count++;
            }

            SetTextColor("white");

        }

        /// <summary>
        /// Set the text color to the wanted color
        /// </summary>
        /// <param name="color">wanted text color</param>
        static public void SetTextColor(string color)
        {
            switch (color.ToLower())
            {
                case "red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "blue":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "cyan":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case "yellow":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "magenta":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case "white":
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "black":
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case "dkgray":
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case "dkyellow":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case "dkmagenta":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case "dkblue":
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
        /// <summary>
        /// Set the background color to the wanted color
        /// </summary>
        /// <param name="color">wanted background color</param>
        static public void SetBackgroundColor(string color)
        {
            switch (color.ToLower())
            {
                case "red":
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case "green":
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;
                case "blue":
                    Console.BackgroundColor = ConsoleColor.Blue;
                    break;
                case "cyan":
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    break;
                case "yellow":
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    break;
                case "magenta":
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    break;
                case "gray":
                    Console.BackgroundColor = ConsoleColor.Gray;
                    break;
                case "white":
                    Console.BackgroundColor = ConsoleColor.White;
                    break;
                case "black":
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case "dkgray":
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    break;
                case "dkyellow":
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
                case "dkmagenta":
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    break;
                case "dkblue":
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;
                case "dkred":
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                default:
                    Console.BackgroundColor = ConsoleColor.White;
                    break;
            }
        }
    }
}

