using KanbanCardModel = MudKanban.Models.KanbanCard;

namespace MudKanban.Tests;

public class KanbanCardTests
{
    [Fact]
    public void KanbanCard_DefaultColor_IsDefault()
    {
        var card = new KanbanCardModel { Title = "Test" };
        Assert.Equal("default", card.Color);
    }

    [Fact]
    public void KanbanCard_Id_IsGeneratedByDefault()
    {
        var card1 = new KanbanCardModel();
        var card2 = new KanbanCardModel();
        Assert.NotEqual(Guid.Empty, card1.Id);
        Assert.NotEqual(card1.Id, card2.Id);
    }
}
