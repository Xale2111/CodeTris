using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace X_CodeTris_Alexandre_King
{
    static public class DatabaseManager
    {
        //Variables 
        static private bool _dbState;           //state of the DB, true => DB is working fine, false => DB is connected
        static private int _userId;             //User ID
        static private string _userName;        //User name
        static private string _difficulty;
        static bool _hasConfiguration = false;
        static private MySqlConnection _connection;

        //user Table infos
        const string USER_TABLE = "`t_user`";
        const string USER_ID_FIELD = "`idUser`";
        const string NICKNAME_FIELD = "`nickname`";
        const string USER_CREATION_DATE_FIELD = "`creationDate`";


        //game table infos
        const string GAME_TABLE = "`t_game`";
        const string SCORE_FIELD = "`score`";
        const string GAME_DATE_FIELD = "`gameDate`";       

        //difficulty table infos
        const string DIFFICULTY_TABLE = "`t_difficulty`";
        const string DIFFICULTY_ID_FIELD = "`idDifficulty`";
        const string DIFFICULTY_NAME_FIELD = "`name`";
        const string DIFFICULTY_LEVEL_FIELD = "`level`";

        //Questions table infos        
        const string QUESTION_TABLE = "`t_question`";
        const string QUESTION_ID_FIELD = "`idQuestion`";
        const string QUESTION_FIELD = "`question`";
        const string ANSWER_FIELD = "`answer`";

        //General table infos
        const string FK_USER_FIELD = "`fkUser`";
        const string FK_DIFFICULTY_FIELD = "`fkDifficulty`";


        //DB variables (from the config.ini file)
        static Dictionary<string, string> _dbConfigurationInfos = new Dictionary<string, string>()
        {            
            {"server", ""},
            {"database", ""},
            {"uid", ""},
            {"password", ""},                   
        };       
       
        
        /// <summary>
        /// Will get the raw infos from the config.ini file via the external manager and then set the variable for the connection
        /// </summary>
        static public void ConfigureDBInfos()
        {
            string rawInfos = ExternalManager.GetDBConfiguration();
            if (rawInfos != string.Empty)
            {
                for (int i = 0; i < _dbConfigurationInfos.Count(); i++)
                {
                    _dbConfigurationInfos[_dbConfigurationInfos.ElementAt(i).Key] = rawInfos.Split(';')[i].Split('=')[1].Trim();
                }
                _hasConfiguration = true;                
            }
        }

        /// <summary>
        /// Open the Database to establish a connection with it
        /// </summary>
        static public void OpenDB()
        {
            //récup infos depuis config.ini
            if (!_hasConfiguration)
            {
                ExternalManager.LogError("config.ini file isn't correctly configured. Configuration infos hasn't been found !");
                _dbState = false;
            }
            else
            {

                string connectionString;
                connectionString = "SERVER=" + _dbConfigurationInfos["server"] + ";" + "DATABASE=" + _dbConfigurationInfos["database"] + ";" + "UID=" + _dbConfigurationInfos["uid"] + ";" + "PASSWORD=" + _dbConfigurationInfos["password"] + ";";
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

        /// <summary>
        /// Get the current state of the DB
        /// </summary>
        /// <returns>state of the DB, true = Connected, working fine / false = Not connected</returns>
        static public bool GetDBState()
        {
            return _dbState;
        }

        /// <summary>
        /// Checks if the current name for the player exist or not in the DB
        /// </summary>
        /// <param name="playerName">name that needs to be checked</param>
        /// <returns>True = name exists, false = name doesn't exists</returns>
        static public bool DoesPlayerNameExist(string playerName)
        {
            try
            {
                MySqlCommand com = _connection.CreateCommand();

                com.CommandType = System.Data.CommandType.Text;
                com.CommandText = "SELECT * FROM t_user where nickname = '" + playerName + "';";
                MySqlDataReader reader = com.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        _userId = reader.GetInt32(0);
                        _userName = reader.GetString(1);
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

        /// <summary>
        /// Stock the player in the Database
        /// </summary>
        /// <param name="playerName">Name that will be stock</param>
        /// <returns>True = Name has been inserted successfully, False = Name hasn't been inserted (Check logs to see what happened)</returns>
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
        /// <summary>
        /// Stock the player's game in the DB
        /// </summary>
        /// <param name="score">final score</param>
        /// <param name="difficulty">game difficulty</param>
        /// <returns>True = Game has been inserted successfully, False = Game hasn't been inserted</returns>
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

        /// <summary>
        /// Find the Player ID with his name
        /// </summary>
        /// <returns>Player ID</returns>
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
        /// <summary>
        /// Find the player name with his ID
        /// </summary>
        /// <param name="id">Player ID</param>
        /// <returns>Player name</returns>
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
        /// <summary>
        /// Find the Difficulty ID with the difficulty level
        /// </summary>
        /// <param name="level">difficulty level</param>
        /// <returns>Difficulty ID</returns>
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

        /// <summary>
        /// Format the Date to correspond to the needed format for the DB (Needs to be YYYY-MM-DD)
        /// </summary>
        /// <returns>Correct date format</returns>
        static private string FormatDate()
        {
            string date = string.Empty;

            string day = DateTime.Now.ToShortDateString().Split('.')[0];
            string month = DateTime.Now.ToShortDateString().Split('.')[1];
            string year = DateTime.Now.ToShortDateString().Split('.')[2];

            date = year + "-" + month + "-" + day;

            return date;
        }

        /// <summary>
        /// Get the highscores of a specific difficulty
        /// </summary>
        /// <param name="difficulty">Difficulty to filter</param>
        /// <returns>A list of Tuple<string,int,string> Each Tuple is a highscore, 
        /// The tuple is used to have the nickname (1st string), the score (2nd string) and the date of the record (3thd string)</returns>
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
        
        static public List<Question> GetAllQuestionOfDifficulty(int difficulty)
        {
            int difficultyID = FindDifficultyIDWithLevel(difficulty);
            _connection.Close();

            string question = string.Empty;
            string answer = string.Empty;
            
            List<Question> questions = new List<Question>();

            OpenDB();
            try
            {                
                MySql.Data.MySqlClient.MySqlCommand com = _connection.CreateCommand();

                com.CommandType = System.Data.CommandType.Text;
                com.CommandText = "SELECT * FROM " + QUESTION_TABLE + "WHERE fkDifficulty = " + difficultyID;

                MySql.Data.MySqlClient.MySqlDataReader reader = com.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        question = reader.GetString(1);
                        answer = reader.GetString(2);
                        Question newQuestion = new Question(question,answer);
                        questions.Add(newQuestion);
                    }
                    reader.Close();
                }

                ExternalManager.LogInfo("All question of difficulty "+ difficulty + " were selected.");                
            }
            catch (Exception e)
            {
                ExternalManager.LogError(e.Message);                
            }

            _connection.Close();
            return questions;
        }
    }
}
