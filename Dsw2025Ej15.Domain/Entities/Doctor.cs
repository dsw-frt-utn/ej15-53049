namespace Dsw2025Ej15.Domain.Entities
{
    public class Doctor : EntityBase
    {
        public string Name { get; set; }
        public string LicenseNumber { get; set; }
        public bool IsActive { get; set; }
        public Guid SpecialityId { get; set; }
        public Speciality Speciality { get; set; }
    }
}