using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface ICompanyRepository
    {
        List<Company> GetCompanyDetailsAsync(int companyID);

        void InsertCompanyDetailsAsync(Company comp);

        List<Company> UpdateCompanyDetailsAsync(Company comp);

        List<Company> DeleteCompanyDetailsAsync(int companyID);

    }
}
