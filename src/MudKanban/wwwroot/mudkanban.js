// MudKanban – minimal HTML5 drag-and-drop interop
// All state lives in Blazor; JS only facilitates browser drag events.

window.MudKanban = (() => {
    let _dotNetRef = null;

    function init(dotNetRef) {
        _dotNetRef = dotNetRef;
    }

    function dispose() {
        _dotNetRef = null;
    }

    // Called from Blazor via @ondragstart – stash card / source column ids
    function onDragStart(cardId, sourceColumnId) {
        window._mudKanbanDrag = { cardId, sourceColumnId };
    }

    // Called from Blazor via @ondrop – notify .NET with target column + index
    function onDrop(targetColumnId, newIndex) {
        if (!_dotNetRef || !window._mudKanbanDrag) return;
        const { cardId, sourceColumnId } = window._mudKanbanDrag;
        window._mudKanbanDrag = null;
        _dotNetRef.invokeMethodAsync('OnCardDropped', cardId, sourceColumnId, targetColumnId, newIndex);
    }

    return { init, dispose, onDragStart, onDrop };
})();
