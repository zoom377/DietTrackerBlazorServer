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

    public partial class DTDialogTemplate<TItem>
    {
        [CascadingParameter] MudDialogInstance _Instance { get; set; }
        [Parameter] public RenderFragment<DialogContext> ItemFields { get; set; }
        [Parameter] public DialogMode Mode { get; set; }
        [Parameter] public TItem Item { get; set; }

        public class DialogContext
        {
            public TItem Item { get; set; }
            public bool Delete { get; set; }
        }

        string GetBorderColour()
        {
            string result = "";

            if (Mode == DialogMode.Add)
                result = "mud-border-success";
            else if (Mode == DialogMode.Edit)
                result = "mud-border-warning";
            else if (Mode == DialogMode.Delete)
                result = "mud-border-error";

            return result;
        }

        protected override async Task OnInitializedAsync()
        {
        }

    }
}