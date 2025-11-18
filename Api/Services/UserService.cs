using Api.Data;
using Api.Models;
namespace Api.Services
{

    public class UserService
    {
        private readonly ApiDbContext _db;

        public UserService(ApiDbContext db) => _db = db;

        public User? SignupUser(string email, string password, string role, string firstname, string lastname, string? specialty, DateTime? brithdate)
        {
            using var transaction = _db.Database.BeginTransaction();
            var user = new User
            {
                Email = email,
                PasswordHash = Hash(password),
                Role = role
            };
            if (role is not ("Patient" or "Doctor"))
                throw new Exception("Email already exists");
            _db.Users.Add(user);
            _db.SaveChanges();
            if (role is "Doctor")
            {
                if (string.IsNullOrWhiteSpace(specialty))
                    throw new ArgumentException("Doctors require a specialty");
                var doctor = new Doctor
                {
                    UserId = user.Id,
                    FirstName = firstname,
                    LastName = lastname,
                    Specialty = specialty
                };
                _db.Doctors.Add(doctor);

            }
            else if (role == "Patient")
            {
                if (brithdate is null)
                    throw new ArgumentException("Patients require a birth date");
                var patient = new Patient
                {
                    UserId = user.Id,
                    FirstName = firstname,
                    LastName = lastname,
                    BirthDate = brithdate.GetValueOrDefault(DateTime.UnixEpoch),
                };
                _db.Patients.Add(patient);
            }
            _db.SaveChanges();
            transaction.Commit();
            return user;
        }
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

    }
}

