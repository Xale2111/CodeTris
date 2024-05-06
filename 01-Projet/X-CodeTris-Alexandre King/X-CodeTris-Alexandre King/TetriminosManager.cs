using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    //à voir si public ou static
    static public class TetriminosManager
    {
        const int TOTAL_TETRIMINOS = 7;
        static Random _random = new Random();
        static Tetriminos _currentTetriminos;
        
        //héritage sur les pièces ? faire une classe par pièce (avec une instance de chaque position)?
        
        static public void DefineNewTetriminos()
        {
            _currentTetriminos = GetRandomTetriminos();            
        }

        static public void DrawTetriminos(int xPos, int yTopPos)
        {
            int yPos = yTopPos;
            int multiplySpaces = 0;
            
            VisualManager.SetTextColor(_currentTetriminos.Color);
            foreach (string spriteLine in _currentTetriminos.AllStates[_currentTetriminos.CurrentState])
            {
                string newSpriteLine = spriteLine;                
                if (spriteLine.Contains("!!"))
                {
                    while (newSpriteLine.Contains("!!"))
                    {
                        multiplySpaces++;
                        newSpriteLine = newSpriteLine.Remove(0, 2);
                    }
                    Console.SetCursorPosition(xPos + (4 * multiplySpaces), yPos);                    
                    Console.Write(newSpriteLine);
                    multiplySpaces = 0;

                }
                else
                {
                    Console.SetCursorPosition(xPos, yPos);
                    Console.Write(spriteLine);
                }
                yPos++;
            }           
        }

        static private Tetriminos GetRandomTetriminos()
        {
            int tetriminos = _random.Next(TOTAL_TETRIMINOS);                                   
            //int tetriminos = 2;
            switch (tetriminos)
            {
                case 0:
                    return new OBlock();

                case 1:
                    return new IBlock();

                case 2:
                    return new TBlock();

                case 3:
                    return new LBlock();

                case 4:
                    return new JBlock();

                case 5:
                    return new ZBlock();

                case 6:
                    return new SBlock();
            }            
            return new OBlock();
        }

        static public int GetCurrentTetriminosVisualWidth()
        {
            return _currentTetriminos.Width*2;
        }
        static public int GetCurrentTetriminosVisualHeight()
        {
            return _currentTetriminos.Height*2;
        }
        static public int GetCurrentTetriminosWidth()
        {
            return _currentTetriminos.Width;
        }
        static public int GetCurrentTetriminosHeight()
        {
            return _currentTetriminos.Height;
        }

        static public string GetTetriminosName()
        {
            return _currentTetriminos.Name;
        }

        static public bool HasACurrentTetriminos()
        {
            if (_currentTetriminos == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static public void ResetTetriminos()
        {
            _currentTetriminos = null;
        }

        static public void MoveTetriminos(int xPos, int yTopPos, int whereXpos, int whereYpos)
        {
            RemoveTetriminos(xPos,yTopPos);
            DrawTetriminos(xPos+ (whereXpos*4), yTopPos+ (whereYpos*2));
        }

        static public void RotateTetriminos(int xPos, int yTopPos)
        { 
            RemoveTetriminos(xPos,yTopPos);
            _currentTetriminos.ChangeState(1);            
        }       

        static private void RemoveTetriminos(int xPos, int yTopPos)
        {
            string newSpriteLine = string.Empty;
            int multiplySpaces = 0;
            int yPos = yTopPos;
            foreach (string spriteLine in _currentTetriminos.AllStates[_currentTetriminos.CurrentState])
            {
                newSpriteLine = spriteLine;

                if (spriteLine.Contains("!!"))
                {
                    while (newSpriteLine.Contains("!!"))
                    {
                        multiplySpaces++;
                        newSpriteLine = newSpriteLine.Remove(0, 2);
                    }
                    Console.SetCursorPosition(xPos + (4 * multiplySpaces), yPos);
                    multiplySpaces = 0;
                }
                else
                {
                    Console.SetCursorPosition(xPos, yPos);
                }

                for (int i = 0; i < newSpriteLine.Length; i++)
                {
                    Console.Write(" ");
                }
                yPos++;
            }
        }

        static public bool[,] GetTetriminosOccupation()
        {
            bool[,] occupation = _currentTetriminos.Occupation;

            return occupation;
        }

        static public bool[,] GetTetriminosRotationOccupation()
        {
            _currentTetriminos.ChangeState(1);
            bool[,] occupation = _currentTetriminos.Occupation;
            _currentTetriminos.ChangeState(-1);

            return occupation;
        }
    }
}
