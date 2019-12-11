using System;
using System.Collections.Generic;
using System.Linq;
using function.model;

namespace function.Repositories
{
    public interface ILoanRepository
    {
        IEnumerable<Loan> GetLoansByStatus(LoanStatus status);
        Loan GetLoanById(string id);
    }

    public class LoanRepository : ILoanRepository
    {
        private readonly ITestDataAccessor _accessor;

        public LoanRepository(ITestDataAccessor accessor)
        {
            _accessor = accessor;
        }

        public IEnumerable<Loan> GetLoansByStatus(LoanStatus status)
        {
            return _accessor.Loans.Where(x => x.Status == status);
        }

        public Loan GetLoanById(string id)
        {
            return _accessor.Loans.FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}