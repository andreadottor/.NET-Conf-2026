namespace Dottor.DotNetConf2026.Web.Models;

/// <summary>
/// Legacy contract kept for the OpenAPI demo: [Obsolete] members are exported as "deprecated": true.
/// </summary>
public sealed record LegacyTicket(int Id, string Title, int Priority)
{
    [Obsolete("Usare Priority (1-4).")]
    public string? Severity { get; init; }
}
