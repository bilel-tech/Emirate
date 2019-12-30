using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace EmirateHMBot.Models
{
    public class HttpCaller
    {
        HttpClient _httpClient;
       public HttpClient _EChannelhttpClient;
        string userToken;
        string refreshToken;
        readonly HttpClientHandler _httpClientHandler = new HttpClientHandler()
        {
            CookieContainer = new CookieContainer(),
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };
        public HttpCaller()
        {
            _httpClient = new HttpClient(_httpClientHandler);
            _EChannelhttpClient = new HttpClient(_httpClientHandler);
            //_EChannelhttpClient.DefaultRequestHeaders.Add("userToken", userToken);
            //_EChannelhttpClient.DefaultRequestHeaders.Add("refreshToken", refreshToken);
        }
        public async Task<(HtmlDocument doc, string error)> GetDoc(string url, int maxAttempts = 1)
        {
            var resp = await GetHtml(url, maxAttempts);
            if (resp.error != null) return (null, resp.error);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(resp.html);
            return (doc, null);
        }
        public async Task<(string html, string error)> GetHtml(string url, int maxAttempts = 1)
        {
            int tries = 0;
            do
            {
                try
                {
                    var response = await _httpClient.GetAsync(url);
                    string html = await response.Content.ReadAsStringAsync();
                    return (html, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    tries++;
                    if (tries == maxAttempts)
                    {
                        return (null, ex.ToString());
                    }
                    await Task.Delay(2000);
                }
            } while (true);
        }
        public async Task<(string html, string error)> GetEchannelHtml(string url,string refreshToken,string userToken, int maxAttempts = 1)
        {
            //this. refreshToken = refreshToken;
            //this.userToken = userToken;
            _EChannelhttpClient.DefaultRequestHeaders.Add("refreshToken", refreshToken);
            _EChannelhttpClient.DefaultRequestHeaders.Add("userToken", userToken);
            //_EChannelhttpClient.DefaultRequestHeaders.Add("User-Agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36");
            int tries = 0;
            do
            {
                try
                {
                    var response = await _EChannelhttpClient.GetAsync(url);
                    string html = await response.Content.ReadAsStringAsync();
                    return (html, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    tries++;
                    if (tries == maxAttempts)
                    {
                        return (null, ex.ToString());
                    }
                    await Task.Delay(2000);
                }
            } while (true);
        }
        public async Task<(string json, string error)> PostJson(string url, string json, int maxAttempts = 1)
        {
            int tries = 0;
            do
            {
                try
                {
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    // content.Headers.Add("x-appeagle-authentication", Token);
                    var r = await _httpClient.PostAsync(url, content);
                    var s = await r.Content.ReadAsStringAsync();
                    return (s, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    tries++;
                    if (tries == maxAttempts)
                    {
                        return (null, e.ToString());
                    }
                    await Task.Delay(2000);
                }
            } while (true);

        }
        public async Task<(string html, string error)> PostFormData(string url, List<KeyValuePair<string, string>> formData, int maxAttempts = 1)
        {
            var formContent = new FormUrlEncodedContent(formData);
            int tries = 0;
            do
            {
                try
                {
                    var response = await _httpClient.PostAsync(url, formContent);
                    string html = await response.Content.ReadAsStringAsync();
                    return (html, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    tries++;
                    if (tries == maxAttempts)
                    {
                        return (null, ex.ToString());
                    }
                    await Task.Delay(2000);
                }
            } while (true);
        }
    }
}
