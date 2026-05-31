using CertifiedStaff.Data;
using CertifiedStaff.DTO.Employees;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CertifiedStaff.Pages.employees;

public class EmployeesInfoModel : PageModel
{
    private readonly AppDbContext _context;
    public EmployeeInfoDTO EmployeeInfo {get; set;} = new();
    public EmployeesInfoModel (AppDbContext context)
    {
        _context = context;
    }

    public void OnGet(int id)
    {
        var certificates = _context.Certifications
            .Where(c => c.IsActive && c.EmployeeId == id)
            .Select(c => new InternalCertificateDTO
            {
                CertificateId = c.CertificationId,
                ProductionLineStation = c.ProductionLineStation.ProductionLine.Name + " " + c.ProductionLineStation.Station.Name,
                TrainingPercentage = c.TrainingPercentage,
                CertificationDate = c.CertificationDate.HasValue ? DateOnly.FromDateTime(c.CertificationDate.Value) : null,
                ExpirationDate = c.ExpirationDate.HasValue ? DateOnly.FromDateTime(c.ExpirationDate.Value) : null,
            });

        var EmployeeData = _context.Employees
            .Where(e => e.IsActive && e.EmployeeId == id)
            .Select(e => new EmployeeInfoDTO
            {
                EmployeeId = e.EmployeeId,
                Employee = e.Name,
                Shift = e.Shift.Name,
                Certificates = certificates.ToList()
            })
            .Single();

        EmployeeInfo = EmployeeData;
    }
}
