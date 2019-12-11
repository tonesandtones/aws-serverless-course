using System;
using System.Threading.Tasks;
using Amazon.Lambda.TestUtilities;
using AutoBogus;
using function.EntryPoints;
using function.model;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace function.Tests.EntryPoints
{
    public class GetLoanByIdHandlerTests
    {
        [Fact]
        public async Task GetLoanByIdTest()
        {
            var testLoans = AutoFaker.Generate<Loan>(50);

            var resolver = TestSasServices.DefaultServiceCollection()
                .WithDefaultFunctionHandlers()
                .WithTestLoans(testLoans)
                .BuildServiceProvider();

            var handler = resolver.GetRequiredService<GetLoanByIdHandler>();
            var request = new ApiGatewayProxyRequestBuilder()
                .WithPathParameter("id", Guid.NewGuid().ToString())
                .Build();

            var response = await handler.HandleAsync(request, new TestLambdaContext());
            response
                .ShouldHaveStatusCode(404)
                .Body
                .ShouldBeNull();
        }

        [Fact]
        public async Task GetLoanByIdShouldReturn400WhenNoIdSpecified()
        {
            var resolver = TestSasServices.DefaultServiceCollection()
                .WithDefaultFunctionHandlers()
                .BuildServiceProvider();

            var handler = resolver.GetRequiredService<GetLoanByIdHandler>();
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