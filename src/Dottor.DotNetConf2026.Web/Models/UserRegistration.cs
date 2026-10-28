using System.ComponentModel.DataAnnotations;
using Dottor.DotNetConf2026.Web.Validation;

namespace Dottor.DotNetConf2026.Web.Models;

public sealed class UserRegistration
{
    [Required(ErrorMessage = "Il nome è obbligatorio.")]
    public string FullName { get; set; } = "";

    [Required(ErrorMessage = "L'email è obbligatoria.")]
    [EmailAddress(ErrorMessage = "Inserire un indirizzo email valido.")]
    [UniqueEmail]
    public string Email { get; set; } = "";

    [Range(18, 100, ErrorMessage = "L'età deve essere compresa tra 18 e 100 anni.")]
    public int Age { get; set; } = 18;
}