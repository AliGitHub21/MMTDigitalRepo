using System;
using MMTDigital.Model;

namespace MMTDigital.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly MMTDigitalClient _MMTDigitalClient;

        public UtilityService(MMTDigitalClient MMTDigitalClient)
        {
            _MMTDigitalClient = MMTDigitalClient;
        }

        public string GetApiUri(string userEmail)
        {
            var urlParameters = "&email=" + userEmail;
            return _MMTDigitalClient.Client.BaseAddress + urlParameters;
        }
    }
}
