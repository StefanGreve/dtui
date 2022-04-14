namespace dtui
{
    internal class Language
    {
        public const string English = "en_US";

        public const string Japanese = "ja_JP";

        public const string Spanish = "es_ES"; // TODO

        public const string Chinese = "zh_CH"; // TODO

        public static bool IsAsian(string language) => language == Japanese || language == Chinese;
    }
}
