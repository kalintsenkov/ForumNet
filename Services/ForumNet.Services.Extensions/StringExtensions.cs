namespace ForumNet.Services.Extensions
{
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        private const string YouTubeEmbedFormat = "https://www.youtube.com/embed/{0}";
        private const string YouTubeRegexPattern = @"youtu(?:\.be|be\.com)/(?:(.*)v(/|=)|(.*/)?)([a-zA-Z0-9-_]+)";

        public static string UrlToYouTubeEmbed(this string url)
        {
            var youTubeRegex = new Regex(YouTubeRegexPattern, RegexOptions.IgnoreCase);
            var youtubeMatch = youTubeRegex.Match(url);
            if (!youtubeMatch.Success)
            {
                return null;
            }

            var youtubeId = youtubeMatch.Groups[^1].Value;
            var embed = string.Format(YouTubeEmbedFormat, youtubeId);

            return embed;
        }
    }
}