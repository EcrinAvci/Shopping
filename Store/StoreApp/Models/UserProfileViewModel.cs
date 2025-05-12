using System.ComponentModel.DataAnnotations;

namespace StoreApp.Models
{
    public class UserProfileViewModel
    {
        public string? UserName { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [Display(Name = "E-posta")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [Display(Name = "Telefon Numarası")]
        [RegularExpression(@"^[0-9\s]*$", ErrorMessage = "Sadece rakam ve boşluk kullanabilirsiniz")]
        [StringLength(14, MinimumLength = 10, ErrorMessage = "Telefon numarası 10-14 karakter arasında olmalıdır")]
        public string? PhoneNumber { get; set; }
    }
} 