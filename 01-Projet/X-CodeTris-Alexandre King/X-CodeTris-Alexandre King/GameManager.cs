﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace X_CodeTris_Alexandre_King
{
    public class GameManager
    {
        const int PLAY_ZONE_WIDTH = 32;
        const int PLAY_ZONE_HEIGHT = 52;
        const int PLAY_ZONE_X_POS = 60;
        const int PLAY_ZONE_Y_POS = 8;
        const int NEXT_TETRIMINOS_X_POS = 10;
        const int NEXT_TETRIMINOS_Y_POS = 8;
        const int NEXT_TETRIMINOS_SIZE = 20;
        const int MAX_HEIGHT_BEFORE_GAME_OVER = 10;


        const string DOWN_INSTRUCTION = "down";
        const string LEFT_INSTRUCTION = "left";
        const string RIGHT_INSTRUCTION = "right";
        const string ROTATE_INSTRUCTION = "rotate";
        const string NATURAL_DOWN_INSTRUCTION = "naturalDown";

        const int QUESTION_QUOTE_X_POS = 180;
        const int QUESTION_QUOTE_Y_POS = 12;
        const int QUESTION_ANSWER_X_POS = 180;
        const int MAX_STRING_LENGTH = 50;
        int _questionAnswerPosY = 14;
        int _correctAnswerPosY = 16;

        const int SCORE_X_POS = 10;
        const int SCORE_Y_POS = 28;
        const string SCORE_TEXT = "score : ";
        double _score = 0;
        double _completedLineMultiplycator = 1;
        int _pointsToAdd = 0;

        QuestionsManager questionsManager;

        //console coordonate
        int _currentTetriminosXPos;
        int _currentTetriminosYPos;

        //Bool Aray coordonate
        int _playZoneTetriminosXPos;
        int _playZoneTetriminosYPos;

        int _numberOfRightAnswer = 0;
        int _numberOfWrongAnswer = 0;        
        int _totalBlockedLane = 0;

        bool _inGame = false;
        bool _gameOver = false;
        bool _isPaused = false;
        bool _isUpdating = false;
        bool _canSpawnNew = true;
        bool _threadsStarted = false;
        int _frameTiming;
        int _frameBeforeNew = 2;
        int _moveYCapacity = 1;
        int _moveXCapacity = 1;
        List<string> _instructionsTetriminos = new List<string>();

        List<Thread> _allThreads = new List<Thread>();

        ConsoleKey _moveLeft;
        ConsoleKey _moveRight;
        ConsoleKey _moveDown;
        ConsoleKey _rotate;
        ConsoleKey _dropDown;
        ConsoleKeyInfo _pressedKey;

        int _difficulty = 0;
        const int EASY_FRAME_TIMING = 250;
        const int MEDIUM_FRAME_TIMING = 220;
        const int HARD_FRAME_TIMING = 180;
        const int EASY_ADD_POINTS = 50;
        const int MEDIUM_ADD_POINTS = 100;
        const int HARD_ADD_POINTS = 200;

        const int EMPTY_CASE_CODE = 0;
        const int FALLING_CASE_CODE = 1;
        const int PLACED_CASE_CODE = 2;
        const int BLOCKED_CASE_CODE = 3;

        string[,] _visualPlayZone = new string[PLAY_ZONE_WIDTH / 2, PLAY_ZONE_HEIGHT / 2]; //Matrix of the playzone visual


        int[,] _playZone = new int[PLAY_ZONE_WIDTH / 2 + 1, PLAY_ZONE_HEIGHT / 2 + 1]; //values divided by 2 because 1 "block" is 2X2 sized (Oblock is 4 "blocks" together)
                                                                                       //0-> empty, 1-> occupied (current tetriminos),
                                                                                       //2-> occupied (placed tetriminos),3->Blocked       
                                                                                       //Refer to the aboves const
        public int NewGame()
        {
            Console.Clear();
            ResetAll();
            DrawPlayArea();
            DrawGameOverArea();
            DrawNextTetriminosArea();
            DefinePlayingKeys();
            DefineDifficulty();
            WriteScoreText();
            WriteScore();
            VisualManager.SetBackgroundColor("gray");
            return StartGame();
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

        private void DefineDifficulty()
        {
            _difficulty = MenuManager.GetDifficultyStatus();

            switch (_difficulty)
            {
                case 0:
                    _frameTiming = EASY_FRAME_TIMING;
                    _pointsToAdd = EASY_ADD_POINTS;
                    break;
                case 1:
                    _frameTiming = MEDIUM_FRAME_TIMING;
                    _pointsToAdd = MEDIUM_ADD_POINTS;
                    break;
                case 2:
                    _frameTiming = HARD_FRAME_TIMING;
                    _pointsToAdd = HARD_ADD_POINTS;
                    break;
                default:
                    _frameTiming = EASY_FRAME_TIMING;
                    _pointsToAdd = EASY_ADD_POINTS;
                    break;
            }
            questionsManager = new QuestionsManager(_difficulty);

        }

        private void ResetAll()
        {
            _playZone = new int[PLAY_ZONE_WIDTH / 2 + 1, PLAY_ZONE_HEIGHT / 2 + 1];
            _numberOfRightAnswer = 0;
            _numberOfWrongAnswer = 0;
            _instructionsTetriminos.Clear();            
            _score = 0;
            _gameOver = false;
            _isUpdating = false;
            _inGame = false;
            _isPaused = false;
            _canSpawnNew = true;
            _threadsStarted = false;
            _pressedKey = default(ConsoleKeyInfo);
            TetriminosManager.ResetTetriminos();

        }

        private void DrawNextTetriminosArea()
        {
            int yPos = NEXT_TETRIMINOS_Y_POS;
            for (int j = 0; j < NEXT_TETRIMINOS_SIZE / 2; j++)
            {
                Console.SetCursorPosition(NEXT_TETRIMINOS_X_POS, yPos);
                for (int i = 0; i < NEXT_TETRIMINOS_SIZE; i++)
                {
                    VisualManager.SetBackgroundColor("gray");
                    Console.Write(" ");
                }
                yPos++;
            }
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

            for (int i = 0; i < PLAY_ZONE_HEIGHT / 2 + 1; i++)
            {
                _playZone[PLAY_ZONE_WIDTH / 2, i] = BLOCKED_CASE_CODE;
            }
            for (int i = 0; i < PLAY_ZONE_WIDTH / 2 + 1; i++)
            {
                _playZone[i, PLAY_ZONE_HEIGHT / 2] = BLOCKED_CASE_CODE;
            }
        }

        private void DrawGameOverArea()
        {
            VisualManager.SetBackgroundColor("dkred");
            for (int i = 0; i < PLAY_ZONE_WIDTH*2; i++)
            {
                Console.SetCursorPosition(PLAY_ZONE_X_POS+i, MAX_HEIGHT_BEFORE_GAME_OVER+PLAY_ZONE_Y_POS );
                Console.Write(" ");                
            }
            VisualManager.SetBackgroundColor("gray");
        }

        private void PauseGame()
        {
            _instructionsTetriminos.Clear();
            _isPaused = true;
        }

        private void ResumeGame()
        {
            _isPaused = false;
            Thread.Sleep(1000);

        }

        private void StopAllThreads()
        {
            int totalThreads = _allThreads.Count();
            for (int i = 0; i < totalThreads; i++)
            {
                try
                {
                    _allThreads[0].Abort();
                    _allThreads.RemoveAt(0);
                }
                catch (Exception)
                {
                    _allThreads.Clear();
                }

            }
        }

        private void StartAllThreads()
        {
            Thread playerInput = new Thread(ManagePlayerInput);
            playerInput.Start();
            _allThreads.Add(playerInput);

            Thread tetriminosManagement = new Thread(ManageTetriminos);
            tetriminosManagement.Start();
            _allThreads.Add(tetriminosManagement);
        }

        private void ManageTetriminos()
        {

            while (_inGame)
            {
                if (!_isPaused)
                {
                    //Thread to let a little timing before each instructions (This helps to limit visual bugs)
                    Thread.Sleep(25);                    
                    if (_instructionsTetriminos.Count > 0)
                    {
                        bool isPlaced = false;
                        switch (_instructionsTetriminos[0])
                        {
                            case DOWN_INSTRUCTION:
                                if (CheckCanMoveInPlayZone(0, 1, true))
                                {
                                    TetriminosManager.MoveTetriminos(_currentTetriminosXPos, _currentTetriminosYPos, 0, _moveYCapacity);
                                    _playZoneTetriminosYPos += _moveYCapacity;
                                    _currentTetriminosYPos += 2;
                                }
                                break;
                            case LEFT_INSTRUCTION:
                                if (CheckCanMoveInPlayZone(-1, 0))
                                {
                                    TetriminosManager.MoveTetriminos(_currentTetriminosXPos, _currentTetriminosYPos, -1 * _moveXCapacity, 0);
                                    _currentTetriminosXPos -= 4;
                                    _playZoneTetriminosXPos -= _moveXCapacity;
                                }
                                break;
                            case RIGHT_INSTRUCTION:
                                if (CheckCanMoveInPlayZone(1, 0))
                                {
                                    TetriminosManager.MoveTetriminos(_currentTetriminosXPos, _currentTetriminosYPos, 1, 0);
                                    _currentTetriminosXPos += 4;
                                    _playZoneTetriminosXPos += _moveXCapacity;
                                }
                                break;
                            case ROTATE_INSTRUCTION:
                                if (CheckCanRotateInPlayZone())
                                {
                                    TetriminosManager.RotateTetriminos(_currentTetriminosXPos, _currentTetriminosYPos);
                                    if (_currentTetriminosXPos + TetriminosManager.GetCurrentTetriminosVisualWidth() * 2 < PLAY_ZONE_X_POS + PLAY_ZONE_WIDTH * 2)
                                    {
                                        TetriminosManager.DrawCurrentTetriminos(_currentTetriminosXPos, _currentTetriminosYPos);
                                    }
                                    else
                                    {
                                        TetriminosManager.DrawCurrentTetriminos(_currentTetriminosXPos - 4, _currentTetriminosYPos);
                                        _currentTetriminosXPos -= 4;
                                        _playZoneTetriminosXPos -= _moveXCapacity;
                                    }
                                    //TODO : Check if when rotating on the ground, the tetriminos glitch under the ground
                                    if (_currentTetriminosYPos + TetriminosManager.GetCurrentTetriminosHeight() < PLAY_ZONE_Y_POS + PLAY_ZONE_HEIGHT)
                                    {
                                        TetriminosManager.DrawCurrentTetriminos(_currentTetriminosXPos, _currentTetriminosYPos);

                                    }
                                    else
                                    {
                                        TetriminosManager.DrawCurrentTetriminos(_currentTetriminosXPos, _currentTetriminosYPos - 1);
                                        _currentTetriminosYPos -= 1;
                                        _playZoneTetriminosXPos -= _moveXCapacity;
                                    }
                                }
                                break;
                            case NATURAL_DOWN_INSTRUCTION:
                                if (CheckCanMoveInPlayZone(0, 1))
                                {
                                    TetriminosManager.MoveTetriminos(_currentTetriminosXPos, _currentTetriminosYPos, 0, _moveYCapacity);
                                    _playZoneTetriminosYPos += _moveYCapacity;
                                    _currentTetriminosYPos += 2;
                                }
                                else
                                {
                                    isPlaced = true;
                                }
                                break;
                            default:
                                break;
                        }
                        UpdateTetriminosOccupation(isPlaced);
                        //Check again before removing if there is more then 1 element (Using a Thread is the problem)
                        if (_instructionsTetriminos.Count() > 0)
                        {
                            _instructionsTetriminos.RemoveAt(0);
                        }
                        if (_currentTetriminosYPos>MAX_HEIGHT_BEFORE_GAME_OVER+PLAY_ZONE_Y_POS)
                        {
                            DrawGameOverArea();
                        }
                    }
                }
            }
        }

        private void CallGameOverScreen()
        {
            int yPosTest = 1;
            string backMenu = "Appuyez sur une touche pour retourner au menu principal.";
            string finalScoreText = "Votre score final est de " + _score + " points";
            string questionsResult = "Vous avez répondu juste à " + _numberOfRightAnswer + " questions et faux à " + _numberOfWrongAnswer + " questions";
            string finalPhrase = "N'hésitez pas à rejouer pour améliorer votre score !";
            string tip = "Conseil : remplir plus de ligne à la fois vous donne plus de points";
            string[] allPhrases = new string[4] { 
            finalScoreText,questionsResult,finalPhrase,tip};
            
            VisualManager.SetTextColor("white");
            VisualManager.SetBackgroundColor("black");
            Console.Clear();
            VisualManager.AddVisualToMainMenu();

            foreach (string phrase in allPhrases)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - phrase.Length / 2, Console.WindowHeight / 2 + yPosTest);
                Console.Write(phrase);
                yPosTest+=2;
            }                        

            DatabaseManager.StockGame(_score, MenuManager.GetDifficultyStatus()+1);

            Console.SetCursorPosition(Console.WindowWidth / 2 - backMenu.Length / 2, Console.WindowHeight / 2 + yPosTest+3);
            Console.Write(backMenu);

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo != null)
            {
                Console.Clear();                
            }
        }

        private int StartGame()
        {
            _inGame = true;
            //count the frames after the tetriminos touched to bottom to let the player 2 frames of action
            int frameCounter = 0;
            while (_inGame)
            {

                if (_gameOver)
                {                    
                    StopAllThreads();
                    PauseGame();
                    CallGameOverScreen();
                    return 0;
                }

                if (!_isPaused)
                {
                    Thread.Sleep(_frameTiming);

                    if (TetriminosManager.HasACurrentTetriminos())
                    {
                        if (_currentTetriminosYPos < PLAY_ZONE_Y_POS + PLAY_ZONE_HEIGHT - TetriminosManager.GetCurrentTetriminosVisualHeight())
                        {
                            _instructionsTetriminos.Add(NATURAL_DOWN_INSTRUCTION);
                        }
                        else
                        {
                            frameCounter++;
                            if (frameCounter > _frameBeforeNew)
                            {
                                UpdateTetriminosOccupation(true);
                                frameCounter = 0;
                            }
                        }
                    }
                    if (_canSpawnNew)
                    {
                        _canSpawnNew = false;
                        _instructionsTetriminos.Clear();
                        TetriminosManager.DefineNewTetriminos();
                        DrawNewTetriminos();
                        DrawNextTetriminos();
                        UpdateTetriminosOccupation(false);
                        _instructionsTetriminos.Add(NATURAL_DOWN_INSTRUCTION);
                    }

                    if (!_threadsStarted)
                    {
                        StartAllThreads();
                        _threadsStarted = true;
                    }
                }
            }
            return -1;

        }

        private void TempDebug()
        {
            for (int j = 0; j < PLAY_ZONE_HEIGHT / 2; j++)
            {
                for (int i = 0; i < PLAY_ZONE_WIDTH / 2; i++)
                {

                    if (_playZone[i, j] == 1 || _playZone[i, j] == 2 || _playZone[i, j] == 3)
                    {
                        Debug.Write("[" + _playZone[i, j] + "]");
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

        private void TempDebugColor()
        {
            for (int j = 0; j < PLAY_ZONE_HEIGHT / 2; j++)
            {
                for (int i = 0; i < PLAY_ZONE_WIDTH / 2; i++)
                {
                    if (_visualPlayZone[i, j] == null)
                    {
                        _visualPlayZone[i, j] = string.Empty;
                    }
                    int numberOfTheColor = 0;
                    switch (_visualPlayZone[i, j].ToLower())
                    {
                        case "green":
                            numberOfTheColor = 1;
                            break;
                        case "red":
                            numberOfTheColor = 2;
                            break;
                        case "magenta":
                            numberOfTheColor = 3;
                            break;
                        case "dkyellow":
                            numberOfTheColor = 4;
                            break;
                        case "cyan":
                            numberOfTheColor = 5;
                            break;
                        case "yellow":
                            numberOfTheColor = 6;
                            break;
                        case "blue":
                            numberOfTheColor = 7;
                            break;
                        case "dkgray":
                            numberOfTheColor = 8;
                            break;
                        default:
                            numberOfTheColor = 0;
                            break;
                    }
                    if (numberOfTheColor != 0)
                    {
                        Debug.Write("[" + numberOfTheColor + "]");
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

        private void DrawNewTetriminos()
        {
            int xStartPosition = PLAY_ZONE_X_POS + PLAY_ZONE_WIDTH;
            _currentTetriminosXPos = xStartPosition;
            _currentTetriminosYPos = PLAY_ZONE_Y_POS;
            _playZoneTetriminosXPos = PLAY_ZONE_WIDTH / 4;
            _playZoneTetriminosYPos = 0;
            TetriminosManager.DrawCurrentTetriminos(_currentTetriminosXPos, _currentTetriminosYPos);
        }

        private void DrawNextTetriminos()
        {
            int yPos = NEXT_TETRIMINOS_Y_POS;
            for (int j = 0; j < NEXT_TETRIMINOS_SIZE / 2; j++)
            {
                Console.SetCursorPosition(NEXT_TETRIMINOS_X_POS, yPos);
                for (int i = 0; i < NEXT_TETRIMINOS_SIZE; i++)
                {
                    Console.Write(" ");
                }
                yPos++;
            }
            TetriminosManager.DrawNextTetriminos(NEXT_TETRIMINOS_X_POS + NEXT_TETRIMINOS_SIZE / 2 - TetriminosManager.GetNextTetriminosWidth(), NEXT_TETRIMINOS_Y_POS + NEXT_TETRIMINOS_SIZE / 4 - TetriminosManager.GetNextTetriminosHeight() / 2);
        }

        private void UpdateTetriminosOccupation(bool touchedBottom)
        {
            if (!_isUpdating)
            {
                _isUpdating = true;
                bool tetriminosIsPlaced = false;
                ClearPlayZoneFromFalling();


                for (int j = 0; j < TetriminosManager.GetCurrentTetriminosHeight(); j++)
                {
                    for (int i = 0; i < TetriminosManager.GetCurrentTetriminosWidth(); i++)
                    {
                        try
                        {
                            if (TetriminosManager.GetTetriminosOccupation()[i, j])
                            {
                                if (touchedBottom)
                                {
                                    _playZone[_playZoneTetriminosXPos + i, _playZoneTetriminosYPos + j] = PLACED_CASE_CODE;
                                    tetriminosIsPlaced = true;
                                    _instructionsTetriminos.Clear();
                                    if (_currentTetriminosYPos<MAX_HEIGHT_BEFORE_GAME_OVER+PLAY_ZONE_Y_POS)
                                    {
                                        SoundManager.StopMusic();
                                        _gameOver = true;
                                    }
                                }
                                else
                                {
                                    _playZone[_playZoneTetriminosXPos + i, _playZoneTetriminosYPos + j] = FALLING_CASE_CODE;
                                }

                                _visualPlayZone[_playZoneTetriminosXPos + i, _playZoneTetriminosYPos + j] = TetriminosManager.GetTetriminosColor();

                            }
                        }
                        catch (Exception e)
                        {
                            Debug.Print(e.Message);
                            //Reverse Rotation                        
                        }
                    }

                }
                if (touchedBottom)
                {
                    TempDebug();
                    List<int> lanes = FindCompletedLane();
                    int amountOfLanes = lanes.Count;
                    int laneToBlock = 0;
                    //ASK Questions (return number of correct answer
                    //Manage number of wrong question
                    if (lanes.Count > 0)
                    {
                        PauseGame();
                        laneToBlock = AskQuestions(amountOfLanes);
                        if (laneToBlock > 0)
                        {
                            for (int i = 0; i < laneToBlock; i++)
                            {
                                BlockLane();
                            }
                        }
                        for (int k = 0; k < amountOfLanes; k++)
                        {
                            RemoveLane(lanes[k]);
                        }
                        MoveLanesVisually();
                        TempDebug();
                        ResumeGame();
                    }
                }
                if (tetriminosIsPlaced)
                {
                    _canSpawnNew = true;
                }
                else
                {
                    _canSpawnNew = false;
                }
                _isUpdating = false;
            }
        }

        private List<int> FindCompletedLane()
        {
            List<int> completedLanes = new List<int>();
            for (int j = 0; j < PLAY_ZONE_HEIGHT / 2; j++)
            {
                bool laneIsCompleted = false;
                for (int i = 0; i < PLAY_ZONE_WIDTH / 2; i++)
                {
                    if (_playZone[i, j] == PLACED_CASE_CODE)
                    {
                        laneIsCompleted = true;
                    }
                    else
                    {
                        laneIsCompleted = false;
                        break;
                    }
                }
                if (laneIsCompleted)
                {
                    completedLanes.Add(j);
                }
            }
            return completedLanes;
        }

        private void RemoveLane(int fullLane)
        {
            for (int i = 0; i < PLAY_ZONE_WIDTH / 2; i++)
            {
                _playZone[i, fullLane] = EMPTY_CASE_CODE;
                _visualPlayZone[i, fullLane] = string.Empty;
            }
            for (int j = fullLane - 1; j > 0; j--)
            {
                for (int i = 0; i < PLAY_ZONE_WIDTH / 2; i++)
                {
                    if (_playZone[i, j] == PLACED_CASE_CODE && _playZone[i, j + 1] != BLOCKED_CASE_CODE)
                    {
                        _playZone[i, j + 1] = _playZone[i, j];
                        _playZone[i, j] = EMPTY_CASE_CODE;
                        _visualPlayZone[i, j + 1] = _visualPlayZone[i, j];
                        _visualPlayZone[i, j] = string.Empty;
                    }
                }
            }
        }
        private void MoveLanesVisually()
        {
            ClearPlayZone();
            RedrawEachTetriminos();
        }

        private int FindLatestBlockedLane()
        {
            int lastBlockLane = PLAY_ZONE_HEIGHT / 2;
            _totalBlockedLane++;
            for (int i = 0; i < PLAY_ZONE_HEIGHT / 2; i++)
            {
                if (_playZone[0, i] == BLOCKED_CASE_CODE)
                {
                    lastBlockLane = i;
                    break;
                }
            }

            return lastBlockLane;
        }
        private void BlockLane()
        {
            int lastBlockLane = FindLatestBlockedLane();

            for (int j = 0; j < _totalBlockedLane; j++)
            {
                for (int i = 0; i < PLAY_ZONE_WIDTH / 2; i++)
                {
                    _playZone[i, lastBlockLane - 1+j] = BLOCKED_CASE_CODE;
                    _visualPlayZone[i, lastBlockLane - 1+j] = "dkgray";
                }
            }
            VisuallyBlockLine(lastBlockLane + 1);
        }
        private void VisuallyBlockLine(int laneToBlock)
        {
            for (int j = 0; j < 2; j++)
            {
                Console.SetCursorPosition(PLAY_ZONE_X_POS,PLAY_ZONE_Y_POS/2+laneToBlock*2+j);
                for (int i = 0; i < PLAY_ZONE_WIDTH; i++)
                {
                    VisualManager.SetTextColor("DKgray");
                    Console.Write("██");
                }
            }
        }

        private void RedrawEachTetriminos()
        {
            for (int j = 0; j < PLAY_ZONE_HEIGHT/2; j++)
            {
                for (int i = 0; i < PLAY_ZONE_WIDTH /2; i++)
                {
                    if (_visualPlayZone[i, j] != null && _visualPlayZone[i, j] != string.Empty)
                    {
                        VisualManager.SetTextColor(_visualPlayZone[i,j]);
                        Console.SetCursorPosition(PLAY_ZONE_X_POS + i*4, PLAY_ZONE_Y_POS + j*2);
                        Console.Write("████");
                        Console.SetCursorPosition(PLAY_ZONE_X_POS + i*4, PLAY_ZONE_Y_POS + j*2+1);
                        Console.Write("████");
                    }
                }
            }
        }

        private void ClearPlayZone()
        {
            for (int j = 0; j < PLAY_ZONE_HEIGHT; j++)
            {
                for (int i = 0; i < PLAY_ZONE_WIDTH*2; i++)
                {
                    Console.SetCursorPosition(PLAY_ZONE_X_POS+i, PLAY_ZONE_Y_POS+j);
                    Console.Write(" ");
                }
            }
        }

        private int AskQuestions(int numberOfCompletedLines)
        {
            if (MenuManager.GetSoundStatus())
            {
                SoundManager.PlaySuspenseSong();
            }
            switch (numberOfCompletedLines)
            {
                case 2:
                    _completedLineMultiplycator = 1.25;
                    break;
                case 3:
                    _completedLineMultiplycator = 1.5;
                    break;
                case 4:
                    _completedLineMultiplycator = 2;
                    break;
                default:
                    _completedLineMultiplycator = 1;
                    break;
            }

            Console.CursorVisible = true;
            VisualManager.SetTextColor("white");
            VisualManager.SetBackgroundColor("black");
            int wrongAnswer = 0;            
            for (int i = 0; i < numberOfCompletedLines; i++)
            {                
                Question currentQuestion = questionsManager.GetRandomQuestion();
                ShowQuestion(currentQuestion.Quote);
                string playerAnswer = ManagePlayerAnswer();
                if (!CheckAnswer(playerAnswer, currentQuestion.Answer))
                {
                    ShowQuestion(currentQuestion.Quote, "red");
                    wrongAnswer++;                    
                }
                else
                {
                    ShowQuestion(currentQuestion.Quote, "green");
                }
                WriteScore();
                Thread.Sleep(2000);

                ClearQuestionZone();                
                _questionAnswerPosY = QUESTION_QUOTE_Y_POS + 2;
                _correctAnswerPosY = _questionAnswerPosY + 2;
                //Ask one question, wait for the result, next question
            }
            Console.CursorVisible = false;
            VisualManager.SetBackgroundColor("gray");
            if (MenuManager.GetSoundStatus())
            {
                SoundManager.PlayTetrisThemeSong();
            }
            return wrongAnswer;
        }

        private void ClearQuestionZone()
        {
            VisualManager.SetBackgroundColor("black");
            for (int j = 0; j < _correctAnswerPosY+10; j++)
            {
                Console.SetCursorPosition(QUESTION_QUOTE_X_POS,QUESTION_QUOTE_Y_POS+j);
                for (int i = 0; i < MAX_STRING_LENGTH+1; i++)
                {
                    Console.Write(" ");
                }
            }
        }

        private void ShowQuestion(string quote, string textColor = "white")
        {
            VisualManager.SetTextColor(textColor);
            string[] seperatedQuote = quote.Split(' ');
            int numberOfSpaces = seperatedQuote.Count();
            string partOfQuote = string.Empty;            
            int count = 0;
            for (int i = 0; i < numberOfSpaces; i++)
            {
                if (partOfQuote.Length + seperatedQuote[i].Length+1 <MAX_STRING_LENGTH)
                {
                    partOfQuote += seperatedQuote[i] + " ";
                }
                else
                {
                    Console.SetCursorPosition(QUESTION_QUOTE_X_POS, QUESTION_QUOTE_Y_POS+count);
                    Console.Write(partOfQuote);
                    count++;
                    _questionAnswerPosY++;
                    partOfQuote = seperatedQuote[i] +" ";
                }
            }
            Console.SetCursorPosition(QUESTION_QUOTE_X_POS, QUESTION_QUOTE_Y_POS + count);
            Console.Write(partOfQuote);      
            VisualManager.SetTextColor("white");
        }

        private string ManagePlayerAnswer()
        {
            string playerAnswer = string.Empty;
            int moveDown = 1;            
            //do while player didn't pressed ENTER
            Console.SetCursorPosition(QUESTION_ANSWER_X_POS, _questionAnswerPosY);
            do
            {
                ConsoleKeyInfo keyInfo;
                keyInfo = Console.ReadKey();
                if (playerAnswer.Length%MAX_STRING_LENGTH==0 && playerAnswer.Length != 0)
                {                    
                    Console.SetCursorPosition(QUESTION_ANSWER_X_POS, _questionAnswerPosY);
                    _correctAnswerPosY++;
                    moveDown++;
                }
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Spacebar:
                        playerAnswer += " ";
                        break;
                    case ConsoleKey.Backspace:
                        if (playerAnswer.Length > 0)
                        {
                            Console.Write(" ");
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            playerAnswer = playerAnswer.Remove(playerAnswer.Length - 1, 1);
                        }
                        else
                        {                            
                            Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                        }
                        break;
                    case ConsoleKey.Enter:
                        return playerAnswer;                                                                

                    default:
                        if (keyInfo.KeyChar.ToString() != "\0")
                        {
                            playerAnswer += keyInfo.KeyChar;                            
                        }
                        else
                        {
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        }

                        break;

                }

            } while (true);
        }

        private bool CheckAnswer(string givenAnswer,string correctAnswer)
        {
            decimal margin = correctAnswer.Length / 12;
            int tolerance = Convert.ToInt32(Math.Floor(margin)*2);
            if (LevenshteinDistance.GetDistance(givenAnswer.ToLower(),correctAnswer.ToLower())<=tolerance)
            {
                //Show green dot                 
                _numberOfRightAnswer++;
                _score += _pointsToAdd * _completedLineMultiplycator;
                return true;
            }            
            else
            {
                //Show Red Dot                
                _numberOfWrongAnswer++;
                ShowCorrectAnswer(correctAnswer);
                return false;
            }
        }        
        private void ShowCorrectAnswer(string correctAnswer)
        {
            string[] seperatedAnswer = correctAnswer.Split(' ');
            int numberOfSpaces = seperatedAnswer.Count();
            string partOfAnswer = string.Empty;
            int count = 1;
            Console.SetCursorPosition(QUESTION_QUOTE_X_POS, _correctAnswerPosY);
            Console.WriteLine("La bonne réponse était :");

            for (int i = 0; i < numberOfSpaces; i++)
            {
                if (partOfAnswer.Length + seperatedAnswer[i].Length + 1 < MAX_STRING_LENGTH)
                {
                    partOfAnswer += seperatedAnswer[i] + " ";
                }
                else
                {
                    Console.SetCursorPosition(QUESTION_QUOTE_X_POS, _correctAnswerPosY + count);
                    Console.Write(partOfAnswer);
                    count++;                    
                    partOfAnswer = seperatedAnswer[i] + " ";
                }
            }
            Console.SetCursorPosition(QUESTION_QUOTE_X_POS, _correctAnswerPosY + count);
            Console.Write(partOfAnswer);
            //Write underneath the given answer the correct one
        }


        private void ClearPlayZoneFromFalling()
        {
            //TODO : A Optimiser si possible
            for (int j = 0; j < PLAY_ZONE_HEIGHT/2; j++)
            {
                for (int i = 0; i < PLAY_ZONE_WIDTH/2; i++)
                {
                    if (_playZone[i,j] == 1)
                    {
                        _playZone[i, j] = 0;
                        _visualPlayZone[i, j] = string.Empty;
                    }
                }
            }
        }

        private void WriteScoreText()
        {
            Console.SetCursorPosition(SCORE_X_POS,SCORE_Y_POS);
            VisualManager.SetTextColor("white");
            VisualManager.SetBackgroundColor("black");
            Console.Write(SCORE_TEXT);            
        }

        private void WriteScore()
        {
            Console.SetCursorPosition(SCORE_X_POS+SCORE_TEXT.Length, SCORE_Y_POS);
            VisualManager.SetTextColor("white");
            VisualManager.SetBackgroundColor("black");
            Console.Write(_score);
            VisualManager.SetBackgroundColor("gray");
        }

        private bool CheckCanMoveInPlayZone(int wantedXPos, int wantedYPos, bool downInstruction = false)
        {
            bool canMoveToWantedPosition = false;
            int countTrueStatement = 0;

            int checkYPosValue = wantedYPos;
            if (downInstruction)
            {
                checkYPosValue = wantedYPos+1;
            }

            for (int j = 0; j < TetriminosManager.GetCurrentTetriminosHeight(); j++)
            {
                for (int i = 0; i < TetriminosManager.GetCurrentTetriminosWidth(); i++)
                {
                    if (TetriminosManager.GetTetriminosOccupation()[i,j])
                    {                        
                        if (countTrueStatement == 4)
                        {
                            canMoveToWantedPosition = true;
                            return canMoveToWantedPosition;
                        }
                        if (wantedXPos == 0)
                        {

                            if (_playZoneTetriminosYPos + j + wantedYPos < PLAY_ZONE_HEIGHT/2+1)
                            {
                                if (_playZone[_playZoneTetriminosXPos + i, _playZoneTetriminosYPos + j + wantedYPos] == PLACED_CASE_CODE
                                    || _playZone[_playZoneTetriminosXPos + i, _playZoneTetriminosYPos + j + wantedYPos] == BLOCKED_CASE_CODE)
                                {
                                    canMoveToWantedPosition = false;
                                    return canMoveToWantedPosition;
                                }
                                else
                                {
                                    canMoveToWantedPosition = true;
                                    countTrueStatement++;
                                }
                            }
                        }
                        else
                        {
                            if (wantedXPos == -1)
                            {
                                if (_playZoneTetriminosXPos - i + wantedXPos >= 0)
                                {
                                    if (_playZone[_playZoneTetriminosXPos + i + wantedXPos, _playZoneTetriminosYPos + j] == PLACED_CASE_CODE
                                        || _playZone[_playZoneTetriminosXPos + i + wantedXPos, _playZoneTetriminosYPos + j] == BLOCKED_CASE_CODE)
                                    {
                                        canMoveToWantedPosition = false;
                                        return canMoveToWantedPosition;
                                    }
                                    else
                                    {
                                        canMoveToWantedPosition = true;
                                    }
                                }
                            }
                            else
                            {
                                if (_playZoneTetriminosXPos + i + wantedXPos < PLAY_ZONE_WIDTH / 2+1)
                                {
                                    if (_playZone[_playZoneTetriminosXPos + i + wantedXPos, _playZoneTetriminosYPos + j] == PLACED_CASE_CODE
                                       || _playZone[_playZoneTetriminosXPos + i + wantedXPos, _playZoneTetriminosYPos + j] == BLOCKED_CASE_CODE)
                                    {
                                        canMoveToWantedPosition = false;
                                        return canMoveToWantedPosition;
                                    }
                                    else
                                    {
                                        canMoveToWantedPosition = true;
                                    }
                                }
                            }
                        }
                    }
                }

            }                        
            return canMoveToWantedPosition;
        }

        private bool CheckCanRotateInPlayZone()
        {
            bool canMoveToWantedPosition = false;

            for (int j = 0; j < TetriminosManager.GetCurrentTetriminosHeight(); j++)
            {
                for (int i = 0; i < TetriminosManager.GetCurrentTetriminosWidth(); i++)
                {
                    if (TetriminosManager.GetTetriminosRotationOccupation()[i, j])
                    {
                        if (_playZone[_playZoneTetriminosXPos, _playZoneTetriminosYPos] == PLACED_CASE_CODE
                            || _playZone[_playZoneTetriminosXPos, _playZoneTetriminosYPos] == BLOCKED_CASE_CODE)
                        {
                            canMoveToWantedPosition = false;
                        }
                        else
                        {
                            canMoveToWantedPosition = true;
                        }
                    }
                }
            }
            //TO DO: Check if next movement will result in the current form (1) to collided with an other form (2 or 3)
            //if no, canMoveToWantedPosition = true and the piece can move
            //else, the piece can move in the wanted direction (canMoveToWantedPosition = false)
            //Check the same for the rotation (Move the piece in the array BUT NOT visually, check, return result?)


            return canMoveToWantedPosition;
        }



        private void ManagePlayerInput()
        {
            do
            {
                if (!_isPaused)
                {


                    _pressedKey = Console.ReadKey(true);
                    //Can't use a switch because the case require to use a constant
                    //(currently using a variable incase player modify the wanted playing keys)
                    if (_pressedKey.Key == _moveLeft)
                    {
                        if (_currentTetriminosXPos - _moveXCapacity * 2 > PLAY_ZONE_X_POS)
                        {
                            _instructionsTetriminos.Add(LEFT_INSTRUCTION);
                        }
                    }
                    else if (_pressedKey.Key == _moveRight)
                    {
                        if (_currentTetriminosXPos + _moveXCapacity * 2 + TetriminosManager.GetCurrentTetriminosVisualWidth() * 2 < PLAY_ZONE_X_POS + PLAY_ZONE_WIDTH * 2)
                        {
                            _instructionsTetriminos.Add(RIGHT_INSTRUCTION);
                        }
                    }
                    else if (_pressedKey.Key == _moveDown)
                    {
                        if (_currentTetriminosYPos + _moveYCapacity < PLAY_ZONE_Y_POS + PLAY_ZONE_HEIGHT - TetriminosManager.GetCurrentTetriminosVisualHeight())
                        {
                            _instructionsTetriminos.Add(DOWN_INSTRUCTION);
                        }

                    }
                    else if (_pressedKey.Key == _rotate)
                    {
                        _instructionsTetriminos.Add(ROTATE_INSTRUCTION);
                    }
                    else if (_pressedKey.Key == _dropDown)
                    {
                        for (int i = 0; i < PLAY_ZONE_HEIGHT/2 - _playZoneTetriminosYPos; i++)
                        {
                            _instructionsTetriminos.Add(DOWN_INSTRUCTION);
                        }
                    }

                    _pressedKey = default(ConsoleKeyInfo);
                }
            } while (_inGame);
        }
    }
}
