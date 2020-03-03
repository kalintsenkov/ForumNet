namespace ForumNet.Services.Common.Extensions
{
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        private const string YouTubeEmbedFormat = "https://www.youtube.com/embed/{0}";

        private static readonly Regex YouTubeVideoRegex
            = new Regex(@"youtu(?:\.be|be\.com)/(?:(.*)v(/|=)|(.*/)?)([a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase);

        public static string UrlToYoutubeEmbed(this string url)
        {
            var youtubeMatch = YouTubeVideoRegex.Match(url);
            if (youtubeMatch.Success)
            {
                return GetYouTubeEmbedCode(youtubeMatch.Groups[^1].Value);
            }

            return null;
        }

        private static string GetYouTubeEmbedCode(string youtubeId)
        {
            return string.Format(YouTubeEmbedFormat, youtubeId);
        }
    }
}