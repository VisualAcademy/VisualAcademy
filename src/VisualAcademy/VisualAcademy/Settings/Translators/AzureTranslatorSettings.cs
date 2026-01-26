namespace VisualAcademy.Settings.Translators
{
    public class AzureTranslatorSettings
    {
        // Non-nullable properties 초기화로 CS8618 경고 제거
        public string Endpoint { get; set; } = string.Empty;
        public string SubscriptionKey { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
    }
}
