using System;
using System.Collections.Generic;

namespace satguruApp.Service.ViewModels
{
    public class DriverWalletSummaryViewModel
    {
        public string? DriverUserId { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalRidePayments { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal PendingWithdrawalAmount { get; set; }
        public int PendingWithdrawalCount { get; set; }
    }

    public class RidePaymentRequestViewModel
    {
        public long RideId { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentMode { get; set; }
        public string? TransactionReference { get; set; }
    }

    public class WithdrawalRequestViewModel
    {
        public string? DriverUserId { get; set; }
        public decimal Amount { get; set; }
        public string? Note { get; set; }
    }

    public class WithdrawalActionViewModel
    {
        public Guid PaymentId { get; set; }
        public string? Action { get; set; } // approve | reject
    }

    public class DriverFinanceResultViewModel
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Guid? PaymentId { get; set; }
        public decimal? UpdatedBalance { get; set; }
    }
}
