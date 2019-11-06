using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class Mapper
    {
        //method named Assert is calling a condition with type bool and message with type string 
        public void Assert(bool condition, string message)
        {
            //if condition is false then an exception is thrown and message will be sho
            if (!condition)
            {
                throw new Exception(message);
            }
        }
        //method called GetstringOrDefault with type of string is calling in the parameters a SqlDataReader(this class allows for data to be read from the sql database) called reader, an int ordinal and string defaultValue
        public string GetStringOrDefault(SqlDataReader reader, int ordinal, string defaultValue = "")
        {
            //reader is accessing built in bool method DBNull(which checks to see if a value in column  in database contains non-existent or missing values) 
            //ordinal is checking the column location
            //reader is checking if it has nulls
            if (reader.IsDBNull(ordinal))
            {
                //if null the defualtValue string will be returned
                return defaultValue;
            }
            else
            {
                //if not null GetString will be return the value of the column as a string
                return reader.GetString(ordinal);
            }
        }
        //this method is very similar to the method above it
        //this method is just get an Int 
        public int GetInt32OrDefault(SqlDataReader reader, int ordinal, int defaultValue = 0)

        {
            if (reader.IsDBNull(ordinal))
            {
                return defaultValue;
            }
            else
            {
                return reader.GetInt32(ordinal);
            }
        }
        public DateTime GetDateTimeOrDefault(SqlDataReader reader, int ordinal, DateTime defaultValue)
        {
            if (reader.IsDBNull(ordinal))
            {
                return defaultValue;
            }
            else
            {
                return reader.GetDateTime(ordinal);
            }
        }
    }
}
