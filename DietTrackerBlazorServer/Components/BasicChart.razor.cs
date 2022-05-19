using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
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
using Blazorise;
using Blazorise.DataGrid;
using Blazorise.Snackbar;
using Blazorise.Charts;
using Blazorise.Components;
using Excubo.Blazor.Canvas;

namespace DietTrackerBlazorServer.Components
{
    public partial class BasicChart
    {
        [Inject] 
        IJSRuntime JS { get; set; }
        Canvas _Canvas { get; set; }

        

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var ctx = await _Canvas.GetContext2DAsync();

            //Draw grid
            await ctx.StrokeStyleAsync("#000000");

            //await ctx.FillRectAsync(30, 30, 60, 60);
            await ctx.FillStyleAsync("#ff0000");
            await ctx.FillRectAsync(0, 0, await GetCanvasWidthAsync(), await GetCanvasHeightAsync());
            await ctx.FillStyleAsync("#00ff00");
            await ctx.FillRectAsync(30, 30, await GetCanvasWidthAsync() - 60, await GetCanvasHeightAsync() - 60);


        }

        async Task<double> GetCanvasWidthAsync()
        {
            return await JS.InvokeAsync<double>("getElementWidth", "BasicChart");
        }

        async Task<double> GetCanvasHeightAsync()
        {
            return await JS.InvokeAsync<double>("getElementHeight", "BasicChart");
        }

        public class Line
        {
            public List<DataPoint> DataPoints;
            public string Color;
        }

        public class DataPoint
        {
            public double X;
            public double Y;
            public string Label;
        }

        class DOMRect
        {
            public double X;
            public double Y;
            public double Width;
            public double Height;
            public double Top;
            public double Right;
            public double Bottom;
            public double Left;
        }
    }
}