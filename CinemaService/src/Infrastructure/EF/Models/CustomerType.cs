using Shared.Infrastructure;

namespace CinemaService.Infrastructure.EF.Models
{
    public class CustomerType : BaseEntity
    {
        public string Name { get; set; } // Children, U22, Adult,..
        public string RoleCondition { get; set; } // Height < 1.3m, Student ID, Age >= 55,...
        public ICollection<PriceRule> PriceRules { get; set; }
    }
}
