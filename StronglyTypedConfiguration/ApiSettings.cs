namespace StronglyTypedConfiguration
{
    public class ApiSettings
    {
        public string AdminEmail { get; set; }
        public JwtSettings JwtSettings { get; set; }
    }

    public class JwtSettings
    {
        public string SigningKey { get; set; }
    }
}
