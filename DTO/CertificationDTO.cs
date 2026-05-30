namespace CertifiedStaff.DTO;

public class CertificationDTO
{
    public int CertificationId {get; set;}
    public required string Employee {get; set;}
    public required string Supervisor {get; set;}
    public required string Shift {get; set;}
    public required string ProductionLineStation {get; set;}
    public int? TrainingPercentage {get; set;}
    public DateOnly? CertificationDate {get; set;}
    public DateOnly? ExpirationDate {get; set;}
}
