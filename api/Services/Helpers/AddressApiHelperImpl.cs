using api.Contracts.Helpers;
using api.Models.BL;
using Azure.Core;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace api.Services.Helpers
{
    public class AddressApiHelperImpl : IAddressApiHelper
    {
        private readonly HttpClient _httpClient;
        private readonly string addressApiHost;
        public AddressApiHelperImpl(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("Accept", "text/xml");
            _httpClient.DefaultRequestHeaders.Add("ContentType", "text/xml;charset=\"utf-8\"");
            addressApiHost = configuration.GetValue<string>("AddressApiHost")
                ?? throw new ArgumentNullException(nameof(addressApiHost));
            initDistrictsFromFile();
        }
        private void initDistrictsFromFile()
        {
            if (districtUsers.Count > 0) return;
            var info = Assembly.GetExecutingAssembly().GetName();
            var name = info.Name;
            using var stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream($"{name}.Content.Districts.xml")!;
            foreach (var el in XDocument.Load(stream).Root.Elements())
            {
                districtUsers.Add(new()
                {
                    DistrictId = int.Parse(el.Element("id").Value),
                    Userid = Guid.Parse(el.Element("userid").Value),
                    OrgId = Guid.Parse(el.Element("orgid").Value)
                });
            }
        }
        private static HashSet<districtUserDTO> districtUsers = new HashSet<districtUserDTO>();
        public async Task<int> GetParent(int streetId, DateTime asbDate)
        {
            string soapString =
                @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://address.infocom.kg/ws/"">
                        <soapenv:Header>
                        <token>4f20b8e66f24052ba3abfc56f43248d5</token>
                    </soapenv:Header>
                    <soapenv:Body>
                        <ws:getParent>
                            <id>" + streetId.ToString() + @"</id>
                            <currentDate>" + asbDate.ToString("yyyy-MM-dd") + @"</currentDate>
                        </ws:getParent>
                    </soapenv:Body>
                </soapenv:Envelope>";
            var result = await PostSOAPRequestAsync(addressApiHost, soapString);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);
            var xdoc = XDocumentExtensions.ToXDocument(xml);
            var retVal = xdoc.Descendants("return").FirstOrDefault();
            if (retVal != null)
                return int.Parse(retVal.Element("id").Value);
            return 0;
        }

        public async Task<int> GetAteByStreetId(int streetId, DateTime asbDate)
        {
            string soapString =
                @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://address.infocom.kg/ws/"">
                        <soapenv:Header>
                        <token>4f20b8e66f24052ba3abfc56f43248d5</token>
                    </soapenv:Header>
                    <soapenv:Body>
                        <ws:getAteByStreet>
                            <street>" + streetId.ToString() + @"</street>
                            <currentDate>" + asbDate + @"</currentDate>
                        </ws:getAteByStreet>
                    </soapenv:Body>
                </soapenv:Envelope>";
            var soapResult = await PostSOAPRequestAsync(addressApiHost, soapString);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(soapResult);
            var xdoc = XDocumentExtensions.ToXDocument(xml);
            var retVal = xdoc.Descendants("item").FirstOrDefault();
            if (retVal != null)
                return int.Parse(retVal.Element("id").Value);
            return 0;
        }

        public async Task<string> GetStreetName(int ateId, DateTime asbDate)
        {
            string soapString =
                @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://address.infocom.kg/ws/"">
                        <soapenv:Header>
                        <token>4f20b8e66f24052ba3abfc56f43248d5</token>
                    </soapenv:Header>
                    <soapenv:Body>
                        <ws:getStreetName>
                            <id>" + ateId.ToString() + @"</id>
                            <currentDate>" + asbDate + @"</currentDate>
                        </ws:getStreetName>
                    </soapenv:Body>
                </soapenv:Envelope>";
            var soapResult = await PostSOAPRequestAsync(addressApiHost, soapString);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(soapResult);
            var xdoc = XDocumentExtensions.ToXDocument(xml);
            var retVal = xdoc.Descendants("return").FirstOrDefault();
            if (retVal != null)
                return retVal.Element("item").Element("nameRu").Value;
            return "улица не найдена id=" + ateId;
        }

        public (Guid orgId, Guid userId) GetUserAndOrgByDistrict(int districtId)
        {
            var districtObj = districtUsers.FirstOrDefault(x => x.DistrictId == districtId);
            if (districtObj != null) return (districtObj.OrgId, districtObj.Userid);
            else throw new ArgumentException(
                "Не могу определить РУСР." +
                " Район не содержится в существующем справочнике районов!" +
                " Значение полученного района: " + districtId, nameof(districtId));
        }
        public Guid GetDistrictUserId(int districtId)
        {
            var districtObj = districtUsers.FirstOrDefault(x => x.DistrictId == districtId);
            if (districtObj != null) return districtObj.Userid;
            else throw new ArgumentException(
                "Не могу определить РУСР." +
                " Район не содержится в существующем справочнике районов!" +
                " Значение полученного района: " + districtId, nameof(districtId));
        }
        public Guid GetUserIdByOrgId(Guid orgId)
        {
            var orgObj = districtUsers.FirstOrDefault(x => x.OrgId == orgId);
            if (orgObj != null) return orgObj.Userid;
            else throw new ArgumentException(
                "Не могу определить пользователя РУСР." +
                " OrgId не содержится в существующем справочнике районов!" +
                " Значение выбранного OrgId: " + orgId, nameof(orgId));
        }
        public bool HasDistrictAteId(int ateId)
        {
            return districtUsers.Any(x => x.DistrictId == ateId);
        }
        private async Task<string> PostSOAPRequestAsync(string url, string text)
        {
            using HttpContent content = new StringContent(text, Encoding.UTF8, "text/xml");
            using HttpRequestMessage request = new(HttpMethod.Post, url);
            request.Headers.Add("SOAPAction", "");
            request.Content = content;
            using HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            //response.EnsureSuccessStatusCode(); // throws an Exception if 404, 500, etc.
            return await response.Content.ReadAsStringAsync();
        }
    }
}
