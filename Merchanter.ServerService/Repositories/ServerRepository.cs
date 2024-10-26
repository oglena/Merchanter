using Merchanter.ServerService.Classes;
using Merchanter.ServerService.Services;
using System.Diagnostics;
using System.Management;

namespace Merchanter.ServerService.Repositories {
    public interface IServerRepository {
        Task<List<MerchanterServer>> GetWorkingServers();
        MerchanterService merchanterService { get; set; }
    }
    public class ServerRepository( MerchanterService merchanterService ) :IServerRepository {
        public List<MerchanterServer> working_servers = new List<MerchanterServer>();

        MerchanterService IServerRepository.merchanterService { get; set; } = new MerchanterService();

        public async Task<List<MerchanterServer>> GetWorkingServers() {
            return await GetServers();
        }

        private async Task<List<MerchanterServer>> GetServers() {
            await Task.Factory.StartNew( () => {
                working_servers = new List<MerchanterServer>() { new MerchanterServer() { customer_id = 0, PID = 0 } };
                return working_servers;
            } );
            var processes = Process.GetProcessesByName( "MerchanterServer" );
            List<MerchanterServer> servers = new List<MerchanterServer>();
            foreach( var item in processes ) {
                if( int.TryParse( GetCustomerId( item ), out int cid ) ) {
                    servers.Add( new MerchanterServer() { PID = item.Id, customer_id = cid, customer = merchanterService.helper.GetCustomer( cid ) } );
                }
            }
            return servers;
        }

        private string GetCustomerId( Process item ) {
            using( ManagementObjectSearcher searcher = new ManagementObjectSearcher( "SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + item.Id ) )
            using( ManagementObjectCollection objects = searcher.Get() ) {
                return objects.Cast<ManagementBaseObject>().SingleOrDefault()?[ "CommandLine" ]?.ToString().Split( '"' )[ 2 ].Trim();
            }
        }
    }
}
