﻿using System;
using System.Collections.Generic;
using System.IO;
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
        static HttpClient _httpClient;
        static HttpClient _httpClient1;
        public HttpClient _EChannelhttpClient;
        string userToken;
        string refreshToken;
        public string cookies = "";

        readonly HttpClientHandler _httpClientHandler = new HttpClientHandler()
        {
            CookieContainer = new CookieContainer(),
            //UseCookies = false,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };
        readonly HttpClientHandler _httpClientHandler1 = new HttpClientHandler()
        {
            CookieContainer = new CookieContainer(),
            UseCookies = false,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };
        public HttpCaller()
        {
            _httpClient = new HttpClient(_httpClientHandler);
            _httpClient.Timeout = TimeSpan.FromMinutes(3);

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
        public async Task<(HtmlDocument doc, string error)> GetDoc1(string url, int maxAttempts = 1)
        {
            var resp = await GetHtml1(url, maxAttempts);
           
            if (resp.error != null) return (null, resp.error);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(resp.html);
            return (doc, null);
        }
        public async Task<(string html, string error)> GetHtml1(string url, int maxAttempts = 3)
        {
            _httpClient1 = new HttpClient(_httpClientHandler1);
            _httpClient1.Timeout = TimeSpan.FromMinutes(3);
            _httpClient1.DefaultRequestHeaders.Add("Cookie", cookies);
            int tries = 0;
            do
            {
                try
                {
                    var response = await _httpClient1.GetAsync(url);
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
        public async Task<(string html, string error)> GetEchannelHtml(string url, string refreshToken, string userToken, int maxAttempts = 1)
        {
            _EChannelhttpClient.DefaultRequestHeaders.Clear();
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
        public static async Task<(Stream html, string error)> PostFormData(string url, List<KeyValuePair<string, string>> formData, int maxAttempts = 1)
        {
            var formContent = new FormUrlEncodedContent(formData);
            _httpClient = new HttpClient();
            int tries = 0;
            do
            {
                try
                {

                    var response = await _httpClient.PostAsync(url, formContent);
                    HttpContent Content = response.Content;
                    var html = await Content.ReadAsStreamAsync();

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
        public async Task<(string html, string error)> PostFormDataForLogIn(string url, List<KeyValuePair<string, string>> formData, int maxAttempts = 1)
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
