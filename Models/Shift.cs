namespace CertifiedStaff.Models;

public class Shift
{
    public int ShiftId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Supervisor> Supervisors { get; set; } = new List<Supervisor>();

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
