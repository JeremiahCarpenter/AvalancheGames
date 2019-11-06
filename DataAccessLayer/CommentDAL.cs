using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CommentDAL
    {
        #region direct properties
        public int CommentID { get; set; }
        public string GameComment { get; set; }
        public int UserID { get; set; }
        public int GameID { get; set; }
        public bool Liked { get; set; }
        #endregion
        #region indirect properties 
        public string GameName { get; set; }
        public string UserName { get; set; }
        #endregion
        public override string ToString()
        {
            return $"CommentID: {CommentID,5} GameComment: {GameComment,250} UserID: {UserID} GameID: {GameID,5} Liked: {Liked} GameName: {GameName,25} UserName: {UserName}";
        }
    }
}
