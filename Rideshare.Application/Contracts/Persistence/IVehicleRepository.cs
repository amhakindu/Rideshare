using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Contracts.Persistence;
public interface IVehicleRepository : IGenericRepository<Vehicle>
{
    Task<Dictionary<int, int>> GetNoVehicle(string option, int year, int month);
}
