using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Api.Models;
using Api.Repsitories;
using Api.Services;
using System.Security.Claims;
namespace Api.Controllers
{

    [Route("api/appointments")]
    public class AppointmentsController : GenericController<Appointment>
    {
        private readonly AppointmentService _svc;
        public AppointmentsController(IRepository<Appointment> repo, AppointmentService svc) : base(repo)
        {
            _svc = svc;
        }

        [HttpPost("book")]
        public async Task<IActionResult> Book(AppointmentRequest req)
        {
            var appt = await _svc.BookAppointment(req.PatientId, req.DoctorId, req.Start, req.End);
            if (appt == null)
                return Conflict("Conflicting appointment.");
            return Ok(appt);
        }
        [Authorize]
        [HttpGet("for")]
        public IActionResult GetWithToken()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role)!;
            var appointments = _svc.GetAppointmentsFor(userId, role);
            return Ok(appointments);
        }
    }
    public record AppointmentRequest(int PatientId, int DoctorId, DateTime Start, DateTime End);

}

