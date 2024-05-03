using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    public class GameManager
    {
        const int PLAY_ZONE_WIDTH = 32;
        const int PLAY_ZONE_HEIGHT = 48;
        const int PLAY_ZONE_X_POS = 60;
        const int PLAY_ZONE_Y_POS = 8;

        //console coordonate
        int _currentTetriminosXPos;
        int _currentTetriminosYPos;
        
        //Bool Aray coordonate
        int _playZoneTetriminosXPos;
        int _playZoneTetriminosYPos;

        //TODO : PieceManager (pour gérer les différentes pièces, les faire tourner etc...)

        int _numberOfRightAnswer = 0;
        int _numberOfWrongAnswer = 0;

        bool _inGame = false;        
        int _frameTiming = 150;
        int _frameBeforeNew = 2;
        bool _hasToMoveDown = false;
        bool _playerMove = false;
        bool _canSpawnNew = true;
        Thread _playerInputThread;

        ConsoleKey _moveLeft;
        ConsoleKey _moveRight;
        ConsoleKey _moveDown;
        ConsoleKey _rotate;
        ConsoleKey _dropDown;
        ConsoleKeyInfo _pressedKey;

        bool[,] _playZone = new bool[PLAY_ZONE_WIDTH/2, PLAY_ZONE_HEIGHT/2];        
        public void NewGame()
        { 
            Console.Clear();
            ResetAll();
            DrawPlayArea();
            DefinePlayingKeys();
            StartGame();           
        }

        private void DefinePlayingKeys()
        {            
            switch (MenuManager.GetPlayingKeys())
            {
                case 0:
                    _moveLeft = ConsoleKey.A;
                    _moveRight = ConsoleKey.D;
                    _moveDown = ConsoleKey.S;
                    _rotate = ConsoleKey.W;
                    break;
                case 1:
                    _moveLeft = ConsoleKey.LeftArrow;
                    _moveRight = ConsoleKey.RightArrow;
                    _moveDown = ConsoleKey.DownArrow;
                    _rotate = ConsoleKey.UpArrow;
                    break;
                default:
                    _moveLeft = ConsoleKey.A;
                    _moveRight = ConsoleKey.D;
                    _moveDown = ConsoleKey.S;
                    _rotate = ConsoleKey.W;
                    break;
            }
            _dropDown = ConsoleKey.Spacebar;
        }

        private void ResetAll()
        {
            _playZone = new bool[PLAY_ZONE_WIDTH, PLAY_ZONE_HEIGHT];
            _numberOfRightAnswer = 0;
            _numberOfWrongAnswer = 0;
            _inGame = false;
            _pressedKey = default(ConsoleKeyInfo);
            _playerMove = false;
            _canSpawnNew = true;

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
            _inGame = true;            
            //Thread pour la gestion des inputs utilisateur
            Thread playerInput = new Thread(ManagePlayerInput);
            playerInput.Start();
            _playerInputThread = playerInput;
            int frameCounter = 0;
            while (_inGame)
            {
                Thread.Sleep(_frameTiming);
                if (TetriminosManager.HasACurrentTetriminos())
                {
                    if (_currentTetriminosYPos < PLAY_ZONE_Y_POS + PLAY_ZONE_HEIGHT - TetriminosManager.GetCurrentTetriminosHeight())
                    {
                        if (_playerMove)
                        {
                            Thread.Sleep(50);
                            _hasToMoveDown = true;
                        }
                        TetriminosManager.MoveTetriminos(_currentTetriminosXPos, _currentTetriminosYPos, 0, 1);
                        _currentTetriminosYPos += 2;
                        _hasToMoveDown = false;
                    }
                    else
                    {
                        frameCounter++;
                        if (frameCounter > _frameBeforeNew)
                        {
                            _canSpawnNew = true;
                            frameCounter = 0;
                        }
                    }
                }
                if (_canSpawnNew)
                {
                    _canSpawnNew = false;
                    _hasToMoveDown = true;
                    TetriminosManager.DefineNewTetriminos();
                    DrawNewTetriminos();
                    _hasToMoveDown = false;
                }
                 

                //Can spawn new tetrominos ? 
                //--> Yes, spawn the one in the "next piece" case
                //--> No, Move the current one by one
            }
        }

        private void DrawNewTetriminos()
        {
            int xStartPosition = PLAY_ZONE_X_POS + PLAY_ZONE_WIDTH;            
            _currentTetriminosXPos = xStartPosition;
            _currentTetriminosYPos = PLAY_ZONE_Y_POS;
            //layZoneTetriminosXPos = ;
            TetriminosManager.DrawTetriminos(_currentTetriminosXPos, _currentTetriminosYPos);
        }

        private void ManagePlayerInput()
        {
            do
            {
                _pressedKey = Console.ReadKey(true);
                _playerMove = true;
                if (!_hasToMoveDown)
                {
                    //Can't use a switch because the case require to use a constant
                    //(currently using a variable incase player modify the wanted playing keys)
                    if (_pressedKey.Key == _moveLeft)
                    {
                        if (_currentTetriminosXPos > PLAY_ZONE_X_POS)
                        {
                            TetriminosManager.MoveTetriminos(_currentTetriminosXPos, _currentTetriminosYPos, -1, 0);
                            _currentTetriminosXPos-=4;
                        }
                    }
                    else if (_pressedKey.Key == _moveRight)
                    {
                        if (_currentTetriminosXPos + TetriminosManager.GetCurrentTetriminosWidth() * 2 < PLAY_ZONE_X_POS + PLAY_ZONE_WIDTH * 2)
                        {
                            TetriminosManager.MoveTetriminos(_currentTetriminosXPos, _currentTetriminosYPos, 1, 0);
                            _currentTetriminosXPos+=4;
                        }
                    }
                    else if (_pressedKey.Key == _moveDown)
                    {
                        if (_currentTetriminosYPos + 1 <= PLAY_ZONE_Y_POS + PLAY_ZONE_HEIGHT - TetriminosManager.GetCurrentTetriminosHeight())
                        {
                            TetriminosManager.MoveTetriminos(_currentTetriminosXPos, _currentTetriminosYPos, 0, 1);
                            _currentTetriminosYPos+=2;
                        }

                    }
                    else if (_pressedKey.Key == _rotate)
                    {

                    }
                    
                    _pressedKey = default(ConsoleKeyInfo);
                }

            } while (_inGame);
        }
    }
}
