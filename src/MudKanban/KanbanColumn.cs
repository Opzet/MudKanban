namespace MudKanban;

/// <summary>
/// Represents a column (lane) on the Kanban board.
/// </summary>
public class KanbanColumn
{
    /// <summary>Unique identifier for the column.</summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>Display title of the column.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Optional WIP (work-in-progress) limit. Null means no limit.</summary>
    public int? WipLimit { get; set; }

    /// <summary>Display order of the column on the board.</summary>
    public int Order { get; set; }
}
