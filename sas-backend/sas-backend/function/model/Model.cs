using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace function.model
{
    public interface IIdentifiable
    {
        string Id { get; set; }
    }

    public class Item : IIdentifiable
    {
        [DynamoDBProperty("id")]
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty("name")]
        public string Name { get; set; }

        [DynamoDBProperty("description")]
        public string Description { get; set; }

        [DynamoDBProperty("category")]
        public string Category { get; set; }

        [DynamoDBProperty("count")]
        public int? Count { get; set; }

        [DynamoDBProperty("consumable")]
        public bool? Consumable { get; set; }
    }

    [DynamoDBTable("sas-loans")]
    public class Loan : IIdentifiable
    {
        [DynamoDBProperty("id")]
        [DynamoDBHashKey]
        [DynamoDBGlobalSecondaryIndexRangeKey("by_status")]
        public string Id { get; set; }

        [DynamoDBProperty("user")]
        public string User { get; set; }

        [DynamoDBProperty("selection")]
        public IList<SelectionItem> Selection { get; set; }

        [DynamoDBProperty("collection_date")]
        public DateTime CollectionDate { get; set; }

        [DynamoDBProperty("return_date")]
        public DateTime ReturnDate { get; set; }

        [DynamoDBProperty("reason")]
        public string Reason { get; set; }

        [DynamoDBProperty("status", typeof(EnumConverter<LoanStatus>))]
        [DynamoDBGlobalSecondaryIndexHashKey("by_status")]
        public LoanStatus Status { get; set; }

        [DynamoDBProperty("events")]
        public IList<LoanEvent> Events { get; set; }
    }

    public class SelectionItem : IIdentifiable
    {
        [DynamoDBProperty("id")]
        public string Id { get; set; }

        [DynamoDBProperty("name")]
        public string Name { get; set; }

        [DynamoDBProperty("description")]
        public string Description { get; set; }

        [DynamoDBProperty("category")]
        public string Category { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LoanStatus
    {
        [EnumMember(Value = "submitted")]
        Submitted,

        [EnumMember(Value = "approved")]
        Approved,

        [EnumMember(Value = "collected")]
        Collected,

        [EnumMember(Value = "returned")]
        Returned,

        [EnumMember(Value = "cancelled")]
        Cancelled
    }

    public class LoanEvent
    {
        [DynamoDBProperty("user")]
        public string User { get; set; }

        [DynamoDBProperty("event_date")]
        public DateTime EventDate { get; set; }

        [DynamoDBProperty("status", typeof(EnumConverter<LoanStatus>))]
        public LoanStatus Status { get; set; }

        [DynamoDBProperty("comment")]
        public string Comment { get; set; }
    }

    //Copied from here: https://gist.github.com/jvwing/ac549044411b7953367e
    public class EnumConverter<TEnum> : IPropertyConverter
    {
        public DynamoDBEntry ToEntry(object value)
        {
            string valueAsString = value.ToString();
            DynamoDBEntry entry = new Primitive(valueAsString);
            return entry;
        }

        public object FromEntry(DynamoDBEntry entry)
        {
            string valueAsString = entry.AsString();
            TEnum valueAsEnum = (TEnum) Enum.Parse(typeof(TEnum), valueAsString);
            return valueAsEnum;
        }
    }
}