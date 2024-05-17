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
    public class OBlock : Tetriminos
    {
        /// <summary>
        /// Constructor of the tetriminos
        /// </summary>
        public OBlock()
        {
            _name = "Oblock";
            _color = "yellow";
            _baseSprite = new string[4]
            { "████████",
              "████████",
              "████████",
              "████████" };
            _currentState = 0;
            _width = 2;
            _height = 2;
            DefineAllStates();
        }
        /// <summary>
        /// Add all the possible states of the tetriminos to a list
        /// </summary>
        private void DefineAllStates()
        {
            _allStates.Add(_baseSprite);            
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
                    _occupation[0, 1] = true;
                    _occupation[1, 1] = true;
                    break;                
                default:
                    break;
            }
        }
    }
}
