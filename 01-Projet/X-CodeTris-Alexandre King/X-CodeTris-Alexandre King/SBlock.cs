using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    public class SBlock : Tetriminos
    {
        public SBlock()
        {
            _name = "Sblock";
            _color = "green";

            _baseSprite = new string[4]
            { "    ████████",
              "    ████████",
              "████████",
              "████████" };
            _currentState = 0;
            _width = 3;
            _height = 2;
        }
    }
}
