namespace CertifiedStaff.DTO.Employees;

public class InternalCertificateDTO
{
    public required int CertificateId {get; set;}
    public required string ProductionLineStation {get; set;}
    public int TrainingPercentage {get; set;}
    public DateOnly? CertificationDate {get; set;}
    public DateOnly? ExpirationDate {get; set;}
}
