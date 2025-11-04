using src.DataTransferObject.Parameter;
using src.Infrastructure.EF.Models;

namespace src.Infrastructure.Repositories.Interfaces
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetPersons(GetPersonsParam param);
        Task<Person> CreatePerson(Person person);
        Task<Person> UpdatePerson(Person person);
        Task<bool> DeletePerson(Guid id);
    }
}
