using MudKanban.Models;
using Xunit;

namespace MudKanban.Tests;

public class KanbanModelTests
{
    [Fact]
    public void KanbanCard_DefaultValues_AreValid()
    {
        var card = new KanbanCard();
        Assert.NotEqual(Guid.Empty, card.Id);
        Assert.Equal(string.Empty, card.Title);
        Assert.Equal("default", card.Color);
        Assert.Null(card.Assignee);
        Assert.Null(card.DueDate);
    }

    [Fact]
    public void KanbanColumn_DefaultValues_AreValid()
    {
        var col = new KanbanColumn();
        Assert.NotEqual(Guid.Empty, col.Id);
        Assert.Equal(string.Empty, col.Title);
        Assert.Null(col.WipLimit);
    }

    [Fact]
    public void KanbanCardMovedEventArgs_PropertiesSet()
    {
        var cardId = Guid.NewGuid();
        var src = Guid.NewGuid();
        var tgt = Guid.NewGuid();
        var args = new KanbanCardMovedEventArgs
        {
            CardId = cardId,
            SourceColumnId = src,
            TargetColumnId = tgt,
            NewIndex = 2
        };
        Assert.Equal(cardId, args.CardId);
        Assert.Equal(src, args.SourceColumnId);
        Assert.Equal(tgt, args.TargetColumnId);
        Assert.Equal(2, args.NewIndex);
    }
}
