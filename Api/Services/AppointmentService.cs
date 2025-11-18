using Api.Data;
using Api.Models;
namespace Api.Services
{
    public class AppointmentService
    {
        private readonly ApiDbContext _db;

        public AppointmentService(ApiDbContext db) => _db = db;

        public bool HasConflict(int doctorId, DateTime start, DateTime end)
        {
            return _db.Appointments.Any(a => a.DoctorId == doctorId && a.Start < end && start < a.End && a.Status == "Booked");
        }

        public async Task<Appointment?> BookAppointment(int patientId, int doctorId, DateTime start, DateTime end)
        {
            if (HasConflict(doctorId, start, end))
                return null;
            var appointment = new Appointment
            {
                PatientId = patientId,
                DoctorId = doctorId,
                Start = start,
                End = end,
                Status = "Booked",
            };

            _db.Appointments.Add(appointment);
            await _db.SaveChangesAsync();
            return appointment;

        }
        public List<Appointment> GetAppointmentsFor(int userId, string role)
        {
            if (role == "Doctor")
            {
                var doctor = _db.Doctors.FirstOrDefault(d => d.UserId == userId);
                if (doctor == null) return new List<Appointment>();

                return _db.Appointments
                    .Where(a => a.DoctorId == doctor.Id)
                    .ToList();
            }
            else if (role == "Patient")
            {
                var patient = _db.Patients.FirstOrDefault(p => p.UserId == userId);
                if (patient == null) return new List<Appointment>();

                return _db.Appointments
                    .Where(a => a.PatientId == patient.Id)
                    .ToList();
            }

            return new List<Appointment>();
        }


    }
}
