using System.ComponentModel.DataAnnotations;

namespace Multi_level.Helper
{
    public class CaptchaResult
    {
        public byte[] CaptchaByteData { get; set; }
        public string CaptchBase64Data => Convert.ToBase64String(CaptchaByteData);
        public DateTime Timestamp { get; set; }
        [Required]
        [StringLength(6)]
        public string CaptchaCode { get; set; }
    }
}