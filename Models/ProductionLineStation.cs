namespace CertifiedStaff.Models;

public class ProductionLineStation
{
    public int ProductionLineStationId {get; set;}
    public int ProductionLineId {get; set;}
    public ProductionLine ProductionLine { get; set; } = null!;
    public int StationId {get; set;}
    public Station Station { get; set; } = null!;
    public bool IsActive {get; set;} = true;
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<Certification> Certifications { get; set; } = new List<Certification>();
}
