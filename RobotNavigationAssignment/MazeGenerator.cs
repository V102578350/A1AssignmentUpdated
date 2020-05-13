using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace RobotNavigationAssignment
{
    public class MazeGenerator
    {
        public Maze GetMaze(StreamReader rdr)
        {
            //Size of Maze
            string[] mazeSizeString = Between(rdr.ReadLine(), "[", "]").Split(',');
            int[] mazeSize = new int[2] { Convert.ToInt32(mazeSizeString[0]), Convert.ToInt32(mazeSizeString[1]) };

            //Init state
            string[] initStateString = Between(rdr.ReadLine(), "(", ")").Split(',');
            int[] initState = new int[] { Convert.ToInt32(initStateString[0]), Convert.ToInt32(initStateString[1]) };


            //Goal States
            string[] goalStatesString = rdr.ReadLine().Split('|');
            goalStatesString = new string[] { Between(goalStatesString[0], "(", ")"), Between(goalStatesString[1], "(", ")") };
            int[][] goalStates = new int[][] {
                    new int[] { Convert.ToInt32(goalStatesString[0].Split(',')[0]), Convert.ToInt32(goalStatesString[0].Split(',')[1]) },
                    new int[] { Convert.ToInt32(goalStatesString[1].Split(',')[0]), Convert.ToInt32(goalStatesString[1].Split(',')[1]) } };

            //Walls
            //Read out rest of mapconfig file
            List<int[]> wallsList = new List<int[]>();
            while (!rdr.EndOfStream)
            {
                string[] wallsString = Between(rdr.ReadLine(), "(", ")").Split(',');
                wallsList.Add(new int[] { Convert.ToInt32(wallsString[0]), Convert.ToInt32(wallsString[1]),
                        Convert.ToInt32(wallsString[2]), Convert.ToInt32(wallsString[3]) });
            }
            int[][] walls = wallsList.ToArray();



            //parse data to int[,] state
            int[,] startState = GenerateMazeState(mazeSize, initState, walls);
            int[,] goalStateA = GenerateMazeState(mazeSize, goalStates[0], walls);
            int[,] goalStateB = GenerateMazeState(mazeSize, goalStates[1], walls);

            //initialize MazeStates
            MazeState StartState = new MazeState(startState);
            MazeState[] GoalStates = new MazeState[goalStates.Length];
            GoalStates[0] = new MazeState(goalStateA);
            GoalStates[1] = new MazeState(goalStateB);

            return new Maze(StartState, GoalStates);
        }
    

        private string Between(string aInput, string aStart, string aEnd)
        {
            int p1 = aInput.IndexOf(aStart) + aStart.Length;
            int p2 = aInput.IndexOf(aEnd, p1);

            if (aEnd == "") return (aInput.Substring(p1));
            else return aInput.Substring(p1, p2 - p1);
        }

        //static function used to compare array int elements
        private static bool isEqual(int[] a, int[] b)
        {
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }

            return true;
        }

        public static int[,] GenerateRandomMaze(int aSize)
        {
            //curSize
            // random generating maze attributes from
            int width = aSize;
            int height = aSize;

            int[,] result = new int[height, width];

            //pick random starting point
            int[] startingPoint = new int[] { 1, 1 };


            for (int column = 0; column < result.GetLength(1); column++)
            {
                for (int row = 0; row < result.GetLength(0); row++)
                {
                    result[row, column] = 1;

                    //wall generation - random
                    if(new Random().Next(0, 10) > 7)
                    {
                        result[row, column] = 2;
                    }

                    //check if is StartPosition
                    if (isEqual(startingPoint, new int[2] { row, column }))
                    {
                        result[row, column] = 0;
                    }
                }
            }
            Console.WriteLine($"Starting point = x : {startingPoint[0]} y : {startingPoint[1]}");

            return result;
        }

        //function used to generate goal state when no already identfied in constructor
        //static function
        public static int[,] GenerateGoalState(MazeState aStartState)
        {
            //get current position so it can be changed back to traversable path
            int[] curPos = aStartState.FindPosition();
            int[,] result = MazeState.CloneState(aStartState.State);

            result[curPos[0], curPos[1]] = 1; //set curPos back to 1 (path)
            result[aStartState.State.GetLength(1) - 2, aStartState.State.GetLength(0) - 2] = 0; //set random position to goalstate

            return result;
        }


        //static function used to return a int[,] 2d array of the maze state
        private int[,] GenerateMazeState(int[] aSize, int[] aState, int[][] aWalls)
        {
            //curPostion = 0, Traversable = 1, Wall = 2
            //aSize = [rows,columns]
            //aState = [{x,y}, {x,y}]
            //aWalls = [{x,y,width,height}, {x,y,width,height}, ...]

            int[,] result = new int[aSize[1], aSize[0]];

            for (int column = 0; column < result.GetLength(1); column++)
            {
                for (int row = 0; row < result.GetLength(0); row++)
                {
                    //set default
                    result[row, column] = 1;


                    //check for state
                    if (isEqual(aState, new int[2] { row, column }))
                    {
                        result[row, column] = 0;
                    }


                    //check for walls
                    for (int i = 0; i < aWalls.Length; i++)
                    {
                        //height
                        for (int j = 0; j < aWalls[i][3]; j++)
                        {
                            //width
                            for (int k = 0; k < aWalls[i][2]; k++)
                            {
                                if (isEqual(new int[2] { aWalls[i][0] + k, aWalls[i][1] + j }, new int[2] { row, column }))
                                {
                                    result[row, column] = 2;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }


}
