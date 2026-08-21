using Bunit;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components;
using Xunit;
using KanbanBoardComponent = MudKanban.Components.MudKanbanBoard;
using KanbanCardModel = MudKanban.Models.KanbanCard;
using KanbanCardMovedEventArgsModel = MudKanban.Models.KanbanCardMovedEventArgs;
using KanbanColumnModel = MudKanban.Models.KanbanColumn;

namespace MudKanban.Tests;

public class KanbanBoardComponentTests : TestContext
{
    public KanbanBoardComponentTests()
    {
        Services.AddMudServices();
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    private (List<KanbanColumnModel> columns, List<KanbanCardModel> cards) CreateSampleData()
    {
        var col1 = new KanbanColumnModel { Title = "To Do", Order = 0 };
        var col2 = new KanbanColumnModel { Title = "Done", Order = 1 };
        var cards = new List<KanbanCardModel>
        {
            new() { Title = "Card A", ColumnId = col1.Id, Order = 0 },
            new() { Title = "Card B", ColumnId = col1.Id, Order = 1 },
            new() { Title = "Card C", ColumnId = col2.Id, Order = 0 },
        };
        return (new List<KanbanColumnModel> { col1, col2 }, cards);
    }

    [Fact]
    public void Board_RendersColumns()
    {
        var (cols, cards) = CreateSampleData();

        var cut = RenderComponent<KanbanBoardComponent>(p => p
            .Add(b => b.Columns, cols)
            .Add(b => b.Items, cards));

        var markup = cut.Markup;
        Assert.Contains("To Do", markup);
        Assert.Contains("Done", markup);
    }

    [Fact]
    public void Board_RendersCards()
    {
        var (cols, cards) = CreateSampleData();

        var cut = RenderComponent<KanbanBoardComponent>(p => p
            .Add(b => b.Columns, cols)
            .Add(b => b.Items, cards));

        var markup = cut.Markup;
        Assert.Contains("Card A", markup);
        Assert.Contains("Card B", markup);
        Assert.Contains("Card C", markup);
    }

    [Fact]
    public void Board_RendersCustomCardTemplate()
    {
        var (cols, cards) = CreateSampleData();

        var cut = RenderComponent<KanbanBoardComponent>(p => p
            .Add(b => b.Columns, cols)
            .Add(b => b.Items, cards)
            .Add<KanbanCardModel>(b => b.CardTemplate, card =>
                $"<div class=\"custom-card\">{card.Title}</div>"));

        var markup = cut.Markup;
        Assert.Contains("custom-card", markup);
        Assert.Contains("Card A", markup);
    }

    [Fact]
    public void Column_ShowsWipWarning_WhenExceeded()
    {
        var col = new KanbanColumnModel { Title = "WIP Col", Order = 0, WipLimit = 1 };
        var cards = new List<KanbanCardModel>
        {
            new() { Title = "C1", ColumnId = col.Id, Order = 0 },
            new() { Title = "C2", ColumnId = col.Id, Order = 1 }, // exceeds limit
        };

        var cut = RenderComponent<KanbanBoardComponent>(p => p
            .Add(b => b.Columns, new List<KanbanColumnModel> { col })
            .Add(b => b.Items, cards));

        // WIP exceeded class or warning chip should be rendered
        var markup = cut.Markup;
        Assert.Contains("mud-kanban-wip-exceeded", markup);
        Assert.Contains("2 / 1", markup);
    }

    [Fact]
    public async Task Board_InvokesItemMoved_OnDrop()
    {
        var (cols, cards) = CreateSampleData();
        KanbanCardMovedEventArgsModel? received = null;

        var cut = RenderComponent<KanbanBoardComponent>(p => p
            .Add(b => b.Columns, cols)
            .Add(b => b.Items, cards)
            .Add(b => b.ItemMoved, args => { received = args; }));

        var board = cut.Instance;
        var card = cards[0]; // Card A in col1
        var targetColId = cols[1].Id;

        // Simulate drop via JS-invokable method
        await cut.InvokeAsync(() =>
            board.OnCardDropped(card.Id.ToString(), cols[0].Id.ToString(), targetColId.ToString(), 0));

        Assert.NotNull(received);
        Assert.Equal(card.Id, received!.CardId);
        Assert.Equal(cols[0].Id, received.SourceColumnId);
        Assert.Equal(targetColId, received.TargetColumnId);
        Assert.Equal(0, received.NewIndex);
    }
}
