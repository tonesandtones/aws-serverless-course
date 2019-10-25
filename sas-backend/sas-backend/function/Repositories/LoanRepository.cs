using System;
using System.Collections.Generic;
using System.Linq;
using function.model;

namespace function
{
    public interface ILoanRepository
    {
        IEnumerable<Loan> GetLoansByStatus(LoanStatus status);
        Loan GetLoanById(string id);
    }

    public class LoanRepository : ILoanRepository
    {
        private readonly TestDataAccessor _accessor;

        public LoanRepository(TestDataAccessor accessor)
        {
            _accessor = accessor;
        }

        public IEnumerable<Loan> GetLoansByStatus(LoanStatus status)
        {
            return _accessor.TestData.Loans.Where(x => x.Status == status);
        }

        public Loan GetLoanById(string id)
        {
            return _accessor.TestData.Loans.FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}