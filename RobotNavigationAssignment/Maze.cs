using System;
using System.Collections.Generic;
using System.Text;

namespace RobotNavigationAssignment
{
    public class Maze
    {
        private MazeState _startState;
        private MazeState[] _goalStates;
        private List<MazeState> _iterations;
        private int _finalPathCost;

        // Search Algorithms
        private BreadthFirstSearch _bfs;
        private DepthFirstSearch _dfs;
        private AStarSearch _as;
        private GreedyBestFirstSearch _gbfs;
        private UniformCostSearch _ucs;

        private string _curMethod;

        public Maze(MazeState aStartState, MazeState[] aGoalStates)
        {
            //asign start and goal state(s)
            _startState = aStartState;
            _goalStates = aGoalStates;
            _iterations = new List<MazeState>();
            _finalPathCost = 0;


            //initialize search alogorithms
            _bfs = new BreadthFirstSearch();
            _dfs = new DepthFirstSearch();
            _as = new AStarSearch();
            _gbfs = new GreedyBestFirstSearch();
            _ucs = new UniformCostSearch();
        }

        public Maze(MazeState aStartState) : this(aStartState, null)
        {
            _goalStates = new MazeState[] { new MazeState(MazeGenerator.GenerateGoalState(aStartState)) };
        }

        public MazeState StartState
        {
            get { return _startState; }
        }

        public MazeState[] GoalStates
        {
            get { return _goalStates; }
        }

        public List<MazeState> Iterations
        {
            get { return _iterations; }
        }

        public int FinalPathCost
        {
            get { return _finalPathCost; }
            set { _finalPathCost = value; }
        }


        //returns iteration count
        public int IterationCount
        {
            get
            {
                switch(_curMethod)
                {
                    case "BFS":
                        return _bfs.iterationCount;
                    case "DFS":
                        return _dfs.iterationCount;
                    case "GBFS":
                        return _gbfs.iterationCount;
                    case "AS":
                        return _as.iterationCount;
                    default:
                        return _ucs.iterationCount;
                }
            }
        }


        //Selects a search method based on parameters from args
        public Direction[] ExecuteMethod(string aMethod)
        {
            Direction[] result = new Direction[0];
            _curMethod = aMethod; //assign aMethod to currentMethod
            switch (aMethod)
            {
                case "BFS":
                    Console.WriteLine($"Executing: {_bfs.name} - {_bfs.category} ({_bfs.id}) : ");
                    _bfs.Reset();
                    result = _bfs.Solve(this);
                    _iterations = _bfs.Searched;
                    break;
                case "DFS":
                    Console.WriteLine($"Executing: {_dfs.name} - {_dfs.category} ({_dfs.id}) : "); 
                    _dfs.Reset();
                    result = _dfs.Solve(this);
                    _iterations = _dfs.Searched;
                    break;
                case "GBFS":
                    Console.WriteLine($"Executing: {_gbfs.name} - {_gbfs.category} ({_gbfs.id}) : ");       
                    _gbfs.Reset();
                    result = _gbfs.Solve(this);
                    _iterations = _gbfs.Searched;
                    break;
                case "AS":
                    Console.WriteLine($"Executing: {_as.name} - {_as.category} ({_as.id}) : ");                    
                    _as.Reset();
                    result = _as.Solve(this);
                    _iterations = _as.Searched;
                    break;
                case "CUS1":
                    Console.WriteLine($"Executing: {_ucs.name} - {_ucs.category} ({_ucs.id}) : ");
                    _ucs.Reset();
                    result = _ucs.Solve(this);
                    _iterations = _ucs.Searched;
                    break;
                default:
                    Console.WriteLine("Please select a valid search method: BFS, DFS, GBFS, AS, CUS1");
                    break;
            }

            return result;
        }

        //outputs a formatted maze state
        public static void OutputState(int[,] aState)
        {
            for(int column = 0; column < aState.GetLength(1); column++)
            {
                for (int row = 0; row < aState.GetLength(0); row++)
                {
                    Console.Write($"{aState[row, column]} ");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        //Check whether two states are equal or not
        public static bool AreEqual(int[,] aStateOne, int[,] aStateTwo)
        {
            for (int column = 0; column < aStateOne.GetLength(1); column++)
            {
                for (int row = 0; row < aStateOne.GetLength(0); row++)
                {
                    if (aStateOne[row, column] != aStateTwo[row, column])
                    {
                        return false;
                    }
                }

            }

            return true;
        }

        //check whether a maze state is in a container
        //implemented this since we can not simply write e.g Search.Contains(aState) since
        //it could not determine if they were equal. This is way we want a function to
        //compare the state of a maze only, rather than a whole MazeState Object.
        public static bool Contains(MazeState aState, List<MazeState> aContainer)
        {
            foreach(MazeState s in aContainer)
            {
                if(AreEqual(aState.State, s.State))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
