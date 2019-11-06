using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Logging;

namespace DataAccessLayer
{
    //Using IDisposable bc this class of ContextDAL is "heavy"
    //it releases unmanaged resouces 
    public class ContextDAL : IDisposable
    {
        #region Context Stuff
        //this connects to SQL and opens the connection to the database and we are calling this connection _connection
        SqlConnection _connection;
        //creating a constructor 
        public ContextDAL()
        {
            //initializes a new instance of the sqlconnection _connection
            _connection = new SqlConnection();
        }

        public string ConnectionString
        {
            //
            //get = return the property value _connectionstring 
            get { return _connection.ConnectionString; }
            //set = assigning value to _connectionstring
            set { _connection.ConnectionString = value; }
        }
        //new method used to check if connected
        void EnsureConnected()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                //there is nothing to do if I am connected
            }
            else if (_connection.State == System.Data.ConnectionState.Broken)
            {
                //if brokene, _connection will be closed and then opened
                _connection.Close();
                _connection.Open();
            }
            else if (_connection.State == System.Data.ConnectionState.Closed)
            {
                //if closed, will be opened
                _connection.Open();
            }
            else
            {
                //other states need no processing
            }
        }
        //Logging the excpetion(errors that occur during execution) being called ex
        bool Log(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            //Logging the exception called ex from the calss logger
            Logger.Log(ex);
            //when finished logging will return false to reset bool for next log check
            return false;
        }
        //calling the Dispose method that will release all resources used during this connection
        public void Dispose()
        {
            _connection.Dispose();
        }
        #endregion
        #region Simulate Exceptions for testing
        // this region is to create DAL layer exceptions on purpose in order to test them to see if the exceptions are properly caught and logged
        public int GenerateNotConnected()
        {
            int propsedReturnValue = -1;
            try
            {
                //by commenting out the EnsureConnected below, this method MAY throw an exeception IF it is the first call on ContextDAL
                //EnsureConnected();
                using (SqlCommand command = new SqlCommand("ObtainRoleCount", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    object answer = command.ExecuteScalar();
                    propsedReturnValue = (int)answer;
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return propsedReturnValue;
        }
        public int GenerateStoredProcedureNotFound()
        {
            int ProposedReturnValue = -1;
            try
            {
                EnsureConnected();
                //the name of the stored procedure is incorrect, so it should throw and exception
                using (SqlCommand command = new SqlCommand("xxxx", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    object answer = command.ExecuteScalar();
                    ProposedReturnValue = (int)answer;
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public int GenerateParametersNotIncluded()
        {
            int ProposedReturnValue = -1;
            try
            {
                EnsureConnected();
                //the parameter to the stored procedure is incorrect so it should thrown an exception
                using(SqlCommand command = new SqlCommand("FindRoleByRoleID", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    //the following line is where the parapeter name is incorrect
                    command.Parameters.AddWithValue("@xxxx", 1);
                    object answer = command.ExecuteReader();
                    ProposedReturnValue = (int)answer;
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        #endregion

        #region Role Stuff
        //Method Called FindRole 
        public RoleDAL FindRoleByRoleID(int RoleID)
        {
            RoleDAL propsosedReturnValue = null;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("FindRoleByRoleID", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RoleID", RoleID);
                    //need to configure the Text, Type and Parameters. 
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        RoleMapper mapper = new RoleMapper(reader);
                        int count = 0;
                        while (reader.Read())
                        {
                            propsosedReturnValue = mapper.RoleFromReader(reader);
                            count++;
                        }
                        if (count > 1)
                        {
                            throw new Exception($"Found more than 1 Role with key {RoleID}");
                        }
                    }
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return propsosedReturnValue;
        }
        public List<RoleDAL> GetRoles(int skip, int take)
        {
            List<RoleDAL> ProposedReturnValue = new List<RoleDAL>();
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("GetRoles", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@skip", skip);
                    command.Parameters.AddWithValue("@take", take);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        RoleMapper mapper = new RoleMapper(reader);
                        while (reader.Read())
                        {
                            RoleDAL role = mapper.RoleFromReader(reader);
                            ProposedReturnValue.Add(role);
                        }
                    }

                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public int ObtainRoleCount()
        {
            int ProposedReturnValue = -1;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("ObtainRoleCount", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    object answer = command.ExecuteScalar();
                    ProposedReturnValue = (int)answer;
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public int CreateRole(string RoleName)
        {
            int ProposedReturnValue = -1;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("CreateRole", _connection ))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RoleName", RoleName);
                    command.Parameters.AddWithValue("@RoleID", 0);
                    command.Parameters["@RoleID"].Direction = System.Data.ParameterDirection.Output;
                    command.ExecuteNonQuery();
                    ProposedReturnValue = Convert.ToInt32(command.Parameters["@RoleID"].Value);
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public void UpdateRole(int RoleID, string RoleName)
        {
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("JustUpdateRoles", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RoleName", RoleName);
                    command.Parameters.AddWithValue("@RoleID", RoleID);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
        }
        public void DeleteRole(int RoleID)
        {
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("DeleteRole", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RoleID", RoleID);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
        }
        #endregion
        #region User Stuff
        //creating a user (parameters are being called so when a user a created they will be filled)
        public int CreateUser(string FirstName, string LastName, string UserName, string Email, string SALT, string HASH, DateTime DateOfBirth, int RoleID)
        {
            int ProposedReturnValue = -1;
            try
            {
                //using try catches everywhere!!! this allows exceptions to be thrown and handle them
                //this ensures the application does not break
                //ensureconnected is a method literally made in this same level of ContextDAL and will ensure connection
                EnsureConnected();
                //using staties is used with an object that implements the Idisposable interface
                //using obtain resource, use it, then properly clean it(introduces a new scope into method and newly created object
                //once leaves scope the newly created object will invoke dispose method 
                using (SqlCommand command = new SqlCommand("CreateUser", _connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", 0);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@SALT", SALT);
                    command.Parameters.AddWithValue("@HASH", HASH);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@RoleID", RoleID);
                    command.Parameters["@UserID"].Direction = System.Data.ParameterDirection.Output; 
                    command.ExecuteNonQuery();
                    ProposedReturnValue = Convert.ToInt32(command.Parameters["@UserID"].Value); //userID is extracted from commandParameteers
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public void UpdateUser(int UserID, string FirstName, string LastName, string UserName, string Email, string SALT, string HASH, DateTime DateOfBirth, int RoleID)
        {
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("JustUpdateUsers", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@SALT", SALT);
                    command.Parameters.AddWithValue("@HASH", HASH);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@RoleID", RoleID);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
        }
        public void DeleteUser(int UserID)
        {
            try
            {
                EnsureConnected();
                using(SqlCommand command = new SqlCommand("DeleteUser", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
        }
        public UserDAL FindUserByUserID(int UserID)
        {
            UserDAL ProposedReturnValue = null;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("FindUserByUserID", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        UserMapper mapper = new UserMapper(reader);
                        int count = 0;
                        while (reader.Read())
                        {
                            ProposedReturnValue = mapper.UserFromReader(reader);
                            count++;
                        }
                        if (count > 1)
                        {
                            throw new Exception($"Found more then 1 User with key {UserID}");
                        }
                    }
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public UserDAL FindUserByUserEmail(string Email)
        {
            UserDAL ProposedReturnValue = null;
            try
            {
                EnsureConnected();
                using(SqlCommand command = new SqlCommand("FindUserByUserEmail", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", Email);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        UserMapper mapper = new UserMapper(reader);
                        int count = 0;
                        while (reader.Read())
                        {
                            ProposedReturnValue = mapper.UserFromReader(reader);
                            count++;
                        }
                        if (count > 1)
                        {
                            throw new Exception($"Found more than 1 User with key {Email}");
                        }
                    }
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }



        public UserDAL FindUserByUserName(string UserName)
        {
            UserDAL ProposedReturnValue = null;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("FindUserByUserName", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", UserName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        UserMapper mapper = new UserMapper(reader);//instantiates the UserMapper and references sqldatareader
                        int count = 0;
                        while (reader.Read())
                        {
                            ProposedReturnValue = mapper.UserFromReader(reader);
                            count++;
                        }
                        if (count > 1)
                        {
                            throw new Exception($"Found more than 1 User with key {UserName}");
                        }
                    }
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }



        //returning a list!!!
        public List<UserDAL> GetUsers(int skip, int take)
        {
            //making a new list called ProposedReturnValue
            List<UserDAL> ProposedReturnValue = new List<UserDAL>();
            try
            {
                EnsureConnected();//helper method
                //command is heavy
                using (SqlCommand command = new SqlCommand("GetUsers", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Skip", skip);//skip and take used for paging
                    command.Parameters.AddWithValue("@Take", take);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //verifing the shape 
                        UserMapper mapper = new UserMapper(reader);
                        //reader.read is the condition and when it returns false it ends the loop
                        //reader.read reads the process for SQL
                        while (reader.Read())
                        {
                            UserDAL user = mapper.UserFromReader(reader);
                            ProposedReturnValue.Add(user);
                        }
                    }
                }
            }
            catch (Exception ex) when (Log(ex))
            {
                
            }
            //populated list
            return ProposedReturnValue;
        }
        public List<UserDAL> GetUsersRelatedToRoleID (int RoleID, int skip, int take)
        {
            List<UserDAL> ProposedReturnValue = new List<UserDAL>();
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("GetUsersRelatedToUserID", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RoleID", RoleID);
                    command.Parameters.AddWithValue("@Skip", skip);
                    command.Parameters.AddWithValue("@Take", take);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        UserMapper mapper = new UserMapper(reader);
                        while (reader.Read())
                        {
                            UserDAL user = mapper.UserFromReader(reader);
                            ProposedReturnValue.Add(user);
                        }
                    }
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public int ObtainUserCount()
        {
            int ProposedReturnValue = -1;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("ObtainUserCount",  _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    object answer = command.ExecuteScalar();
                    ProposedReturnValue = (int)answer;
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        #endregion
        #region Game Stuff
        public int CreateGame(string GameName)
        {
            int ProposedReturnValue = -1;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("CreateGame", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@GameName", GameName);
                    command.Parameters.AddWithValue("@GameID", 0);
                    command.Parameters["@GameID"].Direction = System.Data.ParameterDirection.Output;
                    command.ExecuteNonQuery();
                    ProposedReturnValue = Convert.ToInt32(command.Parameters["@GameID"].Value);

                }
            }
            catch (Exception ex) when (Log(ex))
            {
               
            }
            return ProposedReturnValue;
        }
        public void UpdateGame(int GameID, string GameName)
        {
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("JustUpdateGames", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@GameName", GameName);
                    command.Parameters.AddWithValue("@GameID", GameID);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
        }
        public void DeleteGame(int GameID)
        {
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("DeleteGame", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@GameID", GameID);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
        }
        public GameDAL FindGameByGameID (int GameID)
        {
            GameDAL ProposedReturnValue = null;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("FindGameByGameID", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@GameID", GameID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        GameMapper mapper = new GameMapper(reader);
                        int count = 0;
                        while (reader.Read())
                        {
                            ProposedReturnValue = mapper.GameFromReader(reader);
                            count++;
                        }
                        if (count > 1)
                        {
                            throw new Exception($"Found more than 1 Game with key {GameID}");
                        }
                    }
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public List<GameDAL> GetGames(int skip, int take)
        {
            List<GameDAL> ProposedReturnValue = new List<GameDAL>();
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("GetGames",_connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Skip", skip);
                    command.Parameters.AddWithValue("@Take", take);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        GameMapper mapper = new GameMapper(reader);
                        while (reader.Read())
                        {
                            GameDAL game = mapper.GameFromReader(reader);
                            ProposedReturnValue.Add(game);
                        }
                    }
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public int ObtainGameCount()
        {
            int ProposedReturnValue = -1;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("ObtainGameCount", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    object answer = command.ExecuteScalar();
                    ProposedReturnValue = (int)answer;
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }

        #endregion
        #region Score Stuff
        public int CreateScore(int Score, int UserID, int GameID)
        {
            int ProposedReturnValue = -1;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("CreateScore", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Score", Score);
                    command.Parameters.AddWithValue("@ScoreID", 0);
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@GameID", GameID);
                    command.Parameters["@ScoreID"].Direction = System.Data.ParameterDirection.Output;
                    command.ExecuteNonQuery();
                    ProposedReturnValue = Convert.ToInt32(command.Parameters["@ScoreID"].Value);
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public void UpdateScore(int ScoreID, int Score, int UserID, int GameID)
        {
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("JustUpdateScores", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ScoreID", ScoreID);
                    command.Parameters.AddWithValue("@Score", Score);
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@GameID", GameID);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
        }
        public void DeleteScore(int ScoreID)
        {
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("DeleteScore", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ScoreID", ScoreID);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }

        }
        public ScoreDAL FindScoreByScoreID(int ScoreID)
        {
            ScoreDAL ProposedReturnValue = null;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("FindScoreByScoreID", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ScoreID", ScoreID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ScoreMapper mapper = new ScoreMapper(reader);
                        int count = 0;
                        while  (reader.Read())
                        {
                            ProposedReturnValue = mapper.ScoreFromReader(reader);
                            count++;
                        }
                        if (count > 1)
                        {
                            throw new Exception($"Found more than 1 Score with key {ScoreID}");
                        }
                    }
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public List<ScoreDAL> GetScores(int skip, int take)
        {
            List<ScoreDAL> ProposedReturnValue = new List<ScoreDAL>();
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("GetScores", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Skip", skip);
                    command.Parameters.AddWithValue("@Take", take);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ScoreMapper mapper = new ScoreMapper(reader);
                        while (reader.Read())
                        {
                            ScoreDAL score = mapper.ScoreFromReader(reader);
                            ProposedReturnValue.Add(score);
                        }
                    }
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public List<ScoreDAL> GetScoresRelatedToGameID(int GameID, int skip, int take)
        {
            List<ScoreDAL> ProposedReturnValue = new List<ScoreDAL>();
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("GetScoresRelatedToGameID", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@GameID", GameID);
                    command.Parameters.AddWithValue("@Skip", skip);
                    command.Parameters.AddWithValue("@Take", take);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ScoreMapper mapper = new ScoreMapper(reader);
                        while (reader.Read())
                        {
                            ScoreDAL score = mapper.ScoreFromReader(reader);
                            ProposedReturnValue.Add(score);
                        }
                    }
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public List<ScoreDAL> GetScoresRelatedToUserID(int UserID, int skip, int take)
        {
            List<ScoreDAL> ProposedReturnValue = new List<ScoreDAL>();
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("GetScoresRelatedToUserID", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@Skip", skip);
                    command.Parameters.AddWithValue("@Take", take);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ScoreMapper mapper = new ScoreMapper(reader);
                        while (reader.Read())
                        {
                            ScoreDAL score = mapper.ScoreFromReader(reader);
                            ProposedReturnValue.Add(score);
                        }
                    }
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public int ObtainScoreCount()
        {
            int ProposedReturnValue = -1;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("ObtainScoreCount", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    object answer = command.ExecuteScalar();
                    ProposedReturnValue = (int)answer;
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }

            return ProposedReturnValue;
        }

        public int ObtainUserScoreCount(int id)
        {
            int ProposedReturnValue = -1;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("ObtainUserScoreCount", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", id);
                    object answer = command.ExecuteScalar();
                    ProposedReturnValue = (int)answer;
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }

            return ProposedReturnValue;
        }
        #endregion
        #region Comment Stuff
        public int CreateComment(string GameComment, int UserID, int GameID, bool Liked)
        {
            int ProposedReturnValue = -1;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("CreateComment", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CommentID", 0);
                    command.Parameters.AddWithValue("@GameComment", GameComment);
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@GameID", GameID);
                    command.Parameters.AddWithValue("@Liked", Liked);
                    command.Parameters["@CommentID"].Direction = System.Data.ParameterDirection.Output;
                    command.ExecuteNonQuery();
                    ProposedReturnValue = Convert.ToInt32(command.Parameters["@CommentID"].Value);

                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public void UpdateComment(int CommentID, string GameComment, int UserID, int GameID, bool Liked)
        {
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("JustUpdateComments", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CommentID", CommentID);
                    command.Parameters.AddWithValue("@GameComment", GameComment);
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@GameID", GameID);
                    command.Parameters.AddWithValue("@Liked", Liked);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
        }
        public void DeleteComment(int CommentID)
        {
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("DeleteComment", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CommentID", CommentID);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
        }
        public CommentDAL FindCommentByCommentID(int CommentID)
        {
            CommentDAL ProposedReturnValue = null;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("FindCommentByCommentID", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CommentID", CommentID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        CommentMapper mapper = new CommentMapper(reader);
                        int count = 0;
                        while (reader.Read())
                        {
                            ProposedReturnValue = mapper.CommentFromReader(reader);
                            count++;
                        }
                        if (count > 1)
                        {
                            throw new Exception($"Found more than 1 Comment with key {CommentID}");
                        }
                    }
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }



        public CommentDAL FindCommentByLiked(bool Liked)
        {
            CommentDAL ProposedReturnValue = null;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("FindCommentByLiked", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Liked", Liked);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        CommentMapper mapper = new CommentMapper(reader);
                        int count = 0;
                        while (reader.Read())
                        {
                            ProposedReturnValue = mapper.CommentFromReader(reader);
                            count++;
                        }
                        if (count > 1)
                        {
                            throw new Exception($"Found more than 1 Comment with key {Liked}");
                        }
                    }
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }


        public List<CommentDAL> GetComments(int skip, int take)
        {
            List<CommentDAL> ProposedReturnValue = new List<CommentDAL>();
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("GetComments", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Skip", skip);
                    command.Parameters.AddWithValue("@Take", take);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        CommentMapper mapper = new CommentMapper(reader);
                        while (reader.Read())
                        {
                            CommentDAL comment = mapper.CommentFromReader(reader);
                            ProposedReturnValue.Add(comment);
                        }
                    }
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public List<CommentDAL> GetCommentsRelatedToGameID(int GameID, int skip, int take)
        {
            List<CommentDAL> ProposedReturnValue = new List<CommentDAL>();
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("GetCommentsRelatedToGameID", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@GameID", GameID);
                    command.Parameters.AddWithValue("@Skip", skip);
                    command.Parameters.AddWithValue("@Take", take);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        CommentMapper mapper = new CommentMapper(reader);
                        while (reader.Read())
                        {
                            CommentDAL comment = mapper.CommentFromReader(reader);
                            ProposedReturnValue.Add(comment);
                        }
                    }
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public List<CommentDAL> GetCommentsRelatedToUserID(int UserID, int skip, int take)
        {
            List<CommentDAL> ProposedReturnValue = new List<CommentDAL>();
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("GetCommentsRelatedToUserID", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@Skip", skip);
                    command.Parameters.AddWithValue("@Take", take);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        CommentMapper mapper = new CommentMapper(reader);
                        while (reader.Read())
                        {
                            CommentDAL comment = mapper.CommentFromReader(reader);
                            ProposedReturnValue.Add(comment);
                        }
                    }
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public int ObtainCommentCount()
        {
            int ProposedReturnValue = -1;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("ObtainCommentCount", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    object answer = command.ExecuteScalar();
                    ProposedReturnValue = (int)answer;
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }
            return ProposedReturnValue;
        }
        public int ObtainUserCommentCount(int id)
        {
            int ProposedReturnValue = -1;
            try
            {
                EnsureConnected();
                using (SqlCommand command = new SqlCommand("ObtainUserCommentCount", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", id);
                    object answer = command.ExecuteScalar();
                    ProposedReturnValue = (int)answer;
                }
            }
            catch (Exception ex) when (Log(ex))
            {

            }

            return ProposedReturnValue;
        }
        #endregion

    }
}
