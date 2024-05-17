using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    /// <summary>
    /// Class for the question (This is how a question is define and created)
    /// </summary>
    public class Question
    {
        private string _quote;

        /// <summary>
        /// Quote of the question
        /// </summary>
        public string Quote
        {
            get { return _quote; }
            set { _quote = value; }
        }

        private string _answer;
        /// <summary>
        /// Answer of the quesiton
        /// </summary>
        public string Answer
        {
            get { return _answer; }
            set { _answer = value; }
        }
        /// <summary>
        /// Constructor of the question
        /// </summary>
        /// <param name="question">Quote of the question</param>
        /// <param name="answer">answer of the question</param>
        public Question(string question, string answer)
        {
            this._quote = question;
            this._answer = answer;
        }
    }
}
