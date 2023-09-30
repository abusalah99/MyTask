using Microsoft.AspNetCore.Http.HttpResults;
using MyTask;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyTask;

public class AssignmentUnitOfWork : BaseUnitOfWorkSetting<Assignment>, IAssignmentUnitOfWork
{
    private readonly IHttpContextAccessor _httpContext;
    private readonly IAssignmentRepository _repository;
    public AssignmentUnitOfWork(IAssignmentRepository repository, IHttpContextAccessor httpContext) : base(repository)
    {
        _httpContext = httpContext;
        _repository = repository;
    }

    public override async Task Create(Assignment assignment)
    {
        var context = _httpContext?.HttpContext;

        Guid id = GetUserIdFromClaims(context);

        assignment.UserId = id;
        
        await base.Create(assignment);
    }
    public override async Task Update(Assignment assignment)
    {
        var context = _httpContext?.HttpContext;

        Guid id = GetUserIdFromClaims(context);

        assignment.UserId = id;

        await base.Update(assignment);
    }
    public async Task<IEnumerable<object>> GetAssignmentsCreatedByUser()
    {
        var context = _httpContext?.HttpContext;

        Guid id = GetUserIdFromClaims(context);

        IEnumerable<Assignment> assignments =  await _repository.GetAssignmentsCreatedByUser(id);

        return MapFromAssignmentsToAssignmenstResponse(assignments);
    }

    public async Task<IEnumerable<object>> GetAllAssignmentsForAdmin()
    {
        List<object> assignmentsResponse = new();

        IEnumerable<Assignment> assignments = await _repository.GetAssignmentsWithUsersIncluded();

        return MapFromAssignmentsToAssignmenstResponse(assignments);
    }

    public async Task SetUserAssignmentAsFinished(Guid assignmentId)
        => await SetAssignmentAsFinished(assignmentId);
    public Task SetUserAssignmentAsFinishedForAdmin(Guid assignmentId, Guid userId)
        => SetAssignmentAsFinished(assignmentId, userId);
    public async Task<List<object>> GetAssignmentAssignedToUser()
    {
        List<object> assignmentsResponse = new();

        var context = _httpContext?.HttpContext;

        Guid id = GetUserIdFromClaims(context);

        IEnumerable<Assignment> assignments = await _repository.GetAssignmentsByUserId(id);

        foreach (var assignment in assignments)
        {

            assignmentsResponse.Add(new
            {
                Id = assignment.Id,
                Name = assignment.Name,
                Description = assignment.Description,
                CreatedAt = assignment.CreatedAt,
                DueTo = assignment.DueTo,
                Status = GetUserAssignment(assignment,id).Status,
                FinishedAt = GetUserAssignment(assignment, id).FinishedAt
            });
        }

        return assignmentsResponse;
    }

    private async Task SetAssignmentAsFinished(Guid assignmentId, Guid userId = default(Guid))
    {
        if (userId == default(Guid))
        {
            var context = _httpContext?.HttpContext;

            userId = GetUserIdFromClaims(context);
        }

        IEnumerable<Assignment> assignments = await _repository.GetAssignmentsByUserId(userId);

        Assignment assignment = assignments.FirstOrDefault(e => e.Id == assignmentId)
                                ?? throw new ArgumentException("This Assignment Not Assigned For This User.");

        if (assignment.DueTo > DateTime.Now)
            throw new ArgumentException("This Assignment Is Past Due.");

        assignment.UserAssignment.Where(e => e.UserId == userId && e.AssignmentId == assignmentId)
                                 .ToList()
                                 .ForEach(ua =>
                                 {
                                     ua.Status = "Finished";
                                     ua.FinishedAt = DateTime.Now;
                                 });

        await Update(assignment);
    }

    private List<Object> GetUserResponseForAssignment(Assignment assignment)
    {
        List<object> usersRespons = new();

        List<User> users = assignment.UserAssignment.Where(e => e.AssignmentId == assignment.Id && e.User != null)
                                                    .Select(e => e.User!).ToList();
        foreach (var user in users)
        {
            usersRespons.Add(new
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                ProfilePhotoUrl = user.ProfilePhotoUrl,
                AssignmentStatus = GetUserAssignment(assignment, user.Id).Status,
                AssignmentFinishedAt = GetUserAssignment(assignment, user.Id).FinishedAt
            });
        }

        return usersRespons;
    }
    private UserAssignment GetUserAssignment(Assignment assignment, Guid userId)
        => assignment.UserAssignment.FirstOrDefault(e => e.UserId == userId && e.AssignmentId == assignment.Id) ?? new();
    private List<object> MapFromAssignmentsToAssignmenstResponse(IEnumerable<Assignment> assignments)
    {
        List<object> assignmentsResponse = new();

        foreach (var assignment in assignments)
        {
            assignmentsResponse.Add(new
            {
                Id = assignment.Id,
                Name = assignment.Name,
                Description = assignment.Description,
                CreatedAt = assignment.CreatedAt,
                DueTo = assignment.DueTo,
                User = GetUserResponseForAssignment(assignment)
            });
        }
        return assignmentsResponse;
    } 

}
