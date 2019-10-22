using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace function.model
{
    public abstract class Identifiable
    {
        public string Id { get; set; }
    }
    
    public class Item : Identifiable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int? Count { get; set; }
        public bool? Consumable { get; set; }
    }
    
    public class Loan : Identifiable
    {
        public string User { get; set; }
        public IList<SelectionItem> Selection { get; set; }
        public DateTimeOffset CollectionDate { get; set; }
        public DateTimeOffset ReturnDate { get; set; }
        public string Reason { get; set; }
        public LoanStatus Status { get; set; }
        public IList<LoanEvent> Events { get; set; }
    }

    public class SelectionItem : Identifiable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }

    public enum LoanStatus
    {
        [EnumMember(Value = "submitted")] Submitted,
        [EnumMember(Value = "approved")] Approved,
        [EnumMember(Value = "collected")] Collected,
        [EnumMember(Value = "returned")] Returned,
        [EnumMember(Value = "cancelled")] Cancelled
    }

    public class LoanEvent
    {
        public string User { get; set; }
        public DateTimeOffset EventDate { get; set; }
        public LoanStatus Status { get; set; }
        public string Comment { get; set; }
    }
}