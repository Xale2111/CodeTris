using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace X_CodeTris_Alexandre_King
{
    static public class DatabaseManager
    {

        static private bool _dbState;
        static private int userId;
        static private string userName;
        static private string difficulty;

        //user Table infos
        const string USER_TABLE = "`t_user`";
        const string USER_ID_FIELD = "`idUser`";
        const string NICKNAME_FIELD = "`nickname`";
        const string USER_CREATION_DATE_FIELD = "`creationDate`";

        //game table infos
        const string GAME_TABLE = "`t_game`";
        const string SCORE_FIELD = "`score`";
        const string GAME_DATE_FIELD = "`gameDate`";
        const string FK_USER_FIELD = "`fkUser`";
        const string FK_DIFFICULTY_FIELD = "`fkDifficulty`";

        //difficulty table infos
        const string DIFFICULTY_TABLE = "`t_difficulty`";
        const string DIFFICULTY_ID_FIELD = "`idDifficulty`";
        const string DIFFICULTY_NAME_FIELD = "`name`";
        const string DIFFICULTY_LEVEL_FIELD = "`level`";        

        //DB variables (from the config.ini file)
        static Dictionary<string, string> dbConfigurationInfos = new Dictionary<string, string>()
        {
            {"server", ""},
            {"database", ""},
            {"uid", ""},
            {"password", ""},                   
        };       

        static bool hasConfiguration = false;

        static private MySql.Data.MySqlClient.MySqlConnection _connection;

        static public void ConfigureDBInfos()
        {
            string rawInfos = ExternalManager.GetDBConfiguration();
            if (rawInfos != string.Empty)
            {
                for (int i = 0; i < dbConfigurationInfos.Count(); i++)
                {
                    dbConfigurationInfos[dbConfigurationInfos.ElementAt(i).Key] = rawInfos.Split(';')[i].Split('=')[1].Trim();
                }
                hasConfiguration = true;                
            }
        }

        static public void OpenDB()
        {
            //récup infos depuis config.ini
            if (!hasConfiguration)
            {
                ExternalManager.LogError("config.ini file isn't correctly configured. Configuration infos hasn't been found !");
                _dbState = false;
            }
            else
            {

                string connectionString;
                connectionString = "SERVER=" + dbConfigurationInfos["server"] + ";" + "DATABASE=" + dbConfigurationInfos["database"] + ";" + "UID=" + dbConfigurationInfos["uid"] + ";" + "PASSWORD=" + dbConfigurationInfos["password"] + ";";
                _connection = new MySqlConnection(connectionString);
                try
                {
                    _connection.Open();
                    ExternalManager.LogInfo("Database opened successfully");
                    _dbState = true;
                }
                catch (Exception e)
                {
                    ExternalManager.LogError(e.Message);
                    _dbState = false;
                }
            }
        }

        static public bool GetDBState()
        {
            return _dbState;
        }

        static public bool DoesPlayerNameExist(string playerName)
        {
            try
            {
                MySql.Data.MySqlClient.MySqlCommand com = _connection.CreateCommand();

                com.CommandType = System.Data.CommandType.Text;
                com.CommandText = "SELECT * FROM t_user where nickname = '" + playerName + "';";
                MySql.Data.MySqlClient.MySqlDataReader reader = com.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userId = reader.GetInt32(0);
                        userName = reader.GetString(1);
                    }
                    reader.Close();
                }
                return false;
            }
            catch (Exception e)
            {
                ExternalManager.LogError(e.Message);
                return true;
            }
        }

        static public bool StockPlayer(string playerName)
        {
            OpenDB();
            try
            {
                MySql.Data.MySqlClient.MySqlCommand com = _connection.CreateCommand();

                com.CommandType = System.Data.CommandType.Text;
                com.CommandText = "INSERT INTO " + USER_TABLE + "(" + NICKNAME_FIELD + "," + USER_CREATION_DATE_FIELD + ") VALUES ('" + playerName + "','" + FormatDate() + "');";
                MySql.Data.MySqlClient.MySqlDataReader reader = com.ExecuteReader();
                ExternalManager.LogInfo("Player <<" + playerName + ">> was inserted in the database. Account has been created succesfully");
                return true;
            }
            catch (Exception e)
            {
                ExternalManager.LogError(e.Message);
                return false;
            }

            _connection.Close();
        }

        static public bool StockGame(int score, int difficulty)
        {
            OpenDB();
            try
            {
                int id = FindPlayerIDWithName();
                MySql.Data.MySqlClient.MySqlCommand com = _connection.CreateCommand();

                com.CommandType = System.Data.CommandType.Text;
                com.CommandText = "INSERT INTO " + GAME_TABLE + "(" + SCORE_FIELD + "," + DIFFICULTY_NAME_FIELD + "," + DIFFICULTY_LEVEL_FIELD + "," + FK_DIFFICULTY_FIELD + ") VALUES ('" + score + "','" + FormatDate() + "','" + id + "','" + difficulty + "');";
                MySql.Data.MySqlClient.MySqlDataReader reader = com.ExecuteReader();
                ExternalManager.LogInfo("New game by user <<" + id + " : " + ExternalManager.GetPlayerName() + ">> was inserted succesfully. Final score is " + score);
                return true;
            }
            catch (Exception e)
            {
                ExternalManager.LogError(e.Message);
                return false;
            }

            _connection.Close();
        }


        static private int FindPlayerIDWithName()
        {
            int userID = -1;

            try
            {
                MySql.Data.MySqlClient.MySqlCommand com = _connection.CreateCommand();

                com.CommandType = System.Data.CommandType.Text;
                com.CommandText = "SELECT * FROM " + USER_TABLE + " where nickname = '" + ExternalManager.GetPlayerName() + "' LIMIT 1;";
                MySql.Data.MySqlClient.MySqlDataReader reader = com.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userID = reader.GetInt32(0);
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                ExternalManager.LogError(e.Message);
            }

            return userID;
        }

        static private string FindPlayerNameWithID(int id)
        {
            OpenDB();
            string userName = string.Empty;
            try
            {
                MySql.Data.MySqlClient.MySqlCommand com = _connection.CreateCommand();

                com.CommandType = System.Data.CommandType.Text;
                com.CommandText = "SELECT " + NICKNAME_FIELD + " FROM " + USER_TABLE + " where " + USER_ID_FIELD + " = '" + id + "' LIMIT 1;";
                MySql.Data.MySqlClient.MySqlDataReader reader = com.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userName = reader.GetString(0);
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                ExternalManager.LogError(e.Message);
            }
            _connection.Close();
            return userName;
        }

        static private int FindDifficultyIDWithLevel(int level)
        {
            OpenDB();
            int difficultyID = -1;
            try
            {
                MySql.Data.MySqlClient.MySqlCommand com = _connection.CreateCommand();

                com.CommandType = System.Data.CommandType.Text;
                com.CommandText = "SELECT " + DIFFICULTY_ID_FIELD + " FROM " + DIFFICULTY_TABLE + " where " + DIFFICULTY_LEVEL_FIELD + " = '" + level + "' LIMIT 1;";
                MySql.Data.MySqlClient.MySqlDataReader reader = com.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        difficultyID = reader.GetInt32(0);
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                ExternalManager.LogError(e.Message);
            }
            _connection.Close();
            return difficultyID;
        }


        static private string FormatDate()
        {
            string date = string.Empty;

            string day = DateTime.Now.ToShortDateString().Split('.')[0];
            string month = DateTime.Now.ToShortDateString().Split('.')[1];
            string year = DateTime.Now.ToShortDateString().Split('.')[2];

            date = year + "-" + month + "-" + day;

            return date;
        }

        static public List<Tuple<string, int, string>> GetHighScores(int difficulty)
        {
            List<Tuple<string, int, string>> highscores = new List<Tuple<string, int, string>>();
            List<int> usersId = new List<int>();
            string playerName = string.Empty;
            int playerId = -1;
            int playerScore = 0;
            string gameDate = string.Empty;
            int difficultyID = FindDifficultyIDWithLevel(difficulty);
            OpenDB();
            try
            {
                MySql.Data.MySqlClient.MySqlCommand com = _connection.CreateCommand();

                com.CommandType = System.Data.CommandType.Text;
                com.CommandText = "SELECT * FROM " + GAME_TABLE + "WHERE fkDifficulty = "+ difficultyID + " ORDER BY " + SCORE_FIELD + " DESC LIMIT 10";
                MySql.Data.MySqlClient.MySqlDataReader reader = com.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        playerId = reader.GetInt32(3);
                        playerScore = reader.GetInt32(1);
                        gameDate = reader.GetDateTime(2).ToString();
                        gameDate = gameDate.Split(' ')[0];
                        usersId.Add(playerId);
                        highscores.Add(new Tuple<string, int, string>(playerName, playerScore, gameDate));

                    }
                    reader.Close();
                }

            }
            catch (Exception e)
            {
                ExternalManager.LogError(e.Message);
            }
            _connection.Close();

            int count = 0;

            foreach (int userId in usersId)
            {
                highscores[count] = new Tuple<string, int, string>(FindPlayerNameWithID(userId), highscores[count].Item2, highscores[count].Item3);
                count++;
            }


            return highscores;

        }
    }
}
