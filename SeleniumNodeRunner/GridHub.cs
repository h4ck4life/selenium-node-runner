namespace SeleniumNodeRunner
{
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class GridHub
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("capabilityMatcher")]
        public string CapabilityMatcher { get; set; }

        [JsonProperty("newSessionWaitTimeout")]
        public long NewSessionWaitTimeout { get; set; }

        [JsonProperty("throwOnCapabilityNotPresent")]
        public bool ThrowOnCapabilityNotPresent { get; set; }

        [JsonProperty("registry")]
        public string Registry { get; set; }

        [JsonProperty("cleanUpCycle")]
        public long CleanUpCycle { get; set; }

        [JsonProperty("custom")]
        public Custom Custom { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("servlets")]
        public object[] Servlets { get; set; }

        [JsonProperty("withoutServlets")]
        public object[] WithoutServlets { get; set; }

        [JsonProperty("browserTimeout")]
        public long BrowserTimeout { get; set; }

        [JsonProperty("debug")]
        public bool Debug { get; set; }

        [JsonProperty("port")]
        public long Port { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("timeout")]
        public long Timeout { get; set; }

        [JsonProperty("newSessionRequestCount")]
        public long NewSessionRequestCount { get; set; }

        [JsonProperty("slotCounts")]
        public SlotCounts SlotCounts { get; set; }
    }

    public partial class Custom
    {
    }

    public partial class SlotCounts
    {
        [JsonProperty("free")]
        public long Free { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }

    public partial class GridHub
    {
        public static GridHub FromJson(string json) => JsonConvert.DeserializeObject<GridHub>(json, SeleniumNodeRunner.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this GridHub self) => JsonConvert.SerializeObject(self, SeleniumNodeRunner.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}