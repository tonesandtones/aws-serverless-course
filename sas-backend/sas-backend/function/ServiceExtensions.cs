using Amazon.Lambda.APIGatewayEvents;
using function.Builders;
using function.model;
using function.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Tiger.Lambda;

namespace function
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddSasServices(this IServiceCollection services)
        {
            services.AddTransient<ITestDataAccessor, TestDataAccessor>();
            services.AddTransient<IFactory<IBuilder<APIGatewayProxyResponse>>, ApiGatewayProxyResponseBuilderFactory>();
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
            if (request?.PathParameters?.ContainsKey(parameterName) ?? false)
            {
                return request?.PathParameters?[parameterName];
            }
            return null;
        }
        
        public static string QueryParameter(this APIGatewayProxyRequest request, string parameterName)
        {
            if (request?.QueryStringParameters?.ContainsKey(parameterName) ?? false)
            {
                return request?.QueryStringParameters?[parameterName];
            }
            return null;
        }
    }
}