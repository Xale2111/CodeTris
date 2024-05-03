﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    public class LBlock : Tetriminos
    {
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
        }
    }
}
