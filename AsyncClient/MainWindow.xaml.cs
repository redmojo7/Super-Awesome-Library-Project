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
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using BusinessWebAPI;

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
            
            string URL = "http://localhost:54863/";
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

            string errorMsg = null;
            Student student = null;
            Bitmap avarta = null;

            // send http request to search
            SearchData mySearch = new SearchData(SearchBox.Text);
            
            RestRequest restRequest = new RestRequest("api/searching", Method.Post);
            restRequest.AddHeader("Content-type", "application/json");
            restRequest.AddBody(mySearch);
            RestResponse restResponse = await client.ExecuteAsync(restRequest);

            if (restResponse.IsSuccessful)
            {
                student = JsonConvert.DeserializeObject<Student>(restResponse.Content);
                
                if (student != null && !string.IsNullOrEmpty(student.Avatar))
                {
                    avarta = getProfie(student.Avatar);
                }
            }
            else if (restResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                MessageBox.Show("Search fail! NotFound!", "Message", MessageBoxButton.OK);
            }
            UpdateGUI(student, errorMsg, avarta);


            // Set TextBox SearchBox as editable
            SearchBox.Dispatcher.Invoke(() => SearchBox.IsReadOnly = false);
            // Enable the buttons on your GUI
            SearchButton.Dispatcher.Invoke(() => SearchButton.IsEnabled = true);
            // Set the SearchProgressBar as Continuous
            SearchProgressBar.Dispatcher.Invoke(() => SearchProgressBar.IsIndeterminate = false);
        }

        private async void GoButton_Click(object sender, RoutedEventArgs routedEvent)
        {
            Student student = new Student();
            Bitmap avarta = null;
            try
            {
                string errorMsm = null;
                // send http request to search
                RestRequest restRequest = new RestRequest("api/student/Get", Method.Get);
                restRequest.AddParameter("Id", TotalNum.Text);
                RestResponse restResponse = await client.ExecuteAsync(restRequest);
                if (restResponse.IsSuccessful)
                {
                    student = JsonConvert.DeserializeObject<Student>(restResponse.Content);
                }
                else if (restResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Get fail! NotFound!", "Message", MessageBoxButton.OK);
                }
                else
                {
                    Console.WriteLine(restResponse.Content);
                    MessageBox.Show("Get fail!", "Error", MessageBoxButton.OK);
                }
                
                if (restResponse.IsSuccessful)
                {
                    if (student != null && !string.IsNullOrEmpty(student.Avatar))
                    {
                        avarta = getProfie(student.Avatar);
                    }
                }
                else 
                {
                    MessageResponse exception = JsonConvert.DeserializeObject<MessageResponse>(restResponse.Content);
                    errorMsm = exception.Message;
                    student = new Student();
                }

                //And now, set the values in the GUI!
                UpdateGUI(student, errorMsm, avarta);
            }
            catch (FormatException fe)
            {
                Console.WriteLine(fe.Message);
                // Reset GUI
                UpdateGUI(student, null, null);
            }
            catch (FaultException<ArgumentOutOfRangeException> oe)
            {
                Console.WriteLine(oe.Message);
                // Reset GUI
                UpdateGUI(student, null, null);
            }
        }

        /*
        * UPDATE the Main Window (GUI)
        */
        private void UpdateGUI(Student student, string errorMessage, Bitmap avatar)
        {
            if (student != null)
            {
                this.Dispatcher.Invoke(() =>
                {
                    FNameBox.Text = student.FirstName;
                    LNameBox.Text = student.LastName;
                    BalanceBox.Text = student.Balance.ToString();
                    AcctNoBox.Text = student.AcctNum.ToString();
                    PinBox.Text = student.Pin.ToString();
                    ImageTitleBlock.Text = student.Avatar;
                    ErrorMessageLable.Content = errorMessage;
                    // Set the image source.
                    ProfileImg.Source = (avatar == null ? null : BmpImageFromBmp(avatar));
                });
            }
            else 
            {
                this.Dispatcher.Invoke(() =>
                {
                    FNameBox.Text = "";
                    LNameBox.Text = "";
                    BalanceBox.Text = "";
                    AcctNoBox.Text = "";
                    PinBox.Text = "";
                    ImageTitleBlock.Text = "";
                    ErrorMessageLable.Content = errorMessage;
                    // Set the image source.
                    ProfileImg.Source = (avatar == null ? null : BmpImageFromBmp(avatar));
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
        private Bitmap getProfie(string path) 
        {
            // send http request to search
            Bitmap profile;
            RestRequest restRequest = new RestRequest("api/profile", Method.Get);
            restRequest.AddParameter("path", path);
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

        private async void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            //check
            // Checking seaching key
            // ensures no TextBoxes are empty
            uint pin;
            if (string.IsNullOrEmpty(PinBox.Text) || !uint.TryParse(PinBox.Text, out pin))
            {
                // display popup box
                MessageBox.Show("Please put a valid Pin.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                PinBox.Focus(); // set focus to PinBox
                return;
            }
            uint acctNo;
            if (string.IsNullOrEmpty(AcctNoBox.Text) || !uint.TryParse(AcctNoBox.Text, out acctNo))
            {
                // display popup box
                MessageBox.Show("Please put a valid AcctNo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                AcctNoBox.Focus(); // set focus to AcctNoBox
                return;
            }
            if (string.IsNullOrEmpty(FNameBox.Text) || !Regex.Match(FNameBox.Text, "^[\\p{L} \\.'\\-]+$").Success)
            {
                // display popup box
                MessageBox.Show("Please put valid First Name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                FNameBox.Focus(); // set focus to FNameBox
                return;
            }
            if (string.IsNullOrEmpty(LNameBox.Text) || !Regex.Match(LNameBox.Text, "^[\\p{L} \\.'\\-]+$").Success)
            {
                // display popup box
                MessageBox.Show("Please put a valid Last Name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                LNameBox.Focus(); // set focus to LNameBox
                return;
            }
            int balance;
            if (string.IsNullOrEmpty(BalanceBox.Text) || !int.TryParse(BalanceBox.Text, out balance))
            {
                // display popup box
                MessageBox.Show("Please put a valid Balance.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                BalanceBox.Focus(); // set focus to BalanceBox
                return;
            }

            //uint pin, uint acctNo, string firstName, string lastName, int balance, Bitmap profile
            Student student = new Student(FNameBox.Text, LNameBox.Text, Convert.ToInt32(BalanceBox.Text), 
                Convert.ToInt32(AcctNoBox.Text), Convert.ToInt32(PinBox.Text), ImageTitleBlock.Text);
            try
            {
                string errorMsm = null;
                // send http request to search
                RestRequest restRequest = new RestRequest("api/student/insert", Method.Post);
                restRequest.AddBody(student);
                RestResponse restResponse = await client.ExecuteAsync(restRequest);
                if (restResponse.IsSuccessful)
                {
                    // upload avarta to server
                    if (null != ProfileImg.Source && !string.IsNullOrEmpty((string)FileNameLabel.Content))
                    {
                        RestRequest request = new RestRequest("api/Students/avarta/{id}", Method.Post);
                        var selectedFileName = FileNameLabel.Content;
                        request.AddUrlSegment("id", student.Id);
                        request.AddFile("avarta", (string)selectedFileName);
                        request.AddHeader("Content-Type", "multipart/form-data");
                        RestResponse response = await client.ExecuteAsync(request);
                    }
                    MessageBox.Show("Insert Successful!", "Message", MessageBoxButton.OK);
                    // reset FileNameLabel
                    FileNameLabel.Content = "";
                }
                else if (restResponse.StatusCode == System.Net.HttpStatusCode.Conflict) 
                {
                    Console.WriteLine(restResponse.Content);
                    MessageBox.Show("Insert fail! Already exist!", "Error", MessageBoxButton.OK);
                }
                else
                {
                    Console.WriteLine(restResponse.Content);
                    MessageBox.Show("Insert fail!", "Error", MessageBoxButton.OK);
                }
            }
            catch (FormatException fe)
            {
                Console.WriteLine(fe.Message);
            }
            catch (FaultException<ArgumentOutOfRangeException> oe)
            {
                Console.WriteLine(oe.Message);
            }

        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            uint acctNo;
            if (string.IsNullOrEmpty(AcctNoBox.Text) || !uint.TryParse(AcctNoBox.Text, out acctNo))
            {
                // display popup box
                MessageBox.Show("Please put a valid AcctNo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                AcctNoBox.Focus(); // set focus to AcctNoBox
                return;
            }
            try
            {
                string errorMsm = null;
                // send http request to delete
                RestRequest restRequest = new RestRequest("api/student/{Id}", Method.Delete);
                restRequest.AddUrlSegment("Id", AcctNoBox.Text);
                RestResponse restResponse = await client.ExecuteAsync(restRequest);
                if (restResponse.IsSuccessful)
                {
                    MessageBox.Show("Delete Successful!", "Message", MessageBoxButton.OK);
                    // reset FileNameLabel
                    UpdateGUI(null, null, null);
                }
                else if (restResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Delete fail! NotFound!", "Message", MessageBoxButton.OK);
                }
                else
                {
                    Console.WriteLine(restResponse.Content);
                    MessageBox.Show("Delete fail!", "Error", MessageBoxButton.OK);
                }
            }
            catch (FormatException fe)
            {
                Console.WriteLine(fe.Message);
            }
            catch (FaultException<ArgumentOutOfRangeException> oe)
            {
                Console.WriteLine(oe.Message);
            }

        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            //check
            // Checking seaching key
            // ensures no TextBoxes are empty
            uint pin;
            if (string.IsNullOrEmpty(PinBox.Text) || !uint.TryParse(PinBox.Text, out pin))
            {
                // display popup box
                MessageBox.Show("Please put a valid Pin.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                PinBox.Focus(); // set focus to PinBox
                return;
            }
            uint acctNo;
            if (string.IsNullOrEmpty(AcctNoBox.Text) || !uint.TryParse(AcctNoBox.Text, out acctNo))
            {
                // display popup box
                MessageBox.Show("Please put a valid AcctNo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                AcctNoBox.Focus(); // set focus to AcctNoBox
                return;
            }
            if (string.IsNullOrEmpty(FNameBox.Text) || !Regex.Match(FNameBox.Text, "^[\\p{L} \\.'\\-]+$").Success)
            {
                // display popup box
                MessageBox.Show("Please put valid First Name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                FNameBox.Focus(); // set focus to FNameBox
                return;
            }
            if (string.IsNullOrEmpty(LNameBox.Text) || !Regex.Match(LNameBox.Text, "^[\\p{L} \\.'\\-]+$").Success)
            {
                // display popup box
                MessageBox.Show("Please put a valid Last Name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                LNameBox.Focus(); // set focus to LNameBox
                return;
            }
            int balance;
            if (string.IsNullOrEmpty(BalanceBox.Text) || !int.TryParse(BalanceBox.Text, out balance))
            {
                // display popup box
                MessageBox.Show("Please put a valid Balance.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                BalanceBox.Focus(); // set focus to BalanceBox
                return;
            }

            //uint pin, uint acctNo, string firstName, string lastName, int balance, Bitmap profile
            Student student = new Student(FNameBox.Text, LNameBox.Text, Convert.ToInt32(BalanceBox.Text), 
                Convert.ToInt32(AcctNoBox.Text), Convert.ToInt32(PinBox.Text), ImageTitleBlock.Text);
            try
            {
                string errorMsm = null;
                // send http request to update
                RestRequest restRequest = new RestRequest("api/student/update", Method.Put);
                restRequest.AddBody(student);
                RestResponse restResponse = await client.ExecuteAsync(restRequest);
                if (restResponse.IsSuccessful)
                {
                    // upload avarta to server
                    if (null != ProfileImg.Source && !string.IsNullOrEmpty((string)FileNameLabel.Content))
                    {
                        RestRequest request = new RestRequest("api/Students/avarta/{id}", Method.Post);
                        var selectedFileName = FileNameLabel.Content;
                        request.AddUrlSegment("id", student.Id);
                        request.AddFile("avarta", (string)selectedFileName);
                        request.AddHeader("Content-Type", "multipart/form-data");
                        RestResponse response = await client.ExecuteAsync(request);
                    }
                    MessageBox.Show("Update Successful!", "Message", MessageBoxButton.OK);
                    // reset FileNameLabel
                    FileNameLabel.Content = "";
                }
                else
                {
                    Console.WriteLine(restResponse.Content);
                    MessageBox.Show("Update fail!", "Error", MessageBoxButton.OK);
                }
            }
            catch (FormatException fe)
            {
                Console.WriteLine(fe.Message);
            }
            catch (FaultException<ArgumentOutOfRangeException> oe)
            {
                Console.WriteLine(oe.Message);
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;
            try
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedFileName = dlg.FileName;
                    FileNameLabel.Content = selectedFileName;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(selectedFileName);
                    bitmap.EndInit();
                    ProfileImg.Source = bitmap;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong File type try upload images", "Error", MessageBoxButton.OK);
                ProfileImg.Source = null;
            }
        
        }

        private async void GenerateDBButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string errorMsm = null;
                // send http request to generate db
                RestRequest restRequest = new RestRequest("api/GenerateDB", Method.Get);
                RestResponse restResponse = await client.ExecuteAsync(restRequest);
                if (restResponse.IsSuccessful)
                {
                    MessageBox.Show("GenerateDB Successful!", "Message", MessageBoxButton.OK);
                    // update TotalNum
                    string URL = "http://localhost:54863/";
                    client = new RestClient(URL);
                    RestRequest request = new RestRequest("api/entries", Method.Get);
                    RestResponse numOfThings = client.Execute(request);
                    // Also, tell me how many entries are in the DB.
                    TotalNum.Text = numOfThings.Content;
                }
                else
                {
                    Console.WriteLine(restResponse.Content);
                    MessageBox.Show("GenerateDB fail!", "Error", MessageBoxButton.OK);
                }
            }
            catch (FormatException fe)
            {
                Console.WriteLine(fe.Message);
            }
            catch (FaultException<ArgumentOutOfRangeException> oe)
            {
                Console.WriteLine(oe.Message);
            }
            UpdateGUI(null, null, null);
        }
    }
}
