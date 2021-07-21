﻿using MenU.Models;
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
        private static MenUWebAPI proxy = null;

        public static MenUWebAPI CreateProxy()
        {
            if (proxy == null)
                return new MenUWebAPI();
            return proxy;
        }
        public MenUWebAPI()
        {
            this.baseUri = "";
            //Set client handler to support cookies!!
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();

            //Create client with the handler!
            this.client = new HttpClient(handler, true);
        }

        public async Task<(Account account, int StatusCode)> LoginAsync(string username, string password)
        {
            HttpResponseMessage response;
            try
            {
                response = await this.client.GetAsync($"{this.baseUri}/LoginCredentials?username={username}&pass={password}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    Account acc = JsonSerializer.Deserialize<Account>(content, options);
                    (Account, int) returnTuple = (acc, (int)response.StatusCode);
                    return returnTuple;
                }
                else
                {
                    return (null, StatusCode: (int)response.StatusCode);
                }
            }
            catch (Exception)
            {
                return (null, 409);
            }
        }
        public async Task<(Account account, int StatusCode)> LoginAsync(string token)
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
                    return (acc, (int)response.StatusCode);
                }
                else
                {
                    return (null, StatusCode: (int)response.StatusCode);
                }
            }
            catch (Exception)
            {
                return (null, 409);
            }
        }
        public async Task<(bool isSuccess, int StatusCode)> LogOutAsync()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/LogOut");
                if (response.IsSuccessStatusCode)
                {
                    return (true, StatusCode: (int)response.StatusCode);
                }
                else
                    return (false, (int)response.StatusCode);
            }
            catch (Exception)
            {
                return (false, 409);
            }
        }
        public async Task<(bool exists, int StatusCode)> ExistsAsync(string username, string email)
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
                    return (b, (int)response.StatusCode);
                }
                else
                {
                    return (true, (int)response.StatusCode);
                }
            }
            catch (Exception)
            {
                return (true, 409);
            }
        }
        public async Task<(bool isSuccess, int StatusCode)> SignUpAsync(Account dummyAcc)
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
                    return (b, (int)response.StatusCode);

                }
                else
                {
                    return (false, (int)response.StatusCode);
                }
            }
            catch (Exception)
            {
                return (false, 409);
            }
        }
        public async Task<(string token, int StatusCode)> CreateToken()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/CreateToken");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    string token = JsonSerializer.Deserialize<string>(content, options);
                    return (token, (int)response.StatusCode);
                }
                else
                {
                    return ("", (int)response.StatusCode);
                }
            }
            catch (Exception)
            {
                return ("", 409);
            }
        }
        public async Task<(string salt, int StatusCode)> GenerateSalt()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GenerateSalt");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    string salt = JsonSerializer.Deserialize<string>(content, options);
                    return (salt, (int)response.StatusCode);
                }
                else
                {
                    return ("", (int)response.StatusCode);
                }
            }
            catch (Exception)
            {
                return ("", 409);
            }
        }
        public async Task<(Dictionary<string,string> returnDic, int StatusCode)> GetSaltAndIterations(string username)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetSaltAndIterations?username={username}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    Dictionary<string,string> returnDic = JsonSerializer.Deserialize<Dictionary<string,string>>(content, options);
                    return (returnDic, (int)response.StatusCode);
                }
                else
                {
                    return (null, (int)response.StatusCode);
                }
            }
            catch (Exception)
            {
                return (null, 409);
            }
        }
    }
}
