using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var server = builder.AddSqlServer("sqlserver");

var database = server.AddDatabase("mydb");

var sqlproj = builder.AddSqlProject("sqlproj")
    .WithDacpac("..\\..\\SqlProject2\\bin\\Debug\\SqlProject2.dacpac")
    .WithReference(database);

var apiService = builder.AddProject<Projects.AspireApp1_ApiService>("apiservice")
    .WithReference(database)
    .WaitFor(database)
    .WaitFor(sqlproj)
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.AspireApp1_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
