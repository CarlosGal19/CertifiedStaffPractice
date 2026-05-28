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
    }

    private async Task SeedShifts()
    {
        if(await _context.Shifts.AnyAsync())
        {
           return;
        }

        IEnumerable<Shift> shifts =
        [
            new() { ShiftId = 1, Name = "Matutino" },
            new() { ShiftId = 2, Name = "Vespertino" },
            new() { ShiftId = 3, Name = "Nocturno" }
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
            new() { ProductionLineId = 1, Name = "BNI" },
            new() { ProductionLineId = 2, Name = "BCC" },
            new() { ProductionLineId = 3, Name = "BMF" },
            new() { ProductionLineId = 4, Name = "SML" },
            new() { ProductionLineId = 5, Name = "BOH" }
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
            new() { StationId = 1, Name = "Cutting" },
            new() { StationId = 2, Name = "Soldering" },
            new() { StationId = 3, Name = "Labeling" },
            new() { StationId = 4, Name = "Curing" },
            new() { StationId = 5, Name = "Packing" }
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
            new() { ProductionLineStationId = 1, ProductionLineId = 1, StationId = 1 },
            new() { ProductionLineStationId = 2, ProductionLineId = 1, StationId = 2 },
            new() { ProductionLineStationId = 3, ProductionLineId = 1, StationId = 3 },
            new() { ProductionLineStationId = 4, ProductionLineId = 1, StationId = 4 },
            new() { ProductionLineStationId = 5, ProductionLineId = 1, StationId = 5 },

            // BCC
            new() { ProductionLineStationId = 6, ProductionLineId = 2, StationId = 1 },
            new() { ProductionLineStationId = 7, ProductionLineId = 2, StationId = 2 },
            new() { ProductionLineStationId = 8, ProductionLineId = 2, StationId = 3 },
            new() { ProductionLineStationId = 9, ProductionLineId = 2, StationId = 4 },
            new() { ProductionLineStationId = 10, ProductionLineId = 2, StationId = 5 },

            // BMF
            new() { ProductionLineStationId = 11, ProductionLineId = 3, StationId = 1 },
            new() { ProductionLineStationId = 12, ProductionLineId = 3, StationId = 2 },
            new() { ProductionLineStationId = 13, ProductionLineId = 3, StationId = 3 },
            new() { ProductionLineStationId = 14, ProductionLineId = 3, StationId = 4 },
            new() { ProductionLineStationId = 15, ProductionLineId = 3, StationId = 5 },

            // SML
            new() { ProductionLineStationId = 16, ProductionLineId = 4, StationId = 1 },
            new() { ProductionLineStationId = 17, ProductionLineId = 4, StationId = 2 },
            new() { ProductionLineStationId = 18, ProductionLineId = 4, StationId = 3 },
            new() { ProductionLineStationId = 19, ProductionLineId = 4, StationId = 4 },
            new() { ProductionLineStationId = 20, ProductionLineId = 4, StationId = 5 },

            // BOH
            new() { ProductionLineStationId = 21, ProductionLineId = 5, StationId = 1 },
            new() { ProductionLineStationId = 22, ProductionLineId = 5, StationId = 2 },
            new() { ProductionLineStationId = 23, ProductionLineId = 5, StationId = 3 },
            new() { ProductionLineStationId = 24, ProductionLineId = 5, StationId = 4 },
            new() { ProductionLineStationId = 25, ProductionLineId = 5, StationId = 5 }
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
            new() { SupervisorId = 1, Name = "Juan Perez", ShiftId = 1, ProductionLineId = 1 },
            new() { SupervisorId = 2, Name = "Maria Lopez", ShiftId = 2, ProductionLineId = 1 },
            new() { SupervisorId = 3, Name = "Carlos Ramirez", ShiftId = 3, ProductionLineId = 1 },

            // BCC
            new() { SupervisorId = 4, Name = "Ana Torres", ShiftId = 1, ProductionLineId = 2 },
            new() { SupervisorId = 5, Name = "Luis Hernandez", ShiftId = 2, ProductionLineId = 2 },
            new() { SupervisorId = 6, Name = "Sofia Martinez", ShiftId = 3, ProductionLineId = 2 },

            // BMF
            new() { SupervisorId = 7, Name = "Miguel Garcia", ShiftId = 1, ProductionLineId = 3 },
            new() { SupervisorId = 8, Name = "Laura Sanchez", ShiftId = 2, ProductionLineId = 3 },
            new() { SupervisorId = 9, Name = "Jorge Castillo", ShiftId = 3, ProductionLineId = 3 },

            // SML
            new() { SupervisorId = 10, Name = "Patricia Flores", ShiftId = 1, ProductionLineId = 4 },
            new() { SupervisorId = 11, Name = "Ricardo Vega", ShiftId = 2, ProductionLineId = 4 },
            new() { SupervisorId = 12, Name = "Daniela Cruz", ShiftId = 3, ProductionLineId = 4 },

            // BOH
            new() { SupervisorId = 13, Name = "Fernando Ruiz", ShiftId = 1, ProductionLineId = 5 },
            new() { SupervisorId = 14, Name = "Gabriela Moreno", ShiftId = 2, ProductionLineId = 5 },
            new() { SupervisorId = 15, Name = "Alejandro Navarro", ShiftId = 3, ProductionLineId = 5 }
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
        int employeeId = 1;

        foreach (var pls in await _context.ProductionLineStations.ToListAsync())
        {
            for (int shiftId = 1; shiftId <= 3; shiftId++)
            {
                employees.Add(new Employee
                {
                    EmployeeId = employeeId++,
                    Name = $"Employee-{pls.ProductionLineStationId}-{shiftId}-A",
                    ShiftId = shiftId,
                    ProductionLineStationId = pls.ProductionLineStationId
                });

                employees.Add(new Employee
                {
                    EmployeeId = employeeId++,
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
            new() { CertificationId = 1, EmployeeId = 1, ProductionLineStationId = 1, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 10), ExpirationDate = new DateTime(2026, 7, 10) },
            new() { CertificationId = 2, EmployeeId = 1, ProductionLineStationId = 2, TrainingPercentage = 75, CertificationDate = null, ExpirationDate = null },
            new() { CertificationId = 3, EmployeeId = 1, ProductionLineStationId = 3, TrainingPercentage = 40, CertificationDate = null, ExpirationDate = null },

            // Employee 2
            new() { CertificationId = 4, EmployeeId = 2, ProductionLineStationId = 1, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 2, 1), ExpirationDate = new DateTime(2026, 8, 1) },
            new() { CertificationId = 5, EmployeeId = 2, ProductionLineStationId = 4, TrainingPercentage = 60, CertificationDate = null, ExpirationDate = null },

            // Employee 5
            new() { CertificationId = 6, EmployeeId = 5, ProductionLineStationId = 5, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 5), ExpirationDate = new DateTime(2026, 7, 5) },

            // Employee 8
            new() { CertificationId = 7, EmployeeId = 8, ProductionLineStationId = 6, TrainingPercentage = 30, CertificationDate = null, ExpirationDate = null },

            // Employee 10
            new() { CertificationId = 8, EmployeeId = 10, ProductionLineStationId = 8, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 12), ExpirationDate = new DateTime(2026, 7, 12) },

            // Employee 12
            new() { CertificationId = 9, EmployeeId = 12, ProductionLineStationId = 9, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 2, 10), ExpirationDate = new DateTime(2026, 8, 10) },
            new() { CertificationId = 10, EmployeeId = 12, ProductionLineStationId = 10, TrainingPercentage = 90, CertificationDate = null, ExpirationDate = null },

            // Single certifications
            new() { CertificationId = 11, EmployeeId = 15, ProductionLineStationId = 11, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 15), ExpirationDate = new DateTime(2026, 7, 15) },
            new() { CertificationId = 12, EmployeeId = 18, ProductionLineStationId = 12, TrainingPercentage = 55, CertificationDate = null, ExpirationDate = null },
            new() { CertificationId = 13, EmployeeId = 20, ProductionLineStationId = 13, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 2, 5), ExpirationDate = new DateTime(2026, 8, 5) },
            new() { CertificationId = 14, EmployeeId = 22, ProductionLineStationId = 14, TrainingPercentage = 45, CertificationDate = null, ExpirationDate = null },
            new() { CertificationId = 15, EmployeeId = 25, ProductionLineStationId = 15, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 20), ExpirationDate = new DateTime(2026, 7, 20) },

            // Employee 30
            new() { CertificationId = 16, EmployeeId = 30, ProductionLineStationId = 16, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 1), ExpirationDate = new DateTime(2026, 7, 1) },
            new() { CertificationId = 17, EmployeeId = 30, ProductionLineStationId = 17, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 2, 1), ExpirationDate = new DateTime(2026, 8, 1) },
            new() { CertificationId = 18, EmployeeId = 30, ProductionLineStationId = 18, TrainingPercentage = 70, CertificationDate = null, ExpirationDate = null },

            new() { CertificationId = 19, EmployeeId = 35, ProductionLineStationId = 19, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 18), ExpirationDate = new DateTime(2026, 7, 18) },
            new() { CertificationId = 20, EmployeeId = 40, ProductionLineStationId = 20, TrainingPercentage = 80, CertificationDate = null, ExpirationDate = null },
            new() { CertificationId = 21, EmployeeId = 45, ProductionLineStationId = 21, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 2, 15), ExpirationDate = new DateTime(2026, 8, 15) },
            new() { CertificationId = 22, EmployeeId = 50, ProductionLineStationId = 22, TrainingPercentage = 50, CertificationDate = null, ExpirationDate = null },
            new() { CertificationId = 23, EmployeeId = 55, ProductionLineStationId = 23, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 8), ExpirationDate = new DateTime(2026, 7, 8) },
            new() { CertificationId = 24, EmployeeId = 60, ProductionLineStationId = 24, TrainingPercentage = 35, CertificationDate = null, ExpirationDate = null },
            new() { CertificationId = 25, EmployeeId = 65, ProductionLineStationId = 25, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 2, 20), ExpirationDate = new DateTime(2026, 8, 20) },

            new() { CertificationId = 26, EmployeeId = 70, ProductionLineStationId = 1, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 25), ExpirationDate = new DateTime(2026, 7, 25) },
            new() { CertificationId = 27, EmployeeId = 80, ProductionLineStationId = 5, TrainingPercentage = 65, CertificationDate = null, ExpirationDate = null },
            new() { CertificationId = 28, EmployeeId = 90, ProductionLineStationId = 10, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 2, 12), ExpirationDate = new DateTime(2026, 8, 12) },
            new() { CertificationId = 29, EmployeeId = 100, ProductionLineStationId = 15, TrainingPercentage = 20, CertificationDate = null, ExpirationDate = null },
            new() { CertificationId = 30, EmployeeId = 110, ProductionLineStationId = 20, TrainingPercentage = 100, CertificationDate = new DateTime(2026, 1, 30), ExpirationDate = new DateTime(2026, 7, 30) }
        ];
        await _context.Certifications.AddRangeAsync(certifications);
        await _context.SaveChangesAsync();
    }
}
