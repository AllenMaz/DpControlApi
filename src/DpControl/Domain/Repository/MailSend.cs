using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Repository
{
    public class MailSend : MimeMessage
    {

        private string _smtpServer { get; set; }
        private string _smtpUserName { get; set; }
        private string _smtpPassword { get; set; }
        private bool _enableTls { get; set; }
        private int _port { get; set; }
        private string _displayName { get; set; }

        public MailSend()
        {
            _smtpServer = Startup.Configuration["EmailSettings:SmtpServer"];
            _smtpUserName = Startup.Configuration["EmailSettings:SmtpUserName"];
            _smtpPassword = Startup.Configuration["EmailSettings:SmtpPassword"];
            _displayName = Startup.Configuration["EmailSettings:DisplayName"];
            _enableTls = Convert.ToBoolean(Startup.Configuration["EmailSettings:EnableTls"]);
            _port = Convert.ToInt32(Startup.Configuration["EmailSettings:Port"]);

            //发送放邮件账号必须与STMP账号一致
            if (this.From.Count == 0)
            {
                this.From.Add(new MailboxAddress(_displayName, _smtpUserName));

            }

        }

        public void Send()
        {
            using (var client = new SmtpClient())
            {
                client.Connect(_smtpServer, _port, false);
                //STMP身份认证，用户名是QQ用户名，密码是启用STMP服务时系统分配的密码
                client.Authenticate(_smtpUserName, _smtpPassword);
                client.Send(this);
                client.Disconnect(true);
            }
        }

        public async Task SendAsync()
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _port, false);

                //STMP身份认证，用户名是QQ用户名，密码是启用STMP服务时系统分配的密码
                await client.AuthenticateAsync(_smtpUserName, _smtpPassword);
                //var tlsProtocol = _enableTls ? SslProtocols.Tls : SslProtocols.None;
                //client.SslProtocols = tlsProtocol;
                await client.SendAsync(this);
                await client.DisconnectAsync(true);
            }
        }
    }
}
