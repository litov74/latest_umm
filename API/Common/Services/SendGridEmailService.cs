using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace API.Common.Services
{
    public class SendGridEmailService 
    {
        private readonly SendGridClient _client;
        private readonly EmailAddress _fromAddress;
        private readonly AppSettings appSettings;

        public SendGridEmailService(IConfiguration configuration)
        {
            appSettings = configuration.Get<AppSettings>();
            _client = new SendGridClient(new SendGridClientOptions
            {
                ApiKey = appSettings.EmailSettings.SendGirdApiKey
            });
            _fromAddress = new EmailAddress(appSettings.EmailSettings.FromEmailAddress, "The Change Compass");
        }

        public async Task SendMailAsync(string email, string subject, string htmlMessages)
        {
            var toEmail = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(_fromAddress, toEmail, subject, "", htmlMessages);
            await _client.SendEmailAsync(msg);
        }

        public async Task SendMailTemplateAsync(string templateId,  string email, object payloads)
        {
            var toEmail = new EmailAddress(email);
            var msg =  MailHelper.CreateSingleTemplateEmail(_fromAddress, toEmail, templateId, payloads);
            await _client.SendEmailAsync(msg);
        }
        private async Task<string> GetSendGirdContactIdByEmail(string email)
        {
            var body = new
            {
                query = $"email='{email}'"
            };

            var bodyJsonString = JsonConvert.SerializeObject(body);
            //gridClient.AddAuthorization(new KeyValuePair<string, string>("Authorization", "Bearer " + SendGridKey));
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.EmailSettings.SendGirdApiKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://api.sendgrid.com/v3/marketing/contacts/search");
                request.Content = new StringContent(bodyJsonString);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var res = await client.SendAsync(request);

                if (res.StatusCode == HttpStatusCode.OK)
                {
                    var resBody = await res.Content.ReadAsStringAsync();
                    var resBodyJson = JsonConvert.DeserializeObject<dynamic>(resBody);
                    if (resBodyJson.contact_count != null && resBodyJson.contact_count > 0)
                    {
                        var contactResult = resBodyJson.result[0];
                        return contactResult.id;
                    }
                }

            }
            return "";
        }
        public async Task AddOrUpdateEmailToSendGirdTrialList(string email, string firstName, string lastName)
        {
            if (appSettings.EmailSettings.useSendGrid)
            {
                var list_ids = new List<string> { "3a089318-780a-4c18-9faa-ce1c272eee8f" }; // list trial users.

                var body = new
                {
                    list_ids = list_ids,
                    contacts = new List<object>
                    {
                        new
                        {
                            email,
                            first_name = firstName,
                            last_name = lastName
                        }
                    }
                };

                var bodyJsonString = JsonConvert.SerializeObject(body);
                //gridClient.AddAuthorization(new KeyValuePair<string, string>("Authorization", "Bearer " + SendGridKey));
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.EmailSettings.SendGirdApiKey);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, "https://api.sendgrid.com/v3/marketing/contacts");
                    request.Content = new StringContent(bodyJsonString);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var res = await client.SendAsync(request);
                }
            }
        }
        public async Task RemoveContactFromListTrial(string email)
        {
            if (appSettings.EmailSettings.useSendGrid)
            {
                var id = await GetSendGirdContactIdByEmail(email);
                if (id.Length > 0)
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.EmailSettings.SendGirdApiKey);
                        var url = $"https://api.sendgrid.com/v3/marketing/lists/3a089318-780a-4c18-9faa-ce1c272eee8f/contacts?contact_ids={id}";
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
                        var res = await client.SendAsync(request);

                    }
                }
            }
        }
    }
}
