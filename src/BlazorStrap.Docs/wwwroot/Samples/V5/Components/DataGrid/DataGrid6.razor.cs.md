using BlazorStrap_Docs.SamplesHelpers.Content.Tables;
using BlazorStrap.V5;
using BlazorStrap.V5.DataGrid;

namespace BlazorStrap_Docs.Samples.V5.Components.DataGrid;

public partial class DataGrid6
{
    private PaginationState _pagination = new PaginationState() { ItemsPerPage = 10 };
    private readonly Table2Model _sampleData = new Table2Model();
    private ICollection<Employee> _employees = null!;

    private bool _showEmail = true;
    private bool _showLastName = false;

    protected override void OnInitialized()
    {
        _employees = _sampleData.DataSet;
    }
}
