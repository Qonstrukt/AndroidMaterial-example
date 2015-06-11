using System;
using Android.App;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace Material.Droid.Step1
{
	[Activity (Label = "Incubate News")]			
	public class DetailActivity : Activity
	{
		public const string NewsJsonExtra = "NewsJson";

		private NewsItem newsItem;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Detail);

			ActionBar.SetDisplayHomeAsUpEnabled (true);

			var json = Intent.GetStringExtra (NewsJsonExtra);
			newsItem = JsonConvert.DeserializeObject<NewsItem> (json);

			var titleLabel = FindViewById<TextView> (Resource.Id.titleLabel);
			titleLabel.Text = newsItem.Title;

			var contentLabel = FindViewById<TextView> (Resource.Id.contentLabel);
			contentLabel.TextFormatted = Html.FromHtml (newsItem.Text);

			var imageView = FindViewById<RemoteImageView> (Resource.Id.imageView);
			imageView.SetImageDrawable (null);
			imageView.TrySetRemoteImageURLAsync (newsItem.Image);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Android.Resource.Id.Home:
				Finish ();
				break;
			}

			return base.OnOptionsItemSelected (item);
		}
	}
}

