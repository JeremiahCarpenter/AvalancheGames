using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CommentMapper : Mapper
    {
        int OffsetToCommentID;
        int OffsetToGameComment;
        int OffsetToUserID;
        int OffsetToGameID;
        int OffsetToLiked;
        int OffsetToGameName;
        int OffsetToUserName;

        public CommentMapper (System.Data.SqlClient.SqlDataReader reader)
        {
            OffsetToCommentID = reader.GetOrdinal("CommentID");
            Assert(0 == OffsetToCommentID, $"CommentID is {OffsetToCommentID} not 0 as expected");
            OffsetToGameComment = reader.GetOrdinal("GameComment");
            Assert(1 == OffsetToGameComment, $"GameComment is {OffsetToGameComment} not 1 as expected");
            OffsetToUserID = reader.GetOrdinal("UserID");
            Assert(2 == OffsetToUserID, $"UserID is {OffsetToUserID} not 2 as expected");
            OffsetToGameID = reader.GetOrdinal("GameID");
            Assert(3 == OffsetToGameID, $"GameID is {OffsetToGameID} not 3 as expected");
            OffsetToLiked = reader.GetOrdinal("Liked");
            Assert(4 == OffsetToLiked, $"Liked is {OffsetToLiked} not 4 as expected");
            OffsetToGameName = reader.GetOrdinal("GameName");
            Assert(5 == OffsetToGameName, $"GameName is {OffsetToGameName} not 5 as expected");
            OffsetToUserName = reader.GetOrdinal("UserName");
            Assert(6 == OffsetToUserName, $"UserName is {OffsetToUserName} not 6 as expected");
        }
        public CommentDAL CommentFromReader (System.Data.SqlClient.SqlDataReader reader)
        {
            CommentDAL ProposedReturnValue = new CommentDAL();
            ProposedReturnValue.CommentID = reader.GetInt32(OffsetToCommentID);
            ProposedReturnValue.GameComment = reader.GetString(OffsetToGameComment);
            ProposedReturnValue.UserID = reader.GetInt32(OffsetToUserID);
            ProposedReturnValue.GameID = reader.GetInt32(OffsetToGameID);
            ProposedReturnValue.Liked = reader.GetBoolean(OffsetToLiked);
            ProposedReturnValue.GameName = reader.GetString(OffsetToGameName);
            ProposedReturnValue.UserName = reader.GetString(OffsetToUserName);
            return ProposedReturnValue;
        }

    }
}
