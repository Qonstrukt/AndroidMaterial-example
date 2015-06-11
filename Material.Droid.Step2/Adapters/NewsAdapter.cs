using System;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Refit;

namespace Material.Droid.Step2
{
	public class NewsAdapter : RecyclerView.Adapter
	{
		protected readonly IIncubateApi Api;

		protected NewsItemList NewsList;

		public NewsItem this [int index] {
			get { return NewsList [index]; }
		}

		public override int ItemCount {
			get { return NewsList == null ? 0 : NewsList.Count; }
		}


		public event EventHandler<int> ItemTouched;


		public NewsAdapter ()
		{
			Api = RestService.For<IIncubateApi> ("https://api.incubate.org/");
		}

		public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
		{
			var newsHolder = holder as NewsViewHolder;
			newsHolder.Bind (NewsList [position]);
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
		{
			var holder = new NewsViewHolder (LayoutInflater.From (parent.Context).Inflate (Resource.Layout.ListItem, parent, false));
			holder.Touched += ItemTouched;
			return holder;
		}

		public async void RefreshData (Activity activity)
		{
			NewsList = (await Api.GetNewsItems ().ConfigureAwait (false)).Data;

			activity.RunOnUiThread (NotifyDataSetChanged);
		}


		public class NewsViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
		{
			public readonly TextView TitleLabel;
			public readonly RemoteImageView ImageView;

			public event EventHandler<int> Touched;


			public NewsViewHolder (View itemView) : base (itemView)
			{
				TitleLabel = itemView.FindViewById<TextView> (Resource.Id.titleLabel);
				ImageView = itemView.FindViewById<RemoteImageView> (Resource.Id.imageView);

				itemView.SetOnClickListener (this);
			}

			public void Bind (NewsItem newsitem)
			{
				TitleLabel.Text = newsitem.Title;

				ImageView.SetImageDrawable (null);
				ImageView.TrySetRemoteImageURLAsync (newsitem.Image);
			}

			public void OnClick (View v)
			{
				if (Touched != null)
					Touched.Invoke (this, AdapterPosition);
			}
		}
	}
}

