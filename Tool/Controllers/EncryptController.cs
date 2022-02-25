using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;
using Tool.Helpers;
using Tool.Models;

namespace Tool.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EncryptController : ControllerBase
    {
        /// <summary>
        ///  Aes字串加密
        /// </summary>
        [HttpPost]
        public string AesEncrypt(EncryptInput input)
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Once Job"));

            return new EncryptHelper(input.Key,input.IV).Encrypt(input.EncryptString);
        }

        /// <summary>
        ///  Aes字串解密
        /// </summary>
        [HttpPost]
        public string AesDecrypt(DecryptInput input)
        {
            return new EncryptHelper(input.Key, input.IV).Decrypt(input.DecryptString);
        }

        /// <summary>
        ///  Sha單向加密
        /// </summary>
        [HttpPost]
        public string ShaEncrypt(string encryptString)
        {
            return new EncryptHelper().ShaEncrypt(encryptString);
        }
    }
}
