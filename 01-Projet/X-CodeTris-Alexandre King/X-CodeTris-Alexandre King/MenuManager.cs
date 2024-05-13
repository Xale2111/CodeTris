using System;
using System.Collections.Generic;
using System.Linq;

namespace X_CodeTris_Alexandre_King
{
    public class MenuManager
    {
        //Each menu, Dictionary to know the position in the options and to have the name of the option
        Dictionary<int, string> _mainMenu = new Dictionary<int, string>();
        Dictionary<int, string> _optionMenu = new Dictionary<int, string>();
        Dictionary<int, string> _highscoreMenu = new Dictionary<int, string>();
        Dictionary<int, string> _detailHighscore = new Dictionary<int, string>();
        Dictionary<int, string> _howToPlay = new Dictionary<int, string>();
        Dictionary<int, string> _pauseMenu = new Dictionary<int, string>();

        //Options possibilities 
        string[] _difficultyOptions = new string[3] { "Facile", "Moyen", "Difficile" };
        string[] _keysOptions = new string[2] { "WASD", "Flèches" };

        //Text in the How To Play menu
        string[] _howToPlayMessage = new string[13]
        {
        "Comment Jouer ?",
        "",
        "Vous pouvez lancer le jeu via le menu principal>Jouer.",
        "Ensuite, vous devrez aligner horizontalement les pièces dans la zone dédiée. Les pièces tombent toutes seules.",
        "Cependant, vous pouvez les faires descendre plus vite avec la touche 'S' ou la flèche du bas selon votre séléction dans le menu 'options'.",
        "Pour déplacer les pièces, utilisez les touches 'A' et 'D' ou les flèches gauche et droite.",
        "Avec la touche 'W' ou la flèche du haut vous pouvez faire pivoter les pièces.",
        "Pour chaque ligne complétée, une question sur le C# vous sera posée. La difficulté varie selon votre séléction dans le menu 'options'.",
        "Plus la difficulté est haute, plus vous gagnerez de points en répondant correctement à une question.",
        "Par contre, si vous répondez faux, une ligne de la zone se bloquera, réduisant ainsi l'espace de jeu.",
        "Si les pièces atteignent le haut de la zone, la partie s'arrête.",
        "",
        "Bonne Chance :) !"
        };

               
        
        int[] _xPosMenuOption;          //position x of each option 
        static bool _soundIsOn = false;
        static int _currentDifficulty;
        static int _playKeys = 0;

        //Start position of the menu
        int _menuTopStart;

        string _currentMenu;

        /// <summary>
        /// Constructor of the menu manager
        /// </summary>
        public MenuManager()
        {
            _soundIsOn = ExternalManager.GetSoundStatusAtStart();
            _currentDifficulty = ExternalManager.GetDifficultyAtStart();
            _playKeys = ExternalManager.GetKeysAtStart();
            if (_playKeys>_keysOptions.Length-1)
            {
                _playKeys = 0;
            }
            if (_currentDifficulty > _difficultyOptions.Length-1)
            {
                _currentDifficulty = 0;
            }

            DefineMainMenu();
            DefineOptionMenu();
            DefineHighscoreMenu();
            DefineDetailHighscoreMenu();
            DefineHowToPlayMenu();
            //DefinePauseMenu();
        }

        public void CallMainMenu()
        {
            _currentMenu = "main";
            CreateMenu(_mainMenu);
        }
        private void CallOptionMenu()
        {
            _currentMenu = "option";
            CreateMenu(_optionMenu);
        }
        /*
            public void CallPauseMenu()
            {
                currentMenu = "pause";
                CreateMenu(pauseMenu);
            }*/
        private void CallHighscoreMenu()
        {
            _currentMenu = "highscore";
            CreateMenu(_highscoreMenu);
        }  
        
        private void CallHowToPlayMenu()
        {
            _currentMenu = "howToPlay";
            CreateMenu(_howToPlay);
        }        

        public string GetCurrentMenu()
        {
            return _currentMenu;
        }

        private void DefineMainMenu()
        {
            _mainMenu.Add(0, "Jouer");
            _mainMenu.Add(1, "Options");
            _mainMenu.Add(2, "Score");
            _mainMenu.Add(3, "Comment jouer ?");
            _mainMenu.Add(4, "Quitter");
        }

        private void DefineOptionMenu()
        {
            _optionMenu.Add(0, "Musique");
            _optionMenu.Add(1, "Touches");
            _optionMenu.Add(2, "Difficulté");
            _optionMenu.Add(3, "Retour <=");
        }

        private void DefineHighscoreMenu()
        {
            _highscoreMenu.Add(0, "Facile");
            _highscoreMenu.Add(1, "Moyen");
            _highscoreMenu.Add(2, "Difficile");
            _highscoreMenu.Add(3, "Retour <=");
        }

        private void DefineDetailHighscoreMenu()
        {
            _detailHighscore.Add(0, "Retour <=");
        }
        private void DefineHowToPlayMenu()
        {
            _howToPlay.Add(0, "Retour <=");
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
            if (_currentMenu == "main")
            {
                VisualManager.AddVisualToMainMenu();
                Console.SetCursorPosition(Console.WindowWidth-5,0);
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
            if (_currentMenu == "howToPlay")
            {
                ShowHowToPlayMenu();
            }

            _xPosMenuOption = new int[menu.Count];
            _menuTopStart = Console.WindowHeight / 2 - menu.Count;
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(Console.WindowWidth / 2, _menuTopStart);            
            foreach (var menuOption in menu)
            {
                _xPosMenuOption[menuOption.Key] = Console.WindowWidth / 2 - 4;

                if (menuOption.Value == "Musique")
                {
                    if (_soundIsOn)
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
                    switch (_currentDifficulty)
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
                    Console.WriteLine(_difficultyOptions[_currentDifficulty]);
                }
                else if (menuOption.Value == "Touches")
                {
                    Console.WriteLine(menuOption.Value + " : " + _keysOptions[_playKeys]);
                }
                else
                {
                    Console.WriteLine(menuOption.Value);
                }


                Console.ForegroundColor = ConsoleColor.White;
            }
            if (_currentMenu == "main")
            {
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Welcome " + ExternalManager.GetPlayerName());
            }

            Console.SetCursorPosition(_xPosMenuOption[0], _menuTopStart);
            Console.Write(movementArrow);

        }

        private void ShowDetailHighScoreMenu(int difficulty)
        {
            Console.Clear();            
            char movementArrow = '>';
            List<Tuple<string, int, string>> highscores = new List<Tuple<string, int, string>>();
            highscores = DatabaseManager.GetHighScores(difficulty);
            int count = 0;
            

            foreach (Tuple<string, int, string> highscore in highscores)
            {
                string score = (count + 1) + ". " + highscore.Item1 + " : " + highscore.Item2 + " pts"+". Record date : "+ highscore.Item3;

                Console.SetCursorPosition(Console.WindowWidth/2 - score.Length / 2, Console.WindowHeight/2-highscores.Count()/2+count);
                Console.WriteLine(score);
                count++;
            }

            if (highscores.Count == 0)
            {
                string noRecords = "Actuellement aucun record. Saisissez votre chance !";
                Console.SetCursorPosition(Console.WindowWidth / 2 - noRecords.Length / 2, Console.WindowHeight / 2);
                Console.WriteLine(noRecords);
            }

            _xPosMenuOption = new int[1];
            _xPosMenuOption[0] = Console.WindowWidth / 2 - _detailHighscore[0].Length / 2;
            _menuTopStart = Console.WindowHeight / 2 - highscores.Count() + 3 + count;

            Console.SetCursorPosition(_xPosMenuOption[0], _menuTopStart );
            Console.WriteLine(_detailHighscore[0]);

            Console.SetCursorPosition(Console.WindowWidth / 2 - _detailHighscore[0].Length/2-2, Console.WindowHeight / 2 - highscores.Count() + 3 + count);
            Console.Write(movementArrow);            
        }

        private void ShowHowToPlayMenu()
        {
            int count = 0;
            foreach (string howToPlayInstruction in _howToPlayMessage)
            {
                Console.SetCursorPosition(Console.WindowWidth/2-howToPlayInstruction.Length/2,Console.WindowHeight/4+count);
                Console.Write(howToPlayInstruction);
                count++;
            }
        }
        

        public Dictionary<int, string> GetMainMenu()
        {
            return _mainMenu;
        }

        public Dictionary<int, string> GetOptionMenu()
        {
            return _optionMenu;
        }
        /*
            public Dictionary<int, string> GetPauseMenu()
            {
                return pauseMenu;
            }*/
        public Dictionary<int, string> GetHighscoreMenu()
        {
            return _highscoreMenu;
        }
        public Dictionary<int, string> GetDetailHighscoreMenu()
        {
            return _detailHighscore;
        }
        public Dictionary<int, string> GetHowToPlayMenu()
        {
            return _howToPlay;
        }

        public int GetMenuTopStart()
        {
            return _menuTopStart;
        }

        public int[] GetMenuOptionPos()
        {
            return _xPosMenuOption;
        }

        private void ChangeSoundState(int positionValue)
        {
            _soundIsOn = !_soundIsOn;
            Console.SetCursorPosition(_xPosMenuOption[positionValue] + 2, GetMenuTopStart() + positionValue);
            if (_soundIsOn)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                SoundManager.PlayTetrisThemeSong();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                SoundManager.StopMusic();
            }
            Console.WriteLine(_optionMenu[positionValue]);
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(_xPosMenuOption[positionValue], GetMenuTopStart() + positionValue);
            ExternalManager.StockOptionsOnChange();

        }
        public int GetDifficulty()
        {
            return _currentDifficulty;
        }

        private void ChangeDifficulty(int positionValue)
        {
            int cursorPosition = _xPosMenuOption[positionValue] + _optionMenu[positionValue].Length + 5;
            Console.SetCursorPosition(cursorPosition, GetMenuTopStart() + positionValue);
            if (_currentDifficulty < _difficultyOptions.Length - 1)
            {
                _currentDifficulty++;
            }
            else
            {
                _currentDifficulty = 0;
            }
            switch (_currentDifficulty)
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
            Console.WriteLine(_difficultyOptions[_currentDifficulty] + new string(' ', 10));
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(_xPosMenuOption[positionValue], GetMenuTopStart() + positionValue);
            ExternalManager.StockOptionsOnChange();

        }

        private void ChangePlayKeys(int positionValue)
        {
            int cursorPosition = _xPosMenuOption[positionValue] + _optionMenu[positionValue].Length + 5;
            Console.SetCursorPosition(cursorPosition, GetMenuTopStart() + positionValue);            

            if (_playKeys < _keysOptions.Length - 1)
            {
                _playKeys++;
            }
            else
            {
                _playKeys = 0;
            }
            //Rajoute du blanc après le nom de l'option dans le cas où une option est plus grande qu'une autre (efface le surplus)
            Console.WriteLine(_keysOptions[_playKeys] + new string(' ', 10));
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(_xPosMenuOption[positionValue], GetMenuTopStart() + positionValue);
            ExternalManager.StockOptionsOnChange();

        }

        static public bool GetSoundStatus()
        {
            return _soundIsOn;
        }

        static public int GetDifficultyStatus()
        {
            return _currentDifficulty;
        }

        static public int GetPlayingKeys()
        {
            return _playKeys;
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
                ExternalManager.StockOptionsOnChange();
                Environment.Exit(0);
            }
            else
            {

                switch (menuOptionValue)
                {
                    case 0:
                        Console.Clear();                        
                        return 0;
                    case 1:
                        Console.Clear();
                        CallOptionMenu();
                        return 1;
                    case 2:
                        Console.Clear();
                        CallHighscoreMenu();
                        return 2;
                    case 3:
                        Console.Clear();
                        CallHowToPlayMenu();
                        return 3;
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
                        ExternalManager.StockOptionsOnChange();
                        return 0;
                    case 1:                        
                        ChangePlayKeys(menuOptionValue);
                        ExternalManager.StockOptionsOnChange();

                        return 1;
                    case 2:
                        ChangeDifficulty(menuOptionValue);
                        ExternalManager.StockOptionsOnChange();
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
                return 3;
            }
            else
            {
                _currentMenu = "detailHighscore";
                ShowDetailHighScoreMenu(menuOptionValue);
            }
            return -1;
        }

        public int DetailHighscoreMenuEnter(int menuOptionValue)
        {
            int lastIndex = GetDetailHighscoreMenu().Last().Key;
            if (menuOptionValue == lastIndex)
            {
                Console.Clear();
                CallHighscoreMenu();
                return 0;
            }            
            return -1;
        }

        public int HowToPlayMenuEnter(int menuOptionValue)
        {
            int lastIndex = GetHowToPlayMenu().Last().Key;
            if (menuOptionValue == lastIndex)
            {
                Console.Clear();
                CallMainMenu();
                return 0;
            }
            return -1;
        }
    }
    }
