using System;
using System.Collections.Generic;
using System.Text;

namespace RobotNavigationAssignment
{
    public class DepthFirstSearch : SearchMethod
    {
        public DepthFirstSearch()
        {
            id = "DFS";
            name = "Depth-First Search";
            iterationCount = 0;
            category = SearchCategory.Uninformed;
            Frontier = new Frontier();
            Searched = new List<MazeState>();
        }

        //function used to add to frontier and prevent duplicate states
        public override bool AddToFrontier(MazeState aState, Frontier aFrontier = null)
        {
            Frontier selectedFrontier = aFrontier == null ? Frontier : aFrontier;
            if (selectedFrontier.Contains(aState) || Maze.Contains(aState, Searched))
            {
                return false;
            }
            else
            {

                selectedFrontier.Push(aState);
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

            }

            return null;
        }
    }
}
