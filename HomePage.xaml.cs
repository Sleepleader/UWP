using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace App2
{
    public sealed partial class HomePage : Page
    {

        private const string BingImageUrlApi = "https://bing.com/HPImageArchive.aspx?format=js&idx=0&n=1";
        // 创建HTTP客户端实例（用于网络请求）
        private readonly HttpClient _httpClient = new HttpClient();

        public HomePage()
        {
            this.InitializeComponent();
            // 页面加载时自动获取名言
            Loaded += async (sender, e) => await GetQuoteFromApi();
            LoadBingDailyImage();

        }

        private async void LoadBingDailyImage()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(BingImageUrlApi);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject jsonResponse = JObject.Parse(responseBody);

                    string imageUrl = "https://bing.com" + jsonResponse["images"][0]["url"].ToString();
                    BingDailyImage.Source = new BitmapImage(new Uri(imageUrl));

                    // Extract text information, such as the image title or description
                    string imageDescription = jsonResponse["images"][0]["copyright"].ToString();
                    ImageDescription.Text = imageDescription;
                }
            }
            catch (Exception e)
            {
                // Handle errors
                BingDailyImage.Source = null;
                ImageDescription.Text = "Error loading Bing daily image.";
                System.Diagnostics.Debug.WriteLine($"Error loading Bing daily image: {e.Message}");

            }
        }
        // 从API获取名言并显示
        private async System.Threading.Tasks.Task GetQuoteFromApi()
        {
            try
            {
                // 显示加载动画
                LoadingRing.IsActive = true;
                QuoteContent.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                QuoteAuthor.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                // API地址（返回英文名言）
                string apiUrl = "http://api.forismatic.com/api/1.0/?method=getQuote&format=json&lang=en";

                // 发送GET请求并获取响应内容
                string response = await _httpClient.GetStringAsync(apiUrl);

                // 解析JSON数据
                JObject json = JObject.Parse(response);

                // 提取quoteText和quoteAuthor（注意处理可能的空值）
                string quoteText = json["quoteText"]?.ToString() ?? "No quote available";
                string quoteAuthor = json["quoteAuthor"]?.ToString() ?? "Unknown Author";

                // 将数据绑定到XAML控件
                QuoteContent.Text = quoteText;
                QuoteAuthor.Text = $"- {quoteAuthor}"; // 作者前加横线美化显示
            }
            catch (Exception ex)
            {
                // 异常处理（如网络错误时显示提示）
                QuoteContent.Text = "Failed to load quote.";
                QuoteAuthor.Text = $"Error: {ex.Message}";
            }
            finally
            {
                // 隐藏加载动画，显示内容
                LoadingRing.IsActive = false;
                QuoteContent.Visibility = Windows.UI.Xaml.Visibility.Visible;
                QuoteAuthor.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }
    }
}