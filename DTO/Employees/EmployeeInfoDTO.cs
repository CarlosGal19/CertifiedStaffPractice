namespace CertifiedStaff.DTO.Employees;

public class EmployeeInfoDTO
{
    public int EmployeeId {get; set;}
    public string Employee {get; set;}
    public string Shift {get; set;}
    public List<InternalCertificateDTO> Certificates {get; set;} = [];

}
