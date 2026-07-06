using Dsw2026Ej15.Domain.Entities;

namespace Dsw2026Ej15.Data.Abstractions;

public interface IPersistence
{
    Task<List<Speciality>> GetAllSpecialitiesAsync();
    Task<Speciality?> GetSpecialityByIdAsync(Guid specialityId);
    Task<bool> SpecialityExistsAsync(Guid specialityId);

    Task<List<Doctor>> GetAllActiveDoctorsAsync();
    Task<Doctor?> GetActiveDoctorByIdAsync(Guid doctorId);
    Task<Doctor?> GetDoctorByIdAsync(Guid doctorId);
    Task<bool> DoctorExistsAsync(Guid doctorId);
    Task<bool> LicenseNumberExistsAsync(string licenseNumber);

    Task<Doctor> AddDoctorAsync(Doctor doctor);
    Task UpdateDoctorAsync(Doctor doctor);
    Task DeactivateDoctorAsync(Guid doctorId);
    Task<List<Doctor>> GetAllDoctorsAsync();
}