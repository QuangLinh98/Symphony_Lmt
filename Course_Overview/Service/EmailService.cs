using System.Net;
using System.Net.Mail;
using Course_Overview.Mail;
using Microsoft.Extensions.Options;

namespace Course_Overview.Service
{
	public class EmailService
	{
		private readonly EmailSetting _emailSetting;
		public EmailService(IOptions<EmailSetting> emailSetting)
		{
			_emailSetting = emailSetting.Value;
		}

		public async Task SendMail(string toMail,  string subject, string HtmlContent)
		{
			var fromAddress = new MailAddress(_emailSetting.FromEmail, "Course-Overview");
			var toAddress = new MailAddress(toMail);

			var smtp = new SmtpClient
			{
				Host = _emailSetting.Host,
				Port = _emailSetting.Port,
				EnableSsl = _emailSetting.EnableSsl,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				Credentials = new NetworkCredential(_emailSetting.FromEmail, _emailSetting.FromPassword)
			};

			using var message = new MailMessage(fromAddress, toAddress)
			{
				Subject = subject,
				Body = HtmlContent,
				IsBodyHtml = true   // Đánh dấu nội dung email là HTML
			};
			await smtp.SendMailAsync(message);
		}

	}
}
