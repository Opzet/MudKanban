using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MudKanban;

/// <summary>
/// A Kanban board component for MudBlazor.
/// Supports drag-and-drop cards between columns, WIP limits, and custom card templates.
/// </summary>
public class MudKanbanBoardBase : ComponentBase
{
    private KanbanCard? _draggedCard;

    /// <summary>Columns (lanes) displayed on the board.</summary>
    [Parameter]
    public IReadOnlyList<KanbanColumn> Columns { get; set; } = [];

    /// <summary>Cards displayed across all columns.</summary>
    [Parameter]
    public IReadOnlyList<KanbanCard> Cards { get; set; } = [];

    /// <summary>Raised when the <see cref="Cards"/> collection changes after a drag-and-drop.</summary>
    [Parameter]
    public EventCallback<IReadOnlyList<KanbanCard>> CardsChanged { get; set; }

    /// <summary>
    /// Optional custom template for rendering each card.
    /// When not set, a default MudCard layout is used.
    /// </summary>
    [Parameter]
    public RenderFragment<KanbanCard>? CardTemplate { get; set; }

    /// <summary>Raised when a card is moved to a different column.</summary>
    [Parameter]
    public EventCallback<KanbanCardMovedEventArgs> OnCardMoved { get; set; }

    /// <inheritdoc/>
    protected void HandleDragStart(KanbanCard card)
    {
        _draggedCard = card;
    }

    /// <inheritdoc/>
    protected void HandleDragOver()
    {
        // Needed to allow drop; default prevented in markup.
    }

    /// <inheritdoc/>
    protected async Task HandleDrop(string targetColumnId)
    {
        if (_draggedCard == null || _draggedCard.ColumnId == targetColumnId)
        {
            _draggedCard = null;
            return;
        }

        var previousColumnId = _draggedCard.ColumnId;
        var updatedCards = Cards
            .Select(c => c.Id == _draggedCard.Id
                ? new KanbanCard
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Assignee = c.Assignee,
                    DueDate = c.DueDate,
                    Priority = c.Priority,
                    ColumnId = targetColumnId,
                    Order = c.Order
                }
                : c)
            .ToList();

        await CardsChanged.InvokeAsync(updatedCards);
        await OnCardMoved.InvokeAsync(new KanbanCardMovedEventArgs(_draggedCard, previousColumnId, targetColumnId));

        _draggedCard = null;
    }

    /// <inheritdoc/>
    protected static string GetPriorityClass(KanbanPriority priority) => priority switch
    {
        KanbanPriority.Critical => "mud-kanban-card--critical",
        KanbanPriority.High => "mud-kanban-card--high",
        KanbanPriority.Low => "mud-kanban-card--low",
        _ => ""
    };

    /// <inheritdoc/>
    protected static string GetPriorityIcon(KanbanPriority priority) => priority switch
    {
        KanbanPriority.Critical => Icons.Material.Filled.PriorityHigh,
        KanbanPriority.High => Icons.Material.Filled.KeyboardArrowUp,
        KanbanPriority.Low => Icons.Material.Filled.KeyboardArrowDown,
        _ => Icons.Material.Filled.Remove
    };

    /// <inheritdoc/>
    protected static Color GetPriorityColor(KanbanPriority priority) => priority switch
    {
        KanbanPriority.Critical => Color.Error,
        KanbanPriority.High => Color.Warning,
        KanbanPriority.Low => Color.Info,
        _ => Color.Default
    };
}
