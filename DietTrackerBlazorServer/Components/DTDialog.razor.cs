using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using DietTrackerBlazorServer;
using DietTrackerBlazorServer.Pages;
using DietTrackerBlazorServer.Shared;
using DietTrackerBlazorServer.Data;
using DietTrackerBlazorServer.Model;
using MudBlazor;
using System.Text;
using Humanizer;

namespace DietTrackerBlazorServer.Components
{
    public enum DialogMode
    {
        Add,
        Edit,
        Delete
    }

    public partial class DTDialog<TItem>
    {
        [Inject] IDialogService _DialogService { get; set; }
        [Parameter] public RenderFragment<DialogContext> ItemFields { get; set; }
        protected DialogMode _Mode { get; set; }
        protected TItem _Item { get; set; }
        protected string _Title { get; set; }
        protected bool _Visible { get; set; }
        public MudDialog _Dialog { get; set; }

        public class DialogContext
        {
            public TItem Item { get; set; }
            public bool Disabled { get; set; }
        }

        public async Task<bool> Show(TItem item, DialogMode mode = DialogMode.Add, string title = "")
        {
            _Item = item;
            _Mode = mode;
            var itemName = typeof(TItem).Name.Humanize(LetterCasing.Title);

            if (string.IsNullOrWhiteSpace(title))
            {
                if (mode == DialogMode.Add)
                    title = $"Add {itemName}";
                else if (mode == DialogMode.Edit)
                    title = $"Edit {itemName}";
                else if (mode == DialogMode.Delete)
                    title = $"Delete {itemName}";
            }
            _Title = title;

            var dialogRef = await _DialogService.ShowAsync<DTDialog<HealthMetric>>();
            //dialogRef.re
            var result = await dialogRef.Result;

            return false;
        }

        string GetBorderColour()
        {
            string result = "";

            if (_Mode == DialogMode.Add)
                result = "mud-border-success";
            else if (_Mode == DialogMode.Edit)
                result = "mud-border-warning";
            else if (_Mode == DialogMode.Delete)
                result = "mud-border-error";

            return result;
        }

        protected override async Task OnInitializedAsync()
        {
        }

    }
}