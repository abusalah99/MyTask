using Microsoft.Extensions.Hosting;

namespace MyTask.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AssignmentController : BaseControllerSetting<Assignment>
{
    private readonly IAssignmentUnitOfWork _unitOfWork;
    public AssignmentController(IAssignmentUnitOfWork unitOfWork) : base(unitOfWork)
        => _unitOfWork = unitOfWork;

    [HttpGet, Route("MyCreatedAssignments"), Authorize(Roles = "Creator")]
    public async Task<IActionResult> GetMyCreatedAssignments()
        => Ok(new { Reponse = await _unitOfWork.GetAssignmentsCreatedByUser() });

    [HttpGet, Route("MyAssignedAssignments"), Authorize]
    public async Task<IActionResult> GetMyAssignedAssignments()
        => Ok(new { Reponse = await _unitOfWork.GetAssignmentAssignedToUser() });

    [HttpPost, Authorize(Roles = "Creator")]
    public async Task<IActionResult> Post(Assignment assignment)
    {
        await _unitOfWork.Create(assignment);

        return Ok(new { Response = "Assignment Created" });
    }

    [HttpPut, Route("SetAssignmentAsFinished/{id}"), Authorize]
    public async Task<IActionResult> SetAssignmentAsFinished(Guid id)
    {
        await _unitOfWork.SetUserAssignmentAsFinished(id);

        return Ok(new { Response = "Assignment Set As Finished" });
    }
    [HttpPut, Authorize(Roles = "Creator")]
    public async Task<IActionResult> Put(Assignment assignment)
    {
        await _unitOfWork.Update(assignment);

        return Ok(new { Response = "Assignment Updated" });
    }

}