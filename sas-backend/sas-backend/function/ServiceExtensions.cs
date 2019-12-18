using System.IO;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using function.Builders;
using function.DynamoDb;
using function.model;
using function.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tiger.Lambda;

namespace function
{
    public static class ServiceExtensions
    {
        // ReSharper disable RedundantTypeArgumentsOfMethod - redundant type arguments make it clearer what service is being registered
        public static IServiceCollection AddSasServices(this IServiceCollection services)
        {
            services.AddTransient<ITestDataAccessor, TestDataAccessor>();
            services.AddTransient<IFactory<IBuilder<APIGatewayProxyResponse>>, ApiGatewayProxyResponseBuilderFactory>();
            // services.AddTransient<ILoanRepository, TestDataLoanRepository>();
            services.AddTransient<ILoanRepository, DynamoDbLoanRepository>();
            // services.AddTransient<IItemRepository, TestDataItemRepository>();
            services.AddTransient<IItemRepository, DynamoDbItemRepository>();

            services.AddSingleton<IConfiguration>(s =>
            {
                return (IConfiguration) new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false)
                    .AddJsonFile("appsettings.development.json", true)
                    .AddEnvironmentVariables()
                    .Build();
            });

            services.AddSingleton<SasDynamoDbConfig>(s =>
            {
                return s.GetRequiredService<IConfiguration>()
                    .GetSection("DynamoDb")
                    .Get<SasDynamoDbConfig>();
            });

            services.AddTransient<IDynamoDbClientFactory, DynamoDbClientFactory>();
            
            //register the concrete implementation, satisfied by the factory
            services.AddTransient<AmazonDynamoDBClient>(s => s.GetRequiredService<IDynamoDbClientFactory>().Create());
            //register the interface, satisfied by the concrete impl registration
            services.AddTransient<IAmazonDynamoDB>(s => s.GetRequiredService<AmazonDynamoDBClient>());
            
            services.AddTransient<IDynamoDbContextFactory, DynamoDbContextFactory>();
            
            //register the concrete implementation, satisfied by the factory
            services.AddTransient<DynamoDBContext>(s => s.GetRequiredService<IDynamoDbContextFactory>().Create());
            //register the interface, satisfied by the concrete impl registration
            services.AddTransient<IDynamoDBContext>(s => s.GetRequiredService<DynamoDBContext>());

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