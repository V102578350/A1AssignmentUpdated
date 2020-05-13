using System;
using System.Collections.Generic;
using System.Text;

namespace RobotNavigationAssignment
{
    public class MazeState
    {
        private int[,] _state;
        private MazeState _parent;
        private List<MazeState> _children;
        public int Cost; // g(n) cost so far
        public int PathCost;
        public int HeuristicCost; //h(n) cost to goal
        public int TotalCost; // f(n) = g(n) + h(n)
        public Direction DirectionFromParent;

        public MazeState(int[,] aState)
        {
            _state = aState;
            _parent = null;
            _children = null;
            Cost = 0;
            PathCost = 0;
            HeuristicCost = 0;
            TotalCost = Cost + HeuristicCost;
        }

        public MazeState(int[,] aState, MazeState aParent, Direction aDFP, int aPathCost = 1)
        {
            _state = aState;
            _parent = aParent;
            _children = null;
            Cost = aParent.Cost + aPathCost;
            PathCost = aPathCost;
            HeuristicCost = 0;
            TotalCost = Cost + HeuristicCost;
            DirectionFromParent = aDFP;
        }

        public MazeState Parent
        {
            get { return _parent; }
        }

        public List<MazeState> Children
        {
            get { return _children; } 
            set { _children = value; }
        }

        public int[,] State
        {
            get { return _state; }
            set { _state = value; }
        }

        public int GetTotalCost
        {
            get { return Cost + HeuristicCost; }
        }

        //returns position of '0' in state
        public int[] FindPosition()
        {
            int[] result = { 0, 0 };
            for(int column = 0; column < State.GetLength(1); column++)
            {
                for (int row = 0; row < State.GetLength(0); row++)
                {
                    if(State[row,column] == 0)
                    {
                        return new int[] { row, column };
                    }
                }
            }

            //retun null if nothing else returned previously
            return null;
        }

        public Direction[] FindPossibleMovements()
        {
            //Get our current state position
            int[] position = FindPosition();

            //Use a list instead of array since size cannot be determined.
            //conversion toArray completed at return statement
            List<Direction> directionList = new List<Direction>();

            //check up
            if((position[1] - 1) >= 0)
            {
                //if not wall (could also be if == 1 for traversable)
                if (State[position[0], position[1] - 1] != 2) 
                {
                    directionList.Add(Direction.Up);
                }
            }
            //check left
            if ((position[0] - 1) >= 0)
            {
                //if not wall (could also be if == 1 for traversable)
                if (State[position[0] - 1, position[1]] != 2)
                {
                    directionList.Add(Direction.Left);
                }
            }
            //check down
            if ((position[1] + 1) < State.GetLength(1))
            {
                //if not wall (could also be if == 1 for traversable)
                if (State[position[0], position[1] + 1] != 2)
                {
                    directionList.Add(Direction.Down);
                }
            }
            //check right
            if ((position[0] + 1) < State.GetLength(0))
            {
                //if not wall (could also be if == 1 for traversable)
                if (State[position[0] + 1, position[1]] != 2)
                {
                    directionList.Add(Direction.Right);
                }
            }

            directionList.Reverse();

            return directionList.ToArray();
        }

        //creates copy of state array to prevent 'assign by reference' issue
        public static int[,] CloneState(int[,] aState)
        {
            int[,] result = new int[aState.GetLength(0), aState.GetLength(1)];

            for(int y =0; y < aState.GetLength(1); y++)
            {
                for(int x = 0; x < aState.GetLength(0); x++)
                {
                    //set matching array elements
                    result[x, y] = aState[x, y];
                }
            }

            return result;
        }

        //the variable cost bool is an option for different movement costs
        public MazeState Move(Direction aDirection)
        {
            //Get our current state position
            int[] position = FindPosition();

            //create copy of this.State to be manipulated for new state
          
            MazeState result = new MazeState(CloneState(this.State), this, aDirection);

            switch(aDirection)
            {
                case Direction.Up:
                    result.State[position[0], position[1] - 1] = 0; // set moved positioned to zero (new location1)
                    if (SearchMethod.VariableCost)
                    {
                        result.Cost += 3;
                        result.PathCost = 4;
                    }
                    break;
                case Direction.Left:
                    result.State[position[0] - 1, position[1]] = 0; // set moved positioned to zero (new location1)
                    if (SearchMethod.VariableCost)
                    {
                        result.Cost += 1;
                        result.PathCost = 2;
                    }
                    break;
                case Direction.Down:
                    result.State[position[0], position[1] + 1] = 0; // set moved positioned to zero (new location1)
                    break;
                default: //right direction
                    result.State[position[0] + 1, position[1]] = 0; // set moved positioned to zero (new location1)
                    if (SearchMethod.VariableCost)
                    {
                        result.Cost += 1;
                        result.PathCost = 2;
                    }
                    break;
            }

            //set previous position to traversable
            result.State[position[0], position[1]] = 1;

            return result;
        }

        //creates the child mazestates
        public List<MazeState> CreatePossibleMoves()
        {
            //get possble direction movements
            Direction[] possibleMovements = FindPossibleMovements();
            Children = new List<MazeState>();

            for(int i = 0; i < possibleMovements.Length; i++)
            {
                MazeState newState = Move(possibleMovements[i]);
                Children.Add(newState);
            }

            return Children;

        }

        //Returns a list of directions back from the starting node to the finishing node
        //ex. {Right, Right, Right, Down, Down, Down, Left....}
        public Direction[] GetPathFromParent()
        {
            Stack<Direction> result = new Stack<Direction>();
            MazeState node = this;

            while(node != null)
            {     
                //if our node is StartNode (cost 0) ignore and break loop
                if (node.Cost == 0) break;

                //push direction to result 
                result.Push(node.DirectionFromParent);

                //set the parent variable to the parent of the last parent
                node = node.Parent;

            }

            //convert result Stack<Direction> into Array<Direction>
            return result.ToArray();
        }
    }
}
