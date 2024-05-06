using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    public class OBlock : Tetriminos
    {
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
        private void DefineAllStates()
        {
            _allStates.Add(_baseSprite);            
        }

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
