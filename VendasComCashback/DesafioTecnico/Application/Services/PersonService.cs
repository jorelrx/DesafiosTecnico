using Application.DTOs;
using Application.DTOs.Validations;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.FiltersDb;
using Domain.Repositories;

namespace Application.Services
{
    public  class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public PersonService(IPersonRepository personRepository, IMapper mapper)
        {
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public async Task<ResultServices<PersonDTO>> CreateAsync(PersonDTO personDTO)
        {
            if (personDTO == null) 
                return ResultServices.Fail<PersonDTO>("Objeto deve ser informado!");

            var result = new PersonDTOValidator().Validate(personDTO);
            if (!result.IsValid)
                return ResultServices.RequestError<PersonDTO>("Problemas de validação!", result);

            var person = _mapper.Map<Person>(personDTO);
            var data = await _personRepository.CreateAsync(person);
            return ResultServices.Ok<PersonDTO>(_mapper.Map<PersonDTO>(data));
        }

        public async Task<ResultServices<ICollection<PersonDTO>>> GetAllAsync()
        {
            var people = await _personRepository.GetPeopleAsync();
            return ResultServices.Ok<ICollection<PersonDTO>>(_mapper.Map<ICollection<PersonDTO>>(people));
        }

        public async Task<ResultServices<PersonDTO>> GetByIdAsync(int id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null)
                return ResultServices.Fail<PersonDTO>("Pessoa não encontrada!");

            return ResultServices.Ok(_mapper.Map<PersonDTO>(person));
        }

        public async Task<ResultServices> DeleteAsync(int id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null)
                return ResultServices.Fail("Pessoa não encontrada!");

            await _personRepository.DeleteAsync(person);

            return ResultServices.Ok($"Pessoa do id {id} foi deletada!");
        }

        public async Task<ResultServices> UpdateAsync(PersonDTO personDTO)
        {
            if (personDTO == null)
                return ResultServices.Fail("Objeto deve ser informado!");

            var validation = new PersonDTOValidator().Validate(personDTO);

            if (!validation.IsValid)
                return ResultServices.RequestError("Problemas com a validação dos campos", validation);

            var person = await _personRepository.GetByIdAsync(personDTO.Id);

            if (person == null)
                return ResultServices.Fail("Pessoa não encontrada!");

            person = _mapper.Map<PersonDTO, Person>(personDTO, person);
            await _personRepository.EditAsync(person);
            return ResultServices.Ok("Pessoa editada!");
        }

        public async Task<ResultServices<PagedBaseResponseDTO<PersonDTO>>> GetPagedAsync(FilterDb personFilterDb)
        {
            var peoplePaged = await _personRepository.GetPagedAsync(personFilterDb);
            var result = new PagedBaseResponseDTO<PersonDTO>(peoplePaged.TotalRegistes, _mapper.Map<List<PersonDTO>>(peoplePaged.Data));

            return ResultServices.Ok(result);
        }
    }
}
