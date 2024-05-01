using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    public class MenuManager
    {
        Dictionary<int, string> mainMenu = new Dictionary<int, string>();
        Dictionary<int, string> optionMenu = new Dictionary<int, string>();
        Dictionary<int, string> highscore = new Dictionary<int, string>();
        Dictionary<int, string> pauseMenu = new Dictionary<int, string>();

        string[] difficultyOptions = new string[3] { "Facile", "Moyen", "Difficile" };
        string[] keysOptions = new string[2] { "WASD", "Flèches" };

        int[] xPosMenuOption;
        static bool soundIsOn = false;
        static int currentDifficulty;
        static int playKeys = 0;

        int menuTopStart;

        string currentMenu;

        public MenuManager()
        {
            DefineMainMenu();
            DefineOptionMenu();
            DefineHighscoreMenu();
            //DefinePauseMenu();
        }

        public void CallMainMenu()
        {
            currentMenu = "main";
            CreateMenu(mainMenu);
        }
        private void CallOptionMenu()
        {
            currentMenu = "option";
            CreateMenu(optionMenu);
        }
        /*
            public void CallPauseMenu()
            {
                currentMenu = "pause";
                CreateMenu(pauseMenu);
            }*/
        private void CallHighscoreMenu()
        {
            currentMenu = "highscore";
            CreateMenu(highscore);
        }

        public string GetCurrentMenu()
        {
            return currentMenu;
        }

        private void DefineMainMenu()
        {
            mainMenu.Add(0, "Jouer");
            mainMenu.Add(1, "Options");
            mainMenu.Add(2, "Score");
            mainMenu.Add(3, "Quitter");
        }

        private void DefineOptionMenu()
        {
            optionMenu.Add(0, "Musique");
            optionMenu.Add(1, "Touches");
            optionMenu.Add(2, "Difficulté");
            optionMenu.Add(3, "Retour <=");
        }

        private void DefineHighscoreMenu()
        {
            highscore.Add(0, "Facile");
            highscore.Add(1, "Moyen");
            highscore.Add(2, "Difficile");
            highscore.Add(3, "Retour <=");
        }
        /*
            private void DefinePauseMenu()
            {
                pauseMenu.Add(0, "Return");
                pauseMenu.Add(1, "Options");
                pauseMenu.Add(2, "Go Back To Menu <=");
                pauseMenu.Add(3, "Exit");
            }*/

        private void CreateMenu(Dictionary<int, string> menu)
        {
            char movementArrow = '>';
            if (currentMenu == "main")
            {
                Console.SetCursorPosition(0, Console.WindowHeight - 1);
                if (DatabaseManager.GetDBState())
                {
                Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.Write("DB");
                Console.ForegroundColor = ConsoleColor.White;
            }


            xPosMenuOption = new int[menu.Count];
            menuTopStart = Console.WindowHeight / 2 - menu.Count;

            if (currentMenu == "highscore")
            {/*
                    List<Tuple<string, int, string>> highscores = DatabaseManager.GetHighScores();
                    int count = 0;

                    foreach (Tuple<string, int, string> highscore in highscores)
                    {
                        switch (count)
                        {
                            case 0:
                                WorldGenerator.TextColor("DKyellow");
                                break;
                            case 1:
                                WorldGenerator.TextColor("DKgray");
                                break;
                            case 2:
                                WorldGenerator.TextColor("DKmagenta");
                                break;
                            default:
                                WorldGenerator.TextColor("white");
                                break;
                        }
                        string score = (count + 1) + ". " + highscore.Item1 + " : " + highscore.Item2 + " pts";
                        Console.SetCursorPosition(Console.WindowWidth / 2 - score.Length / 2, menuTopStart - 5 + count);
                        Console.WriteLine(score);
                        count++;
                        WorldGenerator.TextColor("white");

                    }
                    menuTopStart += 10;*/
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(Console.WindowWidth / 2, menuTopStart);
            /*if (currentMenu == "highscore")
            {
                Console.SetCursorPosition(Console.WindowWidth / 2, menuTopStart);

            }*/
            foreach (var menuOption in menu)
            {
                xPosMenuOption[menuOption.Key] = Console.WindowWidth / 2 - 4;

                if (menuOption.Value == "Musique")
                {
                    if (soundIsOn)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                }
                Console.SetCursorPosition(Console.WindowWidth / 2 - 2, Console.CursorTop);
                if (menuOption.Value == "Difficulté")
                {
                    Console.Write(menuOption.Value + " : ");
                    switch (currentDifficulty)
                    {
                        case 0:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case 1:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                    }
                    Console.WriteLine(difficultyOptions[currentDifficulty]);
                }
                else if (menuOption.Value == "Touches")
                {
                    Console.WriteLine(menuOption.Value + " : " + keysOptions[playKeys]);
                }
                else
                {
                    Console.WriteLine(menuOption.Value);
                }


                Console.ForegroundColor = ConsoleColor.White;
            }
            if (currentMenu == "main")
            {
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.White;
                //Console.Write("Welcome " + ExternalManager.GetPlayerName());
            }

            Console.SetCursorPosition(xPosMenuOption[0], menuTopStart);
            Console.Write(movementArrow);

        }

        public Dictionary<int, string> GetMainMenu()
        {
            return mainMenu;
        }

        public Dictionary<int, string> GetOptionMenu()
        {
            return optionMenu;
        }
        /*
            public Dictionary<int, string> GetPauseMenu()
            {
                return pauseMenu;
            }*/
        public Dictionary<int, string> GetHighscoreMenu()
        {
            return highscore;
        }

        public int GetMenuTopStart()
        {
            return menuTopStart;
        }

        public int[] GetMenuOptionPos()
        {
            return xPosMenuOption;
        }

        private void ChangeSoundState(int positionValue)
        {
            soundIsOn = !soundIsOn;
            Console.SetCursorPosition(xPosMenuOption[positionValue] + 2, GetMenuTopStart() + positionValue);
            if (soundIsOn)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine(optionMenu[positionValue]);
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(xPosMenuOption[positionValue], GetMenuTopStart() + positionValue);

        }
        public int GetDifficulty()
        {
            return currentDifficulty;
        }

        private void ChangeDifficulty(int positionValue)
        {
            int cursorPosition = xPosMenuOption[positionValue] + optionMenu[positionValue].Length + 5;
            Console.SetCursorPosition(cursorPosition, GetMenuTopStart() + positionValue);
            if (currentDifficulty < difficultyOptions.Length - 1)
            {
                currentDifficulty++;
            }
            else
            {
                currentDifficulty = 0;
            }
            switch (currentDifficulty)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
            Console.WriteLine(difficultyOptions[currentDifficulty] + new string(' ', 10));
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(xPosMenuOption[positionValue], GetMenuTopStart() + positionValue);
        }

        private void ChangePlayKeys(int positionValue)
        {
            int cursorPosition = xPosMenuOption[positionValue] + optionMenu[positionValue].Length + 5;
            Console.SetCursorPosition(cursorPosition, GetMenuTopStart() + positionValue);            

            if (playKeys < keysOptions.Length - 1)
            {
                playKeys++;
            }
            else
            {
                playKeys = 0;
            }
            //Rajoute du blanc après le nom de l'option dans le cas où une option est plus grande qu'une autre (efface le surplus)
            Console.WriteLine(keysOptions[playKeys] + new string(' ', 10));
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(xPosMenuOption[positionValue], GetMenuTopStart() + positionValue);
        }
        /*
            private void SetScreenResolution(int width, int height)
            {
                int newWidth = CalculateConsoleWidth(width);
                int newHeight = CalculateConsoleHeight(height);
                Console.SetWindowSize(newWidth, newHeight);
            }

            private int CalculateConsoleWidth(double newWidth)
            {
                double size;
                double screenWidth = WindowUtility.GetScreenWidth();
                double maxConsoleWidth = Console.LargestWindowWidth;
                size = (newWidth / screenWidth) * maxConsoleWidth;
                int finalSize = Convert.ToInt32(Math.Floor(size));

                return finalSize;
            }
            private int CalculateConsoleHeight(double newHeight)
            {
                double size;
                double screenHeight = WindowUtility.GetScreenHeight();
                double maxConsoleWidth = Console.LargestWindowHeight;
                size = (newHeight / screenHeight) * maxConsoleWidth;
                int finalSize = Convert.ToInt32(Math.Floor(size));
                return finalSize;
            }
        */
        static public bool GetSoundStatus()
        {
            return soundIsOn;
        }

        static public int GetDifficultyStatus()
        {
            return currentDifficulty;
        }

        static public int GetPlayingKeys()
        {
            return playKeys;
        }


        /*
          MENU ENTER 
         */

        /// <summary>
        /// Management of the main menu when the key "enter" is pressed
        /// </summary>
        /// <param name="menuOptionValue">Which option of the menu was called</param>
        /// <returns>Result of the action so the progam knows what to do next, return -1 if nothing needs to be done</returns>
        public int MainMenuEnter(int menuOptionValue)
        {
            int lastIndex = GetMainMenu().Last().Key;
            if (menuOptionValue == lastIndex)
            {
                // ExternalManager.StockOptionsOnChange();
                Environment.Exit(0);
            }
            else
            {

                switch (menuOptionValue)
                {
                    case 0:
                        Console.Clear();
                        Console.WriteLine("Play game");
                        return 0;
                    case 1:
                        Console.Clear();
                        CallOptionMenu();
                        return 1;
                    case 2:
                        Console.Clear();
                        CallHighscoreMenu();
                        return 2;
                }
            }
            return -1;
        }

        /// <summary>
        /// Management of the option menu when the key "enter" is pressed
        /// </summary>
        /// <param name="menuOptionValue">Which option of the menu was called</param>
        /// <returns>Result of the action so the progam knows what to do next, return -1 if nothing needs to be done</returns>
        public int OptionMenuEnter(int menuOptionValue)
        {
            int lastIndex = GetOptionMenu().Last().Key;
            if (menuOptionValue == lastIndex)
            {
                Console.Clear();
                CallMainMenu();
                return 3;
            }
            else
            {
                switch (menuOptionValue)
                {
                    case 0:
                        ChangeSoundState(menuOptionValue);
                        //ExternalManager.StockOptionsOnChange();
                        return 0;
                    case 1:                        
                        ChangePlayKeys(menuOptionValue);
                        return 1;
                    case 2:
                        ChangeDifficulty(menuOptionValue);
                        //ExternalManager.StockOptionsOnChange();
                        return 2;
                }
            }

            return -1;
        }

        /// <summary>
        /// Management of the pause menu when the key "enter" is pressed
        /// </summary>
        /// <param name="menuOptionValue"></param>
        /// <returns></returns>
        /// 
        /*
        public int PauseMenuEnter(int menuOptionValue)
            {
                int lastIndex = GetPauseMenu().Last().Key;
                if (menuOptionValue == lastIndex)
                {
                    //Confirmation Exit
                    return -1;
                }
                else
                {
                    switch (menuOptionValue)
                    {
                        case 0:
                            //Go back to game
                            Console.Clear();
                            return 0;
                        case 1:
                            //open option menu
                            return 1;
                        case 2:
                            Console.Clear();
                            CallMainMenu();
                            //go back to main menu
                            return 2;
                    }
                }

                return -1;
            }*/

        public int HighscoreMenuEnter(int menuOptionValue)
        {
            int lastIndex = GetHighscoreMenu().Last().Key;
            if (menuOptionValue == lastIndex)
            {
                Console.Clear();
                CallMainMenu();
                return 0;
            }
            switch (menuOptionValue)
            {
                case 0:
                    //show easy highscore
                    return 0;
                case 1:
                    //show medium highscore
                    return 1;
                case 2:
                    //show hard highscore
                    return 2;
            }
            return -1;
        }
    }
    }
