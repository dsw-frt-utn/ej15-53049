using Dsw2026Ej15.Api.DTOs;
using Dsw2026Ej15.Data.Abstractions;
using Dsw2026Ej15.Data.Exceptions;
using Dsw2026Ej15.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Ej15.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IPersistence _persistence;
    private readonly ILogger<DoctorsController> _logger;

    public DoctorsController(IPersistence persistence, ILogger<DoctorsController> logger)
    {
        _persistence = persistence;
        _logger = logger;
    }

    /// <summary>
    /// POST api/doctors
    /// Crea un nuevo médico con estado activo
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(DoctorResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorRequest request)
    {
        _logger.LogInformation("Creando nuevo médico: {Name}, {LicenseNumber}", request.Name, request.LicenseNumber);

        // Validar que name sea requerido
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ValidationException("El nombre del médico es requerido.");
        }

        // Validar que licenseNumber sea requerido
        if (string.IsNullOrWhiteSpace(request.LicenseNumber))
        {
            throw new ValidationException("El número de licencia es requerido.");
        }

        // Validar que specialityId exista
        var specialityExists = await _persistence.SpecialityExistsAsync(request.SpecialityId);
        if (!specialityExists)
        {
            throw new ValidationException("La especialidad especificada no existe.");
        }

        // Validar que el número de licencia sea único
        var licenseExists = await _persistence.LicenseNumberExistsAsync(request.LicenseNumber);
        if (licenseExists)
        {
            throw new ValidationException("Ya existe un médico con este número de licencia.");
        }

        // Crear el médico (se crea activo por defecto)
        var doctor = new Doctor(request.Name, request.LicenseNumber, request.SpecialityId);
        var createdDoctor = await _persistence.AddDoctorAsync(doctor);

        var response = new DoctorResponse
        {
            Id = createdDoctor.Id,
            Name = createdDoctor.Name,
            LicenseNumber = createdDoctor.LicenseNumber,
            IsActive = createdDoctor.IsActive
        };

        _logger.LogInformation("Médico creado exitosamente con ID: {DoctorId}", createdDoctor.Id);
        return CreatedAtAction(nameof(GetDoctorById), new { id = createdDoctor.Id }, response);
    }

    /// <summary>
    /// GET api/doctors
    /// Obtiene todos los médicos activos
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<DoctorResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllActiveDoctors()
    {
        _logger.LogInformation("Obteniendo todos los médicos activos");

        var doctors = await _persistence.GetAllActiveDoctorsAsync();

        var response = doctors.Select(d => new DoctorResponse
        {
            Id = d.Id,
            Name = d.Name,
            LicenseNumber = d.LicenseNumber,
            IsActive = d.IsActive
        }).ToList();

        _logger.LogInformation("Se encontraron {Count} médicos activos", response.Count);
        return Ok(response);
    }

    /// <summary>
    /// GET api/doctors/{id}
    /// Obtiene un médico activo por su ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DoctorDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDoctorById(Guid id)
    {
        _logger.LogInformation("Obteniendo médico con ID: {DoctorId}", id);

        var doctor = await _persistence.GetActiveDoctorByIdAsync(id);

        if (doctor == null)
        {
            _logger.LogWarning("Médico no encontrado o no activo: {DoctorId}", id);
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "El médico no fue encontrado o no está activo.",
                Timestamp = DateTime.UtcNow
            });
        }

        var speciality = await _persistence.GetSpecialityByIdAsync(doctor.SpecialityId);
        var specialityName = speciality?.Name ?? "Especialidad no encontrada";

        var response = new DoctorDetailResponse
        {
            Id = doctor.Id,
            Name = doctor.Name,
            LicenseNumber = doctor.LicenseNumber,
            SpecialityName = specialityName
        };

        _logger.LogInformation("Médico encontrado y retornado: {DoctorId}", id);
        return Ok(response);
    }

    /// <summary>
    /// DELETE api/doctors/{id}
    /// Desactiva un médico (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDoctor(Guid id)
    {
        _logger.LogInformation("Desactivando médico con ID: {DoctorId}", id);

        var doctor = await _persistence.GetActiveDoctorByIdAsync(id);

        if (doctor == null)
        {
            _logger.LogWarning("Médico no encontrado o ya inactivo: {DoctorId}", id);
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "El médico no fue encontrado o no está activo.",
                Timestamp = DateTime.UtcNow
            });
        }

        await _persistence.DeactivateDoctorAsync(id);

        _logger.LogInformation("Médico desactivado exitosamente: {DoctorId}", id);
        return NoContent();
    }
}
