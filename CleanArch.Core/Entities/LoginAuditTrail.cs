

namespace CleanArch.Core.Entities
{
    public class LoginAuditTrail
    {
        public int UserId { get; set; }
        public string DeviceId { get; set; }
        public string IpAddress { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
