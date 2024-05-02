using Snoval.Dev.RememberMe.Ui.WPF.Models;

namespace Snoval.Dev.RememberMe.Ui.WPF;

public static class StringExtensions
{
    public static string ToSocialMediaLink(this string address, AddressType addressType)
    {
        switch (addressType)
        {
            case AddressType.Telegram:
                var s = address;
                if (s.StartsWith('@'))
                {
                    s = s.Remove(0, 1);
                }
                return $"https://t.me/{s}";
            case AddressType.Discord:
                return $"https://discord.com/users/{address}";
            case AddressType.EMail:
                return $"mailto:{address}";
            case AddressType.Generic:
                return address;
            default:
                return address;
        }
    }
}