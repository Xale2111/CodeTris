using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    public class IBlock : Tetriminos
    {
        string[] _state1 = new string[2]
        {
            "████████████████",
            "████████████████"                
        };
        public IBlock()
        {

            _name = "Iblock";
            _color = "cyan";

            _baseSprite = new string[8]
            { "████",
              "████",
              "████",
              "████",
              "████",
              "████",
              "████",
              "████" };
            _currentState = 0;
            _width = 1;
            _height = 4;
            DefineAllStates();
        }

        private void DefineAllStates()
        {
            _allStates.Add(_baseSprite);
            _allStates.Add(_state1);
        }
        override protected void DefineOccupation()
        {
            _occupation = new bool[OCCUPATION_SIZE, OCCUPATION_SIZE];
            switch (_currentState)
            {
                case 0:
                    _occupation[0, 0] = true;
                    _occupation[0, 1] = true;
                    _occupation[0, 2] = true;
                    _occupation[0, 3] = true;
                    _width = 1;
                    _height = 4;
                    break;
                case 1:
                    _occupation[0, 0] = true;
                    _occupation[1, 0] = true;
                    _occupation[2, 0] = true;
                    _occupation[3, 0] = true;
                    _width = 4;
                    _height = 1;
                    break;                
                default:
                    break;
            }
        }
    }
}
