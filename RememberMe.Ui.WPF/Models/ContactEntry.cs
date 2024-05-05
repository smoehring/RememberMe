﻿namespace Snoval.Dev.RememberMe.Ui.WPF.Models;

public class ContactEntry
{
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string ContactAddress { get; set; } = string.Empty;
    public DateTime LastContact { get; set; } = DateTime.Now;
    public AddressType AddressType { get; set; }
    public Guid TimespanUuid { get; set; } = Guid.Empty;

    protected bool Equals(ContactEntry other)
    {
        return Uuid.Equals(other.Uuid);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ContactEntry)obj);
    }

    public override int GetHashCode()
    {
        return Uuid.GetHashCode();
    }
}