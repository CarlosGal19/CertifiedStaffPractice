namespace CertifiedStaff.DTO;

public class NewCertificationDTO
{
    public int EmployeeId {get; set;}
    public int ProductionLineId {get; set;}
    public int StationId {get; set;}
    public int? TrainingPercentage {get; set;} = 0;
}
