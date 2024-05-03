using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    public class LBlock : Tetriminos
    {
        string[] _state1 = new string[4]
        {
        "████████████",
        "████████████",
        "████",
        "████"
        };
        string[] _state2 = new string[6]
        {
        "████████",
        "████████",
        "!!████",
        "!!████",
        "!!████",
        "!!████"
        };
        string[] _state3 = new string[4]
        {
        "!!!!████",
        "!!!!████",
        "████████████",
        "████████████",
        };

        public LBlock()
        {
            _name = "Lblock";
            _color = "dkyellow";

            _baseSprite = new string[6]
            { "████",
              "████",
              "████",
              "████",
              "████████",
              "████████" };
            _currentState = 0;
            _width = 2;
            _height = 3;
            DefineAllStates();
        }
        private void DefineAllStates()
        {
            _allStates.Add(_baseSprite);
            _allStates.Add(_state1);
            _allStates.Add(_state2);
            _allStates.Add(_state3);
        }
    }
}
