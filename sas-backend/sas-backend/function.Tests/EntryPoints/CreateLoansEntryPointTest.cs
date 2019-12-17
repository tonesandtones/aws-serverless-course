using System.Threading.Tasks;
using Amazon.Lambda.TestUtilities;
using function.EntryPoints;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace function.Tests.EntryPoints
{
    public class CreateLoansEntryPointTest
    {
        [Fact]
        public async Task TestPost()
        {
            var resolver = TestSasServices.DefaultServiceCollection().WithDefaultFunctionHandlers().BuildServiceProvider();
            
            var handler = resolver.GetRequiredService<ImportTestDataHandler>();
            var result = await handler.HandleAsync(null, new TestLambdaContext());
        }
    }
}