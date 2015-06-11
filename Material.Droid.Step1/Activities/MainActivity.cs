using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace Material.Droid.Step1
{
	[Activity (Label = "Incubate News", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : ListActivity
	{
		public NewsAdapter NewsAdapter {
			get { return ListAdapter as NewsAdapter; }
		}


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Main);

			ListAdapter = new NewsAdapter ();

			NewsAdapter.RefreshData (this);
		}

		protected override void OnListItemClick (ListView l, View v, int position, long id)
		{
			var newsitem = NewsAdapter [position];
			var intent = new Intent (this, typeof(DetailActivity));
			intent.PutExtra (DetailActivity.NewsJsonExtra, JsonConvert.SerializeObject (newsitem));
			StartActivity (intent);
		}
	}
}


