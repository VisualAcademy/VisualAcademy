namespace VisualAcademy.Settings;

public sealed class StripeSettings
{
    public string DefaultTenant { get; set; } = "VisualAcademy";

    public Dictionary<string, StripeTenantKeys> Tenants { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

public sealed class StripeTenantKeys
{
    public string PublishableKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
}
