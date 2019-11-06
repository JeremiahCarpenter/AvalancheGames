using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class UserDAL
    {
        #region Direct Properties
        //get property used to return property value
        //set property used to assign new value
        //automatic properties
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string SALT { get; set; }
        public string HASH { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int RoleID { get; set; }
        #endregion
        #region Indirect Properties 
        //from inner join
        //this is needed for showing Role names bc users shouldn't be able to see roleID
        public string RoleName { get; set; }
        #endregion
        //override allows child class to overide parent class with name parameters
        public override string ToString()
        {
            return $"User: UserID:{UserID} FirstName: {FirstName} LastName: {LastName} UserName: {UserName} Email: {Email}  SALT: {SALT} HASH: {HASH} DateOfBirth: {DateOfBirth} RoleID: {RoleID} RoleName: {RoleName} ";
        }
    }
}
