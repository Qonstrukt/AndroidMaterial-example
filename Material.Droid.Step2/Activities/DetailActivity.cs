using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Text;
using Android.Transitions;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace Material.Droid.Step2
{
	[Activity (Label = "Incubate News")]			
	public class DetailActivity : AppCompatActivity
	{
		public const string NewsJsonExtra = "NewsJson";

		private NewsItem newsItem;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Detail);

			SupportActionBar.SetDisplayHomeAsUpEnabled (true);

			var json = Intent.GetStringExtra (NewsJsonExtra);
			newsItem = JsonConvert.DeserializeObject<NewsItem> (json);

			var titleLabel = FindViewById<TextView> (Resource.Id.titleLabel);
			titleLabel.Text = newsItem.Title;

			var contentLabel = FindViewById<TextView> (Resource.Id.contentLabel);
			contentLabel.TextFormatted = Html.FromHtml (newsItem.Text);

			var imageView = FindViewById<RemoteImageView> (Resource.Id.imageView);
			imageView.SetImageDrawable (null);
			imageView.TrySetRemoteImageURLAsync (newsItem.Image);

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop) {
				imageView.TransitionName = String.Format ("ImageView{0}", newsItem.Id);
				titleLabel.TransitionName = String.Format ("Title{0}", newsItem.Id);

				Window.EnterTransition = new Fade ();
			}
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Android.Resource.Id.Home:
				SupportFinishAfterTransition ();
				break;
			}

			return base.OnOptionsItemSelected (item);
		}
	}
}

