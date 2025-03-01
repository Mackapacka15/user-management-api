namespace UserManagement.Authentication
{
    public class JwtOptions
    {
        public const string SectionName = "JwtOptions";
        public string SecretKey { get; init; } = null!;
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
    }
}
