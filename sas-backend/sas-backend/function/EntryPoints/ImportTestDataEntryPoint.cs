using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using function.Builders;
using function.DynamoDb;
using function.model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tiger.Lambda;

namespace function.EntryPoints
{
    public class ImportTestDataEntryPoint : Function<APIGatewayProxyRequest, APIGatewayProxyResponse>
    {
        public override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services
                .AddSasServices()
                .AddHandler<ImportTestDataHandler, APIGatewayProxyRequest, APIGatewayProxyResponse>();
        }
    }

    public class ImportTestDataHandler : IHandler<APIGatewayProxyRequest, APIGatewayProxyResponse>
    {
        private readonly ITestDataAccessor _testData;
        private readonly IDynamoDBContext _context;
        private readonly SasDynamoDbConfig _config;
        private readonly IFactory<IBuilder<APIGatewayProxyResponse>> _response;

        public ImportTestDataHandler(
            ITestDataAccessor testData,
            IDynamoDBContext context,
            SasDynamoDbConfig config,
            IFactory<IBuilder<APIGatewayProxyResponse>> response)
        {
            _testData = testData;
            _context = context;
            _config = config;
            _response = response;
        }

        public async Task<APIGatewayProxyResponse> HandleAsync(APIGatewayProxyRequest input, ILambdaContext context)
        {
            var loanBatch = _context.CreateBatchWrite<Loan>(new DynamoDBOperationConfig() {OverrideTableName = _config.LoansTableName});
            loanBatch.AddPutItems(_testData.Loans);
            await loanBatch.ExecuteAsync();

            var itemBatch = _context.CreateBatchWrite<Item>(new DynamoDBOperationConfig() {OverrideTableName = _config.ItemsTableName});
            itemBatch.AddPutItems(_testData.Items);
            await itemBatch.ExecuteAsync();

            return _response.Create().WithStatusCode(200).Build();
        }
    }
}