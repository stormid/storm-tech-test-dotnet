using MediatR;

using Microsoft.AspNetCore.Mvc;

using Storm.TechTask.Core.ProjectAggregate.Queries;
using Storm.TechTask.SharedKernel.Interfaces;

using Swashbuckle.AspNetCore.Annotations;

namespace Storm.TechTask.Api.Endpoints.Project
{
    public class ListEndpoint : BaseEndpoint
        .WithRequest<AllProjects.Query>
        .WithResponse<List<ProjectDto>>
    {
        public ListEndpoint(IMediator mediator, ILoggingService loggingService, ISecurityService securityService) : base(mediator, loggingService, securityService)
        {
        }

        [HttpGet("/Projects")]
        [SwaggerOperation(
            Summary = "Gets a list of all Projects",
            Description = "Gets a list of all Projects",
            OperationId = "Projects.List",
            Tags = new[] { "ProjectEndpoints" })
        ]
        [ProducesResponseType(StatusCodes.Status200OK)] // Added to update Swagger (Task 8)
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Added to update Swagger (Task 8)
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Added to update Swagger (Task 8)
        public override async Task<ActionResult<List<ProjectDto>>> HandleAsync([FromRoute] AllProjects.Query request, CancellationToken cancellationToken)
        {
            var projects = (await _mediator.Send(request, cancellationToken))
                .Select(project => new ProjectDto(project.Id, project.Name))
                .ToList();

            return Ok(projects);
        }
    }
}
