using KanbanColumnModel = MudKanban.Models.KanbanColumn;

namespace MudKanban.Tests;

public class KanbanColumnTests
{
    [Fact]
    public void KanbanColumn_Id_IsGeneratedByDefault()
    {
        var col1 = new KanbanColumnModel { Title = "To Do" };
        var col2 = new KanbanColumnModel { Title = "In Progress" };
        Assert.NotEqual(Guid.Empty, col1.Id);
        Assert.NotEqual(col1.Id, col2.Id);
    }

    [Fact]
    public void KanbanColumn_WipLimit_DefaultIsNull()
    {
        var column = new KanbanColumnModel { Title = "Test" };
        Assert.Null(column.WipLimit);
    }
}
