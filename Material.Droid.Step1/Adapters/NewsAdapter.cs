using System;
using Android.App;
using Android.Views;
using Android.Widget;
using Refit;

namespace Material.Droid.Step1
{
	public class NewsAdapter : BaseAdapter<NewsItem>
	{
		protected readonly IIncubateApi Api;

		protected NewsItemList NewsList;

		public override NewsItem this [int index] {
			get { return NewsList [index]; }
		}

		public override int Count {
			get { return NewsList == null ? 0 : NewsList.Count; }
		}


		public NewsAdapter ()
		{
			Api = RestService.For<IIncubateApi> ("https://api.incubate.org/");
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return new NewsWrapper (this [position]);
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			if (convertView == null)
				convertView = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.ListItem, parent, false);			

			var titleLabel = convertView.FindViewById<TextView> (Resource.Id.titleLabel);
			titleLabel.Text = this [position].Title;

			var imageView = convertView.FindViewById<RemoteImageView> (Resource.Id.imageView);
			imageView.SetImageDrawable (null);
			imageView.TrySetRemoteImageURLAsync (this [position].Image);

			return convertView;
		}

		public async void RefreshData (Activity activity)
		{
			NewsList = (await Api.GetNewsItems ().ConfigureAwait (false)).Data;

			activity.RunOnUiThread (NotifyDataSetChanged);
		}


		public class NewsWrapper : Java.Lang.Object
		{
			public NewsItem News;

			public NewsWrapper (NewsItem news)
			{
				News = news;
			}
		}
	}
}

