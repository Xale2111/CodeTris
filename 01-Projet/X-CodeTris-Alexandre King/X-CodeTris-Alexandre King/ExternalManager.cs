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
        static string paramDirPath = "../../params/";
        static string paramsFilePath = "../../params/params.txt";
        static string animalsFilePath = "../../params/AdjectivesNom.txt";
        static string adjectivesFilePath = "../../params/AnimauxNom.txt";
        static string playerNameFilePath = "../../params/PlayerName.txt";
        
        //logs variables
        static string logsDirPath = "../../logs/";
        static string logsFilePath = "/logs.txt";

        //DB configuration variables        
        static string configFilePath = "../../config.ini";

        static Random rand = new Random();
        static int numberOfName = 50;


        static bool soundStatus;
        static int difficultyStatus;
        static string resolutionStatus;
        static string playerName;


        static public bool GetSoundStatusAtStart()
        {
            return soundStatus;
        }

        static public int GetDifficultyAtStart()
        {
            return difficultyStatus;
        }

        static public string GetResolutionAtStart()
        {
            return resolutionStatus;
        }

        static public string GetDBConfiguration()
        {
            string rawFile = String.Empty;            
            if (File.Exists(configFilePath))
            {
                rawFile = File.ReadAllText(configFilePath);                
            }
            return rawFile;
        }

        static public void LogFile()
        {
            if (!Directory.Exists(logsDirPath))
            {
                Directory.CreateDirectory(logsDirPath);
            }
            if (!File.Exists(logsDirPath + logsFilePath))
            {
                string header = "DATE \t\t\t\t|LEVEL \t\t|ERROR\n";
                File.WriteAllText(logsDirPath + logsFilePath, header);
            }
        }


        static public void LoadInformations()
        {
            string parameters = string.Empty;
            bool sound;
            int difficulty;
            string resolution;
            string name = string.Empty;

            if (File.Exists(paramsFilePath))
            {
                parameters = File.ReadAllText(paramsFilePath);
            }
            else
            {
                parameters = SetDefaultParameters();
            }

            try
            {
                sound = Convert.ToBoolean(parameters.Split(';')[0].Split('=')[1].Trim());
                difficulty = Convert.ToInt32(parameters.Split(';')[1].Split('=')[1].Trim());
                resolution = parameters.Split(';')[2].Split('=')[1].Trim();
            }
            catch (Exception e)
            {
                parameters = SetDefaultParameters();

                sound = Convert.ToBoolean(parameters.Split(';')[0].Split('=')[1].Trim());
                difficulty = Convert.ToInt32(parameters.Split(';')[1].Split('=')[1].Trim());
                resolution = parameters.Split(';')[2].Split('=')[1].Trim();
            }

            if (File.Exists(playerNameFilePath))
            {
                name = File.ReadAllText(playerNameFilePath);
            }
            else
            {
                name = CreatePlayerNickname();
                File.WriteAllText(playerNameFilePath, name);
            }

            if (name.Length <= 5)
            {
                name = CreatePlayerNickname();
                File.WriteAllText(playerNameFilePath, name);
            }

            playerName = name;
            soundStatus = sound;
            difficultyStatus = difficulty;
            resolutionStatus = resolution;


        }

        static private void StockInformations(string informationsToStock = "", string pathToStock = "")
        {
            string parameters = string.Empty;
            if (!Directory.Exists(paramDirPath))
            {
                Directory.CreateDirectory(paramDirPath);
            }
            if (!File.Exists(paramsFilePath))
            {
                parameters = SetDefaultParameters();
            }
            else
            {
                parameters = "sound = " + MenuManager.GetSoundStatus() + ";";
                parameters += "difficulty = " + MenuManager.GetDifficultyStatus() + ";";
                parameters += "keys = " + MenuManager.GetPlayingKeys() + ";";

            }
            File.WriteAllText(paramsFilePath, parameters);
        }

        static private string SetDefaultParameters()
        {
            string defaultParams;
            defaultParams = "sound = True;";
            defaultParams += "difficulty = 1;";
            defaultParams += "resolution = 1440x900";
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
                if (File.Exists(animalsFilePath) && File.Exists(adjectivesFilePath))
                {
                    string animals = File.ReadAllText(animalsFilePath);
                    string adjectives = File.ReadAllText(adjectivesFilePath);

                    string animal = animals.Split(';')[rand.Next(0, numberOfName)];
                    string adjective = adjectives.Split(';')[rand.Next(0, numberOfName)];

                    nickname = animal + adjective + rand.Next(0, 100);
                }
            } while (DatabaseManager.DoesPlayerNameExist(nickname));
            DatabaseManager.StockPlayer(nickname);
            return nickname;
        }

        static public string GetPlayerName()
        {
            return playerName;
        }

        static public void LogError(string error)
        {
            if (File.Exists(logsDirPath+ logsFilePath))
            {
                string message = DateTime.Now.ToString() + "\t\t" + "ERROR" + "\t\t" + error;
                using (StreamWriter w = File.AppendText(logsDirPath + logsFilePath))
                {
                    w.WriteLine(message);
                }
            }
        }

        static public void LogInfo(string info)
        {
            if (File.Exists(logsDirPath + logsFilePath))
            {
                string message = DateTime.Now.ToString() + "\t\t" + "INFO" + "\t\t" + info;
                using (StreamWriter w = File.AppendText(logsDirPath + logsFilePath))
                {
                    w.WriteLine(message);
                }
            }
        }
    }
}
