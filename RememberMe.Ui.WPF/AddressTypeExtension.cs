using MudBlazor;
using Snoval.Dev.RememberMe.Ui.WPF.Models;

namespace Snoval.Dev.RememberMe.Ui.WPF;

public static class AddressTypeExtension
{
    public static string ToFriendlyString(this AddressType addressType)
    {
        return addressType switch
        {
            AddressType.Generic => "Generic",
            AddressType.Telegram => "Telegram",
            AddressType.EMail => "E-Mail",
            AddressType.Discord => "Discord",
            _ => throw new ArgumentOutOfRangeException(nameof(addressType), addressType, null)
        };
    }

    public static string GetIcon(this AddressType addressType)
    {
        return addressType switch
        {
            AddressType.Generic => Icons.Material.Filled.ContactPage,
            AddressType.Telegram => System.Text.Encoding.Default.GetString(Resources.telegram_logo),
            AddressType.EMail => Icons.Material.Filled.AlternateEmail,
            AddressType.Discord => System.Text.Encoding.Default.GetString(Resources.discord_mark_blue),
            _ => throw new ArgumentOutOfRangeException(nameof(addressType), addressType, null)
        };
    }
}