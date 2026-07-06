namespace Dsw2026Ej15.Domain.Entities;

public class Speciality : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Speciality()
    {
    }

    public Speciality(string name, string description) : this()
    {
        Name = name;
        Description = description;
    }
}