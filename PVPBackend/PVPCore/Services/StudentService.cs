using AutoMapper;
using PVPCore.Interfaces.Repositories;
using PVPCore.Interfaces.Services;
using PVPCore.Response.Student;

namespace PVPCore.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public StudentService(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    public StudentResponse GetById(Guid id)
    {
        var student = _studentRepository.GetByIdOrDefault(id);
        var response = _mapper.Map<StudentResponse>(student);
        return response;
    }
}