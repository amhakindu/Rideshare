using Moq;
using Rideshare.Domain.Entities;
using Rideshare.Application.Responses;
using Rideshare.Application.UnitTests.Mocks;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.UnitTests.Mocks
{
    public class MockDriverRepository
    {
        private readonly Mock<IUserRepository> _userRepository;

        public MockDriverRepository()
        {
            _userRepository = new MockUserRepository();
        }

        public Mock<IDriverRepository> GetDriverRepository()
        {
            var drivers = new List<Driver>
            {
                new Driver
                {
                    Id = 1,
                    UserId = "user1",
                    License = "abebe_bekele_license",
                    LicenseNumber = "ab343lt",
                    Verified = true,
                    Experience = 4.5,
                    Address = "ShiroMeda",
                    Rate = new List<double> {4, 4, 1}
                },
                new Driver
                {
                    Id = 2,
                    UserId = "user2",
                    License = "abebe_bekele_license",
                    LicenseNumber = "ab343lt",
                    Experience = 4.5,
                    Verified = false,
                    Address = "ShiroMeda",
                    Rate = new List<double> {3, 4, 1}
                },
                new Driver
                {
                    Id = 3,
                    UserId = "user3",
                    License = "asdfghjkl-qwertyuiop-zxcvbnm",
                    LicenseNumber = "123456789",
                    Experience = 10,
                    Verified = true,
                    Address = "New Mexico, Addis Ababa",
                    Rate = new List<double> {4, 5, 1}
                }
            };

            var mockRepo = new Mock<IDriverRepository>();
            
            mockRepo.Setup(r => r.GetDriverByUserId(It.IsAny<string>())).ReturnsAsync((string userId) => {
                return drivers.Where(driver => driver.UserId == userId).FirstOrDefault();
            });
            mockRepo.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((int pageNumber, int pageSize) =>
            {

                var response = new PaginatedResponse<Driver>();
                var result = drivers.AsQueryable().Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

                response.Count = drivers.Count();
                response.Value = result;
                return response;
            }
                );
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
                return driver != null;
            });

            mockRepo.Setup(r => r.GetDriverWithDetails(It.IsAny<int>())).ReturnsAsync((int id) =>
            {
                var driver = drivers.FirstOrDefault(d => d.Id == id);
                if (driver != null)
                {
                    var user = _userRepository.Object.FindByIdAsync(driver.UserId).Result;
                    driver.User = user;
                    return driver;
                }
                return null;
            });

            mockRepo.Setup(r => r.GetDriversWithDetails(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((int pageNumber, int pageSize) =>
            {
                var response = new PaginatedResponse<Driver>();
                var paginatedDrivers = drivers
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(driver =>
                    {
                        var user = _userRepository.Object.FindByIdAsync(driver.UserId).Result;
                        driver.User = user;
                        return driver;
                    })
                    .ToList();

                response.Count = drivers.Count;
                response.Value = paginatedDrivers;


                return response;
            });


            mockRepo.Setup(r => r.GetCountByStatus()).ReturnsAsync(() =>
            {

                var total = drivers.Count;

                var count = drivers.Select(driver =>
                {
                    var user = _userRepository.Object.FindByIdAsync(driver.UserId).Result;
                    driver.User = user;
                    return driver;
                }).Count(driver => driver.User.LastLogin >= DateTime.Now.AddDays(-30));

                var statusCount = new List<int> { count, total - count };

                return statusCount;



            });

            mockRepo.Setup(r => r.GetEntityStatistics(It.IsAny<int?>(), It.IsAny<int?>())).ReturnsAsync(

                (int? year, int? month) =>
                {

                    if (month != null && year != null)
                    {
                        // Weekly
                        var temp = drivers.Where(entity => entity.DateCreated.Year == year)
                            .Where(entity => entity.DateCreated.Month == month)
                            .GroupBy(entity => entity.DateCreated.Day / 7 + 1)
                            .ToDictionary(group => group.Key, group => group.Count());
                        for (int i = 1; i <= 5; i++)
                        {
                            if (!temp.ContainsKey(i))
                                temp.Add(i, 0);
                        }
                        return temp;
                    }
                    else if (month == null && year == null)
                    {
                        // Yearly
                        var temp = drivers
                            .GroupBy(entity => entity.DateCreated.Year)
                            .ToDictionary(g => g.Key, g => g.Count());
                        for (int i = 2023; i <= DateTime.Now.Year; i++)
                        {
                            if (!temp.ContainsKey(i))
                                temp.Add(i, 0);
                        }
                        return temp;
                    }
                    else
                    {
                        // Monthly
                        Dictionary<int, int> temp = drivers
                            .Where(entity => entity.DateCreated.Year == year)
                            .GroupBy(entity => entity.DateCreated.Month)
                            .ToDictionary(g => g.Key, g => g.Count());
                        for (int i = 1; i < 13; i++)
                        {
                            if (!temp.ContainsKey(i))
                                temp.Add(i, 0);
                        }
                        return temp;
                    }
                }

            );

            return mockRepo;
        }
    }
}
