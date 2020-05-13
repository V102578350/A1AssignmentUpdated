using System;
using System.Collections.Generic;
using System.Text;

namespace RobotNavigationAssignment
{
    public class UniformCostSearch : SearchMethod
    {
        public UniformCostSearch()
        {
            id = "UCS";
            name = "Uniform Cost Search";
            iterationCount = 0;
            category = SearchCategory.Uninformed;

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
            //ensure variable cost mode is on
            SearchMethod.VariableCost = true;

            AddToFrontier(aMaze.StartState);

            while(!Frontier.Empty())
            {
                //Pop frontier state into curState
                MazeState curState = Frontier.Pop();
                Searched.Add(curState);              

                //check if curState is a goalState
                //using a loop for each of the varient goal states listed
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

                //get all possible new states from curState
                List<MazeState> newStates = curState.CreatePossibleMoves();

                foreach (MazeState s in newStates)
                {
                    AddToFrontier(s);
                }
                iterationCount++;

                Frontier.SortByCost();
            }

            return null;
        }
    }
}
