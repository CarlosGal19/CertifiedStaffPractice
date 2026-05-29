using CertifiedStaff.Data;
using CertifiedStaff.DTO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CertifiedStaff.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public List<SelectListItem> ProductionLines { get; set; } = new();
    public List<SelectListItem> Stations { get; set; } = new();
    public List<SelectListItem> Supervisors { get; set; } = new();
    public List<CertificationDTO> OngoingCertificates { get; set; } = new();
    public List<CertificationDTO> CompletedCertificates { get; set; } = new();
    public List<CertificationDTO> ExpiredCertificates { get; set; } = new();

    public readonly int PageSize = 6;

    public int OngoingPage { get; set; } = 1;
    public int CompletedPage { get; set; } = 1;
    public int ExpiredPage { get; set; } = 1;

    public int OngoingTotal { get; set; }
    public int CompletedTotal { get; set; }
    public int ExpiredTotal { get; set; }

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public void OnGet(int ongoingPage = 1, int completedPage = 1, int expiredPage = 1)
    {
        OngoingPage = ongoingPage;
        CompletedPage = completedPage;
        ExpiredPage = expiredPage;

        LoadFilters();
        LoadData();
    }

    public void OnGetData(
        int? productionLineId,
        int? stationId,
        int? supervisorId,
        int ongoingPage = 1,
        int completedPage = 1,
        int expiredPage = 1)
    {
        OngoingPage = ongoingPage;
        CompletedPage = completedPage;
        ExpiredPage = expiredPage;

        LoadFilters();
        LoadData(productionLineId, stationId, supervisorId);
    }

    private void LoadData(
        int? productionLineId = null,
        int? stationId = null,
        int? supervisorId = null)
    {
        var certifications = _context.Certifications
            .AsNoTracking()
            .Where(c => c.IsActive);

        if (productionLineId.HasValue)
        {
            certifications = certifications.Where(c =>
                c.ProductionLineStation.ProductionLineId == productionLineId.Value);
        }

        if (stationId.HasValue)
        {
            certifications = certifications.Where(c =>
                c.ProductionLineStation.StationId == stationId.Value);
        }

        if (supervisorId.HasValue)
        {
            certifications = certifications.Where(c =>
                c.ProductionLineStation.ProductionLine.Supervisors
                    .Any(s => s.SupervisorId == supervisorId.Value));
        }

        var certificationDtos = certifications
            .Select(c => new CertificationDTO
            {
                CertificationId = c.CertificationId,
                Employee = c.Employee.Name,
                Supervisor = c.ProductionLineStation.ProductionLine.Supervisors
                    .Select(s => s.Name)
                    .FirstOrDefault() ?? "NA",
                Shift = c.Employee.Shift.Name,
                ProductionLineStation =
                    c.ProductionLineStation.ProductionLine.Name + " " +
                    c.ProductionLineStation.Station.Name,
                TrainingPercentage = c.TrainingPercentage,
                CertificationDate = c.CertificationDate,
                ExpirationDate = c.ExpirationDate
            });

        OngoingTotal = certificationDtos.Count(c => c.CertificationDate == null);

        CompletedTotal = certificationDtos.Count(c =>
            c.CertificationDate != null &&
            c.ExpirationDate.HasValue &&
            c.ExpirationDate.Value > DateTime.Now);

        ExpiredTotal = certificationDtos.Count(c =>
            c.CertificationDate != null &&
            c.ExpirationDate.HasValue &&
            c.ExpirationDate.Value <= DateTime.Now);

        OngoingCertificates = certificationDtos
            .Where(c => c.CertificationDate == null)
            .Skip((OngoingPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        CompletedCertificates = certificationDtos
            .Where(c => c.CertificationDate != null &&
                        c.ExpirationDate.HasValue &&
                        c.ExpirationDate.Value > DateTime.Now)
            .Skip((CompletedPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        ExpiredCertificates = certificationDtos
            .Where(c => c.CertificationDate != null &&
                        c.ExpirationDate.HasValue &&
                        c.ExpirationDate.Value <= DateTime.Now)
            .Skip((ExpiredPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();
    }

    private void LoadFilters()
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
