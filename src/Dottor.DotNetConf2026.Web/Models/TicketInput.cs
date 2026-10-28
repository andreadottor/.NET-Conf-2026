using System.ComponentModel.DataAnnotations;

namespace Dottor.DotNetConf2026.Web.Models;

public sealed class TicketInput
{
    [Required(ErrorMessage = "Il titolo è obbligatorio.")]
    [StringLength(80, MinimumLength = 5, ErrorMessage = "Il titolo deve contenere da 5 a 80 caratteri.")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "La descrizione è obbligatoria.")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "La descrizione deve contenere da 10 a 2000 caratteri.")]
    public string Description { get; set; } = "";

    [Required(ErrorMessage = "L'email è obbligatoria.")]
    [EmailAddress(ErrorMessage = "Inserire un indirizzo email valido.")]
    public string Email { get; set; } = "";

    [Range(1, 4, ErrorMessage = "Scegliere una priorità da 1 a 4.")]
    public int Priority { get; set; }
}