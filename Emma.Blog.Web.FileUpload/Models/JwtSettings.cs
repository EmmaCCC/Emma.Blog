namespace Emma.Blog.Web.FileUpload.Models
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int Expires { get; set; }
    }
}
