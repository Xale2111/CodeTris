using System;
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
        const int PLAY_ZONE_HEIGHT = 48;
        const int PLAY_ZONE_X_POS = 60;
        const int PLAY_ZONE_Y_POS = 8;

        const string DOWN_INSTRUCTION = "down";
        const string LEFT_INSTRUCTION = "left";
        const string RIGHT_INSTRUCTION = "right";
        const string ROTATE_INSTRUCTION = "rotate";
        const string NATURAL_DOWN_INSTRUCTION = "naturalDown";

        //console coordonate
        int _currentTetriminosXPos;
        int _currentTetriminosYPos;

        //Bool Aray coordonate
        int _playZoneTetriminosXPos;
        int _playZoneTetriminosYPos;
        
        int _numberOfRightAnswer = 0;
        int _numberOfWrongAnswer = 0;

        bool _inGame = false;
        bool _isPaused = false;
        bool _canSpawnNew = true;
        bool _threadsStarted = false;
        int _frameTiming = 180;
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

        const int EMPTY_CASE_CODE = 0;
        const int FALLING_CASE_CODE = 1;
        const int PLACED_CASE_CODE = 2;
        const int BLOCKED_CASE_CODE = 3;

        string[,] _visualPlayZone = new string[PLAY_ZONE_WIDTH / 2, PLAY_ZONE_HEIGHT / 2]; //Matrix of the playzone visual


        int[,] _playZone = new int[PLAY_ZONE_WIDTH / 2, PLAY_ZONE_HEIGHT / 2]; //values divided by 2 because 1 "block" is 2X2 sized (Oblock is 4 "blocks" together)
                                                                               //0-> empty, 1-> occupied (current tetriminos),
                                                                               //2-> occupied (placed tetriminos),3->Blocked       
                                                                               //Refer to the aboves const
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

        private void PauseGame()
        {
            _inGame = false;
            _instructionsTetriminos.Clear();
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
            _threadsStarted = false;
        }

        private void ResumeGame()
        {
            _isPaused = false;
            Thread.Sleep(1000);
            _inGame = true;
            
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
                Thread.Sleep(25);
                if (_instructionsTetriminos.Count > 0)
                {
                    bool isPlaced = false;
                    switch (_instructionsTetriminos[0])
                    {
                        case DOWN_INSTRUCTION:
                            if (CheckCanMoveInPlayZone(0, 1,true))
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
                                    TetriminosManager.DrawTetriminos(_currentTetriminosXPos, _currentTetriminosYPos);
                                }
                                else
                                {
                                    TetriminosManager.DrawTetriminos(_currentTetriminosXPos - 4, _currentTetriminosYPos);
                                    _currentTetriminosXPos -= 4;
                                    _playZoneTetriminosXPos -= _moveXCapacity;
                                }
                                //TODO : Check if when rotating on the ground, the tetriminos glitch under the ground
                                if (_currentTetriminosYPos+TetriminosManager.GetCurrentTetriminosHeight()<PLAY_ZONE_Y_POS+PLAY_ZONE_HEIGHT)
                                {
                                    TetriminosManager.DrawTetriminos(_currentTetriminosXPos, _currentTetriminosYPos);

                                }
                                else
                                {
                                    TetriminosManager.DrawTetriminos(_currentTetriminosXPos, _currentTetriminosYPos-1);
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
                    if (_instructionsTetriminos.Count() >0)
                    {
                        _instructionsTetriminos.RemoveAt(0);
                    }
                }
            }
        }

        private void StartGame()
        {
            _inGame = true;                        

            //count the frames after the tetriminos touched to bottom to let the player 2 frames of action
            int frameCounter = 0;
            while (_inGame)
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
                    UpdateTetriminosOccupation(false);
                    _instructionsTetriminos.Add(NATURAL_DOWN_INSTRUCTION);
                }

                if (!_threadsStarted)
                {
                    StartAllThreads();
                    _threadsStarted = true;
                }

                //TempDebug();

                //Can spawn new tetrominos ? 
                //--> Yes, spawn the one in the "next piece" case
                //--> No, Move the current one by one
            }
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
                    switch (_visualPlayZone[i,j].ToLower())
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
                        default:
                            numberOfTheColor = 0;
                            break;
                    }
                    if (numberOfTheColor != 0)
                    {
                        Debug.Write("["+numberOfTheColor+"]");
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
            TetriminosManager.DrawTetriminos(_currentTetriminosXPos, _currentTetriminosYPos);
        }

        private void UpdateTetriminosOccupation(bool touchedBottom)
        {
            bool tetriminosIsPlaced = false;
            ClearPlayZoneFromFalling();

            
            for (int j = 0; j < TetriminosManager.GetCurrentTetriminosHeight(); j++)
            {
                for (int i = 0; i < TetriminosManager.GetCurrentTetriminosWidth(); i++)
                {
                    if (TetriminosManager.GetTetriminosOccupation()[i,j])
                    {
                        if (touchedBottom)
                        {
                            _playZone[_playZoneTetriminosXPos + i, _playZoneTetriminosYPos + j] = PLACED_CASE_CODE;
                            tetriminosIsPlaced = true;
                            _instructionsTetriminos.Clear();
                        }
                        else
                        {                        
                            _playZone[_playZoneTetriminosXPos+i, _playZoneTetriminosYPos+j] = FALLING_CASE_CODE;                            
                        }

                        _visualPlayZone[_playZoneTetriminosXPos + i, _playZoneTetriminosYPos + j] = TetriminosManager.GetTetriminosColor();

                    }                    
                }
            }
            if (touchedBottom)
            {
                TempDebugColor();
                List<int> lanes = FindCompletedLane();
                int amountOfLanes = lanes.Count;
                int laneToBlock = 0;
                //ASK Questions (return number of correct answer
                //Manage number of wrong question
                if (lanes.Count > 0)
                {
                    laneToBlock = AskQuestions(amountOfLanes);

                    for (int k = 0; k < amountOfLanes; k++)
                    {
                        RemoveLane(lanes[k]);                        
                    }                    
                    MoveLanesVisually();
                }
                if (laneToBlock>0)
                {
                    //Block Lines
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
        }

        private List<int> FindCompletedLane()
        {
            List<int> completedLanes = new List<int>();
            for (int j = 0; j < PLAY_ZONE_HEIGHT/2; j++)
            {
                bool laneIsCompleted = false;
                for (int i = 0; i < PLAY_ZONE_WIDTH/2; i++)
                {
                    if (_playZone[i,j] == PLACED_CASE_CODE)
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
            //à gérer
            for (int i = 0; i < PLAY_ZONE_WIDTH/2; i++)
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
                        _visualPlayZone[i,j+1] = _visualPlayZone[i,j];
                        _visualPlayZone[i,j] = string.Empty;
                    }
                }                
            }            
        }
        private void MoveLanesVisually()
        {            
            ClearPlayZone();
            RedrawEachTetriminos();
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
            int wrongAnswer = 0;
            PauseGame();
            for (int i = 0; i < numberOfCompletedLines; i++)
            {
                ShowQuestion();
                if (!CheckAnswer(ManagePlayerAnswer()))
                {
                    wrongAnswer++;
                }                
                
                //Ask one question, wait for the result, next question
            }

            return wrongAnswer;
        }

        private void ShowQuestion()
        { 
            //Show Question
        }

        private string ManagePlayerAnswer()
        {
            //do while player didn't pressed ENTER
            do
            {

            } while (true);
        }

        private bool CheckAnswer(string givenAnswer)
        {
            if (givenAnswer == "Good Answer")
            {
                //Answer is correct
                //Draw little cube next to the questio to indicate if the anser is true or not (red or green dot)
                _numberOfRightAnswer++;
                return true;
            }
            else
            {
                _numberOfWrongAnswer++;
                ShowCorrectAnswer();
                return false;
            }
        }

        private void ShowCorrectAnswer()
        {
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

                            if (_playZoneTetriminosYPos + j + wantedYPos < PLAY_ZONE_HEIGHT/2)
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
                                if (_playZoneTetriminosXPos + i + wantedXPos < PLAY_ZONE_WIDTH / 2)
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
            
            //TO DO: Check if next movement will result in the current form (1) to collide with an other form (2 or 3)
            //if no, canMoveToWantedPosition = true and the piece can move
            //else, the piece can move in the wanted direction (canMoveToWantedPosition = false)
            //Check the same for the rotation (Move the piece in the array BUT NOT visually, check, return result?)


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
                _pressedKey = Console.ReadKey(true);                
                //Can't use a switch because the case require to use a constant
                //(currently using a variable incase player modify the wanted playing keys)
                if (_pressedKey.Key == _moveLeft)
                {
                    if (_currentTetriminosXPos-_moveXCapacity*2 > PLAY_ZONE_X_POS)
                    {
                        _instructionsTetriminos.Add(LEFT_INSTRUCTION);
                    }
                }
                else if (_pressedKey.Key == _moveRight)
                {
                    if (_currentTetriminosXPos + _moveXCapacity*2 + TetriminosManager.GetCurrentTetriminosVisualWidth() * 2 < PLAY_ZONE_X_POS + PLAY_ZONE_WIDTH * 2)
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

                _pressedKey = default(ConsoleKeyInfo);


            } while (_inGame);
        }
    }
}
