namespace MudKanban;

/// <summary>
/// Event arguments raised when a <see cref="KanbanCard"/> is moved between columns.
/// </summary>
public sealed class KanbanCardMovedEventArgs(KanbanCard card, string fromColumnId, string toColumnId)
{
    /// <summary>The card that was moved.</summary>
    public KanbanCard Card { get; } = card;

    /// <summary>The column the card was moved from.</summary>
    public string FromColumnId { get; } = fromColumnId;

    /// <summary>The column the card was moved to.</summary>
    public string ToColumnId { get; } = toColumnId;
}
