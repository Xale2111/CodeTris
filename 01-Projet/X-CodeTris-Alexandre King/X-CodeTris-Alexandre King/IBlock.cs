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
            _occupation = new bool[4, 4];

            for (int j = 0; j < _height; j++)
            {
                for (int i = 0; i < _width; i++)
                {
                    _occupation[i, j] = true;
                }
            }                                    
        }
    }
}
