using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class AppointmentNotificationService : IAppointmentNotificationService
    {
        private readonly ApiDbContext _context;
        private readonly IEmailSender _email;

        public AppointmentNotificationService(ApiDbContext context, IEmailSender email)
        {
            _context = context;
            _email = email;
        }

        public async Task CheckForUpcomingAppointmentsAsync(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var in30Minutes = now.AddMinutes(30);

            var appointments = await _context.Appointments
                .Where(a => a.Start >= now && a.Start <= in30Minutes && !a.Notified)
                .ToListAsync(cancellationToken);

            foreach (var appt in appointments)
            {
                await _email.SendAsync(appt.Patient.User.Email,
                    "Upcoming Appointment",
                    $"You have an appointment at {appt.Start}.");

                await _email.SendAsync(appt.Doctor.User.Email,
                    "Upcoming Appointment",
                    $"You have an appointment at {appt.Start}.");
                appt.Notified = true;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
