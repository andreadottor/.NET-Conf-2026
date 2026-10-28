namespace Dottor.DotNetConf2026.Web.Services;

public static class DemoCatalog
{
    public static IReadOnlyList<DemoDescription> All { get; } =
    [
        new("circuit-pause", "1. Circuit Pause & Resume", "Deployment locale: persistenza dello stato utente e ripristino su una nuova istanza."),
        new("async-validation", "2. Validazione asincrona", "Univocità dell'email con AsyncValidationAttribute in Blazor e nelle Minimal API."),
        new("static-ssr-validation", "3. Validazione in Static SSR", "DataAnnotations nel browser, senza circuito e senza richieste HTTP per gli errori."),
        new("virtualize", "4. Virtualize", "10.000 eventi ad altezza variabile, navigazione per indice e ancoraggio."),
        new("quickgrid", "5. QuickGrid", "Ordinamento e paginazione nell'URL, cronologia browser e click sulla riga."),
        new("openapi", "6. OpenAPI e union types", "OpenAPI 3.2 e union C# 15 rappresentate mediante anyOf.")
    ];
}

public sealed record DemoDescription(string Slug, string Title, string Description);