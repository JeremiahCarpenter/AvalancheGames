using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ScoreDAL
    {
        #region Direct Properties
        public int ScoreID { get; set; }
        public int Score { get; set; }
        public int UserID { get; set; }
        public int GameID { get; set; }
        #endregion
        #region Indirect Properties
        public string UserName { get; set; }
        public string GameName { get; set; }
        #endregion
        public override string ToString()
        {
            return $"Score: ScoreID: {ScoreID,5} Score: {Score,10} UserID: {UserID}  GameID: {GameID} Email: {UserName,25} GameName: {GameName,25}";
        }
    }
}
