using System;
using Moq;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;

namespace Rideshare.UnitTests.Mocks
{
    public class MockDriverRepository
    {
        
    public static Mock<IDriverRepository> GetDriverRepository(){
            var drivers = new List<Driver>
        {
            new ()
            {
                Id = 1,
                UserId = "2e1b94da-7d0f-4cba-8405-087f77d3c91e",
                License = "abebe_bekele_license",
                LicenseNumber = "ab343lt",
                Verified = true,
                Experience = 4.5,
                Address = "ShiroMeda",
                Rate =new List<double>(){4, 4, 1},
            },
                
            new ()
            {
                
                Id  = 2,
                UserId = "4509108d-dcca-47b7-916e-e0e91d728ad2",
                License = "abebe_bekele_license",
                LicenseNumber = "ab343lt",
                Experience = 4.5,
                Verified = false,
                Address = "ShiroMeda",
                Rate = new List<double> {3, 4, 1},
            }
        };

            var mockRepo = new Mock<IDriverRepository>();

            mockRepo.Setup(r => r.GetAll()).ReturnsAsync(drivers);
            mockRepo.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync((int id) => drivers.FirstOrDefault(d => d.Id == id));
            mockRepo.Setup(r => r.Add(It.IsAny<Driver>())).ReturnsAsync((Driver driver) =>
            {
                drivers.Add(driver);
                return 1;
            });

            mockRepo.Setup(r => r.Update(It.IsAny<Driver>())).ReturnsAsync((Driver driver) =>
            {
                var oldCount = drivers.Count();

                var newDrivers = drivers.Where((r) => r.Id != driver.Id);
                drivers = newDrivers.ToList();
                var newCount = drivers.Count();
                if (newCount != oldCount - 1)
                    return 0;
                drivers.Add(driver);
                return 1;
            });

            mockRepo.Setup(r => r.Delete(It.IsAny<Driver>())).ReturnsAsync((Driver driver) =>
            {
                if (driver != null)
                {
                    driver = drivers.FirstOrDefault(d => d.Id == driver.Id);
                    if (driver != null)
                        drivers.Remove(driver);
                        return 1;
                }
                return 0;

            });

            mockRepo.Setup(r => r.Exists(It.IsAny<int>())).ReturnsAsync((int driverId) =>
            {
                
                var driver = drivers.FirstOrDefault(d => d.Id == driverId);
                if (driver == null)
                    return false;
                return true;

            });

            return mockRepo;
        }
    }
}