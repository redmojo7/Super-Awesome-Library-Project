using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;
using RestSharp;
using Newtonsoft.Json;
using API_Classes;
using System.Net.Http;
using System.Threading.Tasks;
using BusinessWebAPI.CustomException;

namespace AsyncClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RestClient client;

        public MainWindow()
        {
            // Set up the window
            InitializeComponent();  
            
            string URL = "https://localhost:44324/";
            client = new RestClient(URL);
            RestRequest request = new RestRequest("api/entries", Method.Get); 
            RestResponse numOfThings = client.Execute(request);
            // Also, tell me how many entries are in the DB.
            TotalNum.Text = numOfThings.Content;
        }


        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {

            // Checking seaching key
            // ensures no TextBoxes are empty
            if (string.IsNullOrEmpty(SearchBox.Text))
            {
                // display popup box
                MessageBox.Show("Please put a seaching Text.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                SearchBox.Focus(); // set focus to SearchBox
                return;
            }
            // if SearchBox format invalid show message
            if (!Regex.Match(SearchBox.Text, "^[\\p{L} \\.'\\-]+$").Success)
            {
                // first name was incorrect
                MessageBox.Show("Invalid Input for Search Text!", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                SearchBox.Focus();
                return;
            } // end if


            // Set TextBox SearchBox as IsReadOnly
            SearchBox.IsReadOnly = true;
            // Disable the buttons on your GUI
            SearchButton.IsEnabled = false;
            SearchProgressBar.IsEnabled = true;
            // Set SearchProgressBar as Indeterminate
            SearchProgressBar.IsIndeterminate = true;


            // send http request to search
            SearchData mySearch = new SearchData(SearchBox.Text);
            
            RestRequest restRequest = new RestRequest("api/searching", Method.Post);
            restRequest.AddHeader("Content-type", "application/json");
            restRequest.AddBody(mySearch);
            RestResponse restResponse = await client.ExecuteAsync(restRequest);

            // Console.WriteLine(restResponse.Content);
            DataIntermed student = JsonConvert.DeserializeObject<DataIntermed>(restResponse.Content);

            if (student != null)
            {
                restRequest = new RestRequest("api/profile", Method.Get);
                restRequest.AddParameter("acctNo", student.acctNo);
                byte[] bitmapdata = await client.DownloadDataAsync(restRequest);
                using (var ms = new MemoryStream(bitmapdata))
                {
                    student.profile = new Bitmap(ms);
                }
            }
            UpdateGUI(student, null);


            // Set TextBox SearchBox as editable
            SearchBox.Dispatcher.Invoke(() => SearchBox.IsReadOnly = false);
            // Enable the buttons on your GUI
            SearchButton.Dispatcher.Invoke(() => SearchButton.IsEnabled = true);
            // Set the SearchProgressBar as Continuous
            SearchProgressBar.Dispatcher.Invoke(() => SearchProgressBar.IsIndeterminate = false);
        }

        private void GoButton_Click(object sender, RoutedEventArgs routedEvent)
        {
            DataIntermed student = new DataIntermed();
            try
            {
                string errorMsm = null;
                // send http request to search
                RestRequest restRequest = new RestRequest("api/getValues", Method.Get);
                restRequest.AddParameter("index", TotalNum.Text);
                RestResponse restResponse = client.Execute(restRequest);
                student = JsonConvert.DeserializeObject<DataIntermed>(restResponse.Content);
                if (restResponse.IsSuccessful && student != null)
                {
                    student.profile = getProfie(student.acctNo);
                }
                else 
                {
                    MessageResponse exception = JsonConvert.DeserializeObject<MessageResponse>(restResponse.Content);
                    errorMsm = exception.Message;
                    student = new DataIntermed();
                }

                //And now, set the values in the GUI!
                UpdateGUI(student, errorMsm);
            }
            catch (FormatException fe)
            {
                Console.WriteLine(fe.Message);
                // Reset GUI
                UpdateGUI(student, null);
            }
            catch (FaultException<ArgumentOutOfRangeException> oe)
            {
                Console.WriteLine(oe.Message);
                // Reset GUI
                UpdateGUI(student, null);
            }
        }

        /*
        * UPDATE the Main Window (GUI)
        */
        private void UpdateGUI(DataIntermed student, string errorMessage)
        {
            if (student != null)
            {
                this.Dispatcher.Invoke(() =>
                {
                    FNameBox.Text = student.firstName;
                    LNameBox.Text = student.lastName;
                    BalanceBox.Text = student.balance.ToString("C");
                    AcctNoBox.Text = student.acctNo.ToString();
                    PinBox.Text = student.pin.ToString("D4");
                    ErrorMessageLable.Content = errorMessage;
                    // Set the image source.
                    ProfileImg.Source = (student.profile == null ? null : BmpImageFromBmp(student.profile));
                });
            }
        }

        /*
         *  Get the Image Source from a Bitmap
         */
        private BitmapImage BmpImageFromBmp(Bitmap bmp)
        {
            using (var memory = new MemoryStream())
            {
                // Saves this image to the specified stream in the ImageFormat.Png format.
                bmp.Save(memory, ImageFormat.Png);
                // sets the current position within the stream to the beginning of the stream.
                memory.Position = 0;

                // Create source.
                // Provides a specialized BitmapSource that is optimized for loading images using XAML.
                var bitmapImage = new BitmapImage();

                // BitmapImage.UriSource must be in a BeginInit/EndInit block.
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                // Return the image source.
                return bitmapImage;
            }
        }

        /*
         * 
         */
        private Bitmap getProfie(uint acctNo) 
        {
            // send http request to search
            Bitmap profile;
            RestRequest restRequest = new RestRequest("api/profile", Method.Get);
            restRequest.AddParameter("acctNo", acctNo);
            byte[] bitmapdata = client.DownloadData(restRequest);
            using (var ms = new MemoryStream(bitmapdata))
            {
                profile = new Bitmap(ms);
            }
            return profile;
        }

        private class MessageResponse
        {
            public MessageResponse()
            {
            }
            public string Message { get; set; }
        }

    }

}
