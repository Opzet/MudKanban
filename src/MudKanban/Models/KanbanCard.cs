namespace MudKanban.Models;

/// <summary>Represents a single card on the Kanban board.</summary>
public class KanbanCard
{
    /// <summary>Unique identifier for the card.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Short title displayed on the card.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Longer description / body text of the card.</summary>
    public string? Description { get; set; }

    /// <summary>The column this card currently belongs to.</summary>
    public Guid ColumnId { get; set; }

    /// <summary>Zero-based display order within its column.</summary>
    public int Order { get; set; }

    /// <summary>Optional person assigned to this card.</summary>
    public string? Assignee { get; set; }

    /// <summary>Optional due date for the card.</summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// MudBlazor color name for the card accent (e.g. "primary", "secondary",
    /// "success", "warning", "error", "info", "default").
    /// </summary>
    public string Color { get; set; } = "default";
}
