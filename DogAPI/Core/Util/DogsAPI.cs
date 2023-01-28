using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DogAPI.Core.Util
{
    public class DogsAPI
    {
        class Data<T> where T : new()
        {
            [JsonPropertyName("message")]
            public T Message { get; set; } = new T();
        }

        private readonly HttpClient httpClient;
        private readonly string domain;

        public DogsAPI() : this("https://dog.ceo/api") { }
        public DogsAPI(string domain)
        {
            httpClient = new();
            this.domain = domain;
        }

        public T Post<T>(string endPoint) where T : new()
        {
            HttpRequestMessage request = new(HttpMethod.Post, domain + endPoint);

            string json = httpClient.Send(request).Content
                .ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();
            Data<T>? keys = JsonSerializer.Deserialize<Data<T>?>(json);

            if (keys is null) return new T();
            return keys.Message;
        }
        public static T StaticPost<T>(string route) where T : new()
        {
            HttpRequestMessage request = new(HttpMethod.Post, route);

            HttpClient httpClient = new();

            string json = httpClient.Send(request).Content
                .ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();
            Data<T>? keys = JsonSerializer.Deserialize<Data<T>?>(json);

            if (keys is null) return new T();
            return keys.Message;
        }
    }
}
