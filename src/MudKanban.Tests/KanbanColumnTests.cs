namespace MudKanban.Tests;

public class KanbanColumnTests
{
    [Fact]
    public void KanbanColumn_Id_IsGeneratedByDefault()
    {
        var col1 = new KanbanColumn { Title = "To Do" };
        var col2 = new KanbanColumn { Title = "In Progress" };
        Assert.NotEmpty(col1.Id);
        Assert.NotEqual(col1.Id, col2.Id);
    }

    [Fact]
    public void KanbanColumn_WipLimit_DefaultIsNull()
    {
        var column = new KanbanColumn { Title = "Test" };
        Assert.Null(column.WipLimit);
    }
}
