using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class RoleBLL
    {
        public RoleBLL()
        {

        }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public RoleBLL(DataAccessLayer.RoleDAL dal)
        {
            this.RoleID = dal.RoleID;
            this.RoleName = dal.RoleName;
        }
        public override string ToString()
        {
            return $"RoleID: {RoleID} RoleName {RoleName}";
        }
    }

}
