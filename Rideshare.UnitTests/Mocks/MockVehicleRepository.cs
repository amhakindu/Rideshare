using Moq;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.UnitTests.Mocks;
public class MockVehicleRepository
{
    public static Mock<IVehicleRepository> GetVehicleRepository()
    {
        var vehicles = new List<Vehicle>
        {
            new Vehicle
            {
                Id = 1,
                PlateNumber = "AA5097",
                NumberOfSeats = 4,
                Model = "FORD 230",
                Libre = "alibre-01",
                DriverId = 1
            },
            new Vehicle
            {
                Id = 2,
                PlateNumber = "AA5098",
                NumberOfSeats = 4,
                Model = "FORD 231",
                Libre = "alibre-02",
                DriverId = 2
            },
        };
        var vehicleRepo = new Mock<IVehicleRepository>();

        vehicleRepo.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((int pageNumber, int pageSize) =>
        {

            var response = new PaginatedResponse<Vehicle>();
            var result = vehicles.AsQueryable().Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            response.Count = vehicles.Count();
            response.Paginated = result;
            return response;

        }
                        ); vehicleRepo.Setup(repo => repo.Exists(It.IsAny<int>())).ReturnsAsync((int id) => vehicles.Exists(vehicle => vehicle.Id == id));
        vehicleRepo.Setup(repo => repo.Get(It.IsAny<int>()))
            .ReturnsAsync((int id) => vehicles.FirstOrDefault(o => o.Id == id));
        vehicleRepo.Setup(repo => repo.Add(It.IsAny<Vehicle>()))
            .ReturnsAsync((Vehicle vehicle) =>
            {
                vehicle.Id = vehicles.Count + 1;
                vehicles.Add(vehicle);
                return 1;
            });
        vehicleRepo.Setup(repo => repo.Delete(It.IsAny<Vehicle>()))
            .ReturnsAsync((Vehicle vehicle) =>
            {
                if (vehicle != null)
                {
                    vehicle = vehicles.FirstOrDefault(v => v.Id == vehicle.Id);
                    if (vehicle != null)
                        vehicles.Remove(vehicle);
                    return 1;
                }
                return 0;
            });
        vehicleRepo.Setup(repo => repo.Update(It.IsAny<Vehicle>()))
            .ReturnsAsync((Vehicle vehicle) =>
            {
                var vehicleInDb = vehicles.FirstOrDefault(v => v.Id == vehicle.Id);
                if (vehicleInDb == null)
                    return 0;
                var newVehicles = vehicles.Where((r) => r.Id != vehicle.Id);
                vehicles = newVehicles.ToList();
                vehicles.Add(vehicle);
                return 1;
            });
        return vehicleRepo;
    }
}
