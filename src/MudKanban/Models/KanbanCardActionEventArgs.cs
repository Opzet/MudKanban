namespace MudKanban.Models;

/// <summary>
/// Event payload emitted when a card action is selected.
/// </summary>
public sealed class KanbanCardActionEventArgs
{
    /// <summary>
    /// Card that owns the selected action.
    /// </summary>
    public required KanbanCard Card { get; init; }

    /// <summary>
    /// Selected action metadata.
    /// </summary>
    public required KanbanCardAction Action { get; init; }
}