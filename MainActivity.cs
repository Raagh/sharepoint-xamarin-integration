using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Widget;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Linq;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Json;

namespace SPXamarin
{
    public static class App {
        public static Java.IO.File _file;
        public static Java.IO.File _dir;     
        public static Bitmap bitmap;
        public static AuthenticationResult authResult; 
    }

    [Activity(Label = "SPX Integration", MainLauncher = true)]
    public class MainActivity : Activity
    {
       
        private ImageView _imageView;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (App.authResult == null)
            {
                AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(requestCode, resultCode, data);
                Toast.MakeText(this, "Authentication Succesful", ToastLength.Long).Show();
            }

            
            // Make it available in the gallery
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            if (App._file != null)
            {
                Uri contentUri = Uri.FromFile(App._file);
                mediaScanIntent.SetData(contentUri);
                SendBroadcast(mediaScanIntent);

                // Display in ImageView. We will resize the bitmap to fit the display
                // Loading the full sized image will consume to much memory 
                // and cause the application to crash.

                int height = Resources.DisplayMetrics.HeightPixels;
                int width = _imageView.Height;
                App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
                if (App.bitmap != null)
                {
                    _imageView.SetImageBitmap(App.bitmap);
                    App.bitmap = null;
                }

                // Dispose of the Java side bitmap.
                GC.Collect();               
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();

                Button login = FindViewById<Button>(Resource.Id.LoginButton);
                login.Click += Login;

                Button camerabutton = FindViewById<Button>(Resource.Id.CameraButton);
                _imageView = FindViewById<ImageView>(Resource.Id.CameraView);
                camerabutton.Click += TakeAPicture;
            }

            Button itemsButton = FindViewById<Button>(Resource.Id.ItemsButton);
            itemsButton.Click += async (sender, args) =>
            {
                //await CreateItem(App.authResult.AccessToken);
                await CreateItemWithPicture("Item-" + DateTime.Now.ToString(), "https://cokeandcode.sharepoint.com/", App.authResult.AccessToken, App._file.AbsolutePath);
            };


            Button listButton = FindViewById<Button>(Resource.Id.CreateListButton);
            listButton.Click += async (sender, args) =>
            {
                await CreateList(App.authResult.AccessToken);
            };


        }

        internal async void Login(object sender, EventArgs eventArgs)
        {
            if(App.authResult == null)
                App.authResult = await AuthenticationHelper.GetAccessToken("https://cokeandcode.sharepoint.com/", this);
            else
                Toast.MakeText(this, "Already logged in!", ToastLength.Long).Show();
        }

        private void CreateDirectoryForPictures()
        {
            App._dir = new Java.IO.File(
                Environment.GetExternalStoragePublicDirectory(
                    Environment.DirectoryPictures), "SPXIntegration");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = 
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);

            string photoTitle = string.Format("SPX_{0}.jpg", Guid.NewGuid());
            App._file = new Java.IO.File(App._dir, photoTitle);

            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));

            StartActivityForResult(intent, 0);
        }

        private static async Task<string> GetFormDigest(string siteURL, string accessToken)
        {
            //Get the form digest value in order to write data
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(
                  HttpMethod.Post, siteURL + "/_api/contextinfo");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage response = await client.SendAsync(request);

            string responseString = await response.Content.ReadAsStringAsync();

            XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            var root = XElement.Parse(responseString);
            var formDigestValue = root.Element(d + "FormDigestValue").Value;

            return formDigestValue;
        }

        protected async Task<bool> CreateItem(string token)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));
            client.DefaultRequestHeaders.Accept.Add(mediaType);

            var itemToCreateTitle = "Item created on: " + DateTime.Now.ToString("dd/MM HH:mm");
            var body = "{\"__metadata\":{\"type\":\"SP.Data.XamarinTasksListItem\"},\"Title\":\"" + itemToCreateTitle + "\",\"Status\": \"Not Started\"}";
            var contents = new StringContent(body);
            contents.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            try
            {
                var postResult = await client.PostAsync("https://cokeandcode.sharepoint.com/_api/web/lists/GetByTitle('XamarinTasks')/items", contents);
                var result = postResult.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                {
                    Toast.MakeText(this, "List item created successfully!", ToastLength.Long).Show();
                    return true;
                }
            }
            catch (Exception ex)
            {
                var msg = "Unable to create list item. " + ex.Message;
                Toast.MakeText(this, msg, ToastLength.Long).Show();
                return false;
            }

            return false;
        }

        protected async Task<bool> CreateList(string token)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));
            client.DefaultRequestHeaders.Accept.Add(mediaType);

            var body = "{\"__metadata\":{\"type\":\"SP.List\"},\"AllowContentTypes\":true,\"BaseTemplate\":100,\"ContentTypesEnabled\":true,\"Description\":\"Pictures from Xamarin.Android\",\"Title\":\"SPXPictures\"}";

            var contents = new StringContent(body);
            contents.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            try
            {
                var postResult = await client.PostAsync("https://cokeandcode.sharepoint.com/_api/web/lists/", contents);
                var result = postResult.EnsureSuccessStatusCode();
                Toast.MakeText(this, "List created successfully!", ToastLength.Long).Show();

                return true;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "List already exists!", ToastLength.Long).Show();
                return false;
            }
        }

        public async Task<string> CreateItemWithPicture(string title, string siteURL, string accessToken, string filePath)
        {
            string requestUrl = siteURL + "_api/Web/Lists/GetByTitle('SPXPictures')/Items";

            var formDigest = await GetFormDigest(siteURL, accessToken);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");

            HttpRequestMessage request =
                new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            // Note that the form digest is not needed for bearer authentication.  This can
            //safely be removed, but left here for posterity.            
            request.Headers.Add("X-RequestDigest", formDigest);

            var requestContent = new StringContent(
              "{ '__metadata': { 'type': 'SP.Data.SPXPicturesListItem' }, 'Title': '" + title + "'}");
            requestContent.Headers.ContentType =
               System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            request.Content = requestContent;

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                JsonObject d = (JsonObject)JsonValue.Parse(responseString);
                JsonObject results = (JsonObject)d["d"];
                JsonValue newItemId = (JsonValue)results["ID"];
                var endpointUrl = string.Format("{0}({1})/AttachmentFiles/add(FileName='{2}')", requestUrl, newItemId.ToString(), App._file.Name);

                using (var stream = System.IO.File.OpenRead(filePath))
                {
                    HttpContent file = new StreamContent(stream);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var resp = await client.PostAsync(endpointUrl, file);
                    Toast.MakeText(this, "Picture Uploaded!", ToastLength.Long).Show();
                }
                return responseString;
            }

            return (null);
        }

    }
}
