var builder = DistributedApplication.CreateBuilder( args );

builder.AddProject<Projects.MerchanterApp_CMS>( "MerchanterCMS" );
builder.AddProject<Projects.MerchanterApp_ApiService>( "MerchanterApi" );
builder.AddProject<Projects.Merchanter_ServerService>( "ServerService" );


builder.Build().Run();