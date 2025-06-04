var builder = DistributedApplication.CreateBuilder(args);


builder.AddProject<Projects.MerchanterApp_ServerService>("ServerService");
builder.AddProject<Projects.ApiService>("apiservice");
builder.AddProject<Projects.CMS>("CMS");

builder.AddProject<Projects.MerchanterFrontend>("MerchanterFrontend");

builder.Build().Run();