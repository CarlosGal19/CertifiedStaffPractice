using CertifiedStaff.Data;
using CertifiedStaff.DTO.Update;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CertifiedStaff.Pages.certificates;

public class UpdateCertificationModel : PageModel
{
    private readonly AppDbContext _context;
    [BindProperty]
    public GetCertificationDTO CertificationToUpdate { get; set; } = new();
    public List<SelectListItem> ProductionLines { get; set; } = new();
    public List<SelectListItem> Stations { get; set; } = new();
    public List<SelectListItem> Employees { get; set; } = new();

    public UpdateCertificationModel(AppDbContext context)
    {
        _context = context;
    }

    public void OnGet(int id)
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

        Employees = _context.Employees
            .Where(e => e.IsActive)
            .Select(e => new SelectListItem
            {
                Value = e.EmployeeId.ToString(),
                Text = e.Name
            })
            .ToList();

        CertificationToUpdate = _context.Certifications
            .Where(c => c.CertificationId == id && c.IsActive)
            .Select(c => new GetCertificationDTO
            {
                CertificateId = c.CertificationId,
                EmployeeId = c.EmployeeId,
                ProductionLineId = c.ProductionLineStation.ProductionLineId,
                StationId = c.ProductionLineStation.StationId,
                TrainingPercentage = c.TrainingPercentage
            })
            .Single();
    }

    public IActionResult OnPostUpdateCertificate()
    {
        int productionLineStationId = GetProductionLineStationId(
            CertificationToUpdate.ProductionLineId,
            CertificationToUpdate.StationId);

        var certification = _context.Certifications
            .SingleOrDefault(c => c.CertificationId == CertificationToUpdate.CertificateId
                            && c.IsActive);

        if (certification is null)
        {
            return NotFound();
        }

        certification.EmployeeId = CertificationToUpdate.EmployeeId;
        certification.ProductionLineStationId = productionLineStationId;
        certification.TrainingPercentage = CertificationToUpdate.TrainingPercentage;

        if (certification.TrainingPercentage == 100)
        {
            var now = DateTime.Now;

            certification.CertificationDate = now;
            certification.ExpirationDate = now.AddMonths(6);
        }

        _context.SaveChanges();

        return RedirectToPage("/Index");
    }

    private int GetProductionLineStationId(int productionLineId, int stationId)
    {
        return _context.ProductionLineStations
            .Where(pls => pls.ProductionLineId == productionLineId &&
                        pls.StationId == stationId)
            .Select(pls => pls.ProductionLineStationId)
            .Single();
    }
}
