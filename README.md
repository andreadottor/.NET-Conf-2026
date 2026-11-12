# Novità in ASP.NET Core 11 & Blazor

Demo della sessione tenuta durante la **.NET Conf 2026 XE**.

Una sola Blazor Web App su .NET 11, con dati in memoria e nessun servizio esterno: ogni demo mostra una novità di ASP.NET Core 11 e Blazor.

## Le demo

| # | Demo | Pagina | Cosa mostra |
| --- | --- | --- | --- |
| 1 | Circuit Pause & Resume | `/demo/circuit-pause` | Il circuito sopravvive al riavvio del server: stato ripristinato senza ricaricare la pagina |
| 2 | Validazione asincrona | `/demo/async-validation` | `AsyncValidationAttribute` in Blazor e nelle Minimal API |
| 3 | Validazione client-side in Static SSR | `/demo/static-ssr-validation` | DataAnnotations validate nel browser, senza circuito |
| 4 | Virtualize | `/demo/virtualize` | Altezze variabili misurate a runtime e `AnchorMode` |
| 5 | QuickGrid | `/demo/quickgrid` · `/demo/quickgrid-static` | Ordinamento e paginazione nell'URL, `OnRowClick` |
| 6 | OpenAPI 3.2 e union C# 15 | `/demo/openapi` | Una union nativa descritta come `anyOf` nel documento OpenAPI |

## Requisiti

- **.NET 11 SDK** (`11.0.100`, fissato in `global.json`)
- Visual Studio 2026 oppure la CLI `dotnet`

## Avvio

```powershell
dotnet run --project .\src\Dottor.DotNetConf2026.Web --launch-profile https
```

In alternativa, da Visual Studio usare **Ctrl+F5**. Poi aprire https://dottor-dotnetconf2026-web.dev.localhost:7246.

Per la demo 1, con l'app avviata senza debugger, simulare un nuovo deployment da un secondo terminale:

```powershell
.\scripts\deploy-demo.ps1
```

Lo script chiude l'istanza corrente in modo controllato e la riavvia con una nuova versione sugli stessi URL del profilo, cioè HTTP e HTTPS.

## Documentazione

- [Novità di ASP.NET Core 11](https://learn.microsoft.com/aspnet/core/release-notes/aspnetcore-11?view=aspnetcore-11.0)
- [Pausa e ripresa del circuito](https://learn.microsoft.com/aspnet/core/blazor/state-management/server?view=aspnetcore-11.0#server-triggered-circuit-pause)
- [Validazione asincrona](https://learn.microsoft.com/aspnet/core/blazor/forms/validation-advanced?view=aspnetcore-11.0#add-asynchronous-validation)
- [Validazione client-side in Static SSR](https://learn.microsoft.com/aspnet/core/blazor/forms/validation-client-side?view=aspnetcore-11.0)
- [Virtualizzazione](https://learn.microsoft.com/aspnet/core/blazor/components/virtualization?view=aspnetcore-11.0)
- [QuickGrid](https://learn.microsoft.com/aspnet/core/blazor/components/quickgrid?view=aspnetcore-11.0)
- [Union in C# 15](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/union)