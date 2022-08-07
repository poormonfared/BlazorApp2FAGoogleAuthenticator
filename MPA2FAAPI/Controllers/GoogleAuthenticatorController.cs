using Google.Authenticator;
using Microsoft.AspNetCore.Mvc;
using MPA2FAAPI.Models;
using System.Text;

namespace MPA2FAAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GoogleAuthenticatorController : ControllerBase
    {
        private readonly TwoFactorAuthenticator _twoFactorAuthenticatorProvider;
        private string _token;

        String UserName
        {
            get
            {
                return "Poormonfared.Azimi@gmail.com";
            }
        }

        public GoogleAuthenticatorController()
        {
            _twoFactorAuthenticatorProvider = new TwoFactorAuthenticator();
        }

        [HttpGet(Name = "Setup2Factor")]
        public async Task<Setup2FactorAuthemticationInfo> Setup2Factor()
        {
            //generate random data
            Guid guid = Guid.NewGuid();
            _token = (guid.ToString()).Replace("-", "").Substring(0, 10);


            var result = _twoFactorAuthenticatorProvider.GenerateSetupCode("MPA Test", UserName, accountSecretKey: Encoding.ASCII.GetBytes(_token), generateQrCode: true);

            if (result != null)
            {
                return new Setup2FactorAuthemticationInfo
                {
                    Token4Test = _token,
                    BarCodeImageURI = result.QrCodeSetupImageUrl,
                    ManualCode = result.ManualEntryKey
                };
            }

            return null;
        }

        [HttpPost(Name = "Validate2Factor")]
        public async Task<bool> Validate2Factor([FromBody] Validate2FAPin dto)
        {
            _token = dto.Token4Test; //in real world this line shold be replace with userId
            var result = _twoFactorAuthenticatorProvider.ValidateTwoFactorPIN(dto.Token4Test, dto.Pin);

            return result;
        }

    }
}
