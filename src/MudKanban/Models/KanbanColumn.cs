namespace MudKanban.Models;

/// <summary>Represents a column on the Kanban board.</summary>
public class KanbanColumn
{
    /// <summary>Unique identifier for the column.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Display title of the column.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Zero-based display order of the column on the board.</summary>
    public int Order { get; set; }

    /// <summary>
    /// Work-in-progress limit. When the number of cards in this column
    /// exceeds this value a visual warning is shown. <c>null</c> means no limit.
    /// </summary>
    public int? WipLimit { get; set; }
}
