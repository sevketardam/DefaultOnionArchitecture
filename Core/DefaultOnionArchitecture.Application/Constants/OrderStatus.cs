using System.ComponentModel;

namespace DefaultOnionArchitecture.Application.Constants;

public static class OrderStatus
{
    [Description("Ödeme Bekliyor")]
    public const int Pending = 0;

    [Description("Ödeme Alındı")]
    public const int Success = 1;

    [Description("Ödeme İptal Edildi")]
    public const int Cancel = 2;
}
