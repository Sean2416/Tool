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
using System.Threading.Tasks;
using Tool.Configs;
using Tool.Helpers;
using Tool.Models;

namespace Tool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrameworkController : ControllerBase
    {
        private readonly Config Config;
        private readonly FileHelper Helper;

        public FrameworkController(IOptions<Config> config, FileHelper helper)
        {
            Config = config.Value;
            Helper = helper;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            return "Framework Get";
        }

        [HttpPost]
        public FileResult Post(InitProjectInput input)
        {
            try
            {
                new List<string>
                {
                    $"cd /d {Config.TmpPath}",
                    $"dotnet new ABP_JWT -n {input.ProjectName}"
                }.ExecuteCommand();

                var fileBytes = Helper.GetZipFile(input.ProjectName);


                new List<string>
                {
                    $"cd /d {Config.TmpPath}",
                    $"rmdir /Q /S  {input.ProjectName}",
                    $"del {$"{Config.TmpPath}\\{input.ProjectName}.zip"}"

                }.ExecuteCommand();

                return File(fileBytes, "application/zip", $"{input.ProjectName}.zip");
            }
            catch (Exception ex)
            {
                new List<string>
                {
                    $"cd /d {Config.TmpPath}",
                    $"rmdir /Q /S  {input.ProjectName}",
                    $"del {$"{Config.TmpPath}\\{input.ProjectName}.zip"}"
                }.ExecuteCommand();

                return null;
            }
        }
    }
}
