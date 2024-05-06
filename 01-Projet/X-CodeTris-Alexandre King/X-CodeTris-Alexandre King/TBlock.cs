using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    public class TBlock : Tetriminos
    {
        string[] _state1 = new string[6]
        {
        "████",
        "████",
        "████████",
        "████████",
        "████",
        "████"
        };
        string[] _state2 = new string[4]
        {
        "████████████",
        "████████████",
        "!!████",
        "!!████",                
        };
        string[] _state3 = new string[6]
        {
        "!!████",
        "!!████",
        "████████",
        "████████",
        "!!████",
        "!!████"
        };
        public TBlock()
        {
            _name = "Tblock";
            _color = "magenta";

            _baseSprite = new string[4]
            { "!!████",
              "!!████",
              "████████████",
              "████████████" };
            _currentState = 0;
            _width = 3;
            _height = 2;
            DefineAllStates();
        }

        private void DefineAllStates()
        {
            _allStates.Add(_baseSprite);
            _allStates.Add(_state1);
            _allStates.Add(_state2);
            _allStates.Add(_state3);
        }
        override protected void DefineOccupation()
        {
            _occupation = new bool[OCCUPATION_SIZE, OCCUPATION_SIZE];
            switch (_currentState)
            {
                case 0:
                    _occupation[1, 0] = true;
                    _occupation[0, 1] = true;
                    _occupation[1, 1] = true;
                    _occupation[2, 1] = true;
                    _width = 3;
                    _height = 2;
                    break;
                case 1:
                    _occupation[0, 0] = true;
                    _occupation[0, 1] = true;
                    _occupation[1, 1] = true;
                    _occupation[0, 2] = true;
                    _width = 2;
                    _height = 3;
                    break;
                case 2:
                    _occupation[0, 0] = true;
                    _occupation[1, 0] = true;
                    _occupation[2, 0] = true;
                    _occupation[1, 1] = true;
                    _width = 3;
                    _height = 2;
                    break;
                case 3:
                    _occupation[1, 0] = true;
                    _occupation[0, 1] = true;
                    _occupation[1, 1] = true;
                    _occupation[1, 2] = true;
                    _width = 2;
                    _height = 3;
                    break;
                default:
                    break;
            }
        }
    }
}
