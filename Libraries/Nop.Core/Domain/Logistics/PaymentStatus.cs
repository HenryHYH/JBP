namespace Nop.Core.Domain.Logistics
{
    public enum PaymentStatus
    {
        未知 = -2,
        已取消 = -1,
        未支付 = 0,
        已付全款 = 1,
        部分支付 = 2
    }
}
