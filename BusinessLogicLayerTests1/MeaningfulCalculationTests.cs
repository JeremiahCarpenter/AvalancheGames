using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Tests
{
    [TestClass()]
    public class MeaningfulCalculationTests
    {
        List<ScoreBLL> MakeSampleScores(int count)
        {
            List<ScoreBLL> proposedReturnValue = new List<ScoreBLL>();
            for (int i = 0; i < count; i ++)
            {
                ScoreBLL score = new ScoreBLL();
                score.ScoreID = i;
                score.Score = i;
                score.UserID = i;
                score.GameID = i % 3;
                score.GameName = $"Game{i}";
                proposedReturnValue.Add(score);
            }
            return proposedReturnValue;
        }

       [TestMethod]
       public void When_NoScores_Expect_AverageToBeZero()
        {
            //arrange
            MeaningfulCalculation mc = new MeaningfulCalculation();
            var Scores = MakeSampleScores(0);
            double expected = 0;
            //act
            double actual = mc.CalculateUserAverage(Scores);
            //assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void When_ScoreIsNull_Expect_AverageToBeZero()
        {
            //arrange
            MeaningfulCalculation mc = new MeaningfulCalculation();
            double expected = 0;
            //act
            double actual = mc.CalculateUserAverage(null);
            //assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void When_ScoreIsNonNull_Expect_AverageToBeZero()
        {
            //arrange
            MeaningfulCalculation mc = new MeaningfulCalculation();
            List<ScoreBLL> Scores = MakeSampleScores(25);
            double expected = 0;
            //act
            double actual = mc.CalculateUserAverage(Scores);
            //assert
            Assert.AreNotEqual(expected, actual);
        }

    }
}
