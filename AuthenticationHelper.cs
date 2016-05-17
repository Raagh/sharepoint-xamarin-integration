using System.Linq;
using Android.App;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace SPXamarin
{
    public class AuthenticationHelper
    {
        public const string Authority = "https://login.windows.net/common";
        public static System.Uri returnUri = new System.Uri("http://xam-demo-redirect");
        public static string clientId = "6c85e22e-a8e3-4097-aee7-d001a3167816";
        public static AuthenticationContext authContext = null;


        public static async Task<AuthenticationResult> GetAccessToken(string serviceResourceId, Activity activity)
        {
            authContext = new AuthenticationContext(Authority);
            if (authContext.TokenCache.ReadItems().Count() > 0)
                authContext = new AuthenticationContext(authContext.TokenCache.ReadItems().First().Authority);
            var authResult = await authContext.AcquireTokenAsync(serviceResourceId, clientId, returnUri, new AuthorizationParameters(activity));
            return authResult;
        }
    }
}