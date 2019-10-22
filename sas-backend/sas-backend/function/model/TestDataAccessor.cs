using System.IO;
using Newtonsoft.Json;

namespace function.model
{
    public class TestDataAccessor
    {
        private TestData _testData;

        public TestData TestData
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

        private TestData ReadTestData()
        {
            var raw = File.ReadAllText(@"resources/testdata.json");
            return JsonConvert.DeserializeObject<TestData>(raw);
        }
    }
}