using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tradevan_Mail;

namespace Tool.Mail
{
    public class MailManager
    {
        private readonly IMailService MailService;
        public MailManager(IMailService mailService)
        {
            MailService = mailService;
        }


        public async Task SendRegisterMail(string sub)
        {
            var email = new MailRequest
            {
                ToMails = "Sean24163@hotmail.com,Sean24163@gmail.com",
                Subject = sub,
                Body = GetRegisterMailTemplate(),
                Attachments = new Tradevan_Mail.FileInfo
                {
                    FileName = "不准開.txt",
                    FileBytes = FileToByteArray("Mail\\Nuget說明.txt")
                }
            };
            await MailService.SendEmailAsync(email);
        }

        public static byte[] FileToByteArray(string fileName)
        {
            byte[] fileData = null;

            using (FileStream fs = File.OpenRead(fileName))
            {
                var binaryReader = new BinaryReader(fs);
                fileData = binaryReader.ReadBytes((int)fs.Length);
            }
            return fileData;
        }

        private string GetRegisterMailTemplate()
        {
            return MailService.GetBodyFromTemplate(Directory.GetCurrentDirectory() + "\\Mail\\Register.html",
                new List<MailParam> { 
                    new MailParam( "name","Sean Chu"),
                    new MailParam( "link","https://aspnetboilerplate.com/Pages/Documents/Email-Sending")
                });
        }
    }
}
