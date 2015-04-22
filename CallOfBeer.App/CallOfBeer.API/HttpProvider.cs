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
        public async Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string> headers, Dictionary<string, string> parameters = null)
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

                    // Ajout des headers
                    this.AddHeaders(request, headers);

                    result = await httpClient.SendAsync(request);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erreur lors de l'envoie de la requête GET.", ex);
                }
            }
            return result;
        }

        public async Task<HttpResponseMessage> PostAsync(string url, Dictionary<string, string> headers, string body)
        {
            HttpResponseMessage result = null;
            using (HttpClient httpClient = new HttpClient(new HttpClientHandler()))
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage();

                    request.Method = HttpMethod.Post;

                    if (body != null)
                    {
                        request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                    }

                    request.RequestUri = new Uri(url);

                    // Ajout des headers
                    this.AddHeaders(request, headers);

                    result = await httpClient.SendAsync(request);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erreur lors de l'envoie de la requête POST.", ex);
                }
            }
            return result;
        }

        private string BuildParameters(Dictionary<string, string> parameters)
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

        private void AddHeaders(HttpRequestMessage request, Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                try
                {
                    foreach (var item in headers)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Une erreur est survenue lors de la création des headers.", ex);
                }
            }
        }
    }
}
