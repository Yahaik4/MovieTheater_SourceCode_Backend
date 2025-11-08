using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using MovieService.Data;
using MovieService.DataTransferObject.Parameter;
using MovieService.Infrastructure.EF.Models;
using MovieService.Infrastructure.Repositories.Interfaces;
using System.Xml.Linq;

namespace MovieService.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly MovieDbContext _context;

        public PersonRepository(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<Person> CreatePerson(Person person)
        {
            await _context.Persons.AddAsync(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public Task<bool> DeletePerson(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Person?> GetPersonById(Guid id)
        {
            return await _context.Persons.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Person>> GetPersonByIds(List<Guid> ids)
        {
            return await _context.Persons.Where(p =>  ids.Contains(p.Id)).ToListAsync();
        }

        public async Task<IEnumerable<Person>> GetPersons(GetPersonsParam param)
        {
            var query = _context.Persons.AsQueryable();

            if (param.Id.HasValue)
                query = query.Where(p => p.Id == param.Id);

            if (!string.IsNullOrWhiteSpace(param.Name))
                query = query.Where(p => p.FullName.ToLower().Contains(param.Name.ToLower()));

            query = query.Where(p => p.IsDeleted == false);

            return await query.ToListAsync();
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            _context.Persons.Update(person);    
            await _context.SaveChangesAsync();
            return person;
        }
    }
}
