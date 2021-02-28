using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Tool.Configs;

namespace Tool.Helpers
{
    public class FileHelper
    {
        private readonly Config Config;

        public FileHelper(IOptions<Config> config)
        {
            Config = config.Value;
        }

        public  byte[] GetZipFile(string projectName)
        {
            CompressFolder(projectName);

           return System.IO.File.ReadAllBytes($"{Config.TmpPath}\\{projectName}.zip");
        }

        public  void CompressFolder(string projectName)
        {
            ZipFile.CreateFromDirectory($"{Config.TmpPath}\\{projectName}", $"{Config.TmpPath}\\{projectName}.zip");
        }
    }
}
