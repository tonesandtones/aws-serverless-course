using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.TestUtilities;
using AutoBogus;
using DeepEqual.Syntax;
using function.EntryPoints;
using function.model;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace function.Tests.EntryPoints
{
    public class GetLoanByStatusHandlerTests
    {
        [Fact]
        public async Task GetLoanByStatusTest()
        {
            var testLoans = AutoFaker.Generate<Loan>(50);
            const LoanStatus loanStatus = LoanStatus.Approved;
            var expectedLoans = testLoans.Where(x => x.Status == loanStatus).ToList();

            var resolver = TestSasServices.DefaultServiceCollection()
                .WithDefaultFunctionHandlers()
                .WithTestLoans(testLoans)
                .BuildServiceProvider();

            var handler = resolver.GetRequiredService<GetLoansByStatusHandler>();
            var request = new ApiGatewayProxyRequestBuilder()
                .WithPathParameter("status", loanStatus.ToString())
                .Build();

            var response = await handler.HandleAsync(request, new TestLambdaContext());
            response
                .ShouldHaveStatusCode(200)
                .Body
                .ShouldBeParseableAs<List<Loan>>()
                .ShouldDeepEqual(expectedLoans);
        }

        [Fact]
        public async Task GetLoanByIdShouldReturn400WhenNoStatusSpecified()
        {
            var resolver = TestSasServices.DefaultServiceCollection()
                .WithDefaultFunctionHandlers()
                .BuildServiceProvider();

            var handler = resolver.GetRequiredService<GetLoansByStatusHandler>();
            var request = new ApiGatewayProxyRequestBuilder()
                .Build();

            var response = await handler.HandleAsync(request, new TestLambdaContext());
            response
                .ShouldHaveStatusCode(400)
                .Body.ShouldBeParseableAs<ErrorResponse>()
                .StatusCode.ShouldBe(400);
        }
    }
}