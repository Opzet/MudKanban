# MudKanban

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
