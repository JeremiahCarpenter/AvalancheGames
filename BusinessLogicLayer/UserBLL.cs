using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer
{
    //maps from dal
    public class UserBLL
    {
        public UserBLL()
        {
            //empty constructor needed for MVC
        }
        #region Direct Properties
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string SALT { get; set; }
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string HASH { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int RoleID { get; set; }
        #endregion
        #region Indirect Properties
        public string RoleName { get; set; }
        #endregion
        public UserBLL(UserDAL dal)
        {
            this.UserID = dal.UserID;
            this.FirstName = dal.FirstName;
            this.LastName = dal.LastName;
            this.UserName = dal.UserName;
            this.Email = dal.Email;
            this.SALT = dal.SALT;
            this.HASH = dal.HASH;
            this.DateOfBirth = dal.DateOfBirth;
            this.RoleID = dal.RoleID;
            this.RoleName = dal.RoleName;
        }
        public  override string ToString()
        {
            return $"UserID: {UserID} FirstName: {FirstName} LastName: {LastName} UserName: {UserName} Email: {Email} SALT: {SALT} HASH: {HASH} DateOfBirth: {DateOfBirth} RoleID: {RoleID} RoleName: {RoleName}";
        }
    }
}
