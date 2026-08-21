namespace MudKanban.Tests;

public class KanbanCardTests
{
    [Fact]
    public void KanbanCard_DefaultPriority_IsNormal()
    {
        var card = new KanbanCard { Title = "Test" };
        Assert.Equal(KanbanPriority.Normal, card.Priority);
    }

    [Fact]
    public void KanbanCard_Id_IsGeneratedByDefault()
    {
        var card1 = new KanbanCard();
        var card2 = new KanbanCard();
        Assert.NotEmpty(card1.Id);
        Assert.NotEqual(card1.Id, card2.Id);
    }
}
