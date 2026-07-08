using System.Security.Cryptography;
using System.Text;

namespace Clinc.Services
{
    public class LicenseService
    {
        private const string SecretKey = "Clinic2026_SuperSecret_Key";

        public string GetMachineId()
        {
            string machineData = Environment.MachineName +
                                 Environment.UserDomainName +
                                 Environment.OSVersion.VersionString;

            using SHA256 sha = SHA256.Create();

            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(machineData));

            return Convert.ToHexString(hash);
        }

        public string GenerateLicenseKey(string machineId)
        {
            using HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey));

            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(machineId));

            return Convert.ToHexString(hash);
        }

        public bool ValidateLicense(string machineId, string licenseKey)
        {
            return GenerateLicenseKey(machineId)
                .Equals(licenseKey, StringComparison.OrdinalIgnoreCase);
        }
    }
}