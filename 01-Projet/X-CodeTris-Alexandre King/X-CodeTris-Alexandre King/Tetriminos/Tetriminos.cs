using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    /// <summary>
    /// Parent. Tetriminos class dor all the different tetriminos
    /// </summary>
    public class Tetriminos
    {        
        protected string _name;

        /// <summary>
        /// Name of the Tetriminos
        /// </summary>
        public string Name
        {
            get { return _name; }           
        }
        protected string[] _baseSprite;

        /// <summary>
        /// Base sprite of the tetriminos (will appear with this sprite)
        /// </summary>
        public string[] BaseSprite
        {
            get { return _baseSprite; }           
        }

        protected string _color;

        /// <summary>
        /// Color of the tetriminos
        /// </summary>
        public string Color
        {
            get { return _color; }            
        }

        protected int _currentState;
        /// <summary>
        /// Current state of the tetriminos (which sprite)
        /// </summary>
        public int CurrentState
        {
            get { return _currentState; }            
        }

        protected int _width;
        /// <summary>
        /// Width of the tetriminos
        /// </summary>
        public int Width
        {
            get { return _width; }            
        }

        protected int _height;
        /// <summary>
        /// Height of the tetriminos
        /// </summary>
        public int Height
        {
            get { return _height; }            
        }
        protected List<string[]> _allStates = new List<string[]>();

        /// <summary>
        /// Each states of the tetriminos (how will it be when it rotates
        /// </summary>
        public List<string[]> AllStates
        {
            get { return _allStates; }            
        }

        /// <summary>
        /// Size of the occupation array
        /// </summary>
        protected const int OCCUPATION_SIZE = 4;

        protected bool[,] _occupation = new bool[OCCUPATION_SIZE, OCCUPATION_SIZE]; //Sort of grid where every instance of any piece can be drawn.
                                                                                    //This will be used to know what are the occupied case of the play grid
        /// <summary>
        /// Grid of the occupation of the tetriminos
        /// </summary>
        public bool[,] Occupation
        {
            get { return _occupation; }
        }

        /// <summary>
        /// Constructor of the Tetriminos
        /// </summary>
        public Tetriminos()
        {
            DefineOccupation();
        }

        /// <summary>
        /// Go to the next state (rotate)
        /// </summary>
        /// <param name="nextState">which state to go to </param>
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
        }
        /// <summary>
        /// Define The occupation of the teriminos in a 4 by 4 square
        /// This is in virtual so all child can override it but it can still be call be using the parent class
        /// </summary>
        protected virtual void DefineOccupation(){}


    }
}
