using System.ComponentModel.DataAnnotations;
namespace CityNews.Admin.Models
{
    public class LoginModel
    {
        [Display(Name = "用户姓名")]
        [Required(ErrorMessage = "必须填写用户姓名")]
        [MaxLength(20, ErrorMessage = "最大20个字符")]
        public string NickName { get; set; }
        [Display(Name = "密码")]
        [Required(ErrorMessage = "必须填写密码")]
        [MaxLength(30, ErrorMessage = "最大30个字符")]
        public string Password { get; set; }
    }
}