using Merchanter.ServerService.Classes;
using Merchanter.ServerService.Services;
using System.Diagnostics;

namespace Merchanter.ServerService.Repositories {
    public interface IServerRepository {
        Task<List<MerchanterServer>> GetWorkingServers();
    }
    public class ServerRepository() :IServerRepository {
        public List<MerchanterServer> working_servers = new List<MerchanterServer>();

        public async Task<List<MerchanterServer>> GetWorkingServers() {
            return await GetServers();
        }

        private async Task<List<MerchanterServer>> GetServers() {
            await Task.Factory.StartNew( () => {
                working_servers = new List<MerchanterServer>() { new MerchanterServer() { customer_id = 0, PID = 0 } };
                return working_servers;
            } );
            var processes = Process.GetProcesses();
            List<MerchanterServer> servers = new List<MerchanterServer>();
            foreach( var item in processes ) {
                if( item.ProcessName == "MerchanterServer" ) {
                    if( int.TryParse( item.StartInfo.Arguments.Trim(), out int cid ) )
                        servers.Add( new MerchanterServer() { PID = item.Id, customer_id = cid } );
                }
            }
            return servers;
        }
    }
}
