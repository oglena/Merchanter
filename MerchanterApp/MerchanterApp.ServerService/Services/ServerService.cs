using MerchanterApp.ServerService.Classes;
using MerchanterApp.ServerService.Repositories;

namespace MerchanterApp.ServerService.Services {
    public interface IServerService {
        Task<List<MerchanterServer>> GetServers();
        Task<MerchanterServer> StartServer(int _customer_id);
        Task<MerchanterServer> StopServer(int _customer_id);
    }

    public class ServerService(IServerRepository serverRepository) : IServerService {
        public Task<List<MerchanterServer>> GetServers() {
            return serverRepository.GetWorkingServers();
        }
        public Task<MerchanterServer> StartServer(int _customer_id) {
            return serverRepository.StartServer(_customer_id);
        }
        public Task<MerchanterServer> StopServer(int _customer_id) {
            return serverRepository.StopServer(_customer_id);
        }
    }
}
