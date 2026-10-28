namespace Dottor.DotNetConf2026.Web.Models;

public sealed record LogEntry(int Id, DateTime Timestamp, string Title, string Description, string Severity);