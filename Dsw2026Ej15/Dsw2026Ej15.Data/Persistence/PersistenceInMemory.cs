using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Dsw2026Ej15.Data.Abstractions;
using Dsw2026Ej15.Data.Exceptions;
using Dsw2026Ej15.Domain.Entities;

namespace Dsw2026Ej15.Data.Persistence;

public class PersistenceInMemory : IPersistence
{
    private readonly List<Speciality> _specialities = new();
    private readonly List<Doctor> _doctors = new();
    private readonly string _specialitiesFilePath;

    public PersistenceInMemory(IWebHostEnvironment environment)
    {
        _specialitiesFilePath = Path.Combine(environment.ContentRootPath, "Data", "specialities.json");
        LoadSpecialities();
    }

    // ==================== SPECIALITIES ====================

    public async Task<List<Speciality>> GetAllSpecialitiesAsync()
    {
        return await Task.FromResult(_specialities.ToList());
    }

    public async Task<Speciality?> GetSpecialityByIdAsync(Guid specialityId)
    {
        var speciality = _specialities.FirstOrDefault(s => s.Id == specialityId);
        return await Task.FromResult(speciality);
    }

    public async Task<bool> SpecialityExistsAsync(Guid specialityId)
    {
        var exists = _specialities.Any(s => s.Id == specialityId);
        return await Task.FromResult(exists);
    }

    // ==================== DOCTORS ====================

    public async Task<List<Doctor>> GetAllActiveDoctorsAsync()
    {
        var activeDoctors = _doctors
            .Where(d => d.IsActive)
            .ToList();
        return await Task.FromResult(activeDoctors);
    }

    public async Task<Doctor?> GetActiveDoctorByIdAsync(Guid doctorId)
    {
        var doctor = _doctors.FirstOrDefault(d => d.Id == doctorId && d.IsActive);
        return await Task.FromResult(doctor);
    }

    public async Task<Doctor?> GetDoctorByIdAsync(Guid doctorId)
    {
        var doctor = _doctors.FirstOrDefault(d => d.Id == doctorId);
        return await Task.FromResult(doctor);
    }

    public async Task<bool> DoctorExistsAsync(Guid doctorId)
    {
        var exists = _doctors.Any(d => d.Id == doctorId);
        return await Task.FromResult(exists);
    }

    public async Task<bool> LicenseNumberExistsAsync(string licenseNumber)
    {
        var exists = _doctors.Any(d => d.LicenseNumber == licenseNumber);
        return await Task.FromResult(exists);
    }

    public async Task<Doctor> AddDoctorAsync(Doctor doctor)
    {
        _doctors.Add(doctor);
        return await Task.FromResult(doctor);
    }

    public async Task UpdateDoctorAsync(Doctor doctor)
    {
        var existingDoctor = _doctors.FirstOrDefault(d => d.Id == doctor.Id);
        if (existingDoctor != null)
        {
            existingDoctor.Name = doctor.Name;
            existingDoctor.LicenseNumber = doctor.LicenseNumber;
            existingDoctor.IsActive = doctor.IsActive;
            existingDoctor.SpecialityId = doctor.SpecialityId;
        }
        await Task.CompletedTask;
    }

    public async Task DeactivateDoctorAsync(Guid doctorId)
    {
        var doctor = _doctors.FirstOrDefault(d => d.Id == doctorId);
        if (doctor != null)
        {
            doctor.IsActive = false;
        }
        await Task.CompletedTask;
    }

    public async Task<List<Doctor>> GetAllDoctorsAsync()
    {
        return await Task.FromResult(_doctors.ToList());
    }

    // ==================== PRIVATE METHODS ====================

    private void LoadSpecialities()
    {
        if (!File.Exists(_specialitiesFilePath))
        {
            throw new FileNotFoundException($"Archivo de especialidades no encontrado: {_specialitiesFilePath}");
        }

        try
        {
            var json = File.ReadAllText(_specialitiesFilePath);
            var specialities = JsonSerializer.Deserialize<List<Speciality>>(json, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (specialities != null)
            {
                _specialities.AddRange(specialities);
            }
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Error al deserializar el archivo de especialidades.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error al cargar las especialidades.", ex);
        }
    }
}
