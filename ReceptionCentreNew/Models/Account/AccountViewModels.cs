using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Models;
public class LogOnModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [Display(Name = "Запомнить меня")]
    public bool RememberMe { get; set; }
    [Required]
    [Display(Name = "Логин")]
    public string Name { get; set; }
}

public class RegisterModel
{
    [Required]
    [Display(Name = "ФИО")]
    public string Name { get; set; }
    [Required]
    [Display(Name = "Логин")]
    public string Login { get; set; }
    [Display(Name = "Сотрудник")]
    public string EnterUser { get; set; }
    [Display(Name = "Роль")]
    public int roleId { get; set; }
    [Display(Name = "Поставщик")]
    public Guid ProviderId { get; set; }
    [Display(Name = "Лицевой счет")]
    public Guid ContractId { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Значение \"{0}\" должно содержать не менее {2} символов.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    //[DataType(DataType.Password)]
    [Display(Name = "Подтверждение пароля")]
    //[Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
    public string ConfirmPassword { get; set; }
}

public class ChangePasswordModel
{
    [Required]
    [Display(Name = "Логин")]
    public string Login { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Значение \"{0}\" должно содержать не менее {2} символов.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Текущий пароль")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Новый пароль")]
    [StringLength(100, ErrorMessage = "Значение \"{0}\" должно содержать не менее {2} символов.", MinimumLength = 6)]
    public string NewPassword { get; set; }
}

public static class Password
{
    public static string generatePassword()
    {
        char[] mas = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        char[] masInt = "1234567890".ToCharArray();
        char[] masSymbol = "!@#$%^&*()".ToCharArray();
        Random ran = new Random();
        string password = "";
        int indexInt = ran.Next(0, 6);
        for (int i = 0; i < 8; i++)
        {
            if (i == 7)
                password += masSymbol[ran.Next(0, masSymbol.Count())];
            else if (i == indexInt)
                password += masInt[ran.Next(0, masInt.Count())];
            else
                password += mas[ran.Next(0, mas.Count())];
        }
        return password;
    }
}