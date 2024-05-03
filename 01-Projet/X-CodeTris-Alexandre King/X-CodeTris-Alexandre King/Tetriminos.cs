using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    public class Tetriminos
    {        
        protected string _name;

        public string Name
        {
            get { return _name; }           
        }
        protected string[] _baseSprite;

        public string[] BaseSprite
        {
            get { return _baseSprite; }           
        }

        protected string _color;

        public string Color
        {
            get { return _color; }            
        }

        protected int _currentState;

        public int CurrentState
        {
            get { return _currentState; }            
        }

        protected int _width;

        public int Width
        {
            get { return _width; }            
        }

        protected int _height;

        public int Height
        {
            get { return _height; }            
        }        
    }
}
