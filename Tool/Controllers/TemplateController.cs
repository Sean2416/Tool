using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Tool.Configs;
using Tool.Helpers;
using Tool.Models;

namespace Tool.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly Config Config;
        private readonly FileHelper Helper;

        public TemplateController(IOptions<Config> config, FileHelper helper)
        {
            Config = config.Value;
            Helper = helper;
        }

        /// <summary>
        ///  產生以JWT Token為驗證方式的框架
        /// </summary>
        [HttpPost]
        public FileResult CreateJWTTemplate(InitProjectInput input)
        {
            try
            {

                ".\\Bat\\CreateTemplate.bat".ExecuteCommand(
                    new List<string> {
                        Config.TemplatePath,
                        Config.TmpPath,
                        input.ProjectName
                    });            

                var fileBytes = Helper.GetZipFile(input.ProjectName);

                ".\\Bat\\DeleteTemplate.bat".ExecuteCommand(
                    new List<string> {
                        Config.TmpPath,
                        input.ProjectName
                    });

                return File(fileBytes, "application/zip", $"{input.ProjectName}.zip");
            }
            catch
            {
                ".\\Bat\\DeleteTemplate.bat".ExecuteCommand(
                   new List<string> {
                        Config.TmpPath,
                        input.ProjectName
                   });

                return null;
            }
        }
    }
}
