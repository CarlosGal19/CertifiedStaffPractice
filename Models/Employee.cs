namespace CertifiedStaff.Models;

public class Employee
{
    public int EmployeeId {get; set;}
    public string Name {get; set;} = string.Empty;
    public int ShiftId {get; set;}
    public Shift Shift { get; set; } = null!;
    public int ProductionLineStationId {get; set;}
    public ProductionLineStation ProductionLineStation { get; set; } = null!;
    public bool IsActive {get; set;} = true;
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

    public ICollection<Certification> Certifications { get; set; } = new List<Certification>();
}
