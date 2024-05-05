namespace Snoval.Dev.RememberMe.Ui.WPF.Models;

public class ContactTimespan
{
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public int Timespan { get; set; } = 1;
    
    public override string ToString()
    {
        return Name;
    }

    protected bool Equals(ContactTimespan other)
    {
        return Uuid.Equals(other.Uuid);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ContactTimespan)obj);
    }

    public override int GetHashCode()
    {
        return Uuid.GetHashCode();
    }
}