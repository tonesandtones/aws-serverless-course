using System.Collections.Generic;
using System.Threading.Tasks;
using function.model;

namespace function.Repositories
{
    public interface ILoanRepository
    {
        Task<IEnumerable<Loan>> GetLoansByStatus(LoanStatus status);
        Task<Loan> GetLoanById(string id);
    }
}