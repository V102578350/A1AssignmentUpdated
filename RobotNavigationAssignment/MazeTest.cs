using System;
using System.Collections.Generic;
using System.Text;

namespace RobotNavigationAssignment
{


    public class MazeTest
    {
        private TestResult[] mazeSizes;

        public MazeTest(int aIteration)
        {
            mazeSizes = new TestResult[Round(aIteration)];
            for(int i = 0; i < mazeSizes.Length; i++)
            {
                mazeSizes[i] = new TestResult();
            }
        }

        public void LogTest(int aSize, int aPathCost, int aIterationCount)
        {
            if (aPathCost == 0) return;

            int index = Round(aSize) / 10;
            mazeSizes[index].Add(aPathCost, aIterationCount);
            
        }

        public void OutputResults()
        {
            Console.WriteLine("\t---- TEST RESULTS ----");
            for(int i =0; i < mazeSizes.Length; i++)
            {
                if(mazeSizes[i].LogCount < 1)
                {
                    continue;
                } else
                {
                    Console.WriteLine($"\tMaze Size Density Around [{i * 10}]");
                    Console.WriteLine($"\t\t> Average Cost: {mazeSizes[i].GetPathCostAvg()}");
                    Console.WriteLine($"\t\t> Average Iteration Count: {mazeSizes[i].GetIterationCountAvg()}");
                }
            }
            Console.WriteLine("\t--- END OF RESULTS ---");


        }

        public int Round(int aNum)
        {
            int remaining = aNum % 10;
            int result = remaining >= 5 ? (aNum - remaining + 10) : (aNum - remaining);

            return result;
        }
    }
}
