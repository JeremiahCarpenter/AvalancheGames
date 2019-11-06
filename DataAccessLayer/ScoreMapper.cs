using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ScoreMapper : Mapper
    {
        int OffsetToScoreID;
        int OffsetToScore;
        int OffsetToUserID;
        int OffsetToGameID;
        int OffsetToUserName;
        int OffsetToGameName;
        
        public ScoreMapper(System.Data.SqlClient.SqlDataReader reader)
        {
            OffsetToScoreID = reader.GetOrdinal("ScoreID");
            Assert(0 == OffsetToScoreID, $"ScoreID is {OffsetToScoreID} not 0 as expected");
            OffsetToScore = reader.GetOrdinal("Score");
            Assert(1 == OffsetToScore, $"Score is {OffsetToScore} not 1 as expected");
            OffsetToUserID = reader.GetOrdinal("UserID");
            Assert(2 == OffsetToUserID, $"UserID is {OffsetToUserID} not 2 as expected");
            OffsetToGameID = reader.GetOrdinal("GameID");
            Assert(3 == OffsetToGameID, $"GameID is {OffsetToGameID} not 3 as expected");
            OffsetToUserName = reader.GetOrdinal("UserName");
            Assert(4 == OffsetToUserName, $"UserName is {OffsetToUserName} not 4 as expected");
            OffsetToGameName = reader.GetOrdinal("GameName");
            Assert(5 == OffsetToGameName, $"GameName is {OffsetToGameName} not 5 as expected");
        }

        public ScoreDAL ScoreFromReader(System.Data.SqlClient.SqlDataReader reader)
        {
            ScoreDAL ProposedReturnValue = new ScoreDAL();
            ProposedReturnValue.ScoreID = reader.GetInt32(OffsetToScoreID);
            ProposedReturnValue.Score = reader.GetInt32(OffsetToScore);
            ProposedReturnValue.UserID = reader.GetInt32(OffsetToUserID);
            ProposedReturnValue.GameID = reader.GetInt32(OffsetToGameID);
            ProposedReturnValue.UserName = reader.GetString(OffsetToUserName);
            ProposedReturnValue.GameName = reader.GetString(OffsetToGameName);
            return ProposedReturnValue;
        }
    }
}
