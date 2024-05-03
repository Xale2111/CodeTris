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
            VisualManager.SetTextColor(_currentTetriminos.Color);
            foreach (string spriteLine in _currentTetriminos.BaseSprite)
            {
                Console.SetCursorPosition(xPos,yPos);
                Console.Write(spriteLine);
                yPos++;
            }           
        }

        static private Tetriminos GetRandomTetriminos()
        {
            int tetriminos = _random.Next(TOTAL_TETRIMINOS);            
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

        static public int GetCurrentTetriminosWidth()
        {
            return _currentTetriminos.Width*2;
        }
        static public int GetCurrentTetriminosHeight()
        {
            return _currentTetriminos.Height*2;
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

        static public void MoveTetriminos(int xPos, int yTopPos, int whereXpos, int whereYpos)
        {
            int yPos = yTopPos;            
            foreach (string spriteLine in _currentTetriminos.BaseSprite)
            {
                Console.SetCursorPosition(xPos, yPos);
                for (int i = 0; i < spriteLine.Length; i++)
                {
                    Console.Write(" ");
                }
                yPos++;
            }
            DrawTetriminos(xPos+ (whereXpos*4), yTopPos+ (whereYpos*2));
        }
    }
}
