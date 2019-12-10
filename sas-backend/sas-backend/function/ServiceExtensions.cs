using Amazon.Lambda.APIGatewayEvents;
using function.model;
using Microsoft.Extensions.DependencyInjection;
using Tiger.Lambda;

namespace function
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddSasServices(this IServiceCollection services)
        {
            services.AddTransient<ITestDataAccessor, TestDataAccessor>();
            services.AddTransient<IResponseBuilderFactory, ApiGatewayProxyResponseBuilderFactory>();
            services.AddTransient<ILoanRepository, LoanRepository>();
            services.AddTransient<IItemRepository, ItemRepository>();
            return services;
        }
        
        public static IServiceCollection AddHandler<THandler, TIn, TOut>(this IServiceCollection services) where THandler : class, IHandler<TIn, TOut>
        {
            return services.AddTransient<IHandler<TIn, TOut>, THandler>();
        }
    }
    
    public static class LambdaExtensions
    {
        public static string PathParameter(this APIGatewayProxyRequest request, string parameterName)
        {
            return request?.PathParameters?[parameterName];
        }
        
        public static string QueryParameter(this APIGatewayProxyRequest request, string parameterName)
        {
            return request?.QueryStringParameters?[parameterName];
        }
    }
}