namespace CertifiedStaff.Models;

public class Certification
{
    public int CertificationId {get; set;}
    public int EmployeeId {get; set;}
    public Employee Employee { get; set; } = null!;
    public int ProductionLineStationId {get; set;}
    public ProductionLineStation ProductionLineStation { get; set; } = null!;
    public int TrainingPercentage {get; set;}
    public DateTime? CertificationDate {get; set;}
    public DateTime? ExpirationDate {get; set;}
    public bool IsActive {get; set;} = true;
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
}
