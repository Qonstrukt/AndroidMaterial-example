using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views.Animations;
using Android.Widget;


namespace Material.Droid
{
	public class RemoteImageView : ImageView
	{
		/*
		 * Because Android file-access is so slow, we use a static memory
		 * cache as extra layer for faster access to frequently used images.
		 */
		static private LruCache memoryCache;

		static protected LruCache MemoryCache {
			get {
				if (memoryCache == null) {
					memoryCache = new LruCache ((int)(Java.Lang.Runtime.GetRuntime ().MaxMemory () / 1024) / 8);
				}
				return memoryCache;
			}
		}


		private CancellationTokenSource tokenSource;


		private HttpClient client;

		protected HttpClient Client {
			get {
				if (client == null) {
					client = new HttpClient ();
				}
				return client;
			}
		}

		public AuthenticationHeaderValue AuthenticationHeader;


		public RemoteImageView (Context context) : base (context)
		{
		}

		public RemoteImageView (Context context, IAttributeSet attrs) : base (context, attrs)
		{
		}

		public RemoteImageView (Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
		{
		}

		public RemoteImageView (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer)
		{
		}

		public async Task SetRemoteImageURLAsync (string url)
		{
			// Cancel any other remote image setting task which might still be running
			if (tokenSource != null)
				tokenSource.Cancel ();

			using (var cached = (BitmapDrawable)MemoryCache.Get (url)) {
				if (cached != null) {
					SetImageDrawable (cached);
					return;
				}
			}

			tokenSource = new CancellationTokenSource ();
			CancellationToken token = tokenSource.Token;

			var imageStream = await Client.GetStreamAsync (url).ConfigureAwait (false);
			var drawable = new BitmapDrawable (imageStream);
			MemoryCache.Put (url, drawable);

			token.ThrowIfCancellationRequested ();

			Post (() => SetImageDrawable (drawable));

			token.ThrowIfCancellationRequested ();

			Post (() => StartAnimation (new AlphaAnimation (0, 1) {
				Duration = 400,
				FillAfter = true,
				Interpolator = new DecelerateInterpolator ()
			}));
		}

		public async void TrySetRemoteImageURLAsync (string url, Action<RemoteImageView> completionAction = null)
		{
			try {
				await SetRemoteImageURLAsync (url).ConfigureAwait (false);

				if (completionAction != null)
					Post (() => completionAction.Invoke (this));
			} catch (Exception e) {
				Console.WriteLine (e);
			}
		}
	}
}

