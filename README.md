# MudKanban
Kanban board component for MudBlazor

[![Build and Test](https://github.com/Opzet/MudKanban/actions/workflows/build.yml/badge.svg)](https://github.com/Opzet/MudKanban/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/v/MudKanban.svg)](https://www.nuget.org/packages/MudKanban/)

## Features
- Drag-and-drop cards between columns
- WIP (work-in-progress) limits per column
- Priority levels with colour-coded indicators
- Assignee and due date display
- Custom card template support via `RenderFragment<KanbanCard>`

## Installation

```bash
dotnet add package MudKanban
```

## Setup

### App.razor / index.html

Add MudBlazor resources:

```html
<link href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap" rel="stylesheet" />
<link href="_content/MudBlazor/MudBlazor.min.css" rel="stylesheet" />
<!-- ... -->
<script src="_content/MudBlazor/MudBlazor.min.js"></script>
```

### Program.cs

```csharp
builder.Services.AddMudServices();
```

### MainLayout.razor

```razor
<MudThemeProvider />
<MudPopoverProvider />
<MudDialogProvider />
<MudSnackbarProvider />
```

## Usage

```razor
<MudKanbanBoard Columns="@_columns"
                @bind-Cards="@_cards"
                OnCardMoved="@OnCardMoved" />

@code {
    private IReadOnlyList<KanbanColumn> _columns =
    [
        new KanbanColumn { Id = "todo",       Title = "To Do",       Order = 0, WipLimit = 3 },
        new KanbanColumn { Id = "inprogress", Title = "In Progress", Order = 1, WipLimit = 2 },
        new KanbanColumn { Id = "done",       Title = "Done",        Order = 2 },
    ];

    private IReadOnlyList<KanbanCard> _cards =
    [
        new KanbanCard { Title = "First task",  ColumnId = "todo",       Priority = KanbanPriority.High },
        new KanbanCard { Title = "Second task", ColumnId = "inprogress", Priority = KanbanPriority.Normal },
    ];

    private void OnCardMoved(KanbanCardMovedEventArgs args)
    {
        Console.WriteLine($"Card '{args.Card.Title}' moved from {args.FromColumnId} to {args.ToColumnId}");
    }
}
```

### Custom card template

```razor
<MudKanbanBoard Columns="@_columns" @bind-Cards="@_cards">
    <CardTemplate Context="card">
        <MudPaper Class="pa-2">
            <strong>@card.Title</strong>
            <p>@card.Description</p>
        </MudPaper>
    </CardTemplate>
</MudKanbanBoard>
```

## CI/CD

- **Build & Test** — runs on every push to `main` and on pull requests.
- **Publish NuGet** — triggered when a GitHub Release is published; requires a `NUGET_API_KEY` repository secret.
