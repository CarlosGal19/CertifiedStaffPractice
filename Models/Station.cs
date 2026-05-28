namespace CertifiedStaff.Models;

public class Station
{
    public int StationId {get; set;}
    public string Name {get; set;} = string.Empty;
    public bool IsActive {get; set;} = true;
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

    public ICollection<ProductionLineStation> ProductionLineStations { get; set; } = new List<ProductionLineStation>();
}
