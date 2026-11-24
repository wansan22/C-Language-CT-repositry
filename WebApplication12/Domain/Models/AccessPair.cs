using System;

namespace Domain.Models
{
    public class AccessPair
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
