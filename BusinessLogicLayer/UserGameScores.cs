
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataAccessLayer;

namespace BusinessLogicLayer

{
    public class ScoreStats
    {
        public string UserName { get; set; }
        public int Count { get; set; }
        public double AverageScore { get; set; }
        public int HighScore { get; set; }
        public int LowestScore { get; set; }
    }
    public class MeaningfulCalculation
    {
        public double CalculateUserAverage(List<ScoreBLL> Score)
        {
            if (Score == null) return 0;
            if (Score.Count == 0) return 0;
            return Score.Average(s => s.Score);
        }

        public  List<ScoreStats> CalculateStats(List<ScoreBLL> Scores)
        {//linq 
            var Q1 = from scr in Scores group scr by scr.UserName into grp select new ScoreStats() { UserName = grp.Key, Count = grp.Count(), AverageScore = grp.Average(s => s.Score), HighScore = grp.Max(s => s.Score), LowestScore = grp.Min(s => s.Score) };
            return Q1.ToList();
        }

    }



    
}

