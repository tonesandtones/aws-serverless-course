using System.Collections.Generic;
using function.model;

namespace function.Tests.EntryPoints
{
    public class TestDataAccessorTestDouble : ITestDataAccessor
    {
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<Loan> Loans { get; set; }
    }
}