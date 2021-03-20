using Microsoft.AspNetCore.Mvc;
using Tool.Helpers;
using Tool.Models;

namespace Tool.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EncryptController : ControllerBase
    {

        public EncryptController()
        {
        }

        /// <summary>
        ///  字串加密
        /// </summary>
        [HttpPost]
        public string Encrypt(EncryptInput input)
        {
            return new EncryptHelper(input.Key,input.IV).Encrypt(input.EncryptString);
        }

        /// <summary>
        ///  字串解密
        /// </summary>
        [HttpPost]
        public string Decrypt(DecryptInput input)
        {
            return new EncryptHelper(input.Key, input.IV).Decrypt(input.DecryptString);
        }
    }
}
