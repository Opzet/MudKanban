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
    private Guid? _dragOverColumnId;
    private int? _dragOverIndex;

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

    /// <summary>True when any card is currently being dragged.</summary>
    protected bool IsDragging => _draggedCard is not null;

    /// <summary>Starts dragging a specific card.</summary>
    protected void HandleDragStart(KanbanCard card)
    {
        ArgumentNullException.ThrowIfNull(card);

        _draggedCard = card;
        _dragOverColumnId = card.ColumnId;
        _dragOverIndex = card.Order;
    }

    /// <summary>Ends dragging and clears visual drag state.</summary>
    protected void HandleDragEnd()
    {
        ResetDragState();
    }

    /// <summary>Keeps drop targets active while dragging.</summary>
    protected void HandleDragOver()
    {
        // Needed to allow drop; default prevented in markup.
    }

    /// <summary>Tracks the active drop zone index while dragging over a column.</summary>
    protected void HandleDropZoneEnter(Guid columnId, int index)
    {
        if (_draggedCard is null)
        {
            return;
        }

        _dragOverColumnId = columnId;
        _dragOverIndex = index;
    }

    /// <summary>Whether a specific drop zone should display the drag skeleton preview.</summary>
    protected bool IsDropZoneActive(Guid columnId, int index)
    {
        return _draggedCard is not null &&
               _dragOverColumnId == columnId &&
               _dragOverIndex == index;
    }

    /// <summary>Whether the given column is the current active drop target.</summary>
    protected bool IsColumnDropTarget(Guid columnId)
    {
        return _draggedCard is not null && _dragOverColumnId == columnId;
    }

    /// <summary>Whether a card should visually bump to make space for the active drop position.</summary>
    protected bool IsCardBumped(Guid columnId, int cardIndex)
    {
        if (_draggedCard is null || _dragOverColumnId != columnId || _dragOverIndex is null)
        {
            return false;
        }

        return cardIndex >= _dragOverIndex.Value;
    }

    /// <summary>Returns cards for a column while hiding the currently dragged card from its source lane.</summary>
    protected IReadOnlyList<KanbanCard> GetRenderableCards(Guid columnId)
    {
        var cards = Cards
            .Where(c => c.ColumnId == columnId)
            .OrderBy(c => c.Order)
            .ToList();

        if (_draggedCard is null)
        {
            return cards;
        }

        cards.RemoveAll(c => c.Id == _draggedCard.Id);
        return cards;
    }

    /// <summary>Drops the currently dragged card to the target column and index.</summary>
    protected async Task HandleDrop(Guid targetColumnId, int targetIndex)
    {
        if (_draggedCard is null)
        {
            ResetDragState();
            return;
        }

        var movedCard = _draggedCard;
        var movedCardId = movedCard.Id;
        var sourceColumnId = movedCard.ColumnId;

        var cardsByColumn = Cards
            .Where(c => c.Id != movedCardId)
            .GroupBy(c => c.ColumnId)
            .ToDictionary(
                group => group.Key,
                group => group.OrderBy(card => card.Order).ToList());

        if (!cardsByColumn.TryGetValue(targetColumnId, out var targetCards))
        {
            targetCards = [];
            cardsByColumn[targetColumnId] = targetCards;
        }

        var boundedIndex = Math.Clamp(targetIndex, 0, targetCards.Count);

        movedCard.ColumnId = targetColumnId;
        targetCards.Insert(boundedIndex, movedCard);

        foreach (var cardList in cardsByColumn.Values)
        {
            for (var i = 0; i < cardList.Count; i++)
            {
                cardList[i].Order = i;
            }
        }

        var updatedCards = Cards
            .Select(card => card.Id == movedCardId ? movedCard : card)
            .ToList();

        await CardsChanged.InvokeAsync(updatedCards);
        await OnCardMoved.InvokeAsync(new KanbanCardMovedEventArgs
        {
            CardId = movedCardId,
            SourceColumnId = sourceColumnId,
            TargetColumnId = targetColumnId,
            NewIndex = boundedIndex
        });

        ResetDragState();
    }

    /// <summary>Whether a given card is currently being dragged.</summary>
    protected bool IsDraggedCard(Guid cardId)
    {
        return _draggedCard?.Id == cardId;
    }

    private void ResetDragState()
    {
        _draggedCard = null;
        _dragOverColumnId = null;
        _dragOverIndex = null;
    }
}
