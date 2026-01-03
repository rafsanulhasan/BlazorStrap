@using BlazorStrap_Docs.SamplesHelpers.Content.Tables
@using BlazorStrap.V5

<h5>Conditional Column Visibility</h5>
<p>This sample demonstrates dynamically showing/hiding columns while maintaining their original position.</p>

<div class="mb-3">
    <div class="form-check form-check-inline">
        <input class="form-check-input" type="checkbox" id="showEmail" @bind="_showEmail">
        <label class="form-check-label" for="showEmail">Show Email Column</label>
    </div>
    <div class="form-check form-check-inline">
        <input class="form-check-input" type="checkbox" id="showLastName" @bind="_showLastName">
        <label class="form-check-label" for="showLastName">Show Last Name Column</label>
    </div>
</div>

<BSDataGrid IsStriped="true" IsSmall="true" Items="_employees.AsQueryable()" Pagination="_pagination">
    <Columns>
        <PropertyColumn Property="e => e.Id" IsSortable="true" Title="ID"/>
        <PropertyColumn Property="e => e.NameObject.FirstName" IsSortable="true" Title="First Name"/>
        @if (_showLastName)
        {
            <PropertyColumn Property="e => e.NameObject.LastName" IsSortable="true" Title="Last Name"/>
        }
        @if (_showEmail)
        {
            <PropertyColumn Property="e => e.Email" IsSortable="true" Title="Email"/>
        }
        <TemplateColumn Title="Actions">
            <Content>
                <button class="btn btn-sm btn-outline-primary">View</button>
            </Content>
        </TemplateColumn>
    </Columns>
</BSDataGrid>
