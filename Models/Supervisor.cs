namespace CertifiedStaff.Models;

public class Supervisor
{
    public int SupervisorId {get; set;}
    public string Name {get; set;} = string.Empty;
    public int ShiftId {get; set;}
    public Shift Shift { get; set; } = null!;
    public int ProductionLineId {get; set;}
    public ProductionLine ProductionLine { get; set; } = null!;
    public bool IsActive {get; set;} = true;
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
}
