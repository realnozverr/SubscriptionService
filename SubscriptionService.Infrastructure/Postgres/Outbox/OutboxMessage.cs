namespace SubscriptionService.Infrastructure.Postgres.Outbox;

/// <summary>
/// Сообщение в таблице outbox.
/// </summary>
public sealed class OutboxMessage
{
    /// <summary>
    /// Идентификатор сообщения.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Тип доменного события (например, "SubscriptionCreatedEvent").
    /// </summary>
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Содержимое доменного события, сериализованное в JSON.
    /// </summary>
    public string Content { get; init; } = string.Empty;

    /// <summary>
    /// Дата и время возникновения события в UTC.
    /// </summary>
    public DateTime OccurredOnUtc { get; init; }

    /// <summary>
    /// Дата и время обработки сообщения в UTC.
    /// Если null, сообщение еще не было успешно отправлено.
    /// </summary>
    public DateTime? ProcessedOnUtc { get; private set; }

    /// <summary>
    /// Текст ошибки, если при обработке произошел сбой.
    /// </summary>
    public string? Error { get; private set; }

    /// <summary>
    /// Пометка о выполнении ивента.
    /// </summary>
    public void MarkProcessed()
    {
        ProcessedOnUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Пометка о возникновении ошибки
    /// </summary>
    /// <param name="error">Ошибка</param>
    public void MarkFailed(string error)
    {
        Error = error;
    }
}