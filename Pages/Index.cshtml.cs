using CertifiedStaff.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CertifiedStaff.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public List<SelectListItem> ProductionLines { get; set; } = new();
    public List<SelectListItem> Stations {get; set;} = new();
    public List<SelectListItem> Supervisors {get; set;} = new();

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }
    public void OnGet()
    {
        ProductionLines = _context.ProductionLines
            .Where(pl => pl.IsActive)
            .Select(pl => new SelectListItem
            {
                Value = pl.ProductionLineId.ToString(),
                Text = pl.Name
            })
            .ToList();
        Stations = _context.Stations
            .Where(s => s.IsActive)
            .Select(s => new SelectListItem
            {
                Value = s.StationId.ToString(),
                Text = s.Name
            })
            .ToList();
        Supervisors = _context.Supervisors
            .Where(s => s.IsActive)
            .Select(s => new SelectListItem
            {
                Value = s.SupervisorId.ToString(),
                Text = s.Name
            })
            .ToList();
    }
}
