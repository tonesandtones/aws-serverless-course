using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace function.DynamoDb
{
    public class SasDynamoDbConfig
    {
        public string EndpointUrl { get; set; }
        public string RegionEndpoint { get; set; }

        [Required]
        [RegularExpression("[a-zA-Z0-9-_.]{3,255}")] //dynamodb table name requirements
        public string LoansTableName { get; set; }

        [Required]
        [RegularExpression("[a-zA-Z0-9-_.]{3,255}")] //dynamodb table name requirements
        public string ItemsTableName { get; set; }
    }
}