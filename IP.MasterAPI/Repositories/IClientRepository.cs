using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface IClientRepository
    {
        List<Client> GetClientDetailsAsync(int Id);
        void InsertClientDetailsAsync(Client client);
        List<Client> UpdateClientDetailsAsync(Client client);
        List<Client> DeleteClientDetailsAsync(int Id);
        
        
    }
}
