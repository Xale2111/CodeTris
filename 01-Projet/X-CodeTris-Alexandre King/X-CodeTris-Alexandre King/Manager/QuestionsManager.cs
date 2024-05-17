using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    /// <summary>
    /// Manager of the questions. Everything that is question related is managed here
    /// </summary>
    public class QuestionsManager
    {
        //Variables
        Random random = new Random();
        private List<Question> _usedQuestions = new List<Question>();


        private List<Question> _allQuestions = new List<Question>();
        /// <summary>
        /// List of every possible question in the current game
        /// </summary>
        public List<Question> AllQuestions
        {
            get { return _allQuestions; }
        }
        /// <summary>
        /// Constructor of the question manager
        /// </summary>
        /// <param name="difficulty">current difficulty of the game</param>
        public QuestionsManager(int difficulty)
        {
            _allQuestions = DatabaseManager.GetAllQuestionOfDifficulty(difficulty);
        }
        /// <summary>
        /// Get a random question from the list of possibility questions
        /// </summary>
        /// <returns>a question</returns>
        public Question GetRandomQuestion()
        {
            //First ask for each possible question so that the same question isn't ask more often then an other
            //This also makes the game more fun because it prevents the user from having always the same question
            if (_allQuestions.Count == 0)
            {
                //once each question has been asked, reset the possible question list and clear the used question list
                foreach (Question usedQuestion in _usedQuestions)
                {
                    _allQuestions.Add(usedQuestion);
                }
                _usedQuestions.Clear();
            }
            //Once a question has been asked, it is added to the used question list
            int currentQuestionID = random.Next(_allQuestions.Count);
            Question question = _allQuestions[currentQuestionID];
            _usedQuestions.Add(question);
            _allQuestions.RemoveAt(currentQuestionID);
            return question;
        }
    }
}
