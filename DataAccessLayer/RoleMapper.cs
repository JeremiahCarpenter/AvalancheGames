using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class RoleMapper : Mapper
    {
        //making new empty fields 
        int OffsetToRoleID; //expected to be 0
        int OffsetToRoleName; //expected to be 1

        public RoleMapper(System.Data.SqlClient.SqlDataReader reader)
        {
            //checking fields to the sql data base to see if the columns line up (RoleID in this case)
            OffsetToRoleID = reader.GetOrdinal("RoleID");
            //assert takes a bool cond as parameter, and will show this error message if cond is false 
            Assert(0 == OffsetToRoleID, "The RoleID is not 0 as expected");
            //will proceed with no interruption if cond is true
            OffsetToRoleName = reader.GetOrdinal("RoleName");
            Assert(1 == OffsetToRoleName, "The RoleName is not 1 as expected");
        }

        //taking method called RoleFromReader from RoleDAL with parameters (reader from sql data reader)
        public RoleDAL RoleFromReader(System.Data.SqlClient.SqlDataReader reader)
        {
            //accessing RoleDAL and making new RoleDAL properties 
            RoleDAL propsedReturnValue = new RoleDAL();
            //New RoleDAL value = mapper(sqlreader,  rolemapper)
            propsedReturnValue.RoleID = GetInt32OrDefault(reader, OffsetToRoleID);
            propsedReturnValue.RoleName = GetStringOrDefault(reader, OffsetToRoleName);
            return propsedReturnValue;
        }
    }
}
