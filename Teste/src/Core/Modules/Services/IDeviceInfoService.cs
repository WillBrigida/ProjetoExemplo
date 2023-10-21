using static Core.CoreExtensions;

namespace Core
{
    public interface IDeviceInfoService
    {
        public static ePlatform Platform { get; }
    }

    public enum ePlatform
    {
        [RawValueAttribute("Android")]
        Android,
        [RawValueAttribute("iOS")]
        iOS,
        [RawValueAttribute("macOS")]
        macOS,
        [RawValueAttribute("MacCatalyst")]
        MacCatalyst,
        [RawValueAttribute("tvOS")]
        tvOS,
        [RawValueAttribute("Tizen")]
        Tizen,
        [RawValueAttribute("WinUI")]
        WinUI,
        [RawValueAttribute("watchOS")]
        watchOS,
        [RawValueAttribute("Web")]
        Web,
        [RawValueAttribute("Unknown")]
        Unknown
    }
}

