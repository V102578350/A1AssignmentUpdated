using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwinGameSDK;
using RobotNavigationAssignment;

namespace MyGame.src
{
    public class MazeHandler
    {
        private Maze _maze;
        private MazeState _curState;

        private Direction[] _path;
        private bool _traversing; //is true when stepping through directions
        public int index; //used to iterator states in path

        public bool VariableMoveCost; // cost of moving up, left, down, right varies (up = 4, right and left = 2 and down = 1)
        public bool IterationMode; // when Stepping through the path, show the tree search perspecitve (each iteration)
        public bool PlaySound;
        public bool ShowCoords;
        

        public MazeHandler(Maze aMaze)
        {
            _maze = aMaze;
            _curState = aMaze.StartState;
            _traversing = false;
            index = 0;

            VariableMoveCost = false;
            IterationMode = false;
            PlaySound = true;
            ShowCoords = false;

        }

        public Maze GetMaze
        {
            get { return _maze; }
        }

        public MazeState CurrentState
        {
            get { return _curState; }
            set { _curState = value; }
        }
       

        public Direction[] Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public int IterationCount
        {
            get { return GetMaze.IterationCount; }
        }

        public bool Traversing
        {
            get { return _traversing; }
            set { _traversing = value; }
        }

        public int[][] GetGoalStates
        {
            get
            {
                int[][] goalStatePositions = new int[GetMaze.GoalStates.Length][];
                int index = 0;
                foreach (MazeState s in GetMaze.GoalStates)
                {
                    goalStatePositions[index++] = s.FindPosition();
                }

                return goalStatePositions;
            }
        }

        public string GetPathString()
        {
            string pathString = "Path:";
            if(Path != null)
            {
                foreach(Direction movement in Path)
                {
                    pathString += $" {movement}";
                }

                return pathString;
            }

            return pathString + " Select a search method.";
            
        }

        public void Step()
        {
            int limit = 0;
            if(Traversing)
            {
                if(!IterationMode)
                {
                    CurrentState = CurrentState.Move(Path[index]);
                    limit = Path.Length;
                } else
                {
                    CurrentState = GetMaze.Iterations[index];
                    limit = GetMaze.IterationCount;
                }

                Direction lastMove;

                if (!IterationMode)
                {
                    lastMove = Path[index];
                    //create colour path
                    int[] curPos = CurrentState.FindPosition();
                    switch (lastMove)
                    {
                        case Direction.Up:
                            CurrentState.State[curPos[0], curPos[1] + 1] = 3;
                            break;
                        case Direction.Down:
                            CurrentState.State[curPos[0], curPos[1] - 1] = 3;
                            break;
                        case Direction.Left:
                            CurrentState.State[curPos[0] + 1, curPos[1]] = 3;
                            break;
                        default: //right
                            CurrentState.State[curPos[0] - 1, curPos[1]] = 3;
                            break;
                    }
                }
                

                index++;

                // Pop sound effect for added fun
                if(PlaySound) SwinGame.PlaySoundEffect("pop_effect.wav");

                if (index == limit)
                {
                    index = 0;
                    Traversing = false;
                }
            }

        }

        public void Reset()
        {
            CurrentState = GetMaze.StartState;
        }

        public void CheckAction()
        {
            if(!Traversing)
            {
                if (SwinGame.PointInRect(SwinGame.MouseX(), SwinGame.MouseY(), 560, 50, 230, 30))
                {
                    Console.WriteLine("Depth-First Search");
                    Reset();
                    Path = GetMaze.ExecuteMethod("DFS");
                    Traversing = true;

                }
                else if (SwinGame.PointInRect(SwinGame.MouseX(), SwinGame.MouseY(), 560, 90, 230, 30))
                {
                    Console.WriteLine("Breadth-First Search");
                    Reset();
                    Path = GetMaze.ExecuteMethod("BFS");
                    Traversing = true;
                }
                else if (SwinGame.PointInRect(SwinGame.MouseX(), SwinGame.MouseY(), 560, 130, 230, 30))
                {
                    Console.WriteLine("Greedy-Best First Search");
                    Reset();
                    Path = GetMaze.ExecuteMethod("GBFS");
                    Traversing = true;
                }
                else if (SwinGame.PointInRect(SwinGame.MouseX(), SwinGame.MouseY(), 560, 170, 230, 30))
                {
                    Console.WriteLine("A* Search");
                    Reset();
                    Path = GetMaze.ExecuteMethod("AS");
                    Traversing = true;
                }
                else if (SwinGame.PointInRect(SwinGame.MouseX(), SwinGame.MouseY(), 560, 210, 230, 30))
                {
                    Console.WriteLine("Uniform-cost Search");
                    Reset();
                    Path = GetMaze.ExecuteMethod("CUS1");
                    Traversing = true;
                }
            }
        }

        //maze navigation options
        // 1) Variable move costs
        // 2) Iteration mode
        public void CheckOption()
        {
            if(!Traversing)
            {
                //Activate - Disable Variable Costing Traversal
                if (SwinGame.KeyDown(KeyCode.vk_c))
                {
                    if (!VariableMoveCost)
                    {
                        VariableMoveCost = true;
                        SearchMethod.VariableCost = VariableMoveCost;
                    }
                    else
                    {
                        VariableMoveCost = false;
                        SearchMethod.VariableCost = VariableMoveCost;
                    }

                }

                //Activate - Disable iteration mode
                if (SwinGame.KeyDown(KeyCode.vk_v))
                {
                    if (!IterationMode)
                    {
                        IterationMode = true;
                    }
                    else
                    {
                        IterationMode = false;
                    }
                }
            }

            //Activate - Disable popping noise
            if (SwinGame.KeyDown(KeyCode.vk_m))
            {
                if (!PlaySound)
                {
                    PlaySound = true;
                }
                else
                {
                    PlaySound = false;
                }
            }

            //Activate - Disable Showing Coordinates
            if (SwinGame.KeyDown(KeyCode.vk_b))
            {
                if (!ShowCoords)
                {
                    ShowCoords = true;
                }
                else
                {
                    ShowCoords = false;
                }
            }

        }

        //method for drawing maze to the screen
        public void Draw()
        {          
            //draw buttons
            SwinGame.DrawText("Select a Search Method:", Color.Black, 560, 25);
            SwinGame.DrawLine(Color.Black, 560, 35, 790, 35);

            SwinGame.DrawText("Depth-First Search", Color.Black, 600, 60);
            SwinGame.DrawRectangle(Color.Black, 560, 50, 230, 30);

            SwinGame.DrawText("Breadth-First Search", Color.Black, 600, 100);
            SwinGame.DrawRectangle(Color.Black, 560, 90, 230, 30);

            SwinGame.DrawText("Greedy-Best First Search", Color.Black, 580, 140);
            SwinGame.DrawRectangle(Color.Black, 560, 130, 230, 30);

            SwinGame.DrawText("A* Search", Color.Black, 640, 180);
            SwinGame.DrawRectangle(Color.Black, 560, 170, 230, 30);

            SwinGame.DrawText("Uniform-cost Search*", Color.Black, 600, 220);
            SwinGame.DrawRectangle(Color.Black, 560, 210, 230, 30);

            SwinGame.DrawText("Options:", Color.Black, 560, 290);
            SwinGame.DrawLine(Color.Black, 560, 300, 790, 300);

            Color optionClr = Color.Red;

            if (VariableMoveCost) optionClr = Color.Green;
            else optionClr = Color.Red;

            SwinGame.DrawText("C = Variable Move Costs", optionClr, 560, 305);

            if (IterationMode) optionClr = Color.Green;
            else optionClr = Color.Red;

            SwinGame.DrawText("V = Iteration Mode", optionClr, 560, 315);

            if (PlaySound) optionClr = Color.Green;
            else optionClr = Color.Red; 
            SwinGame.DrawText("M = Play Popping Noise :)", optionClr, 560, 325);

            if (ShowCoords) optionClr = Color.Green;
            else optionClr = Color.Red;

            SwinGame.DrawText("B = Display Coordinates", optionClr, 560, 335);

            //draw background
            int width = CurrentState.State.GetLength(0) * 48;
            int height = CurrentState.State.GetLength(1) * 48;

            SwinGame.FillRectangle(Color.Black, 22, 22, width, height);
            //draw cells
            int[][] goalStateLocations = this.GetGoalStates;

            for(int y = 0; y < CurrentState.State.GetLength(1); y++)
            {
                for (int x = 0; x < CurrentState.State.GetLength(0); x++)
                {
                    Color cellKind;

                    //check for a goal state

                    switch (CurrentState.State[x,y])
                    {
                        case 0: //5 = is trail of path
                            cellKind = Color.Orange;
                            break;
                        case 2:
                            cellKind = Color.Gray;
                            break;
                        case 3:
                            cellKind = Color.Salmon;
                            break;
                        default:
                            cellKind = Color.White;
                            foreach (int[] goalPos in goalStateLocations)
                            {
                                if (goalPos[0] == x && goalPos[1] == y)
                                {
                                    cellKind = Color.LimeGreen;
                                }
                            }
                            break;
                    }

                    SwinGame.FillRectangle(cellKind, 25 + (x * 48), 25 + (y * 48), 43, 43);
                    if(ShowCoords) SwinGame.DrawText($"[{x + 1},{y + 1}]", Color.RoyalBlue, 27 + (x * 48), 28 + (y * 48));

                }
            }
            SwinGame.DrawText(GetPathString(), Color.Black, 21, 265);
            SwinGame.DrawText("Iterations: " + IterationCount, Color.Black, 21, 275);

        }
    }
}
