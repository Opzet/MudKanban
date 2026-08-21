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

A native **MudBlazor** companion component library providing a drag-and-drop Kanban board for Blazor applications.

## Features

- 🎨 Native MudBlazor look and feel (dark/light themes)
- 🖱️ Drag-and-drop cards between columns and reorder within a column
- 📦 Strongly typed C# models (`KanbanCard`, `KanbanColumn`)
- 🔔 `EventCallback<KanbanCardMovedEventArgs>` for persistence hooks
- 🎭 Custom `CardTemplate` (`RenderFragment<KanbanCard>`)
- ⚠️ Per-column WIP limits with visual warnings
- 📱 Responsive horizontal scroll layout

---

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
Add to your `index.html` / `_Host.cshtml`:

```html
<link href="_content/MudBlazor/MudBlazor.min.css" rel="stylesheet" />
<link href="_content/MudKanban/mudkanban.css" rel="stylesheet" />

<script src="_content/MudBlazor/MudBlazor.min.js"></script>
<script src="_content/MudKanban/mudkanban.js"></script>
```

Register MudBlazor services in `Program.cs`:

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
---

## Basic Usage

```razor
@using MudKanban.Models
@using MudKanban.Components

<MudKanbanBoard Columns="_columns"
                Items="_cards"
                ItemMoved="OnItemMoved" />

@code {
    private List<KanbanColumn> _columns = new()
    {
        new() { Title = "To Do",      Order = 0 },
        new() { Title = "In Progress",Order = 1, WipLimit = 3 },
        new() { Title = "Done",       Order = 2 },
    };

    private List<KanbanCard> _cards = new()
    {
        new() { Title = "Task 1", ColumnId = /* To Do id */, Order = 0, Color = "primary" },
    };

    private Task OnItemMoved(KanbanCardMovedEventArgs args)
    {
        // Persist the move: args.CardId, args.SourceColumnId,
        //                   args.TargetColumnId, args.NewIndex
        return Task.CompletedTask;
    }
}
```

---

## Data Models

### `KanbanCard`

| Property      | Type       | Description                                      |
|---------------|------------|--------------------------------------------------|
| `Id`          | `Guid`     | Unique identifier (auto-generated)               |
| `Title`       | `string`   | Short card title                                 |
| `Description` | `string?`  | Optional description                             |
| `ColumnId`    | `Guid`     | Parent column                                    |
| `Order`       | `int`      | Zero-based display order within column           |
| `Assignee`    | `string?`  | Optional person name                             |
| `DueDate`     | `DateTime?`| Optional due date (shown red when overdue)       |
| `Color`       | `string`   | MudBlazor color name (e.g. `"primary"`, `"error"`) |

### `KanbanColumn`

| Property   | Type    | Description                               |
|------------|---------|-------------------------------------------|
| `Id`       | `Guid`  | Unique identifier                         |
| `Title`    | `string`| Column header title                       |
| `Order`    | `int`   | Display order                             |
| `WipLimit` | `int?`  | Max cards; `null` means no limit          |

### `KanbanCardMovedEventArgs`

| Property         | Type   | Description                                |
|------------------|--------|--------------------------------------------|
| `CardId`         | `Guid` | The moved card                             |
| `SourceColumnId` | `Guid` | Origin column                              |
| `TargetColumnId` | `Guid` | Destination column                         |
| `NewIndex`       | `int`  | Zero-based index in the target column      |

---

## Custom Card Template

```razor
<MudKanbanBoard Columns="_columns" Items="_cards" ItemMoved="OnItemMoved">
    <CardTemplate Context="card">
        <MudCard>
            <MudCardContent>
                <MudText Typo="Typo.h6">@card.Title</MudText>
                <MudText>@card.Description</MudText>
            </MudCardContent>
        </MudCard>
    </CardTemplate>
</MudKanbanBoard>
```

## CI/CD

- **Build & Test** — runs on every push to `main` and on pull requests.
- **Publish NuGet** — triggered when a GitHub Release is published; requires a `NUGET_API_KEY` repository secret.
---

## WIP Limit Example

```csharp
new KanbanColumn { Title = "In Progress", Order = 1, WipLimit = 3 }
```

When the number of cards in the column exceeds `WipLimit`, the column header turns red and shows a ⚠️ warning chip.

---

## Repository Structure

```
MudKanban/
├── src/
│   ├── MudKanban/               # Component library (Razor Class Library)
│   │   ├── Components/          # MudKanbanBoard, MudKanbanColumn, MudKanbanCard
│   │   ├── Models/              # KanbanCard, KanbanColumn, KanbanCardMovedEventArgs
│   │   └── wwwroot/             # mudkanban.js, mudkanban.css
│   ├── MudKanban.Demo/          # Blazor WebAssembly demo app
│   └── MudKanban.Tests/         # bUnit + xUnit tests
├── build_nuget.ps1              # NuGet pack script
├── MudKanban.sln
└── README.md
```

---

## License

MIT
