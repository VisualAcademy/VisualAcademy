var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.VisualAcademy_ApiService>("apiservice");

builder.AddProject<Projects.VisualAcademy_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
