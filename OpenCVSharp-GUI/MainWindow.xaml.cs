using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using OpenCvSharp;
//using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;
using OpenCVSharp_GUI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
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

//https://github.com/shimat/opencvsharp
//https://github.com/shimat/opencvsharp_samples
//Use https://www.nuget.org/packages/OpenCvSharp3-AnyCPU/ 4.0.0.20181129 

namespace OpenCVSharp_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        private ObservableCollection<String> _operationOrder = new ObservableCollection<String>();
        private Mat _originalMat;
        private Mat _convertedMat;
        private double _imageWidth;
        private double _imageHeight;
        private BitmapSource _originalImage;
        private BitmapSource _convertedImage;
        private int _cannyValue1;
        private int _cannyValue2;
        private int _thrsvalue = 1;
        private int _adpt1;
        private int _adpt2;
        private int _resizeValue;
        private int _blockSize = 3;
        private int _cValue = 1;
        private int _intensityValue = 1;
        private int _intensityCount = 0;
        private AdaptiveThresholdTypes _adptthresholdType;
        private ThresholdTypes _thsType;

        private string _loaded_image = null;
        private string _saved_image = null;


        #region Public Properties
        public static MainWindow Instance { get; private set; }
        public BitmapSource OriginalImage
        {
            get
            {
                return _originalImage;
            }
            set
            {
                _originalImage = value;
                OnPropertyChanged("OriginalImage");
            }
        }


        public string Loaded_Image
        {
            get
            {
                return _loaded_image;
            }
            set
            {
                _loaded_image = value;
            }
        }

        public string Saved_Image
        {
            get
            {
                return _saved_image;
            }
            set
            {
                _saved_image = value;
            }
        }


        public BitmapSource ConvertedImage
        {
            get
            {
                return _convertedImage;
            }
            set
            {
                _convertedImage = value;
                OnPropertyChanged("ConvertedImage");
            }
        }

        public int CannyValue1
        {
            get { return _cannyValue1; }
            set
            {
                _cannyValue1 = value;
                OnPropertyChanged("CannyValue1");
            }
        }

        public int CannyValue2
        {
            get { return _cannyValue2; }
            set
            {
                _cannyValue2 = value;
                OnPropertyChanged("CannyValue2");
            }
        }

        public int ResizeValue
        {
            get
            {
                return _resizeValue;
            }
            set
            {
                _resizeValue = value;
                OnPropertyChanged("ResizeValue");
            }
        }

        public int AdaptiveVal1
        {
            get
            {
                return _adpt1;
            }
            set
            {
                _adpt1 = value;
                OnPropertyChanged("AdaptiveVal1");
            }
        }

        public int AdaptiveVal2
        {
            get
            {
                return _adpt2;
            }
            set
            {
                _adpt2 = value;
                OnPropertyChanged("AdaptiveVal2");
            }
        }

        public int ThresholdValue
        {
            get
            {
                return _thrsvalue;
            }
            set
            {
                _thrsvalue = value;
                OnPropertyChanged("ThresholdValue");
            }
        }

        public double ImageWidth
        {
            get
            {
                return _imageWidth;
            }
            set
            {
                _imageWidth = value;
                OnPropertyChanged("ImageWidth");
            }
        }

        public double ImageHeight
        {
            get
            {
                return _imageHeight;
            }
            set
            {
                _imageHeight = value;
                OnPropertyChanged("ImageHeight");
            }
        }

        public int BlockSize
        {
            get
            {
                return _blockSize;
            }
            set
            {
                _blockSize = value;
                OnPropertyChanged("BlockSize");
            }
        }  

        public int CValue
        {
            get
            {
                return _cValue;
            }
            set
            {
                _cValue = value;
                OnPropertyChanged("CValue");
            }
        }

        public int IntensityValue
        {
            get
            {
                return _intensityValue;
            }
            set
            {
                _intensityValue = value;
                OnPropertyChanged("IntensityValue");
            }
        }

        public int IntensityCount
        {
            get
            {
                return _intensityCount;
            }
            set
            {
                _intensityCount = value;
                OnPropertyChanged("IntensityCount");
            }
        }

        public AdaptiveThresholdTypes AdptThresholdType
        {
            get { return _adptthresholdType; }
            set
            {
                _adptthresholdType = value;
                OnPropertyChanged("ThresholdType");
            }
        }

        public IEnumerable<AdaptiveThresholdTypes> AdptThresholdTypes
        {
            get
            {
                return Enum.GetValues(typeof(AdaptiveThresholdTypes)).Cast<AdaptiveThresholdTypes>();
            }
        }

        public ThresholdTypes ThsType
        {
            get
            {
                return _thsType;
            }
            set
            {
                _thsType = value;
                OnPropertyChanged("ThsType");
            }
        }

        public IEnumerable<ThresholdTypes> ThsTypes
        {
            get
            {
                return Enum.GetValues(typeof(ThresholdTypes)).Cast<ThresholdTypes>();
            }
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            DataContext = this;
            _operationOrder.CollectionChanged += operationOrder_CollectionChanged;
        }

        #region Private Methods
        private void operationOrder_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _convertedMat = _originalMat;
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var item in _operationOrder)
                {
                    ExecuteTransformation(_convertedMat, item);
                }
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                if (_operationOrder.Count > 0)
                {
                    foreach (var item in _operationOrder)
                    {
                        ExecuteTransformation(_convertedMat, item);
                    }
                }
                else
                {
                    ConvertedImage = OriginalImage;
                }

            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            {
                ConvertedImage = OriginalImage;
            }
            GC.Collect();
        }

        private void ExecuteTransformation(Mat image, String transformation)
        {
            Mat gray = new Mat(image.Height, image.Width, MatType.CV_8UC1);
            Mat dest = new Mat(image.Height, image.Width, MatType.CV_8UC1);

            if (image.Channels() > 1)
            {
                gray= image.CvtColor(ColorConversionCodes.BGR2GRAY).Clone();
            }
            else
            {
                gray = image.Clone();
            }

            switch (transformation)
            {
                case "Histogram":
                    Filters.HistogramEqualize(gray, ref dest);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    break;
                case "Erode":
                    Filters.ErodeImage(gray, ref dest);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    break;
                case "EdgeEnhancement":
                    Filters.EdgeEnhancement(gray, ref dest);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    break;
                case "CannyFilter":
                    Filters.CannyFilter(gray, ref dest, CannyValue1, CannyValue2);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    break;
                case "Dilate":
                    Filters.DilateImage(gray, ref dest);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    break;
                case "Denoiser":
                    //Filters.Denoiser(gray, ref dest);
                    Filters.EdgeEnhancement(gray, ref dest);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    break;
                case "Resize":
                    Filters.ScaleImage(gray, ref dest, ResizeValue);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    ImageWidth = Math.Ceiling( _convertedImage.Width);
                    ImageHeight = Math.Ceiling(_convertedImage.Height);
                    break;
                case "BitWiseInverter":
                    Filters.InvertImage(gray, ref dest);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    break;
                case "AdaptiveThreshold":
                    Filters.SetAdaptTreshold(gray, ref dest, AdptThresholdType, ThsType, ThresholdValue, BlockSize, CValue);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    break;
                case "CountPixelByIntensity":
                    IntensityCount = Filters.CountPixelByIntensity(gray, IntensityValue);
                    ConvertedImage = gray.ToBitmap().ToBitmapSource();
                    break;
                default:
                    break;
            }

            //Cv2.Circle(dest, new OpenCvSharp.Point(200, 200), 100, new Scalar(0,0,0), 10, LineTypes.Link8, 0);
            _convertedMat = dest;
                
        }

        private void LoadImage(String filePath)
        {
            _originalMat = new Bitmap(filePath).ToMat();

            if (_originalMat.Channels() > 1)
            {
                _originalMat = _originalMat.CvtColor(ColorConversionCodes.BGR2GRAY).Clone();
            }

            _convertedMat = _originalMat.Clone();
            OriginalImage = _originalMat.ToBitmapSource();
            ConvertedImage = _originalMat.ToBitmapSource();
            ImageWidth = _originalMat.Width;
            ImageHeight = _originalMat.Height;
            ResizeValue = 100;

        }

        private void SaveImage(String filePath)
        {
            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(ConvertedImage));
                encoder.Save(fileStream);
            }
        }
        
        #endregion

        #region Events

        private void enableEffect_Checked(object sender, RoutedEventArgs e)
        {
            if (_originalMat == null)
            {
                this.ShowMessageAsync("Error", "You need to load a image first");
                ((CheckBox)sender).IsChecked = false;
                return;
            }
            String effectName = ((CheckBox)sender).Name;
            if (effectName.Equals("Resize"))
            {
                _operationOrder.Insert(0, effectName);
            }
            else
            {
                _operationOrder.Add(effectName);
            }
        }

        private void enableEffect_Unchecked(object sender, RoutedEventArgs e)
        {
            String effectName = ((CheckBox)sender).Name;
            _operationOrder.Remove(effectName);
            ResizeValue = 100;
        }

        private void startHelp(object sender, RoutedEventArgs e)
        {
            showToolTipWindow();
        }

        private void Canny_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if ((bool)CannyFilter.IsChecked)
            {
                int position = _operationOrder.IndexOf("CannyFilter");
                _operationOrder.Remove("CannyFilter");
                _operationOrder.Insert(position, "CannyFilter");
            }
            
        }

        private void ResizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if ((bool)Resize.IsChecked)
            {
                _operationOrder.Remove("Resize");
                _operationOrder.Add("Resize");
            }

        }

        private void UpdateAdaptivethreshold(object sender, RoutedEventArgs e)
        {
            if ((bool)AdaptiveThreshold.IsChecked)
            {
                int position = _operationOrder.IndexOf("AdaptiveThreshold");
                _operationOrder.Remove("AdaptiveThreshold");
                _operationOrder.Insert(position, "AdaptiveThreshold");
            }
            
        }

        private void UpdateIntensityCount(object sender, RoutedEventArgs e)
        {
            if ((bool)CountPixelByIntensity.IsChecked)
            {
                int position = _operationOrder.IndexOf("CountPixelByIntensity");
                _operationOrder.Remove("CountPixelByIntensity");
                _operationOrder.Insert(position, "CountPixelByIntensity");
            }
        }

        private void loadImage_Click(object sender, RoutedEventArgs e)
        {
            var dialogFile = new System.Windows.Forms.OpenFileDialog();
            var imageExtensions = string.Join(";", ImageCodecInfo.GetImageDecoders().Select(ici => ici.FilenameExtension));
            dialogFile.Filter = string.Format("Images|{0}|All Files|*.*", imageExtensions);
            var result = dialogFile.ShowDialog();
                        
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    Loaded_Image = dialogFile.FileName;
                    if (!String.IsNullOrEmpty(Loaded_Image))
                    {
                        LoadImage(Loaded_Image);
                        Top_Main_Window.Title = "OpenCVSharp-GUI   Loaded: " + Loaded_Image;
                    }
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                    break;
                default:
                    break;
            }
        }

        private void saveImage_Click(object sender, RoutedEventArgs e)
        {
            string use=System.IO.Path.GetFileNameWithoutExtension(Loaded_Image);
            use += "_Elab.png";
            Saved_Image = use = System.IO.Path.GetDirectoryName(Loaded_Image) + "\\" + use;
            SaveImage(Saved_Image);
            Top_Main_Window.Title = "OpenCVSharp-GUI   Saved: " + Saved_Image;
        }

        #endregion

        #region Window Events
        private async void showToolTipWindow()
        {
            await this.ShowChildWindowAsync(new ToolTipWindow() { IsModal = true, EnableDropShadow=true}, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }

        public async Task ClearDialogs()
        {
            var oldDialog = await MainWindow.Instance.GetCurrentDialogAsync<BaseMetroDialog>();
            if (oldDialog != null)
                await MainWindow.Instance.HideMetroDialogAsync(oldDialog);
        }

        public async Task OpenDialog(String title, String message)
        {
            await ClearDialogs();

            await this.ShowMessageAsync(title, message);
        }

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }




        #endregion

        
    }
}
