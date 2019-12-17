using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace function.model
{
    public interface ITestDataAccessor
    {
        IEnumerable<Item> Items { get; }
        IEnumerable<Loan> Loans { get; }
    }

    public class TestDataAccessor : ITestDataAccessor
    {
        private TestData _testData;

        private TestData TestData
        {
            get
            {
                if (_testData == null)
                {
                    _testData = ReadTestData();
                }

                return _testData;
            }
        }

        public IEnumerable<Item> Items => TestData.Items;
        public IEnumerable<Loan> Loans => TestData.Loans;

        private TestData ReadTestData()
        {
            var raw = File.ReadAllText(@"resources/testdata.json");
            return JsonConvert.DeserializeObject<TestData>(raw);
        }
    }
}