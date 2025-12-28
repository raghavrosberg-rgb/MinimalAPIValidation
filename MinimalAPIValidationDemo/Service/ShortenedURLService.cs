using MinimalAPIValidationDemo.Models;
using MinimalAPIValidationDemo.Utility;
using System.Data.SqlTypes;
using System.Text.Json;

namespace MinimalAPIValidationDemo.Service
{
    public class ShortenedURLService
    {
        private readonly List<ShortenedURL> _shortenedurl = [];
        private readonly string jsonShortUrlsDataPath = "Data/shorturls.json";
        public ShortenedURLService()
        {
            var mockData = File.ReadAllText(jsonShortUrlsDataPath);
            _shortenedurl = JsonSerializer.Deserialize<List<ShortenedURL>>(mockData, JsonSerializerOptions.Web) ?? [];
        }
        public string GenerateShortUrl(HttpContext httpContext, ShortenedURLDTO request)
        {
            var existingUrl = _shortenedurl.Where(s => s.LongUrl == request.Url).FirstOrDefault();
            if (existingUrl != null)
            {
                return existingUrl.ShortUrl;
            }

            if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
            {
                throw new Exception("The specified URL is invalid.");
            }
            var code = UrlShortenerSettings.Code;
            var newShortenedUrl = new ShortenedURL
            {
                Id = Guid.NewGuid(),
                LongUrl = request.Url,
                Code = code,
                ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{code}",
                CreatedOnUtc = DateTime.UtcNow
            };

            if (!_shortenedurl.Contains(newShortenedUrl))
            {   
                _shortenedurl.Add(newShortenedUrl);
                string updatedJson = JsonSerializer.Serialize(_shortenedurl, UrlShortenerSettings.CachedJsonSerializerOptions);
                File.WriteAllText(jsonShortUrlsDataPath, updatedJson);
                return newShortenedUrl.ShortUrl;
            }

            throw new Exception("Could not generate short url.");
        }

        public string GetLongUrl(string code)
        { 
            return _shortenedurl.FirstOrDefault(s => s.Code == code)?.LongUrl ?? "URL not found.";
        }
    }
}
