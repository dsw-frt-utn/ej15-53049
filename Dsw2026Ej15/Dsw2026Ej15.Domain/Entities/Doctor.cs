namespace Dsw2026Ej15.Domain.Entities;

public class Doctor : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Speciality? Speciality { get; set; }
    public Guid SpecialityId { get; set; }

    public Doctor()
    {
        IsActive = true;
    }

    public Doctor(string name, string licenseNumber, Guid specialityId) : this()
    {
        Name = name;
        LicenseNumber = licenseNumber;
        SpecialityId = specialityId;
    }
}
