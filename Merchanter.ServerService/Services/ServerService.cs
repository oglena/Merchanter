using Merchanter.Classes;
using Merchanter.ServerService.Classes;
using Merchanter.ServerService.Repositories;

namespace Merchanter.ServerService.Services {
    public interface IServerService {
        Task<List<MerchanterServer>> GetServers();
    }

    public class ServerService( IServerRepository serverRepository ) :IServerService {

        public Task<List<MerchanterServer>> GetServers() {
            return serverRepository.GetWorkingServers();
        }
    }
}
