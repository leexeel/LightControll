using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Net;
using System.Text.RegularExpressions;

namespace LightControll
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            ToggleButton tb = FindViewById<ToggleButton>(Resource.Id.toggleButton1);
            tb.Click += (o, e) =>
            {
                // Perform action on clicks  
                if (tb.Checked)
                {
                    Toast.MakeText(this, "On", ToastLength.Short).Show();
                    WebClient syncClient = new WebClient();
                    string content = syncClient.DownloadString("http://192.168.10.161/REL1");
                }
                else
                {
                    Android.Widget.Toast.MakeText(this, "Off", ToastLength.Short).Show();
                    WebClient syncClient = new WebClient();
                    string content = syncClient.DownloadString("http://192.168.10.161/REL1");
                };
            };
            WebClient ansClient = new WebClient();
            string currentStatus = ansClient.DownloadString("http://192.168.10.161/getStatus");

            Regex filter = new Regex(@"(REL1:\d)");
            var match = filter.Match(currentStatus);
            if (match.Success)
            {
                if (match.Value.Split(':')[1] == "1")
                {
                    tb.Checked = true;
//                    Toast.MakeText(this, "Aici On" + match.Value.Split(':')[1], ToastLength.Short).Show();
                }
                else
                {
                    tb.Checked = false;
//                    Toast.MakeText(this, "Aici Off" + match.Value.Split(':')[1], ToastLength.Short).Show();
                }
            }

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(5);

            var timer = new System.Threading.Timer((e) =>
            {
                WebClient ansClient = new WebClient();
                string currentStatus = ansClient.DownloadString("http://192.168.10.161/getStatus");

                Regex filter = new Regex(@"(REL1:\d)");
                var match = filter.Match(currentStatus);
                if (match.Success)
                {
                    if (match.Value.Split(':')[1] == "1")
                    {
                        tb.Checked = true;
                    }
                    else
                    {
                        tb.Checked = false;
                    }
                }
            }, null, startTimeSpan, periodTimeSpan);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}