using System;
using System.Collections.Generic;
using System.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json;

namespace GoogleTasksSite.Models
{
    public class GoogleTasksRepository
    {
        private const string url = "https://accounts.google.com/o/oauth2/auth";
        private const string responseCode = "code";
        private const string clientId = "188689364935.apps.googleusercontent.com";
        readonly string redirectUri = HttpUtility.UrlEncode("http://localhost:50128");
        readonly string scope = HttpUtility.UrlEncode("https://www.googleapis.com/auth/tasks.readonly");
        private const string clientSecret = "ArG99W5SDZHQo9JJu3_xirqv";
        private const string grantType = "authorization_code";
        private const string tokenUrl = "https://accounts.google.com/o/oauth2/token";
        private const string restUri = "https://www.googleapis.com/tasks/v1/users/@me/lists";
        private const string tasksRestURI = "https://www.googleapis.com/tasks/v1/lists/{0}/tasks";
        private const string localhost = "http://localhost:50128";

        private HttpClient _httpClient;

        public GoogleTasksRepository()
        {
            _httpClient = new HttpClient();
        }

        public string GetReturnCodeURL()
        {
            return string.Format("{0}?response_type={1}&client_id={2}&redirect_uri={3}&scope={4}", url, responseCode, clientId, redirectUri, scope);
        }

        public Token GetToken(string code)
        {
            var buffer = new Dictionary<string, string>
                                                        {
                                                            {"code", code},
                                                            {"redirect_uri", localhost},
                                                            {"grant_type", grantType},
                                                            {"client_id", clientId},
                                                            {"client_secret", clientSecret}
                                                        };

            var content = new FormUrlEncodedContent(buffer);
            var result = _httpClient.PostAsync(tokenUrl, content).Result;
            var value = JsonValue.Parse(result.Content.ReadAsStringAsync().Result);
            return JsonConvert.DeserializeObject<Token>(value.ToString());
        }

        public List<TaskList> GetTaskLists(Token token)
        {
            var authenticationHeader = new AuthenticationHeaderValue(token.Token_type, token.Access_token);
            _httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
            var listsResult = _httpClient.GetAsync(restUri).Result.Content.ReadAsStringAsync().Result;
            var items = (JsonArray) JsonValue.Parse(listsResult).AsDynamic().items;
            return JsonConvert.DeserializeObject<List<TaskList>>(items.ToString());
        }

        public List<Task> GetTasks(string taskListId, Token token)
        {
            var authenticationHeader = new AuthenticationHeaderValue(token.Token_type, token.Access_token);
            _httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
            var tasksResult = _httpClient.GetAsync(String.Format(tasksRestURI, taskListId)).Result.Content.ReadAsStringAsync().Result;
            var items = (JsonArray) JsonValue.Parse(tasksResult).AsDynamic().items;
            return JsonConvert.DeserializeObject<List<Task>>(items.ToString());
        }
    }
}