using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CallOfBeer.API
{
    internal class HttpProvider
    {
        public async Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string> parameters = null)
        {
            HttpResponseMessage result = null;

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage();

                    request.Method = HttpMethod.Get;

                    string newUrl = string.Concat(url, this.BuildParameters(parameters));

                    request.RequestUri = new Uri(newUrl);

                    result = await httpClient.SendAsync(request);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erreur lors de l'envoie de la requête GET.", ex);
                }
            }
            return result;
        }

        public async Task<HttpResponseMessage> PostAsync(string url, string body)
        {
            HttpResponseMessage result = null;
            using (HttpClient httpClient = new HttpClient(new HttpClientHandler()))
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage();

                    request.Method = HttpMethod.Post;

                    request.Content = new StringContent(body, Encoding.UTF8, "application/json");

                    request.RequestUri = new Uri(url);

                    result = await httpClient.SendAsync(request);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erreur lors de l'envoie de la requête POST.", ex);
                }
            }
            return result;
        }

        public string BuildParameters(Dictionary<string, string> parameters)
        {
            string result = string.Empty;
            if (parameters != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append('?');

                int nbParameters = parameters.Count;
                foreach (var item in parameters)
                {
                    sb.Append(WebUtility.UrlEncode(item.Key));
                    sb.Append('=');
                    sb.Append(WebUtility.UrlEncode(item.Value));
                    nbParameters -= 1;
                    if (nbParameters > 0)
                    {
                        sb.Append('&');
                    }
                }

                result = sb.ToString();
            }
            return result;
        }
    }
}
