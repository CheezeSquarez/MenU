using MenU.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using MenU.Models;

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
        public const string BASE_URI = "http://10.0.2.2:39135";
        public const string DEFAULT_IMG_URI = "http://10.0.2.2:39135/imgs/";
        private static MenUWebAPI proxy = null;
        public static MenUWebAPI CreateProxy()
        {
            if (proxy == null)
                proxy = new MenUWebAPI();
            return proxy;
        }
        private MenUWebAPI()
        {
            //Set client handler to support cookies!    !
            HttpClientHandler handler = new HttpClientHandler();     
            handler.CookieContainer = new System.Net.CookieContainer(); 
            //handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            //Create client with the handler!
            this.client = new HttpClient(handler, true) /*{ Timeout = new TimeSpan(150000) } */;

        }

        public async Task<(Account account, int StatusCode)> LoginAsync(string token)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{BASE_URI}/accounts/TokenLogin?token={token}");

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNameCaseInsensitive = true
                };

                if (response.IsSuccessStatusCode)
                {
                    
                    string content = await response.Content.ReadAsStringAsync();
                    Account acc = System.Text.Json.JsonSerializer.Deserialize<Account>(content, options);
                    return (acc, (int)response.StatusCode);
                }
                else
                {
                    return (null, StatusCode: (int)response.StatusCode);
                }
            }
            catch (Exception e)
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
            catch (Exception e)
            {
                return (false, 503);
            }
        }
        public async Task<(bool isSuccess, int StatusCode)> LogOutAsync(string authToken)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{BASE_URI}/accounts/LogOut?token={authToken}");
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
                    
                    string token = await response.Content.ReadAsStringAsync();
                    return (token, (int)response.StatusCode);
                }
                else
                {
                    return ("", (int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                    
                    string salt = await response.Content.ReadAsStringAsync();
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
        public async Task<(bool,int)> ChangePass(Credentials creds)
        {
            HttpResponseMessage response;
            try
            {
                string json = JsonConvert.SerializeObject(creds);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await this.client.PostAsync($"{BASE_URI}/accounts/ChangePass", content);
                if (response.IsSuccessStatusCode)
                {
                   
                    string c = await response.Content.ReadAsStringAsync();
                    bool deserialized = JsonConvert.DeserializeObject<bool>(c);
                    return (deserialized, (int)response.StatusCode);
                }
                else
                {
                    return (false, (int)response.StatusCode);
                }
            }
            catch (Exception e)
            {
                return (false, 503);
            }
        }

        public async Task<(Account, int)> UpdateAccountInfo(Account updatedAcc)
        {
            HttpResponseMessage response;
            
            
            try
            {
                string json = JsonConvert.SerializeObject(updatedAcc, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await this.client.PostAsync($"{BASE_URI}/accounts/UpdateAccountInfo", content);
                if (response.IsSuccessStatusCode)
                {
                    string c = await response.Content.ReadAsStringAsync();
                    Account b = JsonConvert.DeserializeObject<Account>(c);
                    return (b, (int)response.StatusCode);
                }
                else
                {
                    return (null, (int)response.StatusCode);
                }
            }
            catch (Exception e)
            {
                return (null, 503);
            }
        }
        public async Task<(List<string>, int StatusCode)> GetDefaultPfps()
        {

            string url = $"{BASE_URI}/accounts/GetDefaultPfps";
            try
            {
                HttpResponseMessage response = await this.client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {

                    string content = await response.Content.ReadAsStringAsync();
                    List<string> sources = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(content);
                    sources = sources.Select(x => BASE_URI + "/" + x).ToList();
                    sources.Add("custom");
                    return (sources, (int)response.StatusCode);
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

        public async Task<(List<Tag>, int StatusCode)> GetAllTags()
        {
            string url = $"{BASE_URI}/restaurants/GetAllTags";
            try
            {
                HttpResponseMessage response = await this.client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {

                    string content = await response.Content.ReadAsStringAsync();
                    List<Tag> tags = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Tag>>(content);
                    return (tags, (int)response.StatusCode);
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
        public async Task<(List<Allergen>, int StatusCode)> GetAllAllergens()
        {
            string url = $"{BASE_URI}/restaurants/GetAllAllergens";
            try
            {
                HttpResponseMessage response = await this.client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {

                    string content = await response.Content.ReadAsStringAsync();
                    List<Allergen> allergens = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Allergen>>(content);
                    return (allergens, (int)response.StatusCode);
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
        public async Task<(int Id, int StatusCode)> StampDish()
        {
            string url = $"{BASE_URI}/restaurants/StampDish";
            try
            {
                HttpResponseMessage response = await this.client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {

                    string content = await response.Content.ReadAsStringAsync();
                    int dishId = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(content);
                    return (dishId, (int)response.StatusCode);
                }
                else
                {
                    return (-1, StatusCode: (int)response.StatusCode);
                }
            }
            catch (Exception)
            {
                return (-1, 503);
            }
        }

        public async Task<(int Id, int StatusCode)> StampRestaurant()
        {
            string url = $"{BASE_URI}/restaurants/StampRestaurant";
            try
            {
                HttpResponseMessage response = await this.client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {

                    string content = await response.Content.ReadAsStringAsync();
                    int restaurantId = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(content);
                    return (restaurantId, (int)response.StatusCode);
                }
                else
                {
                    return (-1, StatusCode: (int)response.StatusCode);
                }
            }
            catch (Exception)
            {
                return (-1, 503);
            }
        }

        public async Task<(RestaurantResult, int)> AddRestaurant(RestaurantDTO r)
        {
            HttpResponseMessage response;
            string url = $"{BASE_URI}/restaurants/AddRestaurant";
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNameCaseInsensitive = true
                };
                string json = System.Text.Json.JsonSerializer.Serialize<RestaurantDTO>(r, options);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                response = await this.client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string c = await response.Content.ReadAsStringAsync();
                    RestaurantResult b = JsonConvert.DeserializeObject<RestaurantResult>(c);
                    return (b, (int)response.StatusCode);
                }
                else
                {
                    return (null, (int)response.StatusCode);
                }
            }
            catch (Exception)
            {
                return (null, 503);
            }
        }
        public async Task<(bool, int)> AddDish(Dish d)
        {
            HttpResponseMessage response;

            try
            {
                string json = JsonConvert.SerializeObject(d);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await this.client.PostAsync($"{BASE_URI}/restaurants/AddDish", content);
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
        public async Task<(Restaurant, int)> UpdateRestaurant(RestaurantDTO rDTO)
        {
            HttpResponseMessage response;

            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize<RestaurantDTO>(rDTO);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await this.client.PostAsync($"{BASE_URI}/restaurants/UpdateRestaurant", content);
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNameCaseInsensitive = true
                };

                if (response.IsSuccessStatusCode)
                {
                    string c = await response.Content.ReadAsStringAsync();
                    Restaurant b = System.Text.Json.JsonSerializer.Deserialize<Restaurant>(c, options);
                    return (b, (int)response.StatusCode);
                }
                else
                {
                    return (null, (int)response.StatusCode);
                }
            }
            catch (Exception)
            {
                return (null, 503);
            }
        }

        public async Task<(List<Restaurant>, int)> GetRestaurantByString(string searchTerm)
        {
            string url = $"{BASE_URI}/restaurants/FindRestaurantsByLetters?letters={searchTerm}";
            try
            {
                HttpResponseMessage response = await this.client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {

                    string content = await response.Content.ReadAsStringAsync();
                    List<Restaurant> restaurants = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Restaurant>>(content);
                    return (restaurants, (int)response.StatusCode);
                }
                else
                {
                    return (null, (int)response.StatusCode);
                }
            }
            catch (Exception e)
            {
                return (null, 503);
            }
        }

        public async Task<(Restaurant, int)> FindRestaurantById(int id)
        {
            string url = $"{BASE_URI}/restaurants/FindRestaurantById?resId={id}";
            try
            {
                HttpResponseMessage response = await this.client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Restaurant restaurant = Newtonsoft.Json.JsonConvert.DeserializeObject<Restaurant>(content);
                    return (restaurant, (int)response.StatusCode);
                }
                else
                {
                    return (null, (int)response.StatusCode);
                }
            }
            catch (Exception e)
            {
                return (null, 503);
            }
        }

        public async Task<(int,int)> PostReview(Review r)
        {
            string url = $"{BASE_URI}/restaurants/PostReview";
            HttpResponseMessage response;

            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize<Review>(r);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await this.client.PostAsync(url, content);
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNameCaseInsensitive = true
                };

                if (response.IsSuccessStatusCode)
                {
                    string c = await response.Content.ReadAsStringAsync();
                    int b = System.Text.Json.JsonSerializer.Deserialize<int>(c, options);
                    return (b, (int)response.StatusCode);
                }
                else
                {
                    return (-1, (int)response.StatusCode);
                }
            }
            catch (Exception)
            {
                return (-1, 503);
            }
        }

        public async Task<(Dish, int)> GetDishById(int id)
        {
            string url = $"{BASE_URI}/restaurants/GetDishById?dishId={id}";
            try
            {
                HttpResponseMessage response = await this.client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Dish dish = Newtonsoft.Json.JsonConvert.DeserializeObject<Dish>(content);
                    return (dish, (int)response.StatusCode);
                }
                else
                {
                    return (null, (int)response.StatusCode);
                }
            }
            catch (Exception e)
            {
                return (null, 503);
            }
        }

        public async Task<(List<Review>, int)> GetDishReviews(int id)
        {
            string url = $"{BASE_URI}/restaurants/GetDishReviews?id={id}";
            try
            {
                HttpResponseMessage response = await this.client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<Review> reviews = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Review>>(content);
                    return (reviews, (int)response.StatusCode);
                }
                else
                {
                    return (null, (int)response.StatusCode);
                }
            }
            catch (Exception e)
            {
                return (null, 503);
            }
        }


        public async Task<(bool,int)> UploadImage(Models.FileInfo fileInfo, string targetFileName)
        {
            try
            {
                var multipartFormDataContent = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(fileInfo.Name));
                multipartFormDataContent.Add(fileContent, "file", targetFileName);
                HttpResponseMessage response = await client.PostAsync($"{BASE_URI}/accounts/UploadImage", multipartFormDataContent);
                if (response.IsSuccessStatusCode)
                {
                    return (true, (int)response.StatusCode);
                }
                else
                    return (false, (int)response.StatusCode);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return (false, 503);
            }
        }

        public async Task<string> Test()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{BASE_URI}/accounts/Test");
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
