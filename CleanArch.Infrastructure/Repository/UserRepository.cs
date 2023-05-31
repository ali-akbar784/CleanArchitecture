using CleanArch.Application.Interfaces;
using CleanArch.Core.Entities;
using CleanArch.Sql.Queries;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace CleanArch.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration configuration;
        // dummy list is created here as db connection is not created yet
        private readonly MySqlConnectionStringBuilder connectionString;
        private List<User> _users = new List<User> {
            new User {
                UserId=1,FirstName = "mytest", LastName = "User", UserName = "mytestuser", Password = "test123"
            },
            new User {
                UserId = 2, FirstName = "mytest2", LastName = "User2", UserName = "aliakbar", Password = "EE60315C8F7D7A1F5883713DEDEDCDE2"
            }
        };
        public UserRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = new MySqlConnectionStringBuilder(configuration.GetConnectionString("DBConnection"));
        }
        public async Task<User> GetByIdAsync(long id)
        {
            //    return _users.Where(x => x.UserId == id).FirstOrDefault();
            //using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
            using (IDbConnection connection = new MySqlConnection(connectionString.ConnectionString))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<User>(UserQueries.UserById, new { UserId = id });
                return result;
            }
        }
        public async Task<User> GetUserLoginAsync(string userName, string password)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString.ConnectionString))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<User>(UserQueries.AuthenticateUser, new { UserName = userName, Password = password });
                return result;
            }
        }

        public async Task<int> AddAsync(User entity)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString.ConnectionString))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(UserQueries.AddUser, entity);
                return result;
            }
        }
        public async Task<int> AddLoginAuditTrail(LoginAuditTrail entity)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString.ConnectionString))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(UserQueries.LoginAuditTrail, entity);
                return result;
            }
        }

        public async Task<string> UpdateAsync(User entity)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString.ConnectionString))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(UserQueries.UpdateUser, entity);
                return result.ToString();
            }
        }
        public async Task<int> GetLoginAuditTrail(int userId)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString.ConnectionString))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<int>(UserQueries.GetUserLoginAudit, new { UserId = userId });
                return result ;
            }

        }
        public async Task<int> AddUserBalance(int userId, decimal balance)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString.ConnectionString))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(UserQueries.AddUserBalance, new { UserId = userId, Balance = balance });
                return result;
            }
        }
        public async Task<decimal> GetUserBalance(int userId)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString.ConnectionString))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<decimal>(UserQueries.GetUserBalance, new { UserId = userId });
                return result;
            }
        }
    }
}
