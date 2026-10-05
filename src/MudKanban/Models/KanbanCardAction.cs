using MudBlazor;

namespace MudKanban.Models;

/// <summary>
/// Describes a selectable action displayed on a Kanban card.
/// </summary>
public class KanbanCardAction
{
    /// <summary>
    /// Stable action identifier (for example: "edit", "assign", "delete").
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Visible action label shown in the card menu.
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Optional MudBlazor icon name for the action.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Optional color to emphasize important actions.
    /// </summary>
    public Color Color { get; set; } = Color.Default;

    /// <summary>
    /// Disables selection when true.
    /// </summary>
    public bool Disabled { get; set; } = false;
}