using IP.JobsAPI.Models;
using System.Collections.Generic;

namespace IP.JobsAPI.Repositories
{
    public interface IExceptionLogRepository
    {
        List<ExceptionLog> GetExceptionLogDetailsAsync(int ID);

        void InsertExceptionLogDetailsAsync(ExceptionLog eLog);

        List<ExceptionLog> UpdateExceptionLogDetailsAsync(ExceptionLog eLog);

        List<ExceptionLog> DeleteExceptionLogDetailsAsync(int ID);

    }
}
