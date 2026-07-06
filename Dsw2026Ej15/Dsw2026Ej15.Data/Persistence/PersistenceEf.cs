using Dsw2026Ej15.Data.Abstractions;
using Dsw2026Ej15.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Data.Persistence;

public class PersistenceEf : IPersistence
{
    private readonly AppDbContext _context;

    public PersistenceEf(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Speciality>> GetAllSpecialitiesAsync()
    {
        return await _context.Specialities
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<Speciality?> GetSpecialityByIdAsync(Guid specialityId)
    {
        return await _context.Specialities
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == specialityId);
    }

    public async Task<bool> SpecialityExistsAsync(Guid specialityId)
    {
        return await _context.Specialities.AnyAsync(x => x.Id == specialityId);
    }

    public async Task<List<Doctor>> GetAllActiveDoctorsAsync()
    {
        return await _context.Doctors
            .AsNoTracking()
            .Include(x => x.Speciality)
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<Doctor?> GetActiveDoctorByIdAsync(Guid doctorId)
    {
        return await _context.Doctors
            .AsNoTracking()
            .Include(x => x.Speciality)
            .FirstOrDefaultAsync(x => x.Id == doctorId && x.IsActive);
    }

    public async Task<Doctor?> GetDoctorByIdAsync(Guid doctorId)
    {
        return await _context.Doctors
            .AsNoTracking()
            .Include(x => x.Speciality)
            .FirstOrDefaultAsync(x => x.Id == doctorId);
    }

    public async Task<bool> DoctorExistsAsync(Guid doctorId)
    {
        return await _context.Doctors.AnyAsync(x => x.Id == doctorId);
    }

    public async Task<bool> LicenseNumberExistsAsync(string licenseNumber)
    {
        return await _context.Doctors.AnyAsync(x => x.LicenseNumber == licenseNumber);
    }

    public async Task<Doctor> AddDoctorAsync(Doctor doctor)
    {
        await _context.Doctors.AddAsync(doctor);
        await _context.SaveChangesAsync();
        return doctor;
    }

    public async Task UpdateDoctorAsync(Doctor doctor)
    {
        var existingDoctor = await _context.Doctors.FirstOrDefaultAsync(x => x.Id == doctor.Id);
        if (existingDoctor == null)
        {
            return;
        }

        existingDoctor.Name = doctor.Name;
        existingDoctor.LicenseNumber = doctor.LicenseNumber;
        existingDoctor.IsActive = doctor.IsActive;
        existingDoctor.SpecialityId = doctor.SpecialityId;

        await _context.SaveChangesAsync();
    }

    public async Task DeactivateDoctorAsync(Guid doctorId)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.Id == doctorId);
        if (doctor == null)
        {
            return;
        }

        doctor.IsActive = false;
        await _context.SaveChangesAsync();
    }

    public async Task<List<Doctor>> GetAllDoctorsAsync()
    {
        return await _context.Doctors
            .AsNoTracking()
            .Include(x => x.Speciality)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
}