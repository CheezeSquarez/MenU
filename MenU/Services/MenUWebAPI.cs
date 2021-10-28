using MenU.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
        private const string BASE_URI = "http://10.0.2.2:39135";
        private static MenUWebAPI proxy = null;

        public static MenUWebAPI CreateProxy()
        {
            if (proxy == null)
                return new MenUWebAPI();
            return proxy;
        }
        public MenUWebAPI()
        {
            //Set client handler to support cookies!!
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            //Create client with the handler!
            this.client = new HttpClient(handler, true);
        }

        public async Task<(Account account, int StatusCode)> LoginAsync(string token)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{BASE_URI}/accounts/TokenLogin?token={token}");
                if (response.IsSuccessStatusCode)
                {
                    
                    string content = await response.Content.ReadAsStringAsync();
                    Account acc = Newtonsoft.Json.JsonConvert.DeserializeObject<Account>(content);
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

        public async Task<(Account account, int StatusCode)> LoginAsync(string username, string password)
        {
            
            try
            {
                Credentials credentials = new Credentials() { username = username, password = password };
                string json = JsonConvert.SerializeObject(credentials);

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{BASE_URI}/accounts/LoginCredentials", content);

                if (response.IsSuccessStatusCode)
                {
                    
                    string responseSerialized = await response.Content.ReadAsStringAsync();
                    Account deserializedAccount = JsonConvert.DeserializeObject<Account>(responseSerialized);
                    return (deserializedAccount, (int)response.StatusCode);

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


        public async Task<(bool isSuccess, int StatusCode)> SignUpAsync(Account dummyAcc)
        {
            try
            {
                string json = JsonConvert.SerializeObject(dummyAcc);

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{BASE_URI}/accounts/SignUp", content);

                if (response.IsSuccessStatusCode)
                {
                    
                    string c = await response.Content.ReadAsStringAsync();
                    bool b = JsonConvert.DeserializeObject<bool>(c);
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
        public async Task<(bool isSuccess, int StatusCode)> LogOutAsync()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{BASE_URI}/accounts/LogOut");
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
                HttpResponseMessage response = await this.client.GetAsync($"{BASE_URI}/accounts/DoesExist?uName={username}&email={email}");
                if (response.IsSuccessStatusCode)
                {
                    
                    string content = await response.Content.ReadAsStringAsync();
                    bool b = JsonConvert.DeserializeObject<bool>(content);
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
        public async Task<(string token, int StatusCode)> CreateToken()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{BASE_URI}/accounts/CreateToken");
                if (response.IsSuccessStatusCode)
                {
                    
                    string content = await response.Content.ReadAsStringAsync();
                    string token = JsonConvert.DeserializeObject<string>(content);
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
                HttpResponseMessage response = await this.client.GetAsync($"{BASE_URI}/accounts/GenerateSalt");
                if (response.IsSuccessStatusCode)
                {
                    
                    string content = await response.Content.ReadAsStringAsync();
                    string salt = JsonConvert.DeserializeObject<string>(content);
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
                string uri = $"{BASE_URI}/accounts/GetSaltAndIterations?username={username}";
                HttpResponseMessage response = await this.client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    
                    string content = await response.Content.ReadAsStringAsync();
                    Dictionary<string,string> returnDic = JsonConvert.DeserializeObject<Dictionary<string,string>>(content);
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
                response = await this.client.GetAsync($"{BASE_URI}/accounts/ChangePass?id={id}&pass={hashedPass}");
                if (response.IsSuccessStatusCode)
                {
                   
                    string content = await response.Content.ReadAsStringAsync();
                    bool deserialized = JsonConvert.DeserializeObject<bool>(content);
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
                response = await this.client.GetAsync($"{BASE_URI}/accounts/UpdateAccount?Username={uName}&FirstName={fName}&LastName={lName}");
                if (response.IsSuccessStatusCode)
                {
                    
                    string content = await response.Content.ReadAsStringAsync();
                    bool deserialized = JsonConvert.DeserializeObject<bool>(content);
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
