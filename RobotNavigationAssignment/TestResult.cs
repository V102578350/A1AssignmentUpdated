using System;
using System.Collections.Generic;
using System.Text;

namespace RobotNavigationAssignment
{
    public class TestResult
    {
        private List<int> pathCosts;
        private List<int> iterationCounts;
        public int LogCount;

        public TestResult()
        {
            pathCosts = new List<int>();
            iterationCounts = new List<int>();
            LogCount = 0;
        }

        public void Add(int aPathCost, int aIterationCount)
        {
            pathCosts.Add(aPathCost);
            iterationCounts.Add(aIterationCount);
            LogCount++;
        }

        public int GetPathCostAvg()
        {
            int result = 0;

            foreach (int i in pathCosts)
                result += i;

            return result / pathCosts.Count;
        }

        public int GetIterationCountAvg()
        {
            int result = 0;

            foreach (int i in iterationCounts)
                result += i;

            return result / iterationCounts.Count;
        }
    }
}
