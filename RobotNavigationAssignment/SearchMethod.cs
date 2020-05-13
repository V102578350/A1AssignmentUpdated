using System;
using System.Collections.Generic;
using System.Text;

namespace RobotNavigationAssignment
{
    public abstract class SearchMethod
    {
        public Frontier Frontier;
        public List<MazeState> Searched;

        //identifiers
        public string id;
        public string name;
        public int iterationCount; //count the number of search iterations
        public static bool VariableCost;
        public SearchCategory category;

        public SearchMethod() { }

        //Solve the maze from its StartState to GoalState
        public abstract Direction[] Solve(Maze aMaze);

        public abstract bool AddToFrontier(MazeState aState, Frontier aFrontier = null);

        // returns the manhattan distance between two nodes
        public int GetManhattanDistance(MazeState aState, MazeState aDest)
        {
            //Find possition of two states
            int[] aCords = aState.FindPosition();
            int[] bCords = aDest.FindPosition();

            //calculate distance - returning absolute value
            //eg aCord : (4,4)   bCord : (8, 8)
            //abs(4 - 8) + abs(4 - 8) = 8
            return Math.Abs(aCords[0] - bCords[0]) + Math.Abs(aCords[1] - bCords[1]);
        }

        // remove and return first element from frontier - in addition
        // it is added to the searched list
        public MazeState PopFrontier()
        {
            MazeState result = Frontier.Pop();
            Searched.Add(result);
            return result;
        }

        public void Reset()
        {
            Frontier = new Frontier();
            Searched = new List<MazeState>();
            iterationCount = 0;
        }

        //returns the closest goal state from the current state
        // - this funciton allows flexibility for the program to determine the
        // best goal to traverse to out of a collection
        public MazeState PickDesiredGoalState(MazeState aState, MazeState[] aGoalStates)
        {
            MazeState result = aGoalStates[0];
            for (int i = 1; i < aGoalStates.Length; i++)
            {
                if (GetManhattanDistance(aState, result) > GetManhattanDistance(aState, aGoalStates[i]))
                {
                    result = aGoalStates[i];
                }
            }

            return result;
        }

    }
}
