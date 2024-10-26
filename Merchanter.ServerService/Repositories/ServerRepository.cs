using Merchanter.ServerService.Classes;
using Merchanter.ServerService.Services;
using System.Diagnostics;
using System.Management;
using Microsoft.Extensions.Configuration;

namespace Merchanter.ServerService.Repositories {
    public interface IServerRepository {
        Task<List<MerchanterServer>> GetWorkingServers();
        Task<MerchanterServer> StartServer( int _customer_id );
        Task<MerchanterServer> StopServer( int _customer_id );
        MerchanterService merchanterService { get; set; }
    }
    public class ServerRepository( MerchanterService merchanterService, IConfiguration configuration ) :IServerRepository {
        public List<MerchanterServer> working_servers = new List<MerchanterServer>();

        MerchanterService IServerRepository.merchanterService { get; set; } = new MerchanterService();
        private static readonly IConfiguration config = new ConfigurationBuilder().AddJsonFile( "appsettings.json" ).AddEnvironmentVariables().Build();

        public async Task<List<MerchanterServer>> GetWorkingServers() {
            return await GetServers();
        }

        public async Task<MerchanterServer> StartServer( int _customer_id ) {
            MerchanterServer started_server = new MerchanterServer();
            await Task.Factory.StartNew( () => {
                ProcessStartInfo process = new ProcessStartInfo();
                process.WorkingDirectory = configuration[ "AppSettings:MerchanterServerFilePath" ];
                process.FileName = "MerchanterServer.exe";
                process.Arguments = _customer_id.ToString();
                process.WindowStyle = ProcessWindowStyle.Normal;
                process.UseShellExecute = true;
                var started_process = Process.Start( process );
                started_server = new MerchanterServer() { customer_id = _customer_id, PID = started_process != null ? started_process.Id : 0 };
            } );
            return started_server;
        }

        public async Task<MerchanterServer> StopServer( int _customer_id ) {
            MerchanterServer stopped_server = new MerchanterServer();
            await Task.Factory.StartNew( () => {
                var processes = Process.GetProcessesByName( "MerchanterServer" );
                foreach( var item in processes ) {
                    if( int.TryParse( GetCustomerId( item ), out int cid ) ) {
                        try {
                            item.Kill();
                            stopped_server = new MerchanterServer() { customer_id = _customer_id, PID = item.Id, customer = merchanterService.helper.GetCustomer( cid ) };
                        }
                        catch {
                        }
                    }
                }
            } );
            return stopped_server;
        }

        private async Task<List<MerchanterServer>> GetServers() {
            await Task.Factory.StartNew( () => {
                var processes = Process.GetProcessesByName( "MerchanterServer" );
                List<MerchanterServer> servers = new List<MerchanterServer>();
                foreach( var item in processes ) {
                    if( int.TryParse( GetCustomerId( item ), out int cid ) ) {
                        servers.Add( new MerchanterServer() { PID = item.Id, customer_id = cid, customer = merchanterService.helper.GetCustomer( cid ) } );
                    }
                }
                working_servers = servers;
            } );
            return working_servers;
        }

        private string GetCustomerId( Process item ) {
            using( ManagementObjectSearcher searcher = new ManagementObjectSearcher( "SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + item.Id ) )
            using( ManagementObjectCollection objects = searcher.Get() ) {
                return objects.Cast<ManagementBaseObject>().SingleOrDefault()?[ "CommandLine" ]?.ToString().Split( '"' )[ 2 ].Trim();
            }
        }
    }
}
