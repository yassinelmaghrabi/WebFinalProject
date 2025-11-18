namespace Api.Services
{
    public interface IAppointmentNotificationService
    {
        Task CheckForUpcomingAppointmentsAsync(CancellationToken cancellationToken);
    }
}

