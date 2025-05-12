var builder = DistributedApplication.CreateBuilder(args);

var merchanter_api = builder.AddProject<Projects.MerchanterApp_ApiService>("MerchanterApi");
var server_service = builder.AddProject<Projects.Merchanter_ServerService>("ServerService");
//builder.AddProject<Projects.MerchanterApp_CMS>("MerchanterCMS").WithReference(server_service).WaitFor(server_service);


builder.AddProject<Projects.MerchanterFrontend>("MerchanterFrontend")
    .WithReference(merchanter_api).WaitFor(merchanter_api)
    .WithReference(server_service).WaitFor(server_service);

builder.Build().Run();