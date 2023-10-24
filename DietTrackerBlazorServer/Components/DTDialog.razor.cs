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
        [Parameter] public RenderFragment<DialogContext> ItemFields { get; set; }

        public DialogMode Mode { get; private set; }
        protected TItem _Item { get; set; }
        protected string _Title { get; set; }
        protected bool _Visible { get; set; }


        public class DialogContext
        {
            public TItem Item { get; set; }
            public bool Disabled { get; set; }
        }

        public async Task Show(TItem item, DialogMode mode = DialogMode.Add, string title = "")
        {
            _Item = item;
            Mode = mode;
            _Visible = true;

            var itemName = Utilities.SplitPascal(typeof(TItem).Name);

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
        }

        

        protected override async Task OnInitializedAsync()
        {
        }

    }
}