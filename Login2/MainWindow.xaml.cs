using Prisma_data_analyz.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using static System.Net.WebRequestMethods;

namespace Login2
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
         

        }
        protected string ExecuteInterfaceByUrl(string url, FormUrlEncodedContent para=null)
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            
            using (var http = new HttpClient(handler))
            {
                http.DefaultRequestHeaders.Add("Authorization", " Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiVXNlck5hbWVZayIsImV4cCI6MTU2OTc3MzEzNiwiaXNzIjoiaWdib21fd2ViIiwiYXVkIjoiaWdib21fd2ViIn0.jYG7J_rBfXKuTiZkBrkF53q9_m2u3ZUPZ2DjDkn6Wi0");
                var responseRpt = http.GetAsync(url).Result;
                var resultRpt = responseRpt.Content.ReadAsStringAsync().Result;

                return resultRpt;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
            
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            var http = new HttpClient(handler);
           
            var responseRpt = http.GetAsync(@"https://localhost:5001/api/todo/GetPicture").Result;
            var resultRpt = responseRpt.Content.ReadAsByteArrayAsync().Result;
            FileHelper.ByteToFile(resultRpt, "download.jpg");
            // Console.WriteLine(ExecuteInterfaceByUrl(@"https://localhost:5001/api/Todo/56"));
        }
    }
}
