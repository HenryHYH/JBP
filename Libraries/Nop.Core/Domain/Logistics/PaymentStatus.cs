namespace Nop.Core.Domain.Logistics
{
    public enum PaymentStatus
    {
        Unknown = 99,
        Cancelled = 20,
        Pending = 1,
        Paid = 10,
        PartiallyPaid = 15
    }
}
