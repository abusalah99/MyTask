namespace MyTask.Controllers.Admin
{
    [Route("api/Admin/Assignment")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AdminAssignmentContoller : BaseControllerSetting<Assignment>
    {
        private readonly IAssignmentUnitOfWork _unitOfWork;

        public AdminAssignmentContoller(IAssignmentUnitOfWork unitOfWork) : base(unitOfWork) 
               => _unitOfWork = unitOfWork;

        [HttpGet]
        public async Task<IActionResult> Get()
        => Ok(new { Reponse = await _unitOfWork.GetAllAssignmentsForAdmin() });
        [HttpGet, Route("MyCreatedAssignments")]
        public async Task<IActionResult> Test()
           => Ok(new { Reponse = await _unitOfWork.GetAssignmentsCreatedByUser() });

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id) => Ok(new { Response = await Read(id) });

        [HttpPost]
        public async Task<IActionResult> Post(Assignment assignment)
        {
            await _unitOfWork.Create(assignment);

            return Ok(new { Response = "Assignment Created" });
        }

        [HttpPut, Route("SetAssignmentAsFinished/{assignmentId}/{userId}"), Authorize]
        public async Task<IActionResult> SetAssignmentAsFinished(Guid assignmentId, Guid userId)
        {
            await _unitOfWork.SetUserAssignmentAsFinishedForAdmin(assignmentId, userId);

            return Ok(new { Response = "Assignment Set As Finished" });
        }
        [HttpPut]
        public async Task<IActionResult> Put(Assignment assignment)
        {
            await _unitOfWork.Update(assignment);

            return Ok(new { Response = "Assignment Updated" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => await Delete(id);
    }
}
