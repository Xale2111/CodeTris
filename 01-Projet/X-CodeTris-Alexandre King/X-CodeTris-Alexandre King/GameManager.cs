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
        
        int _numberOfRightAnswer = 0;
        int _numberOfWrongAnswer = 0;

        bool _inGame = false;
        int _frameTiming = 150;
        int _frameBeforeNew = 2;
        int _moveYCapacity = 1;
        int _moveXCapacity = 1;
        List<string> _instructionsTetriminos = new List<string>();

        bool _hasToMoveDown = false;
        bool _playerMove = false;
        bool _canSpawnNew = true;
        List<Thread> _allThreads = new List<Thread>();

        ConsoleKey _moveLeft;
        ConsoleKey _moveRight;
        ConsoleKey _moveDown;
        ConsoleKey _rotate;
        ConsoleKey _dropDown;
        ConsoleKeyInfo _pressedKey;
        int _maxInput = 5;
        int _numberOfPlayerInput;
        bool _maxInputReached;

        int[,] _playZone = new int[PLAY_ZONE_WIDTH / 2, PLAY_ZONE_HEIGHT / 2]; //values divided by2 because 1 "block" is 2X2 sized (Oblock is 4 "blocks" together)
                                                                               //0-> empty, 1-> occupied (current tetriminos),
                                                                               //2-> occupied (placed tetriminos),3->Blocked       
                                                                               //Faire constantes/enum
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
            _playZone = new int[PLAY_ZONE_WIDTH / 2, PLAY_ZONE_HEIGHT / 2];
            _numberOfRightAnswer = 0;
            _numberOfWrongAnswer = 0;
            _inGame = false;
            _pressedKey = default(ConsoleKeyInfo);
            _playerMove = false;
            _canSpawnNew = true;
            _numberOfPlayerInput = 0;

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

        private void StartAllThreads()
        {
            Thread playerInput = new Thread(ManagePlayerInput);
            playerInput.Start();
            _allThreads.Add(playerInput);

            Thread limitInput = new Thread(LimitInput);
            limitInput.Start();
            _allThreads.Add(limitInput);

            Thread tetriminosManagement = new Thread(ManageTetriminos);
            tetriminosManagement.Start();
            _allThreads.Add(tetriminosManagement);
        }

        private void LimitInput()
        {
            while (_inGame)
            {
                if (_maxInputReached)
                {
                    Thread.Sleep(_frameTiming * 2);
                    _numberOfPlayerInput = 0;
                    _maxInputReached = false;
                }
            }
        }

        private void ManageTetriminos()
        {/*
            while (_inGame)
            {
                if (_instructionsTetriminos.Count > 0)
                {
                    switch (_instructionsTetriminos[0])
                    {
                        case "down":
                            break;
                        case "left":
                            break;
                        default:
                            break;
                    }
                    _instructionsTetriminos.RemoveAt(0);
                }
            }*/
        }

        private void StartGame()
        {
            _inGame = true;
            //Thread to manage the user inputs
            StartAllThreads();


            //count the frames after the tetriminos touched to bottom to let the player 2 frames of action
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
                        if (_currentTetriminosYPos < PLAY_ZONE_Y_POS + PLAY_ZONE_HEIGHT - TetriminosManager.GetCurrentTetriminosHeight())
                        {
                            TetriminosManager.MoveTetriminos(_currentTetriminosXPos, _currentTetriminosYPos, 0, _moveYCapacity);
                            _playZoneTetriminosYPos += _moveYCapacity;
                            _currentTetriminosYPos += 2;
                        }
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
            _playZoneTetriminosXPos = PLAY_ZONE_WIDTH / 4;
            _playZoneTetriminosYPos = 0;
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
                            TetriminosManager.MoveTetriminos(_currentTetriminosXPos, _currentTetriminosYPos, -1 * _moveXCapacity, 0);
                            _currentTetriminosXPos -= 4;
                            _playZoneTetriminosXPos -= _moveXCapacity;

                        }
                    }
                    else if (_pressedKey.Key == _moveRight)
                    {
                        if (_currentTetriminosXPos + TetriminosManager.GetCurrentTetriminosWidth() * 2 < PLAY_ZONE_X_POS + PLAY_ZONE_WIDTH * 2)
                        {
                            TetriminosManager.MoveTetriminos(_currentTetriminosXPos, _currentTetriminosYPos, 1, 0);
                            _currentTetriminosXPos += 4;
                            _playZoneTetriminosXPos += _moveXCapacity;
                        }
                    }
                    else if (_pressedKey.Key == _moveDown)
                    {
                        if (_currentTetriminosYPos + 1 <= PLAY_ZONE_Y_POS + PLAY_ZONE_HEIGHT - TetriminosManager.GetCurrentTetriminosHeight())
                        {
                            TetriminosManager.MoveTetriminos(_currentTetriminosXPos, _currentTetriminosYPos, 0, 1);
                            _currentTetriminosYPos += 2;
                            _playZoneTetriminosYPos += _moveYCapacity;
                        }

                    }
                    else if (_pressedKey.Key == _rotate)
                    {
                        if (_numberOfPlayerInput < _maxInput)
                        {
                            _numberOfPlayerInput++;
                            TetriminosManager.RotateTetriminos(_currentTetriminosXPos, _currentTetriminosYPos);
                            if (_currentTetriminosXPos + TetriminosManager.GetCurrentTetriminosWidth() * 2 < PLAY_ZONE_X_POS + PLAY_ZONE_WIDTH * 2)
                            {
                                TetriminosManager.DrawTetriminos(_currentTetriminosXPos,_currentTetriminosYPos);
                            }
                            else
                            {
                                TetriminosManager.DrawTetriminos(_currentTetriminosXPos-4, _currentTetriminosYPos);
                                _currentTetriminosXPos -= 4;
                            }
                            
                        }
                        else
                        {
                            _maxInputReached = true;
                        }
                    }

                    _pressedKey = default(ConsoleKeyInfo);
                }

            } while (_inGame);
        }
    }
}
