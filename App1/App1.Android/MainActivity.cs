using System;
using Xamarin.Forms;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms;
using App1.Servicos;
using App1.Droid.Servicos;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Firebase.Auth;
using Android.Gms.Auth.Api;
using Firebase;
using Android.Widget;
using Android.Content;
using Android.Gms.Tasks;

namespace App1.Droid
{
    [Activity(Label = "App1", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IOnSuccessListener, IOnFailureListener
    {

        public static GoogleSignInOptions gso;
        public static GoogleApiClient googleApiClient;

        public static FirebaseAuth firebaseAuth;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            DependencyService.Register<IMessage, MessageAndroid>();
            DependencyService.Register<ILogin, Login>();


            gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken("177259958250-pgdselr0gaco8tuu81hkpvc1qmii73bt.apps.googleusercontent.com")
                .RequestEmail()
                .Build();

            googleApiClient = new GoogleApiClient.Builder(this)
                .AddApi(Auth.GOOGLE_SIGN_IN_API, gso).Build();
            googleApiClient.Connect();

            firebaseAuth = GetFirebaseAuth();


            LoadApplication(new App());
        }


        private FirebaseAuth GetFirebaseAuth()
        {
            var app = FirebaseApp.InitializeApp(this);
            FirebaseAuth mAuth;

            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetProjectId("bepprice-d1c34")
                    .SetApplicationId("bepprice-d1c34")
                    .SetApiKey("AIzaSyC1JxRYM7Y6wZLlIOnfNyQOZWN8AuZSrnY")
                    .SetDatabaseUrl("https://bepprice-d1c34-default-rtdb.firebaseio.com/")
                    .SetStorageBucket("bepprice-d1c34.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(this, options);
                mAuth = FirebaseAuth.Instance;
            }
            else
            {
                mAuth = FirebaseAuth.Instance;
            }
            return mAuth;
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }




        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 1)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                if (result.IsSuccess)
                {
                    GoogleSignInAccount account = result.SignInAccount;
                    LoginWithFirebase(account);
                }
            }
        }

        private void LoginWithFirebase(GoogleSignInAccount account)
        {
            var credentials = GoogleAuthProvider.GetCredential(account.IdToken, null);
            firebaseAuth.SignInWithCredential(credentials).AddOnSuccessListener(this).AddOnFailureListener(this);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            //displayNameText.Text = "Display Name: " + firebaseAuth.CurrentUser.DisplayName;
            //emailText.Text = "Email: " + firebaseAuth.CurrentUser.Email;
            //photourlText.Text = "Photo URL: " + firebaseAuth.CurrentUser.PhotoUrl.Path;
            App1.Models.GoogleInfo.mailFromGoogle = firebaseAuth.CurrentUser.Email;
            Toast.MakeText(this, "Login successful", ToastLength.Short).Show();
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            Toast.MakeText(this, "Login Failed", ToastLength.Short).Show();
        }

        [Obsolete]
        public void LogaTerc(){

            if (MainActivity.firebaseAuth.CurrentUser == null)
            {
                var intent = Auth.GoogleSignInApi.GetSignInIntent(MainActivity.googleApiClient);
                ((Activity)Forms.Context).StartActivityForResult(intent, 1);
            }
            else
            {
                MainActivity.firebaseAuth.SignOut();
            }
        }

    }

    public class Login : ILogin
    {

        public void SigninButton_Click()
        {
            MainActivity main = new MainActivity();
            main.LogaTerc();
        }
    }


}