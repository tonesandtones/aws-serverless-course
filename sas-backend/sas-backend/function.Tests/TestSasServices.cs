using System.Collections.Generic;
using function.EntryPoints;
using function.model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace function.Tests.EntryPoints
{
    public static class TestSasServices
    {
        public static ServiceCollection DefaultServiceCollection()
        {
            return new ServiceCollection().WithDefaultSasRegistrations();
        }
        
        private static ServiceCollection WithDefaultSasRegistrations(this ServiceCollection services)
        {
            services.AddSasServices();
            return services;
        }
        
        public static ServiceCollection WithDefaultFunctionHandlers(this ServiceCollection services)
        {
            services.AddTransient<GetItemsHandler>();
            services.AddTransient<GetItemByIdHandler>();
            services.AddTransient<GetLoanByIdHandler>();
            services.AddTransient<GetLoansByStatusHandler>();
            return services;
        }
        
        public static ServiceCollection WithTestData(this ServiceCollection services, IEnumerable<Item> items, IEnumerable<Loan> loans)
        {
            services.RemoveAll(typeof(ITestDataAccessor));
            services.AddSingleton(typeof(ITestDataAccessor), new TestDataAccessorTestDouble
            {
                Items = items,
                Loans = loans
            });
            return services;
        }
        
        public static ServiceCollection WithTestItems(this ServiceCollection services, IEnumerable<Item> items)
        {
            return services.WithTestData(items, null);
        }
        
        public static ServiceCollection WithTestLoans(this ServiceCollection services, IEnumerable<Loan> loans)
        {
            return services.WithTestData(null, loans);
        }
    }
}