using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //make the cursor invisible
            //Console.CursorVisible = false;

            //name of the window
            Console.Title = "CodeTris";

            //Make sure the log file exist, otherwise create one
            ExternalManager.LogFile();                       

            //Define the console size to the maximum possible size
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            //remove the ability to resize the console and to maximaxied it
            ConsoleUtility.DeleteResizeMenu();
            //Move to window to the top left corner
            WindowUtility.MoveWindowToTopLeftCorner();

            //Configure DB infos and open it
            DatabaseManager.ConfigureDBInfos();
            DatabaseManager.OpenDB();

            //load options
            ExternalManager.LoadInformations();

            //Managers
            Console.ForegroundColor = ConsoleColor.White;
            MenuManager menuManager = new MenuManager();
            menuManager.CallMainMenu();
            GameManager gameManager = new GameManager();

            if (ExternalManager.GetSoundStatusAtStart())
            {
                SoundManager.PlayTetrisThemeSong();
            }                       

            //Variables
            bool inMenu = true;
            int menuSelector = 0;
            int lastIndexMenu = menuManager.GetMainMenu().Last().Key;
            int menuTop = menuManager.GetMenuTopStart();
            int cursorLeftPos;

            MenuManagement();
            Console.ReadLine();


            void MenuManagement()
            {
                do
                {                    

                    if (menuSelector == 0)
                    {
                        cursorLeftPos = menuManager.GetMenuOptionPos()[menuSelector];
                        Console.SetCursorPosition(cursorLeftPos, menuTop);
                    }
                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.DownArrow:
                            if (menuSelector < lastIndexMenu)
                            {
                                menuSelector++;
                                cursorLeftPos = menuManager.GetMenuOptionPos()[menuSelector];
                                Console.SetCursorPosition(cursorLeftPos, Console.CursorTop + 1);
                            }
                            else
                            {
                                menuSelector = 0;
                                cursorLeftPos = menuManager.GetMenuOptionPos()[menuSelector];
                                Console.SetCursorPosition(cursorLeftPos, menuTop);
                            }                            
                            Console.Write(">");
                            Console.SetCursorPosition(cursorLeftPos, Console.CursorTop);
                            break;
                        case ConsoleKey.UpArrow:
                            if (menuSelector > 0)
                            {
                                menuSelector--;
                                cursorLeftPos = menuManager.GetMenuOptionPos()[menuSelector];
                                Console.SetCursorPosition(cursorLeftPos, Console.CursorTop - 1);
                            }
                            else
                            {
                                menuSelector = lastIndexMenu;
                                cursorLeftPos = menuManager.GetMenuOptionPos()[menuSelector];
                                Console.SetCursorPosition(cursorLeftPos, menuTop + menuSelector);
                            }
                           
                            Console.Write(">");
                            Console.SetCursorPosition(cursorLeftPos, Console.CursorTop);
                            break;

                        //ENTER
                        case ConsoleKey.Enter:
                            switch (menuManager.GetCurrentMenu())
                            {
                                case "main":
                                    ManageMainMenuEnter();
                                    break;

                                case "option":
                                    ManageOptionMenuEnter();
                                    break;
                                
                                case "highscore":
                                    ManageHighscoreMenuEnter();
                                    break;
                                case "detailHighscore":
                                    ManageDetailsHighscoreMenuEnter();
                                    break;
                                case "howToPlay":
                                    ManageHowToPlayMenuEnter();
                                    break;
                                    /*
                                case "pause":
                                    ManagePauseMenuEnter();
                                    break;*/
                            }
                            break;

                        default:
                            
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            Console.Write(">");
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);

                            break;

                    }

                } while (inMenu);
            }

            void ManageMainMenuEnter()
            {
                switch (menuManager.MainMenuEnter(menuSelector))
                {
                    case 0:
                        inMenu = false;
                        gameManager.NewGame();

                        break;
                    case 1:
                        lastIndexMenu = menuManager.GetOptionMenu().Last().Key;
                        menuTop = menuManager.GetMenuTopStart();
                        menuSelector = 0;
                        break;
                    case 2:
                        lastIndexMenu = menuManager.GetHighscoreMenu().Last().Key;
                        menuTop = menuManager.GetMenuTopStart();
                        menuSelector = 0;
                        break;
                    case 3:                        
                        lastIndexMenu = menuManager.GetHowToPlayMenu().Last().Key;
                        menuTop = menuManager.GetMenuTopStart();
                        menuSelector = 0;
                        break;

                }
            }

            void ManageOptionMenuEnter()
            {
                switch (menuManager.OptionMenuEnter(menuSelector))
                {
                    case 0: 
                        menuSelector = 0;
                        break;    
                    case 1:                        
                        menuSelector = 1;
                        break;
                    case 2:
                        menuSelector = 2;
                        break;
                    case 3:
                        lastIndexMenu = menuManager.GetMainMenu().Last().Key;
                        menuTop = menuManager.GetMenuTopStart();
                        menuSelector = 0;
                        break;

                }
            }
            
            void ManageHighscoreMenuEnter()
            {
                switch (menuManager.HighscoreMenuEnter(menuSelector))
                {                    
                    case 3:
                        lastIndexMenu = menuManager.GetMainMenu().Last().Key;
                        menuTop = menuManager.GetMenuTopStart();
                        menuSelector = 0;
                        break;
                    default:
                        lastIndexMenu = menuManager.GetDetailHighscoreMenu().Last().Key;
                        menuTop = menuManager.GetMenuTopStart();
                        menuSelector = 0;
                        break;
                }
            }

            void ManageDetailsHighscoreMenuEnter()
            {
                switch (menuManager.DetailHighscoreMenuEnter(menuSelector))
                {
                    default:
                        lastIndexMenu = menuManager.GetHighscoreMenu().Last().Key;
                        menuTop = menuManager.GetMenuTopStart();
                        menuSelector = 0;
                        break;
                        
                }
            }

            void ManageHowToPlayMenuEnter()
            {
                switch (menuManager.HowToPlayMenuEnter(menuSelector))
                {
                    default:
                        lastIndexMenu = menuManager.GetMainMenu().Last().Key;
                        menuTop = menuManager.GetMenuTopStart();
                        menuSelector = 0;
                        break;

                }
            }


            /*
            void ManagePauseMenuEnter()
            {
                switch (menuManager.PauseMenuEnter(menuSelector))
                {
                    //Case 0 == return to option menu
                    //Case 1 == change resolution (resolution is managed in the MenuManager.cs file)
                    case 0:
                        //go back to game
                        inMenu = false;
                        int gameResult = gameManager.ResumeGame();
                        ManageGameResult(gameResult);
                        break;
                    case 1:
                        //options menu
                        break;
                    case 2:
                        //validation before quitting (Keep game going or not ?)

                        lastIndexMenu = menuManager.GetMainMenu().Last().Key;
                        menuTop = menuManager.GetMenuTopStart();
                        menuSelector = 0;
                        break;
                    case -1:
                        //validation before quitting
                        break;
                }
            }*/
            /*
            void ManageGameResult(int gameResult)
            {
                switch (gameResult)
                {
                    case 1:
                        //game is finished
                        inMenu = true;
                        menuManager.CallMainMenu();
                        menuTop = menuManager.GetMenuTopStart();
                        lastIndexMenu = menuManager.GetMainMenu().Last().Key;
                        break;
                    case 2:
                        Console.Clear();
                        inMenu = true;
                        menuManager.CallPauseMenu();
                        menuTop = menuManager.GetMenuTopStart();
                        lastIndexMenu = menuManager.GetPauseMenu().Last().Key;
                        break;
                    default:
                        break;
                }
            }*/


        }
    }

}
