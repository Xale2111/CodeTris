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
        //Variables
        const int TOTAL_TETRIMINOS = 7;         //Total of tetriminos
        static Random _random = new Random();
        static Tetriminos _currentTetriminos;                
        
        /// <summary>
        /// Define a new tetriminos
        /// </summary>
        static public void DefineNewTetriminos()
        {
            _currentTetriminos = GetRandomTetriminos();            
        }

        /// <summary>
        /// Draw the tetriminos
        /// </summary>
        /// <param name="xPos">X position of where to start drawing</param>
        /// <param name="yTopPos">Y position of where to start drawing</param>
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

        /// <summary>
        /// Get a random tetriminos
        /// </summary>
        /// <returns>a tetriminos (child of the Tetriminos class)</returns>
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

        /// <summary>
        /// Get the visual width (because it use more visual space then the width indicate)
        /// </summary>
        /// <returns>visual width</returns>
        static public int GetCurrentTetriminosVisualWidth()
        {
            return _currentTetriminos.Width*2;
        }
        /// <summary>
        /// Get the visual height (because it use more visual space then the height indicate)
        /// </summary>
        /// <returns>visual height</returns>
        static public int GetCurrentTetriminosVisualHeight()
        {
            return _currentTetriminos.Height*2;
        }
        /// <summary>
        /// Get the real widht of the tetriminos
        /// </summary>
        /// <returns>real width</returns>
        static public int GetCurrentTetriminosWidth()
        {
            return _currentTetriminos.Width;
        }
        /// <summary>
        /// Get the real height of the tetriminos
        /// </summary>
        /// <returns>real height</returns>
        static public int GetCurrentTetriminosHeight()
        {
            return _currentTetriminos.Height;
        }
        /// <summary>
        /// Get the tetriminos Name
        /// </summary>
        /// <returns></returns>
        static public string GetTetriminosName()
        {
            return _currentTetriminos.Name;
        }
        /// <summary>
        /// Check if a current tetriminios is define
        /// </summary>
        /// <returns>True = a teetriminos is define, False = No define tetriminos</returns>
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

        /// <summary>
        /// Set the current tetriminos to null
        /// </summary>
        static public void ResetTetriminos()
        {
            _currentTetriminos = null;
        }

        /// <summary>
        /// Move the tetriminos to a wanted position
        /// </summary>
        /// <param name="xPos">current x position</param>
        /// <param name="yTopPos">current y position</param>
        /// <param name="whereXpos">where in x the tetriminos should go</param>
        /// <param name="whereYpos">where is Y the tetriminos should go</param>
        static public void MoveTetriminos(int xPos, int yTopPos, int whereXpos, int whereYpos)
        {
            RemoveTetriminos(xPos,yTopPos);
            DrawTetriminos(xPos+ (whereXpos*4), yTopPos+ (whereYpos*2));
        }

        /// <summary>
        /// Rotate the tereiminos
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yTopPos"></param>
        static public void RotateTetriminos(int xPos, int yTopPos)
        { 
            RemoveTetriminos(xPos,yTopPos);
            _currentTetriminos.ChangeState(1);            
        }       

        /// <summary>
        /// Remove visual the tetriminos
        /// </summary>
        /// <param name="xPos">current X position</param>
        /// <param name="yTopPos">current Y position (top of the tetriminos)</param>
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
        /// <summary>
        /// Get the tetriminos occupation
        /// </summary>
        /// <returns>array of bool, if true = case is full, false = case is empty</returns>
        static public bool[,] GetTetriminosOccupation()
        {
            bool[,] occupation = _currentTetriminos.Occupation;

            return occupation;
        }
        /// <summary>
        /// Get the occupation of the tetriminos if he rotates
        /// </summary>
        /// <returns></returns>
        static public bool[,] GetTetriminosRotationOccupation()
        {
            _currentTetriminos.ChangeState(1);
            bool[,] occupation = _currentTetriminos.Occupation;
            _currentTetriminos.ChangeState(-1);

            return occupation;
        }
    }
}
