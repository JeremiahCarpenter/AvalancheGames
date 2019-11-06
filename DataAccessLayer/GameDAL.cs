using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class GameDAL
    {
        public int GameID { get; set; }
        public string GameName { get; set; }
        public override string ToString()
        {
            return $"GameID: {GameID,5} GameName: {GameName}";
        }
    }
}
