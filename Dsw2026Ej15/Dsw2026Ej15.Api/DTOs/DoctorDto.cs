namespace Dsw2026Ej15.Api.DTOs;

public class CreateDoctorRequest
{
    public string Name { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public Guid SpecialityId { get; set; }
}

public class DoctorResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class DoctorDetailResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string SpecialityName { get; set; } = string.Empty;
}