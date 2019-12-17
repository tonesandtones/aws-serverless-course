using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using function.model;

namespace function.Repositories
{
    public class TestDataLoanRepository : ILoanRepository
    {
        private readonly ITestDataAccessor _accessor;

        public TestDataLoanRepository(ITestDataAccessor accessor)
        {
            _accessor = accessor;
        }

        public Task<IEnumerable<Loan>> GetLoansByStatus(LoanStatus status)
        {
            return Task.FromResult(_accessor.Loans.Where(x => x.Status == status));
        }

        public Task<Loan> GetLoanById(string id)
        {
            return Task.FromResult(_accessor.Loans.FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}