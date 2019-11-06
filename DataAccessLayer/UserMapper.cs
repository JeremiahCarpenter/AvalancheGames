using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    //three parts to this class
    public class UserMapper : Mapper
    {
        //feilds
        //checking shape (column offset) in database //int is primitive type(bulit in typs are primative) efficient 
        int OffsetToUserID; //expected to be 0
        int OffsetToFirstName; //expected to be 1
        int OffsetToLastName; //expected to be 2
        int OffsetToUserName; //expected to be 3
        int OffsetToEmail;
        int OffsetToSALT;
        int OffsetToHASH;
        int OffsetToDateOfBirth;
        int OffsetToRoleID;
        int OffsetToRoleName; //expected to be 9

        //constructor part
        public UserMapper (System.Data.SqlClient.SqlDataReader reader)
        {
            //checking fields to the sql data base to see if the columns line up
            OffsetToUserID = reader.GetOrdinal("UserID");
            //assert takes a bool cond as parameter, and will show this error message if cond is false 
            Assert(0 == OffsetToUserID, $"UserID is {OffsetToUserID} not 0 as expected");
            //will proceed with no interruption if cond is true
            OffsetToFirstName = reader.GetOrdinal("FirstName");
            Assert(1 == OffsetToFirstName, $"FirstName is {OffsetToFirstName} not 1 as expected");
            OffsetToLastName = reader.GetOrdinal("LastName");
            Assert(2 == OffsetToLastName, $"LastName is {OffsetToLastName} not 2 as expected");
            OffsetToUserName = reader.GetOrdinal("UserName");
            Assert(3 == OffsetToUserName, $"UserName is {OffsetToUserName} not 3 as expected");
            OffsetToEmail = reader.GetOrdinal("Email");
            Assert(4 == OffsetToEmail, $"Email is {OffsetToEmail} not 4 as expected");
            OffsetToSALT = reader.GetOrdinal("SALT");
            Assert(5 == OffsetToSALT, $"SALT is {OffsetToSALT} not 5 as expected");
            OffsetToHASH = reader.GetOrdinal("HASH");
            Assert(6 == OffsetToHASH, $"HASH is {OffsetToHASH} not 6 as expected");
            OffsetToDateOfBirth = reader.GetOrdinal("DateOfBirth");
            Assert(7 == OffsetToDateOfBirth, $"DateOfBirth is {OffsetToDateOfBirth} not 7 as expected");
            OffsetToRoleID = reader.GetOrdinal("RoleID");
            Assert(8 == OffsetToRoleID, $"RoleID is {OffsetToRoleID} not 8 as expected");
            OffsetToRoleName = reader.GetOrdinal("RoleName");
            Assert(9 == OffsetToRoleName, $"RoleName is {OffsetToRoleName} not 9 as expected");
        }
        //method that does all the work
        public UserDAL UserFromReader(System.Data.SqlClient.SqlDataReader reader)
        { //mapping the record 
            UserDAL ProposedReturnValue = new UserDAL(); //instantiating
            //primitive type of
            //reader["UserID"]  is very slow and makes a lot of garbage
            //reader[0] makes a lot of garbage
            //reader.GetInt32(0) is fast, but hard codes the offset to 0
            //reader.GetInt32(OffsetToUserID) is best and allows verification
            //verifing user userdal from reader 
            ProposedReturnValue.UserID = reader.GetInt32(OffsetToUserID);
            ProposedReturnValue.FirstName = reader.GetString(OffsetToFirstName);
            ProposedReturnValue.LastName = reader.GetString(OffsetToLastName);
            ProposedReturnValue.UserName = reader.GetString(OffsetToUserName);
            ProposedReturnValue.Email = reader.GetString(OffsetToEmail);
            ProposedReturnValue.SALT = reader.GetString(OffsetToSALT);
            ProposedReturnValue.HASH = reader.GetString(OffsetToHASH);
            ProposedReturnValue.DateOfBirth = this.GetDateTimeOrDefault(reader,OffsetToDateOfBirth,new DateTime(1800,01,01));
            ProposedReturnValue.RoleID = reader.GetInt32(OffsetToRoleID);
            ProposedReturnValue.RoleName = reader.GetString(OffsetToRoleName);
            return ProposedReturnValue;
        }
    }
}
