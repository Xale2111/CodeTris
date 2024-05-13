using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    public class Question
    {
        private string _question;

        public string Quote
        {
            get { return _question; }
            set { _question = value; }
        }

        private string _answer;

        public string Answer
        {
            get { return _answer; }
            set { _answer = value; }
        }
        
        public Question(string question, string anser)
        {
            this._question = question;
            this._answer = anser;
        }
    }
}
