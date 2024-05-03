using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    public class IBlock : Tetriminos
    {
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
        }
    }
}
