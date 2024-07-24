using AutoMapper;
using PVPCore.Interfaces.Repositories;
using PVPCore.Interfaces.Services;
using PVPCore.Requests.Classroom;
using PVPCore.Response.Classroom;
using PVPDomain.Entities;

using ValidationException = PVPDomain.Exceptions.ValidationException;

namespace PVPCore.Services;

public class ClassromService : IClassroomService
{
    private readonly IClassroomRepository _classroomRepository;
    private readonly IMapper _mapper;

    public ClassromService(IClassroomRepository  classroomRepository, IMapper mapper)
    {
        _classroomRepository = classroomRepository;
        _mapper = mapper;
    }
    public Guid AddNewClass(ClassroomRequest classroomRequest, Guid teacherId)
    {
        var classroom = _mapper.Map<Classroom>(classroomRequest);
        var existingClassroom = _classroomRepository.GetByNameTeacherClass(classroomRequest.Classname, teacherId);
        if (existingClassroom is not null)
        {
            throw new ValidationException("Teacher have already class with such name");
        }
        classroom.TeacherId = teacherId;
        var id = _classroomRepository.PostClassroom(classroom);
        return id;
    }

    public IEnumerable<ClassroomResponse> GetAllTeacherClasses(Guid teacherId)
    {
        var classes = _classroomRepository.GetAllTeacherClassrooms(teacherId);
        var classesList = classes.Select(x => _mapper.Map<ClassroomResponse>(x)).ToList();
        return classesList;
    }
}