using System;
using System.Collections.Generic;
using System.Text;

namespace RobotNavigationAssignment
{
    public class GreedyBestFirstSearch : SearchMethod
    {
        public GreedyBestFirstSearch()
        {
            id = "GBFS";
            name = "Greedy Best First Search";
            iterationCount = 0;
            category = SearchCategory.Informed;
            Frontier = new Frontier();
            Searched = new List<MazeState>();
        }

        public override bool AddToFrontier(MazeState aState, Frontier aFrontier = null)
        {
            Frontier selectedFrontier = aFrontier == null ? Frontier : aFrontier;
            if (selectedFrontier.Contains(aState) || Maze.Contains(aState, Searched))
            {
                return false;
            }
            else
            {
                selectedFrontier.Enqueue(aState);
            }

            return true;
        }

        public override Direction[] Solve(Maze aMaze)
        {
            //push the StartState to the frontier
            AddToFrontier(aMaze.StartState);

            while (!Frontier.Empty())
            {
                //Pop frontier state into curState
                MazeState curState = PopFrontier();

                //check if curState is a goalState
                //using a loop for each of the variant goal states listed
                for (int i = 0; i < aMaze.GoalStates.Length; i++)
                {
                    if (Maze.AreEqual(curState.State, aMaze.GoalStates[i].State))
                    {
                        Maze.OutputState(curState.State);
                        aMaze.FinalPathCost = curState.Cost;
                        Console.WriteLine($"Path cost: {aMaze.FinalPathCost}");
                        iterationCount++;
                        return curState.GetPathFromParent();
                    }
                }

                //Find most desirable goal state - i.e. which is closes to the current state
                MazeState ClosestGoal = PickDesiredGoalState(curState, aMaze.GoalStates);

                //get all possible new states from curState
                List<MazeState> newStates = curState.CreatePossibleMoves();
                newStates.Reverse(); //Reverse elements to ensure priority is U > L > D > R

                foreach (MazeState s in newStates)
                {
                    s.HeuristicCost = GetManhattanDistance(s, ClosestGoal);
                    AddToFrontier(s);
                }

                //Sort the frontier by f(n) = h(n)
                Frontier.SortByHeuristicCost();
                iterationCount++;

            }
            return null;
        }


    }
}
