﻿using BlazorComponentUtilities;
using BlazorStrap.Util.Components;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorStrap
{
    public abstract class BSLabelBase : ColumnBase
    {
        [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> UnknownParameters { get; set; }

        protected string Classname =>
        new CssBuilder()
           .AddClass("form-check-label", IsCheck)
           .AddClass("col-form-label", GetColumnClass(null) != null)
           .AddClass(GetColumnClass(null))
           .AddClass(Class)
        .Build();

        [Parameter] public bool IsCheck { get; set; }
        [Parameter] public string Class { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
    }
}
