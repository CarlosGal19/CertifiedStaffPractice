using CertifiedStaff.Data;
using CertifiedStaff.DTO;
using CertifiedStaff.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CertifiedStaff.Pages.certificates;

public class NewCertificateModel: PageModel
{
    private readonly AppDbContext _context;
    public List<SelectListItem> ProductionLines { get; set; } = new();
    public List<SelectListItem> Stations { get; set; } = new();
    public List<SelectListItem> Employees { get; set; } = new();
    [BindProperty]
    public NewCertificationDTO NewCertification { get; set; } = new();

    public NewCertificateModel(AppDbContext context)
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

        Employees = _context.Employees
            .Where(e => e.IsActive)
            .Select(e => new SelectListItem
            {
                Value = e.EmployeeId.ToString(),
                Text = e.Name
            })
            .ToList();
    }

    public IActionResult OnPostAddCertificate()
    {
        int productionLineStationId = GetProductionLineStationId(
            NewCertification.ProductionLineId,
            NewCertification.StationId);

        Certification certification = new Certification
        {
            EmployeeId = NewCertification.EmployeeId,
            ProductionLineStationId = productionLineStationId,
            TrainingPercentage = NewCertification.TrainingPercentage ?? 0
        };

        if (certification.TrainingPercentage == 100)
        {
            DateTime now = DateTime.Now;
            certification.CertificationDate = now;
            certification.ExpirationDate = now.AddMonths(6);
        }

        _context.Certifications.Add(certification);
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
