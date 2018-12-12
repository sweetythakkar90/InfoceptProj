using IP.JobsAPI.Models;
using System.Collections.Generic;

namespace IP.JobsAPI.Repositories
{
    public interface IGlobalRepository
    {
        string CipherText(string str, string convertTool);
    }
}
