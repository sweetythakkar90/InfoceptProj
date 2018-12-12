using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface IExceptionLogRepository
    {
        List<ExceptionLog> GetExceptionLogDetailsAsync(int ID);

        void InsertExceptionLogDetailsAsync(ExceptionLog eLog);

        List<ExceptionLog> UpdateExceptionLogDetailsAsync(ExceptionLog eLog);

        List<ExceptionLog> DeleteExceptionLogDetailsAsync(int ID);

    }
}
