using CertifiedStaff.Data;
using CertifiedStaff.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;

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

    public readonly int PageSize = 5;

    public int OngoingPage { get; set; } = 1;
    public int CompletedPage { get; set; } = 1;
    public int ExpiredPage { get; set; } = 1;

    public int OngoingTotal { get; set; }
    public int CompletedTotal { get; set; }
    public int ExpiredTotal { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? ProductionLineId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? StationId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? SupervisorId { get; set; }

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public void OnGet(
        int ongoingPage = 1,
        int completedPage = 1,
        int expiredPage = 1)
    {
        OngoingPage = ongoingPage;
        CompletedPage = completedPage;
        ExpiredPage = expiredPage;

        LoadFilters();
        LoadData(ProductionLineId, StationId, SupervisorId);
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
            var supervisor = _context.Supervisors
                .AsNoTracking()
                .FirstOrDefault(s => s.SupervisorId == supervisorId.Value);

            certifications = certifications.Where(c =>
                c.Employee.ShiftId == supervisor.ShiftId &&
                c.ProductionLineStation.ProductionLineId == supervisor.ProductionLineId);
        }

        var certificationDtos = certifications
            .Select(c => new CertificationDTO
            {
                CertificationId = c.CertificationId,
                Employee = c.Employee.Name,
                Supervisor = _context.Supervisors
                    .Where(s => s.ShiftId == c.Employee.ShiftId
                            && s.ProductionLineId == c.ProductionLineStation.ProductionLineId)
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
            .OrderBy(c => c.CertificationDate)
            .Skip((CompletedPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        ExpiredCertificates = certificationDtos
            .Where(c => c.CertificationDate != null &&
                        c.ExpirationDate.HasValue &&
                        c.ExpirationDate.Value <= DateTime.Now)
            .OrderByDescending(c => c.ExpirationDate)
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

    public IActionResult OnGetExportExcel()
    {
        using var wb = new XLWorkbook();

        // =========================
        // Ongoing
        // =========================
        var ongoing = wb.Worksheets.Add("OngoingCertificates");

        ongoing.Cell(1, 1).Value = "Employee";
        ongoing.Cell(1, 2).Value = "Supervisor";
        ongoing.Cell(1, 3).Value = "Shift";
        ongoing.Cell(1, 4).Value = "Line / Station";
        ongoing.Cell(1, 5).Value = "Training %";

        int row = 2;

        var ongoingData = _context.Certifications
            .Where(c => c.CertificationDate == null && c.IsActive)
            .Select(c => new
            {
                c.Employee.Name,
                Supervisor = c.Employee.Shift.Supervisors
                    .Select(s => s.Name)
                    .FirstOrDefault() ?? "NA",
                Shift = c.Employee.Shift.Name,
                LineStation = c.ProductionLineStation.ProductionLine.Name + " " +
                            c.ProductionLineStation.Station.Name,
                c.TrainingPercentage
            })
            .ToList();

        foreach (var item in ongoingData)
        {
            ongoing.Cell(row, 1).Value = item.Name;
            ongoing.Cell(row, 2).Value = item.Supervisor;
            ongoing.Cell(row, 3).Value = item.Shift;
            ongoing.Cell(row, 4).Value = item.LineStation;
            ongoing.Cell(row, 5).Value = item.TrainingPercentage;
            row++;
        }

        // =========================
        // Completed
        // =========================
        var completed = wb.Worksheets.Add("CompletedCertificates");

        completed.Cell(1, 1).Value = "Employee";
        completed.Cell(1, 2).Value = "Supervisor";
        completed.Cell(1, 3).Value = "Shift";
        completed.Cell(1, 4).Value = "Line / Station";
        completed.Cell(1, 5).Value = "Certification Date";
        completed.Cell(1, 6).Value = "Expiration Date";

        row = 2;

        var completedData = _context.Certifications
            .Where(c => c.CertificationDate != null &&
                        c.ExpirationDate.HasValue &&
                        c.ExpirationDate.Value > DateTime.Now &&
                        c.IsActive)
            .Select(c => new
            {
                c.Employee.Name,
                Supervisor = c.Employee.Shift.Supervisors
                    .Select(s => s.Name)
                    .FirstOrDefault() ?? "NA",
                Shift = c.Employee.Shift.Name,
                LineStation = c.ProductionLineStation.ProductionLine.Name + " " +
                            c.ProductionLineStation.Station.Name,
                c.CertificationDate,
                c.ExpirationDate
            })
            .ToList();

        foreach (var item in completedData)
        {
            completed.Cell(row, 1).Value = item.Name;
            completed.Cell(row, 2).Value = item.Supervisor;
            completed.Cell(row, 3).Value = item.Shift;
            completed.Cell(row, 4).Value = item.LineStation;
            completed.Cell(row, 5).Value = item.CertificationDate;
            completed.Cell(row, 6).Value = item.ExpirationDate;
            row++;
        }

        // =========================
        // Expired
        // =========================
        var expired = wb.Worksheets.Add("ExpiredCertificates");

        expired.Cell(1, 1).Value = "Employee";
        expired.Cell(1, 2).Value = "Supervisor";
        expired.Cell(1, 3).Value = "Shift";
        expired.Cell(1, 4).Value = "Line / Station";
        expired.Cell(1, 5).Value = "Certification Date";
        expired.Cell(1, 6).Value = "Expiration Date";

        row = 2;

        var expiredData = _context.Certifications
            .Where(c => c.CertificationDate != null &&
                        c.ExpirationDate.HasValue &&
                        c.ExpirationDate.Value <= DateTime.Now &&
                        c.IsActive)
            .Select(c => new
            {
                c.Employee.Name,
                Supervisor = c.Employee.Shift.Supervisors
                    .Select(s => s.Name)
                    .FirstOrDefault() ?? "NA",
                Shift = c.Employee.Shift.Name,
                LineStation = c.ProductionLineStation.ProductionLine.Name + " " +
                            c.ProductionLineStation.Station.Name,
                c.CertificationDate,
                c.ExpirationDate
            })
            .ToList();

        foreach (var item in expiredData)
        {
            expired.Cell(row, 1).Value = item.Name;
            expired.Cell(row, 2).Value = item.Supervisor;
            expired.Cell(row, 3).Value = item.Shift;
            expired.Cell(row, 4).Value = item.LineStation;
            expired.Cell(row, 5).Value = item.CertificationDate;
            expired.Cell(row, 6).Value = item.ExpirationDate;
            row++;
        }

        // =========================
        // RETURN FILE
        // =========================
        using var stream = new MemoryStream();
        wb.SaveAs(stream);

        return File(stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "certifications.xlsx");
            }
}
