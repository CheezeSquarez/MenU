using MenU.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MenU.Services
{
    /// <summary>
    /// Proxy method archetecture: Each method returns a tuple containing the desired return value. In addition, the tuple will contain the response code; 
    /// this will be used to send to the error handler in order to display an appropiate error message.
    /// Error code dictionary can be found in the app.cs file
    /// </summary>
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
            this.baseUri = "http://10.0.2.2:39135";
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
                response = await this.client.GetAsync($"{this.baseUri}/accounts/LoginCredentials?username={username}&pass={password}");
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
                return (null, 503);
            }
        }
        public async Task<(Account account, int StatusCode)> LoginAsync(string token)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/accounts/TokenLogin?token={token}");
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
                return (null, 503);
            }
        }
        public async Task<(bool isSuccess, int StatusCode)> LogOutAsync()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/accounts/LogOut");
                if (response.IsSuccessStatusCode)
                {
                    return (true, StatusCode: (int)response.StatusCode);
                }
                else
                    return (false, (int)response.StatusCode);
            }
            catch (Exception)
            {
                return (false, 503);
            }
        }
        public async Task<(bool exists, int StatusCode)> ExistsAsync(string username, string email)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/accounts/DoesExist?uName={username}&email={email}");
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
                return (true, 503);
            }
        }
        public async Task<(bool isSuccess, int StatusCode)> SignUpAsync(Account dummyAcc)
        {
            try
            {
                string json = JsonSerializer.Serialize(dummyAcc);

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/accounts/SignUp", content);

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
                return (false, 503);
            }
        }
        public async Task<(string token, int StatusCode)> CreateToken()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/accounts/CreateToken");
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
                return ("", 503);
            }
        }
        public async Task<(string salt, int StatusCode)> GenerateSalt()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/accounts/GenerateSalt");
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
                return ("", 503);
            }
        }
        public async Task<(Dictionary<string,string> returnDic, int StatusCode)> GetSaltAndIterations(string username)
        {
            try
            {
                string uri = $"{this.baseUri}/accounts/GetSaltAndIterations?username={username}";
                HttpResponseMessage response = await this.client.GetAsync(uri);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (null, 503);
            }
        }
        public async Task<(bool,int)> ChangePass(string hashedPass, int id)
        {
            HttpResponseMessage response;
            try
            {
                response = await this.client.GetAsync($"{this.baseUri}/accounts/ChangePass?id={id}&pass={hashedPass}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    bool deserialized = JsonSerializer.Deserialize<bool>(content, options);
                    return (deserialized, (int)response.StatusCode);
                }
                else
                {
                    return (false, (int)response.StatusCode);
                }
            }
            catch (Exception)
            {
                return (false, 503);
            }
        }

        public async Task<(bool, int)> UpdateAccountInfo(string uName, string fName, string lName)
        {
            HttpResponseMessage response;
            try
            {
                response = await this.client.GetAsync($"{this.baseUri}/accounts/UpdateAccount?Username={uName}&FirstName={fName}&LastName={lName}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    bool deserialized = JsonSerializer.Deserialize<bool>(content, options);
                    return (deserialized, (int)response.StatusCode);
                }
                else
                {
                    return (false, (int)response.StatusCode);
                }
            }
            catch (Exception)
            {
                return (false, 503);
            }
        }
    }
}
