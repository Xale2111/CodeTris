using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
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


        static bool _soundStatus;
        static int _difficultyStatus;
        static int _keysStatus;
        static string _playerName;


        static public bool GetSoundStatusAtStart()
        {
            return _soundStatus;
        }

        static public int GetDifficultyAtStart()
        {
            return _difficultyStatus;
        }

        static public int GetKeysAtStart()
        {
            return _keysStatus;
        }

        static public string GetDBConfiguration()
        {
            string rawFile = String.Empty;            
            if (File.Exists(CONFIG_FILE_PATH))
            {
                rawFile = File.ReadAllText(CONFIG_FILE_PATH);                
            }
            return rawFile;
        }

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
            _soundStatus = sound;
            _difficultyStatus = difficulty;
            _keysStatus = keys;

            StockInformations();

        }

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

        static private string SetDefaultParameters()
        {
            string defaultParams;
            defaultParams = "sound = True;";
            defaultParams += "difficulty = 1;";
            defaultParams += "keys = 0";
            return defaultParams;
        }

        static public void StockOptionsOnChange()
        {
            StockInformations();
        }

        static private string CreatePlayerNickname()
        {
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

        static public string GetPlayerName()
        {
            return _playerName;
        }

        static public void LogError(string error)
        {
            if (File.Exists(LOGS_DIR_PATH+ LOGS_FILE_PATH))
            {
                string message = DateTime.Now.ToString() + "\t\t" + "ERROR" + "\t\t" + error;
                using (StreamWriter w = File.AppendText(LOGS_DIR_PATH + LOGS_FILE_PATH))
                {
                    w.WriteLine(message);
                }
            }
        }

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
