using IP.MasterAPI.Models;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace IP.MasterAPI.Repositories
{
    public interface IStatusTypeRepository
    {
        List<StatusType> GetStatusTypeDetailsAsync(int ID);

        void InsertStatusTypeDetailsAsync(StatusType statusType);

        List<StatusType> UpdateStatusTypeDetailsAsync(StatusType statusType);

        List<StatusType> DeleteStatusTypeDetailsAsync(int ID);

    }
}
