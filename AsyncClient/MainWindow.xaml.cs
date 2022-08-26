using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using BusinessServer;

namespace AsyncClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StudentBusinessServerInterface foob;
        private string searchvalue;

        public MainWindow()
        {
            // Set up the window
            InitializeComponent();  

            // This is a factory that generates remote connections to our remote class. This is what hides the RPC stuff!
            ChannelFactory<StudentBusinessServerInterface> channelFactory; NetTcpBinding tcp = new NetTcpBinding();
            // Set the URL and create the connection!
            string URL = "net.tcp://localhost:8200/BusinessServer";
            channelFactory = new ChannelFactory<StudentBusinessServerInterface>(tcp, URL);
            foob = channelFactory.CreateChannel();
            // Also, tell me how many entries are in the DB.
            TotalNum.Text = foob.GetNumEntries().ToString();
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

            searchvalue = SearchBox.Text;
            Task<StudentATO> searchTask = new Task<StudentATO>(SearchRequest);
            searchTask.Start();
            ///StudentATO stu = await searchTask;
            // set timeout for async call
            int timeout = 4000;
            if (await Task.WhenAny(searchTask, Task.Delay(timeout)) == searchTask)
            {
                StudentATO stu = searchTask.Result;
                // task completed within timeout
                if (null != stu)
                {
                    System.Console.WriteLine("searching : " + stu.ToString());
                    UpdateGUI(stu.acctNo, stu.pin, stu.firstName, stu.lastName, stu.balance, stu.profile, null);
                }
                else
                {
                    MessageBox.Show("Sorry, cannot found a student!", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // timeout logic
                MessageBox.Show("Sorry, search time out!", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            // Set TextBox SearchBox as editable
            SearchBox.Dispatcher.Invoke(() => SearchBox.IsReadOnly = false);
            // Enable the buttons on your GUI
            SearchButton.Dispatcher.Invoke(() => SearchButton.IsEnabled = true);
            // Set the SearchProgressBar as Continuous
            SearchProgressBar.Dispatcher.Invoke(() => SearchProgressBar.IsIndeterminate = false);
        }

        private StudentATO SearchRequest()
        {
            uint acctNo = 0, pin = 0;
            string firstName = null, lastName = null;
            int balance = 0;
            Bitmap profileBitmap;
            foob.GetValuesForSearch(searchvalue, out acctNo, out pin, out balance, out firstName, out lastName, out profileBitmap);
            //And now, set the values in the GUI!
            if (acctNo != 0)
            {
                return new StudentATO(pin, acctNo, firstName, lastName, balance, profileBitmap);
            }
            return null;
        }

        private void GoButton_Click(object sender, RoutedEventArgs routedEvent)
        {
            int index = 0;
            uint acctNo = 0, pin = 0;
            string firstName = null, lastName = null;
            int balance = 0;
            Bitmap profileBitmap;
            try
            {
                // On click, Get the index....
                index = int.Parse(TotalNum.Text);
                // Then, run our RPC function, using the out mode parameters...
                foob.GetValuesForEntry(index, out acctNo, out pin, out balance,
                    out firstName, out lastName, out profileBitmap);
                //And now, set the values in the GUI!
                UpdateGUI(acctNo, pin, firstName, lastName, balance, profileBitmap, null);
            }
            catch (FormatException fe)
            {
                Console.WriteLine(fe.Message);
                // Reset GUI
                UpdateGUI(0, 0, null, null, 0, null, string.Format("The Parameter '{0}' Is Incorrect.", TotalNum.Text));
            }
            catch (FaultException<ArgumentOutOfRangeException> oe)
            {
                Console.WriteLine(oe.Message);
                // Reset GUI
                UpdateGUI(0, 0, null, null, 0, null, string.Format("The Parameter '{0}' Is Out of Range.", TotalNum.Text));
            }
        }

        /*
        * UPDATE the Main Window (GUI)
        */
        private void UpdateGUI(uint acctNo, uint pin, string firstName, string lastName, int balance, Bitmap profileBitmap, string errorMessage)
        {
            this.Dispatcher.Invoke(() =>
            {
                FNameBox.Text = firstName;
                LNameBox.Text = lastName;
                BalanceBox.Text = balance.ToString("C");
                AcctNoBox.Text = acctNo.ToString();
                PinBox.Text = pin.ToString("D4");
                ErrorMessageLable.Content = errorMessage;
                // Set the image source.
                ProfileImg.Source = (profileBitmap == null ? null : BmpImageFromBmp(profileBitmap));
            });
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

    }
}
