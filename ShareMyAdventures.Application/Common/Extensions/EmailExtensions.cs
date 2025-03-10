using Microsoft.AspNetCore.WebUtilities;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using System.Text;

namespace ShareMyAdventures.Application.Common.Extensions;

internal static class EmailExtensions
{

    internal static string GetConfirmAccountEmail(Participant applicationUser, string token, string url)
    {

        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var param = new Dictionary<string, string>
        {
            {"token", token },
            {"email", applicationUser.Email! }
        };

        var query = QueryHelpers.AddQueryString("", param!);
        var callback = $"{url}/confirm-email{query}";

        var htmlContent = string.Format(
			@"
            Welcome {0}!<br /><br />
            Thank you for registering with Share my Adventures.<br /><br />
            Please confirm your account by clicking this link: <br>
            <a href='{1}'>Confirm account</a><br /><br />
            Regards,<br />
            Share my Adventures", 
            applicationUser.Email, callback);
        return htmlContent;
    }



    internal static string GetResetPasswordEmail(string email, string token, string url)
    {
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var param = new Dictionary<string, string>
        {
            {"token", token },
            {"email", email! }
        };

        var query = QueryHelpers.AddQueryString("", param!);
        var callback = $"{url}/reset-password{query}";

        var htmlContent = string.Format(@"
Hi {0}.<br /><br />
You can reset your password by clicking on this link or by copy and pasting the link below into your browser. <br />
        <a href='{1}'>Reset Password</a>.<br/><br/>
Regards,<br />
Share my Adventures", email, callback);
        return htmlContent;
    }
}
