using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    public class ZBlock : Tetriminos
    {
        string[] _state1 = new string[6]
        {
        "!!████",
        "!!████",
        "████████",
        "████████",
        "████",
        "████"
        };
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
        private void DefineAllStates()
        {
            _allStates.Add(_baseSprite);
            _allStates.Add(_state1);
        }
    }
}
