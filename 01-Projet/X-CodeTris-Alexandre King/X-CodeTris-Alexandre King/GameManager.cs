using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    public class GameManager
    {
        const int PLAY_ZONE_WIDTH = 32;
        const int PLAY_ZONE_HEIGHT = 46;
        const int PLAY_ZONE_X_POS = 32;
        const int PLAY_ZONE_Y_POS = 8;

        //TODO : PieceManager (pour gérer les différentes pièces, les faire tourner etc...)

        int numberOfRightAnswer = 0;
        int numberOfWrongAnswer = 0;
        
        bool[,] playZone = new bool[PLAY_ZONE_WIDTH, PLAY_ZONE_HEIGHT];

        public void NewGame()
        { 
            Console.Clear();
            ResetAll();
            DrawPlayArea();
            StartGame();
        }

        private void ResetAll()
        {
            playZone = new bool[PLAY_ZONE_WIDTH, PLAY_ZONE_HEIGHT];
            numberOfRightAnswer = 0;
            numberOfWrongAnswer = 0;
        }
        private void DrawPlayArea()
        {
            int xPos = PLAY_ZONE_X_POS;
            int yPos = PLAY_ZONE_Y_POS;

            VisualManager.SetBackgroundColor("gray");                        
            for (int j = 0; j < PLAY_ZONE_HEIGHT; j++)
            {
                for (int i = 0; i < PLAY_ZONE_WIDTH; i++)
                {
                    Console.SetCursorPosition(xPos, yPos);
                    Console.Write("  "); 
                    xPos += 2;
                }
                xPos = PLAY_ZONE_X_POS;
                yPos++;
            }
        }

        private void StartGame()
        { 
        
        }
    }
}
