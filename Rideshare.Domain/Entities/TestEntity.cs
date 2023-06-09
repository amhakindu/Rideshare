using Rideshare.Domain.Common;

namespace Rideshare.Domain.Entities;

public class TestEntity: BaseEntity
{
    public string Name { get; set; } = "";
    public int OtherField { get; set; } 
}
