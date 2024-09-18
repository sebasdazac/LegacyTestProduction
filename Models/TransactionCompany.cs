using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class TransactionCompany
    {
        public long Id { get; set; }
        public long IdCompany { get; set; }
        public long IdPlanCompany { get; set; }
        public double Price { get; set; }
        public DateTime DateTransaction { get; set; }
        public string StateTransaction { get; set; } = null!;
        public string NumberReference { get; set; } = null!;
        public string Currency { get; set; } = null!;
        public string PaymentForm { get; set; } = null!;
        public string? CodeNameBank { get; set; }
        public string? CodeTraceability { get; set; }
        public string? CodeService { get; set; }
        public int? TransactionalCycle { get; set; }
        public string PaymentPlataform { get; set; } = null!;
    }
}
