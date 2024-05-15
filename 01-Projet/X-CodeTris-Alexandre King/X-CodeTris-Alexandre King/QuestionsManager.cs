using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    public class QuestionsManager
    {
        Random random = new Random();
        private List<Question> _usedQuestions = new List<Question>();

        private List<Question> _allQuestions = new List<Question>();

        public List<Question> AllQuestions
        {
            get { return _allQuestions; }            
        }

        public QuestionsManager(int difficulty)
        {
           _allQuestions = DatabaseManager.GetAllQuestionOfDifficulty(difficulty);
        }

        public Question GetRandomQuestion()
        {
            if (_allQuestions.Count == 0)
            {
                foreach (Question usedQuestion in _usedQuestions)
                {
                    _allQuestions.Add(usedQuestion);
                }
                _usedQuestions.Clear();
            }
            int currentQuestionID = random.Next(_allQuestions.Count);            
            Question question = _allQuestions[currentQuestionID];
            _usedQuestions.Add(question);
            _allQuestions.RemoveAt(currentQuestionID);
            return question;
        }
    }
}
