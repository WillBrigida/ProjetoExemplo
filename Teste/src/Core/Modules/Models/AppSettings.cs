namespace Core.Modules.Models
{
    public class DBSettings
    {
        public string? Host { get; set; }
        public string? Port { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
    }

    public class JWTSettings
    {
        public string? Secret { get; set; }
        public int ExpirationHours { get; set; }
        public string? ValidIssuer { get; set; }
        public string? ValidAudience { get; set; }
    }

    public class URISettings
    {
        public string? UriBase { get; set; }
    }
}
