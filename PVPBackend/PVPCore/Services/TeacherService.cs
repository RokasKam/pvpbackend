using AutoMapper;
using PVPCore.Interfaces.Repositories;
using PVPCore.Interfaces.Services;
using PVPCore.Response.Teacher;

namespace PVPCore.Services;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IMapper _mapper;

    public TeacherService(ITeacherRepository teacherRepository, IMapper mapper)
    {
        _teacherRepository = teacherRepository;
        _mapper = mapper;
    }
    public TeacherResponse GetById(Guid id)
    {
        var teacher = _teacherRepository.GetByIdOrDefault(id);
        var response = _mapper.Map<TeacherResponse>(teacher);
        return response;
    }
}