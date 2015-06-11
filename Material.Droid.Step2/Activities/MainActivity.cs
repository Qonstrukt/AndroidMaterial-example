using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Util;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Newtonsoft.Json;

namespace Material.Droid.Step2
{
	[Activity (Label = "Incubate News", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : AppCompatActivity
	{
		protected NewsAdapter NewsAdapter;

		protected RecyclerView RecyclerView;

		protected FloatingActionButton FAB;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Main);

			NewsAdapter = new NewsAdapter ();
			NewsAdapter.RefreshData (this);
			NewsAdapter.ItemTouched += NewsItemTouched;

			RecyclerView = FindViewById<RecyclerView> (Android.Resource.Id.List);
			RecyclerView.SetLayoutManager (new LinearLayoutManager (this));
			RecyclerView.SetAdapter (NewsAdapter);

			FAB = FindViewById<FloatingActionButton> (Resource.Id.button);
		}

		private void NewsItemTouched (object sender, int position)
		{
			var newsItem = NewsAdapter [position];

			var sharedElements = new [] {
				new Pair (((NewsAdapter.NewsViewHolder)sender).TitleLabel, String.Format ("Title{0}", newsItem.Id)),
				new Pair (((NewsAdapter.NewsViewHolder)sender).ImageView, String.Format ("ImageView{0}", newsItem.Id))	
			};

			var intent = new Intent (this, typeof(DetailActivity));
			intent.PutExtra (DetailActivity.NewsJsonExtra, JsonConvert.SerializeObject (newsItem));
			StartActivity (intent, ActivityOptionsCompat.MakeSceneTransitionAnimation (this, sharedElements).ToBundle ());
		}
	}
}


