using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    /// <summary>
    /// Child of the tetriminos class
    /// </summary>
    public class ZBlock : Tetriminos
    {
        //possible States of the tetriminos (!! is equal to space)
        string[] _state1 = new string[6]
        {
        "!!████",
        "!!████",
        "████████",
        "████████",
        "████",
        "████"
        };
        /// <summary>
        /// Constructor of the tetriminos
        /// </summary>
        public ZBlock()
        {
            _name = "Zblock";
            _color = "red";

            _baseSprite = new string[4]
            { "████████",
              "████████",
              "!!████████",
              "!!████████" };
            _currentState = 0;
            _width = 3;
            _height = 2;
            DefineAllStates();
        }
        /// <summary>
        /// Add all the possible states of the tetriminos to a list
        /// </summary>
        private void DefineAllStates()
        {
            _allStates.Add(_baseSprite);
            _allStates.Add(_state1);
        }
        /// <summary>
        /// Define the occupation of each state
        /// </summary>
        override protected void DefineOccupation()
        {
            _occupation = new bool[OCCUPATION_SIZE, OCCUPATION_SIZE];
            switch (_currentState)
            {
                case 0:
                    _occupation[0, 0] = true;
                    _occupation[1, 0] = true;
                    _occupation[1, 1] = true;
                    _occupation[2, 1] = true;
                    _width = 3;
                    _height = 2;
                    break;
                case 1:
                    _occupation[1, 0] = true;
                    _occupation[1, 1] = true;
                    _occupation[0, 1] = true;
                    _occupation[0, 2] = true;
                    _width = 2;
                    _height = 3;
                    break;                
                default:
                    break;
            }
        }
    }
}
