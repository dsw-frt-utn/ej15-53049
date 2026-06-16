using Dsw2026Ej15.Domain;

namespace Dsw2026Ej15.Data;

public interface IPersistence
{
    // Métodos para Especialidades
    List<Speciality> GetSpecialities();
    Speciality? GetSpecialityById(Guid id);

    // Métodos para Médicos
    List<Doctor> GetDoctors();
    void AddDoctor(Doctor doctor);
    Doctor? GetDoctorById(Guid id);
}