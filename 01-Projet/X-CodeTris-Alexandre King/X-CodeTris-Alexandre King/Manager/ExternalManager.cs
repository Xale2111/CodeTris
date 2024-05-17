using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    /// <summary>
    /// Manager of external elements of the project such as txt file 
    /// </summary>
    static public class ExternalManager
    {
        //parameters variables
        const string PARAMS_DIR_PATH = "../../params/";
        const string PARAMS_FILE_PATH = "../../params/params.txt";
        const string ANIMALS_FILE_PATH = "../../params/AdjectivesNom.txt";
        const string ADJECTIVES_FILE_PATH = "../../params/AnimauxNom.txt";
        const string PLAYER_NAME_FILE_PATH = "../../params/PlayerName.txt";

        //logs variables
        const string LOGS_DIR_PATH = "../../logs/";
        const string LOGS_FILE_PATH = "/logs.txt";

        //DB configuration variables        
        const string CONFIG_FILE_PATH = "../../config.ini";

        static Random _rand = new Random();
        const int _numberOfName = 50;

        //Variables of the status of the parameters and the player's name
        static bool _soundState;
        static int _difficultyState;
        static int _keysState;
        static string _playerName;

        /// <summary>
        /// Get the sound state when starting the game
        /// </summary>
        /// <returns>True = sound is on, False = sound is off</returns>
        static public bool GetSoundStatusAtStart()
        {
            return _soundState;
        }

        /// <summary>
        /// Get the Difficulty level when lauching the game
        /// </summary>
        /// <returns>level of difficulty</returns>
        static public int GetDifficultyAtStart()
        {
            return _difficultyState;
        }

        /// <summary>
        /// Get the playing keys when lauching the game
        /// </summary>
        /// <returns>Id of the keys (0 = WASD, 1=Arrows, in case more are added, it's an integrer)</returns>
        static public int GetKeysAtStart()
        {
            return _keysState;
        }
        /// <summary>
        /// Get the raw content of the config.ini file for the DB
        /// </summary>
        /// <returns>raw content of the config.ini file</returns>
        static public string GetDBConfiguration()
        {
            string rawFile = String.Empty;
            if (File.Exists(CONFIG_FILE_PATH))
            {
                rawFile = File.ReadAllText(CONFIG_FILE_PATH);
            }
            return rawFile;
        }
        /// <summary>
        /// Makes sure the logs file exists
        /// </summary>
        static public void LogFile()
        {
            if (!Directory.Exists(LOGS_DIR_PATH))
            {
                Directory.CreateDirectory(LOGS_DIR_PATH);
            }
            if (!File.Exists(LOGS_DIR_PATH + LOGS_FILE_PATH))
            {
                string header = "DATE \t\t\t\t|LEVEL \t\t|ERROR\n";
                File.WriteAllText(LOGS_DIR_PATH + LOGS_FILE_PATH, header);
            }
        }

        /// <summary>
        /// Loads the informations (parameters, playername, etc...). If the file doesn't exist will set parameters to Default (refer to SetDefaultParameters function)
        /// </summary>
        static public void LoadInformations()
        {
            string parameters = string.Empty;
            bool sound;
            int difficulty;
            int keys;
            string name = string.Empty;

            if (File.Exists(PARAMS_FILE_PATH))
            {
                parameters = File.ReadAllText(PARAMS_FILE_PATH);
            }
            else
            {
                parameters = SetDefaultParameters();
            }

            try
            {
                sound = Convert.ToBoolean(parameters.Split(';')[0].Split('=')[1].Trim());
                difficulty = Convert.ToInt32(parameters.Split(';')[1].Split('=')[1].Trim());
                keys = Convert.ToInt32(parameters.Split(';')[2].Split('=')[1].Trim());
            }
            catch (Exception e)
            {
                parameters = SetDefaultParameters();

                sound = Convert.ToBoolean(parameters.Split(';')[0].Split('=')[1].Trim());
                difficulty = Convert.ToInt32(parameters.Split(';')[1].Split('=')[1].Trim());
                keys = Convert.ToInt32(parameters.Split(';')[2].Split('=')[1].Trim());
            }

            if (File.Exists(PLAYER_NAME_FILE_PATH))
            {
                name = File.ReadAllText(PLAYER_NAME_FILE_PATH);
            }
            else
            {
                name = CreatePlayerNickname();
                File.WriteAllText(PLAYER_NAME_FILE_PATH, name);
            }

            if (name.Length <= 5)
            {
                name = CreatePlayerNickname();
                File.WriteAllText(PLAYER_NAME_FILE_PATH, name);
            }

            _playerName = name;
            _soundState = sound;
            _difficultyState = difficulty;
            _keysState = keys;

        }
        /// <summary>
        /// Stock some informations to a file. 
        /// </summary>
        /// <param name="informationsToStock">Informations to stock</param>
        /// <param name="pathToStock">path of the file. Where the informations will be stocked</param>
        static private void StockInformations(string informationsToStock = "", string pathToStock = "")
        {
            string parameters = string.Empty;
            if (!Directory.Exists(PARAMS_DIR_PATH))
            {
                Directory.CreateDirectory(PARAMS_DIR_PATH);
            }
            if (!File.Exists(PARAMS_FILE_PATH))
            {
                parameters = SetDefaultParameters();
            }
            else
            {
                parameters = "sound = " + MenuManager.GetSoundStatus() + ";";
                parameters += "difficulty = " + MenuManager.GetDifficultyStatus() + ";";
                parameters += "keys = " + MenuManager.GetPlayingKeys() + ";";

            }
            File.WriteAllText(PARAMS_FILE_PATH, parameters);
        }
        /// <summary>
        /// Set the parameters to default (those are set by hand in case there's a problem with a file)
        /// </summary>
        /// <returns></returns>
        static private string SetDefaultParameters()
        {
            string defaultParams;
            defaultParams = "sound = True;";
            defaultParams += "difficulty = 1;";
            defaultParams += "keys = 0";
            return defaultParams;
        }
        /// <summary>
        /// Stock all parameters when one is change or when the games exit (ensure they will be the same next time the user use the program)
        /// </summary>
        static public void StockOptionsOnChange()
        {
            StockInformations();
        }

        /// <summary>
        /// Create a nickname for the player if he doesn't have one
        /// the nickname is a combination of an adjectiv, a animal and a random number from 0 to 99
        /// </summary>
        /// <returns>The player's name</returns>
        static private string CreatePlayerNickname()
        {
            //first check if the name exist in DB or not (this does limits the amount of possible name, if more names needs to be added, add adjectiv and animal names) 
            string nickname = string.Empty;
            do
            {
                if (File.Exists(ANIMALS_FILE_PATH) && File.Exists(ADJECTIVES_FILE_PATH))
                {
                    string animals = File.ReadAllText(ANIMALS_FILE_PATH);
                    string adjectives = File.ReadAllText(ADJECTIVES_FILE_PATH);

                    string animal = animals.Split(';')[_rand.Next(0, _numberOfName)];
                    string adjective = adjectives.Split(';')[_rand.Next(0, _numberOfName)];

                    nickname = animal + adjective + _rand.Next(0, 100);
                }
            } while (DatabaseManager.DoesPlayerNameExist(nickname));
            DatabaseManager.StockPlayer(nickname);
            return nickname;
        }

        /// <summary>
        /// Get the player name
        /// </summary>
        /// <returns>Player name</returns>
        static public string GetPlayerName()
        {
            return _playerName;
        }

        /// <summary>
        /// Log an error
        /// </summary>
        /// <param name="error">Error to log</param>
        static public void LogError(string error)
        {
            if (File.Exists(LOGS_DIR_PATH + LOGS_FILE_PATH))
            {
                string message = DateTime.Now.ToString() + "\t\t" + "ERROR" + "\t\t" + error;
                using (StreamWriter w = File.AppendText(LOGS_DIR_PATH + LOGS_FILE_PATH))
                {
                    w.WriteLine(message);
                }
            }
        }
        /// <summary>
        /// Log an info
        /// An info can be something like good insertion in DB or connection to DB successfull
        /// </summary>
        /// <param name="info">info to log</param>
        static public void LogInfo(string info)
        {
            if (File.Exists(LOGS_DIR_PATH + LOGS_FILE_PATH))
            {
                string message = DateTime.Now.ToString() + "\t\t" + "INFO" + "\t\t" + info;
                using (StreamWriter w = File.AppendText(LOGS_DIR_PATH + LOGS_FILE_PATH))
                {
                    w.WriteLine(message);
                }
            }
        }
    }
}
