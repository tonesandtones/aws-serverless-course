using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using function.model;
using function.Repositories;

namespace function.DynamoDb
{
    public class DynamoDbLoanRepository : ILoanRepository
    {
        private readonly IDynamoDBContext _context;
        private readonly SasDynamoDbConfig _config;

        public DynamoDbLoanRepository(IDynamoDBContext context, SasDynamoDbConfig config)
        {
            _context = context;
            _config = config;
        }

        public async Task<IEnumerable<Loan>> GetLoansByStatus(LoanStatus status)
        {
            return await _context.FromQueryAsync<Loan>(new QueryOperationConfig()
                {
                    IndexName = "by_status",
                    Filter = new QueryFilter("status", QueryOperator.Equal, status.ToString())
                }, Cfg())
                .GetRemainingAsync();
        }

        public Task<Loan> GetLoanById(string id)
        {
            return _context.LoadAsync<Loan>(id, Cfg());
        }

        private DynamoDBOperationConfig Cfg()
        {
            return new DynamoDBOperationConfig {OverrideTableName = _config.LoansTableName};
        }
    }
}