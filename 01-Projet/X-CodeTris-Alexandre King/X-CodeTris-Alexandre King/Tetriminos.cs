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

        protected const int OCCUPATION_SIZE = 4;

        protected bool[,] _occupation = new bool[OCCUPATION_SIZE, OCCUPATION_SIZE]; //Sort of grid where every instance of any piece can be drawn.
                                                       //This will be used to know what are the occupied case of the play grid
        public bool[,] Occupation
        {
            get { return _occupation; }
        }

        public Tetriminos()
        {
            DefineOccupation();
        }

        public void ChangeState(int nextState)
        {
            if (_currentState+nextState >_allStates.Count()-1)
            {
                _currentState = 0;
            }
            else if (_currentState+nextState <0)
            {
                _currentState = _allStates.Count()-1;
            }
            else{
                _currentState += nextState;
            }            
            DefineOccupation();
            TempDebug();
        }
        protected virtual void DefineOccupation(){}

        private void TempDebug()
        {/*
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
            Debug.WriteLine("--------------------------");*/

        }


    }
}
