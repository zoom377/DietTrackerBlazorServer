namespace DietTrackerBlazorServer.Extensions
{
    public static class Extensions
    {
        public static TBackgroundService GetBackgroundService<TBackgroundService>(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetServices<IHostedService>()
                .OfType<TBackgroundService>()
                .FirstOrDefault();
        }
    }
}
