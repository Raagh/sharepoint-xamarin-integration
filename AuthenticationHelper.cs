using System.Linq;
using Android.App;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace SPXamarin
{
    /// <summary>
    ///  This class authenticates with AZURE AD.
    /// </summary>
    public class AuthenticationHelper
    {
        public const string Authority = "https://login.windows.net/common";
        public static System.Uri returnUri = new System.Uri("http://xam-demo-redirect");
        //Client id from Azure AD app registration process
        public static string clientId = "6c85e22e-a8e3-4097-aee7-d001a3167816"; 
        public static AuthenticationContext authContext = null;

        /// <summary>
        /// GetAccessToken recieves the android activity and the SharePoint Url, and returns the authentication result.
        /// </summary>
        /// <param name="serviceResourceId">SharePoint Server URL</param>
        /// <param name="activity">Android activity triggering the authentication process</param>
        /// <returns></returns>
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