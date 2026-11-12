namespace Dottor.DotNetConf2026.Web.Services;

public static class DemoCatalog
{
    public static IReadOnlyList<DemoDescription> All { get; } =
    [
        new("circuit-pause", "1. Circuit Pause & Resume", "Deployment locale e pausa automatica della scheda nascosta: persistenza dello stato utente e ripristino del circuito."),
        new("async-validation", "2. Validazione asincrona", "Univocità dell'email con AsyncValidationAttribute in Blazor e nelle Minimal API."),
        new("static-ssr-validation", "3. Validazione in Static SSR", "DataAnnotations nel browser senza circuito, e messaggio flash con TempData dopo il POST."),
        new("cacheview", "4. CacheView", "Caching dell'output di un sottoalbero in Static SSR, con scadenza e VaryByQuery."),
        new("virtualize", "5. Virtualize", "10.000 eventi ad altezza variabile, navigazione per indice e ancoraggio."),
        new("quickgrid", "6. QuickGrid", "Ordinamento e paginazione nell'URL, cronologia browser e click sulla riga."),
        new("openapi", "7. OpenAPI e union types", "OpenAPI 3.2, union C# 15 rappresentate mediante anyOf e [Obsolete] come deprecated.")
    ];
}

public sealed record DemoDescription(string Slug, string Title, string Description);