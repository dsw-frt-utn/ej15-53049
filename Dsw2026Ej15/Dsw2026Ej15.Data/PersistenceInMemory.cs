using System.Text.Json;
using Dsw2026Ej15.Domain;

namespace Dsw2026Ej15.Data;

public class PersistenceInMemory : IPersistence
{
    private readonly List<Doctor> _doctors = new();
    private List<Speciality> _specialities = new();

    public PersistenceInMemory()
    {
        LoadSpecialities();
    }

    // Punto 3.f.iv: Método privado para cargar especialidades desde JSON
    private void LoadSpecialities()
    {
        try
        {
            var fileName = "specialities.json";
            if (File.Exists(fileName))
            {
                var json = File.ReadAllText(fileName);
                _specialities = JsonSerializer.Deserialize<List<Speciality>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar especialidades: {ex.Message}");
        }
    }

    public List<Speciality> GetSpecialities() => _specialities;
    public Speciality? GetSpecialityById(Guid id) => _specialities.FirstOrDefault(s => s.Id == id);
    public List<Doctor> GetDoctors() => _doctors;
    public void AddDoctor(Doctor doctor) => _doctors.Add(doctor);
    public Doctor? GetDoctorById(Guid id) => _doctors.FirstOrDefault(d => d.Id == id);
}