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
using Humanizer;

namespace DietTrackerBlazorServer.Components
{
    public partial class DTDialog<TItem>
    {
        [Inject] IDialogService _DialogService { get; set; }
        [Parameter] public RenderFragment<DTDialogTemplate<TItem>.DialogContext> ChildContent { get; set; }

        public async Task<bool> ShowAsync(TItem item, DialogMode mode = DialogMode.Add, string title = "")
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add(nameof(DTDialogTemplate<TItem>.Item), item);
            parameters.Add(nameof(DTDialogTemplate<TItem>.Mode), mode);
            parameters.Add(nameof(DTDialogTemplate<TItem>.ItemFields), ChildContent);

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

            DialogOptions options = new DialogOptions();

            var dialogReference = await _DialogService.ShowAsync<DTDialogTemplate<TItem>>(title, parameters);
            var result = await dialogReference.Result;

            return !result.Canceled;
        }

    }
}