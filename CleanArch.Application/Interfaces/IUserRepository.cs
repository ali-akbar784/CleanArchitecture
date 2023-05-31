using CleanArch.Core.Entities;

namespace CleanArch.Application.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserLoginAsync(string userName, string password);
        Task<int> AddLoginAuditTrail(LoginAuditTrail entity);
        Task<decimal> GetUserBalance(int userId);
        Task<int> GetLoginAuditTrail(int userId);
        Task<int> AddUserBalance(int userId, decimal balance);
    }
}
