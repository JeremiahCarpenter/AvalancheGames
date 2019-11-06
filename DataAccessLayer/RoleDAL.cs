using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class RoleDAL
    {
        #region Direct Role Properties 
        //Direct role properties 
        //automatic properties 
        //stating the same items that are in the sqldatabase table
        //itnitializing properites 
        //this sets it up to be compaired and thus eventually created/updated/deleted
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        #endregion
        //method override is overriding the built in method called ToString and replaces it
        public override string ToString()
        {
            //ToString will now return
            //RoleID: (the id of the role) RoleName: (name of the role)  
            //in {} accessing the properties 
            return $"RoleID: {RoleID,5} RoleName: {RoleName}";
        }
    }
}
