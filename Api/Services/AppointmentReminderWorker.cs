
namespace Api.Services
{
    public class AppointmentReminderWorker : BackgroundService
    {
        private readonly IServiceProvider _srv;

        public AppointmentReminderWorker(IServiceProvider services)
        {
            _srv = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _srv.CreateScope())
                {
                    var svc = scope.ServiceProvider.GetRequiredService<IAppointmentNotificationService>();
                    await svc.CheckForUpcomingAppointmentsAsync(stoppingToken);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}

