using System;
using System.Net.Http;
using System.Threading.Tasks;

using Plugin.Settings;
using Plugin.Connectivity;
using Q42.HueApi;

namespace TextMood
{
	abstract class PhillipsHueBridgeServices : BaseHttpClientService
	{
		public static string PhillipsHueBridgeIPAddress
		{
			get => CrossSettings.Current.GetValueOrDefault(nameof(PhillipsHueBridgeIPAddress), string.Empty);
			set => CrossSettings.Current.AddOrUpdateValue(nameof(PhillipsHueBridgeIPAddress), value);
		}

        public static async Task Temp()
		{
			var authClient = new RemoteAuthenticationClient("IShHigzKTbhYv8FIAf5x3ynSAEi5uwFB", "kkI4h3m4JAsGMkTa", "5788fede-988a-40fc-bd92-c81824659243");
			var authUrl = authClient.BuildAuthorizeUri("sample", "consoleapp");

			var client = new HttpClient();

			var result = await client.GetStringAsync(authUrl);

		}

		public static async ValueTask<HttpResponseMessage> UpdateLightBulbColor(int hue)
		{
			var isBridgeReachable = await IsBridgeReachable().ConfigureAwait(false);
			if (!isBridgeReachable)
				return default;

			var hueRequest = new PhillipsHueRequestModel
			{
				On = true,
				Hue = hue,
				Saturation = hue >= 0 ? 255 : 0,
				Brightness = 255
			};

			return await PutObjectToAPI($"http://{PhillipsHueBridgeIPAddress}/api/5pE71iepzEeuKQY1SSfwuiATTfih3dy0YZDAwhCh/lights/1/state", hueRequest).ConfigureAwait(false);
		}

		static async ValueTask<bool> IsBridgeReachable()
		{
			try
			{
				return CrossConnectivity.Current.IsConnected
										&& await CrossConnectivity.Current.IsRemoteReachable(PhillipsHueBridgeIPAddress).ConfigureAwait(false);
			}
			catch (ArgumentNullException)
			{
				return false;
			}
		}
	}
}