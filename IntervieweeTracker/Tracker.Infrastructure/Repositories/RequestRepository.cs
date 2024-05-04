using Dapper;
using Tracker.Application.Repositories;
using Tracker.Domain.Requests;
using Tracker.Domain.Requests.Workflows;
using Tracker.Domain.Users;
using Tracker.Infrastructure.DbObjects;

namespace Tracker.Infrastructure.Repositories;

public class RequestRepository : IRequestRepository
{
    private readonly DbContext _dbContext;

    public RequestRepository(DbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task Create(Request request, CancellationToken cancellationToken)
    {
        await using var connection = _dbContext.Connection;

        const string insertWorkflowSql = """
                                         INSERT INTO workflows
                                         VALUES(id=@Id, title=@Title, workflow_template_id=@WorkflowTemplateId)
                                         """;

        const string insertStepSql = """
                                     INSERT INTO steps
                                     VALUES(
                                            id=@Id,
                                            title=@Title,
                                            "order"=@Order,
                                            user_id=@UserId,
                                            role_id=@RoleId,
                                            comment=@Comment,
                                            status=@Status,
                                            planning_date=@PlanningDate,
                                            workflow_id=@WorkflowId)
                                     """;

        const string insertRequestSql = """
                                        INSERT INTO requests
                                        VALUES(
                                               id=@Id,
                                               candidate_name=@CandidateName,
                                               candidate_resume=@CandidateResume,
                                               candidate_email=@CandidateEmail,
                                               workflow_id=@WorkflowId)
                                        """;

        var stepsToInsert = request.Workflow.Steps.Select(
                step => new
                {
                    step.Id,
                    step.Title,
                    step.Order,
                    step.UserId,
                    step.RoleId,
                    step.Comment,
                    step.Status,
                    step.PlanningDate,
                    WorkflowId = request.Workflow.Id
                })
            .ToArray();

        var requestToInsert = new
        {
            request.Id,
            CandidateName = request.Document.Name,
            CandidateResume = request.Document.Resume,
            CandidateEmail = request.Document.Email,
            WorkflowId = request.Workflow.Id
        };

        await connection.QueryAsync(insertWorkflowSql, request.Workflow);
        await connection.QueryAsync(insertStepSql, stepsToInsert);
        await connection.QueryAsync(insertRequestSql, requestToInsert);
    }

    public async Task<IReadOnlyCollection<Request>> GetByIds(IReadOnlyCollection<Guid> requestIds,
        CancellationToken cancellationToken)
    {
        await using var connection = _dbContext.Connection;
        const string getRequestsSql = """
                                      SELECT
                                          req.id,
                                          req.candidate_name,
                                          req.candidate_resume,
                                          req.candidate_email,
                                          req.workflow_id,
                                          w.workflow_template_id,
                                          w.title
                                      FROM requests AS req
                                      LEFT JOIN workflows AS w ON w.id = req.workflow_id
                                      WHERE req.id IN @RequestIds
                                      """;

        var requestDbObjects = await connection.QueryAsync<RequestDbObject>(getRequestsSql, requestIds);
        requestDbObjects = requestDbObjects.ToArray();

        var workflowIds = requestDbObjects.Select(requestDbObject => requestDbObject.WorkflowId).ToArray();

        const string getStepsSql = "SELECT * FROM steps WHERE steps.workflow_id in @WorkflowIds";

        var stepDbObjects = await connection.QueryAsync<StepDbObject>(getStepsSql, workflowIds);

        return requestDbObjects.Select(requestDbObject =>
                new Request(
                    requestDbObject.Id,
                    requestDbObject.UserId,
                    new Document(
                        requestDbObject.CandidateName,
                        new Email(requestDbObject.CandidateEmail),
                        new Uri(requestDbObject.CandidateResume)),
                    new Workflow(
                        requestDbObject.WorkflowId,
                        requestDbObject.WorkflowTemplateId,
                        requestDbObject.WorkflowTitle,
                        stepDbObjects
                            .Where(stepDbObject => stepDbObject.WorkflowId == requestDbObject.WorkflowId)
                            .Select(stepDbObject => new Step(
                                stepDbObject.Id,
                                stepDbObject.Title,
                                stepDbObject.Order,
                                stepDbObject.UserId,
                                stepDbObject.RoleId,
                                stepDbObject.Comment,
                                stepDbObject.Status switch
                                {
                                    0 => StepStatus.Undefined,
                                    1 => StepStatus.Pending,
                                    2 => StepStatus.Approved,
                                    3 => StepStatus.Rejected,
                                    _ => throw new ArgumentOutOfRangeException()
                                },
                                stepDbObject.PlanningDate))
                            .ToArray()
                    )))
            .ToArray();
    }
}