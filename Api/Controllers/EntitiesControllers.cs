using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Api.Repsitories;
using Api.Services;

namespace Api.Controllers
{
    [Route("api/users")]
    public class UsersController : GenericController<User>
    {
        public UsersController(IRepository<User> repository) : base(repository) { }
    }

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

