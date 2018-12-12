using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface ILocationRepository
    {
        List<Location> GetLocationDetailsAsync(int LocationID);
        void InsertLocationDetailsAsync(Location Location);
        List<Location> UpdateLocationDetailsAsync(Location Location);
        List<Location> DeleteLocationDetailsAsync(int LocationID);
        
    }
}
