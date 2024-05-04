using Dapper;
using Tracker.Application.Repositories;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Infrastructure.Repositories;

public class WorkflowTemplateRepository : IWorkflowTemplateRepository
{
    private readonly DbContext _dbContext;

    public WorkflowTemplateRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Create(WorkflowTemplate workflowTemplate, CancellationToken cancellationToken)
    {
        await using var connection = _dbContext.Connection;

        const string insertStepTemplatesSql = """
                                              INSERT INTO step_templates
                                              VALUES (@Id, @Title, @Order, @UserId, @RoleId, @WorkflowTemplateId)
                                              """;
        const string sql = "INSERT INTO workflow_templates(id, title) VALUES (@Id, @Title)";

        var stepsToInsert = workflowTemplate.Steps.Select(step => new
            {
                step.Id,
                step.Title,
                step.Order,
                step.UserId,
                step.RoleId,
                WorkflowTemplateId = workflowTemplate.Id
            })
            .ToArray();

        await connection.ExecuteAsync(insertStepTemplatesSql, stepsToInsert);
        await connection.ExecuteAsync(sql, workflowTemplate);
    }

    public async Task Update(WorkflowTemplate workflowTemplate, CancellationToken cancellationToken)
    {
        await using var connection = _dbContext.Connection;
        const string updateWorkflowTemplateSql = """
                                                 UPDATE workflow_templates
                                                 SET title = @Title
                                                 WHERE id = @Id
                                                 """;

        const string updateStepTemplateSql = """
                                             UPDATE step_templates
                                             SET title = @Title, "order" = @Order, user_id = @UserId, role_id = @RoleId
                                             WHERE id = @Id
                                             """;
        
        var stepsToUpdate = workflowTemplate.Steps.Select(step => new
            {
                step.Id,
                step.Title,
                step.Order,
                step.UserId,
                step.RoleId,
                WorkflowTemplateId = workflowTemplate.Id
            })
            .ToArray();
        
        await connection.ExecuteAsync(updateStepTemplateSql, stepsToUpdate);
        await connection.ExecuteAsync(updateWorkflowTemplateSql, workflowTemplate);
    }

    public async Task<IReadOnlyCollection<WorkflowTemplate>> GetByIds(IReadOnlyCollection<Guid> workflowTemplateIds,
        CancellationToken cancellationToken)
    {
        await using var connection = _dbContext.Connection;
        var parameters = new DynamicParameters(workflowTemplateIds);
        const string sql = "SELECT * FROM users WHERE id IN @RoleIds";
        var result = await connection.QueryAsync<WorkflowTemplate>(sql, parameters);

        return result.ToArray();
    }
}