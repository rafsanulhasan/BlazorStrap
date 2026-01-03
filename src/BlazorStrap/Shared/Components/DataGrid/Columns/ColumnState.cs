using System.Diagnostics;

namespace BlazorStrap.Shared.Components.DataGrid.Columns;

public class ColumnState<TGridItem>
{
    private readonly List<ColumnBase<TGridItem>> _columns = new();
    private readonly List<SortColumn<TGridItem>> _sortColumns = new();
    private readonly Dictionary<string, int> _renderOrder = new();
    private int _renderCounter = 0;
    private string? _firstColumnKey = null;

    public ICollection<ColumnBase<TGridItem>> Columns => _columns;
    public ICollection<SortColumn<TGridItem>> SortColumns => _sortColumns;
    public int SortOrder(Guid id) => _sortColumns.FirstOrDefault(x => x.Id == id)?.Order ?? 0;
    public bool IsSorted(Guid id) => _sortColumns.Any(x => x.Id == id && x.Sorted);
    public bool IsSortedDescending(Guid id) => _sortColumns.Any(x => x.Id == id && x.Descending);


    public void AddColumn(ColumnBase<TGridItem> column)
    {
        if (column.InitialSortDescending && !column.InitialSorted)
        {
            Debug.WriteLine("Warning: Column set to InitialSortDescending without being set to InitialSorted. Ignoring InitialSortDescending.");
        }

        var stableKey = column.StableKey;
        bool isExistingColumn = _columns.Any(x => x.Id == column.Id);

        // Update render order for columns with stable keys
        if (stableKey != null)
        {
            // Detect start of new render cycle: when the first column re-renders
            if (isExistingColumn && stableKey == _firstColumnKey)
            {
                _renderCounter = 0;
            }

            // Track first column
            if (_firstColumnKey == null)
            {
                _firstColumnKey = stableKey;
            }

            _renderOrder[stableKey] = _renderCounter++;
        }

        // Check if already added (same component instance)
        if (isExistingColumn)
        {
            // Re-sort in case a new column appeared before this one in the render order
            SortColumnsByRenderOrder();
            return;
        }

        if (column.IsSortable || column.CustomSort != null)
        {
            var sortColumn = new SortColumn<TGridItem>(column.Id, false, _sortColumns.Count, false, column.PropertyPath, column);
            _sortColumns.Add(sortColumn);

            // Assigns the initial sort column based on Parameter only one column can be initially sorted
            if (column.InitialSorted)
            {
                foreach (var otherColumns in (_sortColumns.Where(x => x.Column.IsSortable)))
                {
                    otherColumns.Sorted = false;
                }
                sortColumn.Descending = column.InitialSortDescending;
                sortColumn.Sorted = true;
            }
        }

        _columns.Add(column);
        SortColumnsByRenderOrder();
    }

    private void SortColumnsByRenderOrder()
    {
        if (_columns.Count <= 1) return;

        var sorted = _columns
            .OrderBy(c =>
            {
                if (c.StableKey != null && _renderOrder.TryGetValue(c.StableKey, out var order))
                    return order;
                return int.MaxValue;
            })
            .ToList();

        // Check if order actually changed to avoid unnecessary list operations
        bool changed = false;
        for (int i = 0; i < _columns.Count; i++)
        {
            if (!ReferenceEquals(_columns[i], sorted[i]))
            {
                changed = true;
                break;
            }
        }

        if (changed)
        {
            _columns.Clear();
            _columns.AddRange(sorted);
        }
    }

    public void RemoveColumn(ColumnBase<TGridItem> column)
    {
        _sortColumns.RemoveAll(x => x.Column.Id == column.Id);
        _columns.Remove(column);
    }

    public BSDataGridCoreBase<TGridItem>? DataGrid { get; set; }

    internal Func<Task>? OnStateChange { get; set; }
}