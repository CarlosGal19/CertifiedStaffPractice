using CertifiedStaff.Data;
using CertifiedStaff.Models;
using Microsoft.EntityFrameworkCore;

public class DataSeeder
{
    private readonly AppDbContext _context;

    public DataSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task Seed()
    {
        await SeedShifts();
        await SeedProductionLines();
        await SeedStations();
        await SeedProductionLineStations();
        await SeedSupervisors();
        await SeedEmployees();
        await SeedCertifications();
    }

    private async Task SeedShifts()
    {
        if(await _context.Shifts.AnyAsync())
        {
           return;
        }

        IEnumerable<Shift> shifts =
        [
            new() { Name = "Matutino" },
            new() { Name = "Vespertino" },
            new() { Name = "Nocturno" }
        ];

        await _context.Shifts.AddRangeAsync(shifts);
        await _context.SaveChangesAsync();
    }

    private async Task SeedProductionLines()
    {
        if(await _context.ProductionLines.AnyAsync())
        {
            return;
        }

        IEnumerable<ProductionLine> productionLines =
        [
            new() { Name = "BNI" },
            new() { Name = "BCC" },
            new() { Name = "BMF" },
            new() { Name = "SML" },
            new() { Name = "BOH" }
        ];

        await _context.ProductionLines.AddRangeAsync(productionLines);
        await _context.SaveChangesAsync();
    }

    private async Task SeedStations()
    {
        if (await _context.Stations.AnyAsync())
        {
            return;
        }

        IEnumerable<Station> stations =
        [
            new() { Name = "Cutting" },
            new() { Name = "Soldering" },
            new() { Name = "Labeling" },
            new() { Name = "Curing" },
            new() { Name = "Packing" }
        ];

        await _context.Stations.AddRangeAsync(stations);
        await _context.SaveChangesAsync();
    }

    private async Task SeedProductionLineStations()
    {
        if (await _context.ProductionLineStations.AnyAsync())
        {
            return;
        }

        IEnumerable<ProductionLineStation> productionLineStations =
        [
            // BNI
            new() { ProductionLineId = 1, StationId = 1 },
            new() { ProductionLineId = 1, StationId = 2 },
            new() { ProductionLineId = 1, StationId = 3 },
            new() { ProductionLineId = 1, StationId = 4 },
            new() { ProductionLineId = 1, StationId = 5 },

            // BCC
            new() { ProductionLineId = 2, StationId = 1 },
            new() { ProductionLineId = 2, StationId = 2 },
            new() { ProductionLineId = 2, StationId = 3 },
            new() { ProductionLineId = 2, StationId = 4 },
            new() { ProductionLineId = 2, StationId = 5 },

            // BMF
            new() { ProductionLineId = 3, StationId = 1 },
            new() { ProductionLineId = 3, StationId = 2 },
            new() { ProductionLineId = 3, StationId = 3 },
            new() { ProductionLineId = 3, StationId = 4 },
            new() { ProductionLineId = 3, StationId = 5 },

            // SML
            new() { ProductionLineId = 4, StationId = 1 },
            new() { ProductionLineId = 4, StationId = 2 },
            new() { ProductionLineId = 4, StationId = 3 },
            new() { ProductionLineId = 4, StationId = 4 },
            new() { ProductionLineId = 4, StationId = 5 },

            // BOH
            new() { ProductionLineId = 5, StationId = 1 },
            new() { ProductionLineId = 5, StationId = 2 },
            new() { ProductionLineId = 5, StationId = 3 },
            new() { ProductionLineId = 5, StationId = 4 },
            new() { ProductionLineId = 5, StationId = 5 }
        ];

        await _context.ProductionLineStations.AddRangeAsync(productionLineStations);
        await _context.SaveChangesAsync();
    }

    private async Task SeedSupervisors()
    {
        if (await _context.Supervisors.AnyAsync())
        {
            return;
        }

        IEnumerable<Supervisor> supervisors =
        [
            // BNI
            new() { Name = "Juan Perez", ShiftId = 1, ProductionLineId = 1 },
            new() { Name = "Maria Lopez", ShiftId = 2, ProductionLineId = 1 },
            new() { Name = "Carlos Ramirez", ShiftId = 3, ProductionLineId = 1 },

            // BCC
            new() { Name = "Ana Torres", ShiftId = 1, ProductionLineId = 2 },
            new() { Name = "Luis Hernandez", ShiftId = 2, ProductionLineId = 2 },
            new() { Name = "Sofia Martinez", ShiftId = 3, ProductionLineId = 2 },

            // BMF
            new() { Name = "Miguel Garcia", ShiftId = 1, ProductionLineId = 3 },
            new() { Name = "Laura Sanchez", ShiftId = 2, ProductionLineId = 3 },
            new() { Name = "Jorge Castillo", ShiftId = 3, ProductionLineId = 3 },

            // SML
            new() { Name = "Patricia Flores", ShiftId = 1, ProductionLineId = 4 },
            new() { Name = "Ricardo Vega", ShiftId = 2, ProductionLineId = 4 },
            new() { Name = "Daniela Cruz", ShiftId = 3, ProductionLineId = 4 },

            // BOH
            new() { Name = "Fernando Ruiz", ShiftId = 1, ProductionLineId = 5 },
            new() { Name = "Gabriela Moreno", ShiftId = 2, ProductionLineId = 5 },
            new() { Name = "Alejandro Navarro", ShiftId = 3, ProductionLineId = 5 }
        ];

        await _context.Supervisors.AddRangeAsync(supervisors);
        await _context.SaveChangesAsync();
    }

    private async Task SeedEmployees()
    {
        if (await _context.Employees.AnyAsync())
        {
            return;
        }

        var employees = new List<Employee>();

        foreach (var pls in await _context.ProductionLineStations.ToListAsync())
        {
            for (int shiftId = 1; shiftId <= 3; shiftId++)
            {
                employees.Add(new Employee
                {
                    Name = $"Employee-{pls.ProductionLineStationId}-{shiftId}-A",
                    ShiftId = shiftId,
                    ProductionLineStationId = pls.ProductionLineStationId
                });

                employees.Add(new Employee
                {
                    Name = $"Employee-{pls.ProductionLineStationId}-{shiftId}-B",
                    ShiftId = shiftId,
                    ProductionLineStationId = pls.ProductionLineStationId
                });
            }
        }

        await _context.Employees.AddRangeAsync(employees);
        await _context.SaveChangesAsync();
    }

    private async Task SeedCertifications()
    {
        if (await _context.Certifications.AnyAsync())
        {
            return;
        }

        IEnumerable<Certification> certifications =
        [
            // Employee 1
            new() { EmployeeId = 1, ProductionLineStationId = 1, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 10), ExpirationDate = new DateTime(2026, 7, 10) },
            new() { EmployeeId = 1, ProductionLineStationId = 2, TrainingPercentage = 75, CertificationDate = null, ExpirationDate = null },
            new() { EmployeeId = 1, ProductionLineStationId = 3, TrainingPercentage = 40, CertificationDate = null, ExpirationDate = null },

            // Employee 2
            new() { EmployeeId = 2, ProductionLineStationId = 1, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 2, 1), ExpirationDate = new DateTime(2026, 8, 1) },
            new() { EmployeeId = 2, ProductionLineStationId = 4, TrainingPercentage = 60, CertificationDate = null, ExpirationDate = null },

            // Employee 5
            new() { EmployeeId = 5, ProductionLineStationId = 5, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 5), ExpirationDate = new DateTime(2026, 7, 5) },

            // Employee 8
            new() { EmployeeId = 8, ProductionLineStationId = 6, TrainingPercentage = 30, CertificationDate = null, ExpirationDate = null },

            // Employee 10
            new() { EmployeeId = 10, ProductionLineStationId = 8, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 12), ExpirationDate = new DateTime(2026, 7, 12) },

            // Employee 12
            new() { EmployeeId = 12, ProductionLineStationId = 9, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 2, 10), ExpirationDate = new DateTime(2026, 8, 10) },
            new() { EmployeeId = 12, ProductionLineStationId = 10, TrainingPercentage = 90, CertificationDate = null, ExpirationDate = null },

            // Single certifications
            new() { EmployeeId = 15, ProductionLineStationId = 11, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 15), ExpirationDate = new DateTime(2026, 7, 15) },
            new() { EmployeeId = 18, ProductionLineStationId = 12, TrainingPercentage = 55, CertificationDate = null, ExpirationDate = null },
            new() { EmployeeId = 20, ProductionLineStationId = 13, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 2, 5), ExpirationDate = new DateTime(2026, 8, 5) },
            new() { EmployeeId = 22, ProductionLineStationId = 14, TrainingPercentage = 45, CertificationDate = null, ExpirationDate = null },
            new() { EmployeeId = 25, ProductionLineStationId = 15, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 20), ExpirationDate = new DateTime(2026, 7, 20) },

            // Employee 30
            new() { EmployeeId = 30, ProductionLineStationId = 16, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 1), ExpirationDate = new DateTime(2026, 7, 1) },
            new() { EmployeeId = 30, ProductionLineStationId = 17, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 2, 1), ExpirationDate = new DateTime(2026, 8, 1) },
            new() { EmployeeId = 30, ProductionLineStationId = 18, TrainingPercentage = 70, CertificationDate = null, ExpirationDate = null },

            new() { EmployeeId = 35, ProductionLineStationId = 19, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 18), ExpirationDate = new DateTime(2026, 7, 18) },
            new() { EmployeeId = 40, ProductionLineStationId = 20, TrainingPercentage = 80, CertificationDate = null, ExpirationDate = null },
            new() { EmployeeId = 45, ProductionLineStationId = 21, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 2, 15), ExpirationDate = new DateTime(2026, 8, 15) },
            new() { EmployeeId = 50, ProductionLineStationId = 22, TrainingPercentage = 50, CertificationDate = null, ExpirationDate = null },
            new() { EmployeeId = 55, ProductionLineStationId = 23, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 8), ExpirationDate = new DateTime(2026, 7, 8) },
            new() { EmployeeId = 60, ProductionLineStationId = 24, TrainingPercentage = 35, CertificationDate = null, ExpirationDate = null },
            new() { EmployeeId = 65, ProductionLineStationId = 25, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 2, 20), ExpirationDate = new DateTime(2026, 8, 20) },

            new() { EmployeeId = 70, ProductionLineStationId = 1, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 25), ExpirationDate = new DateTime(2026, 7, 25) },
            new() { EmployeeId = 80, ProductionLineStationId = 5, TrainingPercentage = 65, CertificationDate = null, ExpirationDate = null },
            new() { EmployeeId = 90, ProductionLineStationId = 10, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 2, 12), ExpirationDate = new DateTime(2026, 8, 12) },
            new() { EmployeeId = 100, ProductionLineStationId = 15, TrainingPercentage = 20, CertificationDate = null, ExpirationDate = null },
            new() { EmployeeId = 110, ProductionLineStationId = 20, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 30), ExpirationDate = new DateTime(2026, 7, 30) }
        ];
        await _context.Certifications.AddRangeAsync(certifications);
        await _context.SaveChangesAsync();
    }
}
