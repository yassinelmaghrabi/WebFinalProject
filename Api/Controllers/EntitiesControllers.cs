using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Api.Repsitories;
using Api.Services;
namespace Api.Controllers
{
    [Route("api/users")]
    public class UsersController : GenericController<User>
    {
        private readonly UserService _svc;
        public UsersController(IRepository<User> repository, UserService svc) : base(repository)
        {
            _svc = svc;
        }
        [HttpPost("signup")]
        public IActionResult Signup(SignupRequest req)
        {
            var user = _svc.SignupUser(req.email, req.password, req.role, req.firstname, req.lastname, specialty: req.specialty, brithdate: req.brithdate);
            if (user is null)
                return BadRequest("Invalid request");
            return Ok(user);
        }
    }
    public record SignupRequest(string email, string password, string role, string firstname, string lastname, string? specialty, DateTime? brithdate);


    [Route("api/patients")]
    public class PatientsController : GenericController<Patient>
    {
        public PatientsController(IRepository<Patient> repository) : base(repository) { }
    }

    [Route("api/doctors")]
    public class DoctorsController : GenericController<Doctor>
    {
        public DoctorsController(IRepository<Doctor> repository) : base(repository) { }
    }

    [Route("api/medicalrecords")]
    public class MedicalRecordsController : GenericController<MedicalRecord>
    {
        public MedicalRecordsController(IRepository<MedicalRecord> repository) : base(repository) { }
    }
}

