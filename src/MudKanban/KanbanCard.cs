namespace MudKanban;

/// <summary>
/// Represents a single card on the Kanban board.
/// </summary>
public class KanbanCard
{
    /// <summary>Unique identifier for the card.</summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>Title displayed on the card.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Optional description text.</summary>
    public string? Description { get; set; }

    /// <summary>Assignee name or identifier.</summary>
    public string? Assignee { get; set; }

    /// <summary>Optional due date for the card.</summary>
    public DateTimeOffset? DueDate { get; set; }

    /// <summary>Priority level of the card.</summary>
    public KanbanPriority Priority { get; set; } = KanbanPriority.Normal;

    /// <summary>The column (lane) this card belongs to.</summary>
    public string ColumnId { get; set; } = string.Empty;

    /// <summary>Display order within the column.</summary>
    public int Order { get; set; }
}
