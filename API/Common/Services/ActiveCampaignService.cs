using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace API.Common.Services
{
    public class ActiveCampaignService
    {
        private readonly string ApiUrl = "";
        private readonly string ApiKey = "";
        private readonly int ListTrialId = -1;
        public ActiveCampaignService()
        {
            ApiUrl = ConfigurationManager.AppSettings["ActiveCampaignApiUrl"];
            ApiKey = ConfigurationManager.AppSettings["ActiveCampaignApiKey"];
            int.TryParse(ConfigurationManager.AppSettings["ActiveCampaignListTrialId"], out ListTrialId);
        }

        private bool IsValid()
        {
            return ListTrialId != -1 && !string.IsNullOrEmpty(ApiUrl) && !string.IsNullOrEmpty(ApiKey);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns>contactId</returns>
        public async Task<int> CreateOrUpdateContact(string email, string firstName, string lastName)
        {
            if (!IsValid())
            {
                return -1;
            }
            var body = new ActiveCampaignContactBody
            {
                contact = new ActiveCampaignContact
                {
                    email = email,
                    firstName = firstName,
                    lastName = lastName
                }
            };
            var bodyJsonString = JsonConvert.SerializeObject(body);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Api-Token", ApiKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl + "/api/3/contact/sync");
                request.Content = new StringContent(bodyJsonString);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var res = await client.SendAsync(request);
                if (res.IsSuccessStatusCode)
                {
                    var resBody = await res.Content.ReadAsStringAsync();
                    var resBodyJson = JsonConvert.DeserializeObject<ActiveCampaignContactBody>(resBody);
                    if (resBodyJson != null && resBodyJson.contact != null)
                    {
                        return resBodyJson.contact.id;
                    }
                }
            }
            return -1;
        }

        public async Task UpdateContactListTrial(int contactId, ContactListStatus status)
        {
            if (!IsValid())
            {
                return;
            }

            if (contactId != -1)
            {
                var body = new ActiveCampaignContactListBody
                {
                    contactList = new ActiveCampaignContactList
                    {
                        contact = contactId,
                        list = ListTrialId,
                        status = (int)status
                    }
                };
                var bodyJsonString = JsonConvert.SerializeObject(body);
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Api-Token", ApiKey);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiUrl + "/api/3/contactLists");
                    request.Content = new StringContent(bodyJsonString);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var res = await client.SendAsync(request);
                }
            }
        }
    }
    public class ActiveCampaignContact
    {
        public int id { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class ActiveCampaignContactBody
    {
        public ActiveCampaignContact contact { get; set; }
    }
    public enum ContactListStatus
    {
        Active = 1,
        Unsubscribed = 2
    }
    public class ActiveCampaignContactList
    {
        public int list { get; set; }
        public int contact { get; set; }
        public int status { get; set; }
    }

    public class ActiveCampaignContactListBody
    {
        public ActiveCampaignContactList contactList { get; set; }
    }
}
