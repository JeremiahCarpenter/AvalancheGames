using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class CommentBLL
    {
        public CommentBLL()
        {

        }
        #region direct properties
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int CommentID { get; set; }
        public string GameComment { get; set; }
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int UserID { get; set; }
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int GameID { get; set; }
        public bool Liked { get; set; }
        #endregion
        #region indirect properties 
        public string GameName { get; set; }
        public string UserName { get; set; }
        #endregion
        public CommentBLL(DataAccessLayer.CommentDAL dal)
        {
            this.CommentID = dal.CommentID;
            this.GameComment = dal.GameComment;
            this.UserID = dal.UserID;
            this.GameID = dal.GameID;
            this.Liked = dal.Liked;
            this.GameName = dal.GameName;
            this.UserName = dal.UserName;


        }
        public override string ToString()
        {
            return $"CommentID: {CommentID} GameComment: {GameComment} UserID: {UserID} GameID: {GameID} Liked: {Liked} GameName: {GameName} UserName: {UserName}";
        }
    }
}
