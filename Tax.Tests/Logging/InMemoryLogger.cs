using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Tax.Tests.Logging;

public sealed class LogEntry
{
    public LogLevel LogLevel { get; init; }
    public string Category { get; init; } = string.Empty;
    public EventId EventId { get; init; }
    public string Message { get; init; } = string.Empty;
    public Exception? Exception { get; init; }
    public List<object> Scopes { get; } = new List<object>();
}

public sealed class InMemoryLoggerProvider : ILoggerProvider, ISupportExternalScope
{
    private IExternalScopeProvider? _scopeProvider;
    private readonly ConcurrentBag<LogEntry> _logs = new();

    public IEnumerable<LogEntry> Logs => _logs;

    public ILogger CreateLogger(string categoryName) => new InMemoryLogger(categoryName, _logs, GetScopeProvider());

    public void Dispose() { }

    public void SetScopeProvider(IExternalScopeProvider scopeProvider) => _scopeProvider = scopeProvider;

    private IExternalScopeProvider GetScopeProvider() => _scopeProvider ??= new LoggerExternalScopeProvider();
}

internal sealed class InMemoryLogger : ILogger
{
    private readonly string _category;
    private readonly ConcurrentBag<LogEntry> _logs;
    private readonly IExternalScopeProvider _scopeProvider;

    public InMemoryLogger(string category, ConcurrentBag<LogEntry> logs, IExternalScopeProvider scopeProvider)
    {
        _category = category;
        _logs = logs;
        _scopeProvider = scopeProvider;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => _scopeProvider.Push(state);

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);
        var entry = new LogEntry
        {
            LogLevel = logLevel,
            Category = _category,
            EventId = eventId,
            Message = message,
            Exception = exception
        };

        // capture current scopes
        _scopeProvider.ForEachScope((scope, acc) =>
        {
            entry.Scopes.Add(scope!);
        }, state);

        _logs.Add(entry);
    }
}
