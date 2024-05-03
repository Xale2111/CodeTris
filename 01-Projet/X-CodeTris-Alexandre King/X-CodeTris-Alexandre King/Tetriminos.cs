using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        protected List<string[]> _allStates = new List<string[]>();

        public List<string[]> AllStates
        {
            get { return _allStates; }            
        }

        protected bool[,] _occupation = new bool[4,4]; //Sort of grid where every instance of any piece can be drawn.
                                                       //This will be used to know what are the occupied case of the play grid
        public bool[,] Occupation
        {
            get { return _occupation; }
        }

        public void ChangeState()
        {
            if (_currentState== _allStates.Count()-1)
            {
                _currentState = 0;
            }
            else
            {
                _currentState++;
            }
            int newWidth = _height;
            int newHeight = _width;
            _width = newWidth;
            _height = newHeight;
            DefineOccupation();
            TempDebug();
        }
        protected virtual void DefineOccupation(){}

        private void TempDebug()
        {
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (_occupation[i,j])
                    {
                        Debug.Write("[•]");
                    }
                    else
                    {
                        Debug.Write("[ ]");
                    }
                }
                Debug.WriteLine("");

            }
            Debug.WriteLine("--------------------------");

        }


    }
}
