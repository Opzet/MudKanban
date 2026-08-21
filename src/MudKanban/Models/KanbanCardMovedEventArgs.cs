namespace MudKanban.Models;

/// <summary>Event arguments raised when a card is moved to a new column or position.</summary>
public class KanbanCardMovedEventArgs
{
    /// <summary>The card that was moved.</summary>
    public Guid CardId { get; init; }

    /// <summary>The column the card was dragged from.</summary>
    public Guid SourceColumnId { get; init; }

    /// <summary>The column the card was dropped into.</summary>
    public Guid TargetColumnId { get; init; }

    /// <summary>The zero-based index within the target column after the move.</summary>
    public int NewIndex { get; init; }
}
