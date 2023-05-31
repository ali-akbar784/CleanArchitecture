using System.Diagnostics.CodeAnalysis;

namespace CleanArch.Sql.Queries
{
    [ExcludeFromCodeCoverage]
	public static class UserQueries
	{
        //  public static string UserById => "SELECT * FROM [User] (NOLOCK) WHERE [UserId] = @UserId";
        //  public static string AuthenticateUser => "SELECT * FROM [User] (NOLOCK) WHERE [UserName] = @UserName and [Passwrod]= @Password";

        //  public static string AddUser =>
        //      @"INSERT INTO [User] ([FirstName], [LastName], [UserName], [Password],[Device],[IpAddress]) 
        //  		VALUES (@FirstName, @LastName, @UserName, @Password,@Device,@IpAddress)";
        //  public static string LoginAuditTrail =>
        //      @"INSERT INTO [LoginAuditTrail] ([FirstName], [LastName], [UserName], [Password],[Device],[IpAddress]) 
        //  		VALUES (@FirstName, @LastName, @UserName, @Password,@Device,@IpAddress)";

        //  public static string UpdateUser =>
        //@"UPDATE [User] 
        //            SET [FirstName] = @FirstName, 
        //  		[LastName] = @LastName, 
        //  		[Email] = @Email, 
        //  		[PhoneNumber] = @PhoneNumber
        //            WHERE [UserId] = @UserId";
        //  public static string GetUserLoginAudit => "SELECT Top 1 AuditId FROM [LoginAuditTrail] (NOLOCK) WHERE [UserId] = @UserId";
        //  public static string AddUserBalance =>
        //      @"INSERT INTO [UserBalance] ([UserId], [Balance]) 
        //  		VALUES (@UserId, @Balance)";
        //  public static string GetUserBalance =>
        //      @"SELECT * from [UserBalance] Where [UserId]= @UserId";

        //Mysql Queries
        public static string UserById => "SELECT * FROM User  WHERE UserId = @UserId";
        public static string AuthenticateUser => "SELECT * FROM  User WHERE  UserName  = @UserName and Password= @Password";

        public static string AddUser =>
            @"INSERT INTO User (FirstName, LastName, UserName, Password,Device,IpAddress) 
				VALUES (@FirstName, @LastName, @UserName, @Password,@Device,@IpAddress)";
        public static string LoginAuditTrail =>
            @"INSERT INTO LoginAuditTrail (UserId, DeviceId, IPAddress, LoginTime) 
				VALUES (@UserId, @DeviceId, @IPAddress, @LoginTime)";

        public static string UpdateUser =>
            @"UPDATE [User] 
            SET [FirstName] = @FirstName, 
				[LastName] = @LastName, 
				[Email] = @Email, 
				[PhoneNumber] = @PhoneNumber
            WHERE [UserId] = @UserId";
        public static string GetUserLoginAudit => "SELECT AuditId FROM LoginAuditTrail WHERE UserId = @UserId limit 1";
        public static string AddUserBalance =>
            @"INSERT INTO UserBalance (UserId, Balance) 
				VALUES (@UserId, @Balance)";
        public static string GetUserBalance =>
            @"SELECT Balance from UserBalance Where UserId= @UserId";
    }
}
