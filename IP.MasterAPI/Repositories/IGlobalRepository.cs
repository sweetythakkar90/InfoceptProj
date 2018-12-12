using IP.MasterAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace IP.MasterAPI.Repositories
{
    public interface IGlobalRepository
    {
        string CipherText(string str, string convertTool);
        void LogData(Exception ex);
        DataTable GetQueryResult(string query);

    }
}
