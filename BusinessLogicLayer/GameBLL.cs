using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class GameBLL
    {
        public GameBLL()
        {

        }

        #region Properties
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int GameID { get; set; }
        public string GameName { get; set; }
        #endregion
        public GameBLL(GameDAL dal)
        {
            this.GameID = dal.GameID;
            this.GameName = dal.GameName;
        }
        public override string ToString()
        {
            return $"GameID: {GameID} GameName: {GameName}";
        }

    }
}
