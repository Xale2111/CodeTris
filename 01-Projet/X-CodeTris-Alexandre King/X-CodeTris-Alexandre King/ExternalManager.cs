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
        static string _paramDirPath = "../../params/";
        static string _paramsFilePath = "../../params/params.txt";
        static string _animalsFilePath = "../../params/AdjectivesNom.txt";
        static string _adjectivesFilePath = "../../params/AnimauxNom.txt";
        static string _playerNameFilePath = "../../params/PlayerName.txt";
        
        //logs variables
        static string _logsDirPath = "../../logs/";
        static string _logsFilePath = "/logs.txt";

        //DB configuration variables        
        static string _configFilePath = "../../config.ini";

        static Random _rand = new Random();
        static int _numberOfName = 50;


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
            if (File.Exists(_configFilePath))
            {
                rawFile = File.ReadAllText(_configFilePath);                
            }
            return rawFile;
        }

        static public void LogFile()
        {
            if (!Directory.Exists(_logsDirPath))
            {
                Directory.CreateDirectory(_logsDirPath);
            }
            if (!File.Exists(_logsDirPath + _logsFilePath))
            {
                string header = "DATE \t\t\t\t|LEVEL \t\t|ERROR\n";
                File.WriteAllText(_logsDirPath + _logsFilePath, header);
            }
        }


        static public void LoadInformations()
        {
            string parameters = string.Empty;
            bool sound;
            int difficulty;
            int keys;
            string name = string.Empty;

            if (File.Exists(_paramsFilePath))
            {
                parameters = File.ReadAllText(_paramsFilePath);
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

            if (File.Exists(_playerNameFilePath))
            {
                name = File.ReadAllText(_playerNameFilePath);
            }
            else
            {
                name = CreatePlayerNickname();
                File.WriteAllText(_playerNameFilePath, name);
            }

            if (name.Length <= 5)
            {
                name = CreatePlayerNickname();
                File.WriteAllText(_playerNameFilePath, name);
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
            if (!Directory.Exists(_paramDirPath))
            {
                Directory.CreateDirectory(_paramDirPath);
            }
            if (!File.Exists(_paramsFilePath))
            {
                parameters = SetDefaultParameters();
            }
            else
            {
                parameters = "sound = " + MenuManager.GetSoundStatus() + ";";
                parameters += "difficulty = " + MenuManager.GetDifficultyStatus() + ";";
                parameters += "keys = " + MenuManager.GetPlayingKeys() + ";";

            }
            File.WriteAllText(_paramsFilePath, parameters);
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
                if (File.Exists(_animalsFilePath) && File.Exists(_adjectivesFilePath))
                {
                    string animals = File.ReadAllText(_animalsFilePath);
                    string adjectives = File.ReadAllText(_adjectivesFilePath);

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
            if (File.Exists(_logsDirPath+ _logsFilePath))
            {
                string message = DateTime.Now.ToString() + "\t\t" + "ERROR" + "\t\t" + error;
                using (StreamWriter w = File.AppendText(_logsDirPath + _logsFilePath))
                {
                    w.WriteLine(message);
                }
            }
        }

        static public void LogInfo(string info)
        {
            if (File.Exists(_logsDirPath + _logsFilePath))
            {
                string message = DateTime.Now.ToString() + "\t\t" + "INFO" + "\t\t" + info;
                using (StreamWriter w = File.AppendText(_logsDirPath + _logsFilePath))
                {
                    w.WriteLine(message);
                }
            }
        }
    }
}
