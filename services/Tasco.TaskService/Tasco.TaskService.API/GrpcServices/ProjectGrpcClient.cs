using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Tasco.TaskService.API.Protos;

namespace Tasco.TaskService.Service.Services
{
    public class ProjectGrpcClient
    {
        private readonly ProjectService.ProjectServiceClient _client;

        public ProjectGrpcClient(IConfiguration configuration)
        {
            var projectServiceUrl = configuration["Services:ProjectService:Url"] ?? "http://localhost:5002";
            var channel = GrpcChannel.ForAddress(projectServiceUrl);
            _client = new ProjectService.ProjectServiceClient(channel);
        }

        public async Task<bool> CheckProjectExistsAsync(Guid projectId)
        {
            var request = new ProjectRequestById { Id = projectId.ToString() };
            var response = await _client.CheckProjectExistsAsync(request);
            return response.Exists;
        }
    }
} 