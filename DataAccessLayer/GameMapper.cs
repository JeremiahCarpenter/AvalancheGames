using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class GameMapper : Mapper
    {
        int OffsetToGameID;
        int OffsetToGameName;
        public GameMapper(System.Data.SqlClient.SqlDataReader reader)
        {
            OffsetToGameID = reader.GetOrdinal("GameID");
            Assert(0 == OffsetToGameID, "The GameId is not 0 as expected");
            OffsetToGameName = reader.GetOrdinal("GameName");
            Assert(1 == OffsetToGameName, "The GameName is not 1 as expecped");
        }
        public GameDAL GameFromReader(System.Data.SqlClient.SqlDataReader reader)
        {
            GameDAL proposedReturnVale = new GameDAL();
            proposedReturnVale.GameID = GetInt32OrDefault(reader, OffsetToGameID);
            proposedReturnVale.GameName = GetStringOrDefault(reader, OffsetToGameName);
            return proposedReturnVale;
        }
    }
}
