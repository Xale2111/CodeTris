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
            Console.CursorVisible = false;

            //name of the window
            Console.Title = "CodeTris";

            //Make sure the log file exist, otherwise create one
            ExternalManager.LogFile();                       

            //Define the console size to the maximum possible size
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            //remove the ability to resize the console and to maximaxied it
            //ConsoleUtility.DeleteResizeMenu();
            
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

            //check if the sound is on or off 
            //if is on, start playing the music
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
            //Should'nt technically do this line in the program 
            Console.ReadLine();

            ///Manage the menus and how the player moves across each menu
            void MenuManagement()
            {
                do
                {                    
                    //set the cursor to the first option of the menu
                    if (menuSelector == 0)
                    {
                        cursorLeftPos = menuManager.GetMenuOptionPos()[menuSelector];
                        Console.SetCursorPosition(cursorLeftPos, menuTop);
                    }
                    //read the given key 
                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    //Depending on which key the user pressed, do something different 
                    switch (keyInfo.Key)
                    {
                        //go to the next option
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
                            //Go to the previous option
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
                        //Depending on the selected option of the current menu, will do something different
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
                            //by default, will place the cursor back to the correct position
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            Console.Write(">");
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);

                            break;

                    }

                } while (inMenu);
            }

            ///Manage the main menu when the ENTER key is pressed
            void ManageMainMenuEnter()
            {
                //depending on the selection, will go something different
                switch (menuManager.MainMenuEnter(menuSelector))
                {                    
                    case 0:
                        inMenu = false;
                        int gameResult = gameManager.NewGame();
                        ManageGameResult(gameResult);
                        break;
                    //Go to the option menu
                    case 1:
                        lastIndexMenu = menuManager.GetOptionMenu().Last().Key;
                        menuTop = menuManager.GetMenuTopStart();
                        menuSelector = 0;
                        break;
                    //Go to the highscore menu
                    case 2:
                        lastIndexMenu = menuManager.GetHighscoreMenu().Last().Key;
                        menuTop = menuManager.GetMenuTopStart();
                        menuSelector = 0;
                        break;
                    //go to the How to play menu
                    case 3:                        
                        lastIndexMenu = menuManager.GetHowToPlayMenu().Last().Key;
                        menuTop = menuManager.GetMenuTopStart();
                        menuSelector = 0;
                        break;
                    //Exit isn't written here because it escapes the program
                }
            }
            ///Manage the game result
            void ManageGameResult(int gameResult)
            {
                switch (gameResult)
                {
                    //Game is over
                    case 0:
                        inMenu = true;
                        menuManager.CallMainMenu();
                        if (MenuManager.GetSoundStatus())
                        {
                            SoundManager.PlayTetrisThemeSong();
                        }
                        lastIndexMenu = menuManager.GetMainMenu().Last().Key;
                        menuTop = menuManager.GetMenuTopStart();
                        menuSelector = 0;
                        break;
                    default:
                        break;
                }
            }

            ///Manage the option menu when the ENTER key is pressed
            void ManageOptionMenuEnter()
            {                
                switch (menuManager.OptionMenuEnter(menuSelector))
                {
                    //Case 0 to 2 is manage via the menu manager
                    case 0: 
                        menuSelector = 0;
                        break;    
                    case 1:                        
                        menuSelector = 1;
                        break;
                    case 2:
                        menuSelector = 2;
                        break;
                    //Go back to the main menu
                    case 3:
                        lastIndexMenu = menuManager.GetMainMenu().Last().Key;
                        menuTop = menuManager.GetMenuTopStart();
                        menuSelector = 0;
                        break;

                }
            }
            
            ///Manage the Highscore menu when the ENTER key is pressed
            void ManageHighscoreMenuEnter()
            {
                switch (menuManager.HighscoreMenuEnter(menuSelector))
                {                   
                    //go back to the main menu
                    case 3:
                        lastIndexMenu = menuManager.GetMainMenu().Last().Key;
                        menuTop = menuManager.GetMenuTopStart();
                        menuSelector = 0;
                        break;
                    //Will show a details highscore menu
                    default:
                        lastIndexMenu = menuManager.GetDetailHighscoreMenu().Last().Key;
                        menuTop = menuManager.GetMenuTopStart();
                        menuSelector = 0;
                        break;
                }
            }

            ///Manage the details highscore menu when the ENTER key is pressed
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

            ///Manage the How to play menu when the ENTER key is pressed
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
