using System.ComponentModel;

namespace DefaultOnionArchitecture.Application.Constants;

public static class PaymentStatus
{
    [Description("Sipariş Bekliyor")]
    public const int Pending = 0;

    [Description("Sipariş Onaylandı")]
    public const int Confirm = 1;

    [Description("Sipariş İptal Edildi")]
    public const int Cancel = 2;

    [Description("Sipariş Tamamlandı")]
    public const int Success = 3;
}
