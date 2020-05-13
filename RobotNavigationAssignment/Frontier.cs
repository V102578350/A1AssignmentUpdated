using System;
using System.Collections.Generic;
using System.Text;

namespace RobotNavigationAssignment
{
    public class Frontier
    {
  
        private List<MazeState> _states = new List<MazeState>();
        public Frontier() { }

        //adds MazeState to end of linkedList
        public void Enqueue(MazeState aState)
        {
            _states.Add(aState);
        }

        public void SortByHeuristicCost()
        {
            for (int i = 0; i < _states.Count - 1; i++)
            {
                for (int j = 0; j < _states.Count - i - 1; j++)
                {
                    if (_states[j].HeuristicCost > _states[j + 1].HeuristicCost)
                    {
                        MazeState temp = _states[j];
                        _states[j] = _states[j + 1];
                        _states[j + 1] = temp;
                    }
                }

            }

        }

        public void SortByCost()
        {
            for (int i = 0; i < _states.Count - 1; i++)
            {
                for (int j = 0; j < _states.Count - i - 1; j++)
                {
                    if (_states[j].PathCost > _states[j + 1].PathCost)
                    {
                        MazeState temp = _states[j];
                        _states[j] = _states[j + 1];
                        _states[j + 1] = temp;
                    }
                }

            }
                
        }

        public void SortByEvaluationCost()
        {
            for (int i = 0; i < _states.Count - 1; i++)
            {
                for (int j = 0; j < _states.Count - i - 1; j++)
                {
                    if (_states[j].GetTotalCost > _states[j + 1].GetTotalCost)
                    {
                        MazeState temp = _states[j];
                        _states[j] = _states[j + 1];
                        _states[j + 1] = temp;
                    }
                }
            }
        }

        public MazeState Dequeue()
        {
            MazeState result = _states[_states.Count - 1];
            _states.RemoveAt(_states.Count - 1);
            return result;
        }


        public MazeState Pop()
        {
            MazeState result = _states[0];
            _states.RemoveAt(0);
            return result;
        }

        public void OutputTest()
        {
            foreach(MazeState s in _states)
            {
                Console.Write($" {s.HeuristicCost}|{s.GetTotalCost}  ");
            }
            Console.WriteLine();
        }

        public bool Contains(MazeState aState)
        {
            return Maze.Contains(aState, _states);
        }

        public void Push(MazeState aState)
        {
            _states.Insert(0, aState);
        }

        public bool Empty()
        {
            if(_states.Count == 0)
            {
                return true;
            }

            return false;
        }


    }
}
