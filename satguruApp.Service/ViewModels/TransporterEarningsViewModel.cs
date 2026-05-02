using System;
using System.Collections.Generic;

namespace satguruApp.Service.ViewModels
{
    public class TransporterEarningsViewModel
    {
        public decimal TotalRevenue { get; set; }
        public decimal CommissionPaid { get; set; }
        public decimal NetEarnings { get; set; }
        public List<TransporterSettlementViewModel> Settlements { get; set; } = new();
    }

    public class TransporterSettlementViewModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
    }
}
