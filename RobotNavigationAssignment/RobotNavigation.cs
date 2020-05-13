using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RobotNavigationAssignment
{
    class RobotNavigation
    {
        static void Main(string[] args)
        {
            //Search Algorithm Information: https://www.geeksforgeeks.org/a-search-algorithm/ (Heuristic Calculations and more)

            //Check command-line args
            if (args.Length < 3)
            {
                Console.WriteLine("Required args: <mapconfig file> <search method> <variable move cost>");
                Console.WriteLine("Map Config File: Enter filename with configuration OR enter 0 for random mazes");
                Console.WriteLine("Search methods: DFS, BFS, AS, GBFS, CUS1");
                Console.WriteLine("Variable move cost: 0 = false, 1 = true;");
                System.Environment.Exit(0);
            }

            //assign args to variables
            string file = args[0];
            string method = args[1];
            SearchMethod.VariableCost = args[2] == "1" ? true : false;
            bool runtest = file == "0" ? true : false; //runtest if file == 0

            //initalize Test Object
            MazeTest mazeTest = new MazeTest(400); //(50*50)/2

            //create maze object
            Maze maze = new Maze(null, null);

            if (File.Exists(file))
            {

                StreamReader rdr = new StreamReader(file);
                MazeGenerator mazeGenerator = new MazeGenerator();
                maze = mazeGenerator.GetMaze(rdr);

                Output(maze, method);


            } else if(runtest)
            {
                int sizeIncrement = 20;
                for (int i = 0; i < 45; i++)
                {        
                    if (i > sizeIncrement) sizeIncrement += 5;

                    Console.WriteLine($"Test iteration no: {i + 1}");
                    MazeState randomState = new MazeState(MazeGenerator.GenerateRandomMaze(sizeIncrement));
                    maze = new Maze(randomState);
                    
                    Output(maze, method);

                    mazeTest.LogTest(maze.StartState.State.GetLength(0) * maze.StartState.State.GetLength(1), maze.FinalPathCost, maze.IterationCount);
                }

                mazeTest.OutputResults();

            } else
            {
                Console.WriteLine($"The file: {file} was not found. Please try again!");
                Environment.Exit(1);
            }

            
        }

        public static void Output(Maze aMaze, string aMethod)
        {
            Console.WriteLine("Starting State:");
            Maze.OutputState(aMaze.StartState.State);

            // run the selected method
            Direction[] path = aMaze.ExecuteMethod(aMethod);
            if (SearchMethod.VariableCost)
                Console.WriteLine("Variable move cost: UP = 4, DOWN = 1, LEFT = 2, RIGHT = 2");
            else
                Console.WriteLine("Variable move cost: false (all movements cost 1)");

            if(path != null)
            {
                for (int i = 0; i < path.Length; i++)
                {
                    Console.Write($"{path[i].ToString()}; ");
                }
               
            } else
            {
                Console.WriteLine("No path found!");
            }

            Console.WriteLine("Iterations: " + aMaze.IterationCount);
        }
    }
}
