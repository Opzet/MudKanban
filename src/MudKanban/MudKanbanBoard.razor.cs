using Microsoft.AspNetCore.Components;
using MudKanban.Models;

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

    /// <summary>Starts dragging a specific card.</summary>
    protected void HandleDragStart(KanbanCard card)
    {
        ArgumentNullException.ThrowIfNull(card);
        _draggedCard = card;
    }

    /// <summary>Keeps drop targets active while dragging.</summary>
    protected void HandleDragOver()
    {
        // Needed to allow drop; default prevented in markup.
    }

    /// <summary>Drops the currently dragged card to the target column.</summary>
    protected async Task HandleDrop(Guid targetColumnId)
    {
        if (_draggedCard is null || _draggedCard.ColumnId == targetColumnId)
        {
            _draggedCard = null;
            return;
        }

        var draggedCardId = _draggedCard.Id;
        var sourceColumnId = _draggedCard.ColumnId;

        var sourceCards = Cards
            .Where(c => c.ColumnId == sourceColumnId)
            .OrderBy(c => c.Order)
            .ToList();
        sourceCards.RemoveAll(c => c.Id == draggedCardId);
        for (var i = 0; i < sourceCards.Count; i++)
        {
            sourceCards[i].Order = i;
        }

        var targetCards = Cards
            .Where(c => c.ColumnId == targetColumnId && c.Id != draggedCardId)
            .OrderBy(c => c.Order)
            .ToList();

        var movedCard = Cards.First(c => c.Id == draggedCardId);
        movedCard.ColumnId = targetColumnId;
        targetCards.Add(movedCard);

        for (var i = 0; i < targetCards.Count; i++)
        {
            targetCards[i].Order = i;
        }

        var updatedCards = Cards
            .Select(c => c.Id == draggedCardId
                ? movedCard
                : c)
            .ToList();

        await CardsChanged.InvokeAsync(updatedCards);
        await OnCardMoved.InvokeAsync(new KanbanCardMovedEventArgs
        {
            CardId = draggedCardId,
            SourceColumnId = sourceColumnId,
            TargetColumnId = targetColumnId,
            NewIndex = targetCards.Count - 1
        });

        _draggedCard = null;
    }
}
