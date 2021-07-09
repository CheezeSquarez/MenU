using MenU.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MenU.Services
{

    class MenUWebAPI
    {
        private HttpClient client;
        private string baseUri;

        public MenUWebAPI(string baseUri)
        {
            //Set client handler to support cookies!!
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();

            //Create client with the handler!
            this.client = new HttpClient(handler, true);
            this.baseUri = baseUri;
        }

        public async Task<Account> LoginAsync(string username, string password)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/LoginCredentials?username={username}&pass={password}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    Account acc = JsonSerializer.Deserialize<Account>(content, options);
                    return acc;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<Account> LoginAsync(string token)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/TokenLogin?token={token}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    Account acc = JsonSerializer.Deserialize<Account>(content, options);
                    return acc;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<bool> LogOutAsync()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/LogOut");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> ExistsAsync(string username, string email)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/DoesExist?uName={username}&email={email}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    bool b = JsonSerializer.Deserialize<bool>(content, options);
                    return b;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return true;
            }
        }
        public async Task<bool> SignUpAsync(Account dummyAcc)
        {
            try
            {
                string json = JsonSerializer.Serialize(dummyAcc);

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/SignUp", content);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string c = await response.Content.ReadAsStringAsync();
                    bool b = JsonSerializer.Deserialize<bool>(c, options);
                    return b;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
