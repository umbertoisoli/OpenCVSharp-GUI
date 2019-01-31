using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVSharp_GUI
{
    public sealed class Filters
    {
        private static readonly Lazy<Filters> lazy = new Lazy<Filters>(() => new Filters());
        
        public static Filters Instance { get { return lazy.Value; } }

        public static void ErodeImage(Mat gray, ref Mat eroded)
        {
            //Cv.Erode(gray, eroded);
            byte[] kernelValues = { 0, 1, 0, 1, 1, 1, 0, 1, 0 }; // cross (+)
            Mat kernel = new Mat(3, 3, MatType.CV_8UC1, kernelValues);
            Cv2.Erode(gray, eroded, kernel, new Point(-1, -1), 1, BorderTypes.Default);
        }

        public static void DilateImage(Mat gray, ref Mat dilated)
        {
            //Cv.Dilate(gray, dilated);
            byte[] kernelValues = { 0, 1, 0, 1, 1, 1, 0, 1, 0 }; // cross (+)
            Mat kernel = new Mat(3, 3, MatType.CV_8UC1, kernelValues);
            Cv2.Dilate(gray,dilated,kernel,new Point(-1,-1),1,BorderTypes.Default);
        }

        public static void HistogramEqualize(Mat gray, ref Mat equalized)
        {
            Cv2.EqualizeHist(gray, equalized);
        }

        public static void SetAdaptTreshold(Mat gray, ref Mat threshold, AdaptiveThresholdTypes aptTValue, ThresholdTypes thsType, double ThresholdValue, int AdaptativeVal1, int AdaptativeVal2)
        {
            try
            {
                //gray.AdaptiveThreshold(threshold, ThresholdValue, aptTValue, thsType, AdaptativeVal1, AdaptativeVal1);
                threshold = gray.AdaptiveThreshold(128,aptTValue,thsType,AdaptativeVal1,AdaptativeVal2);
            }
            catch (Exception e)
            {
                MainWindow.Instance.OpenDialog("Invalid value combination", e.Message);
            }
            
        }

        public static void EdgeEnhancement(Mat gray, ref Mat enhancedImage)
        {
            float[] data = { -1, -1, -1, -1, -1,  -1, 2, 2, 2, -1,
                             -1,  2,  8,  2, -1,  -1, 2, 2, 2, -1, -1,-1,-1,-1,-1
                    };
            OpenCvSharp.InputArray datix = OpenCvSharp.InputArray.Create(data);

            //CvMat kernel = new CvMat(5, 5, MatrixType.U8C1, data);
            //Cv2.Normalize(kernel, kernel, 8, 0,NormTypes.L1);
            //Cv2.Filter2D(gray, enhancedImage, kernel);


            //Nuovo
            float[] kernelValues = { -1, -1, -1, -1, -1,
                                     -1,  2,  2,  2, -1,
                                     -1,  2,  8,  2, -1,
                                     -1,  2,  2,  2, -1,
                                     -1, -1, -1, -1, -1  };

            Mat kernel = new Mat(5, 5, MatType.CV_8UC1, kernelValues);


            Cv2.EqualizeHist(gray, enhancedImage);
            Point centro = new Point(-1, -1);
            Cv2.Filter2D(gray, enhancedImage,-1, kernel, centro,0,BorderTypes.Default);
        }

        public static void CannyFilter(Mat gray, ref Mat canny, int value1, int value2)
        {
            Cv2.Canny(gray, canny, value1, value2);
        }

        //private static void UpdateCanny()
        //{
        //    if (operationOrder.Count > 0)
        //    {
        //        List<String> temp = new List<string>();
        //        temp = operationOrder.Take(operationOrder.Count).ToList();
        //        operationOrder.Clear();
        //        operationOrder = new ObservableCollection<String>(temp.Take(temp.Count - 1).ToList());
        //        operationOrder.CollectionChanged += OperationOrder_CollectionChanged;
        //        operationOrder.Add(temp.Last());
        //    }
        //}

        public static void Denoiser(Mat gray, ref Mat denoised)
        {
            Cv2.FastNlMeansDenoising(gray, denoised);
        }

        public static void ScaleImage(Mat gray, ref Mat scalled, double ResizeValue)
        {
            double RValue = ResizeValue/100;
            int width = (int)(gray.Width * (RValue / 100));
            int height = (int)(gray.Height * (RValue / 100));
            //scalled = new IplImage(new CvSize(width, height), BitDepth.U8, 1);
            //scalled = new Mat(height, width, MatType.CV_8UC1);  // (new Mat. CvSize(width, height), BitDepth.U8, 1);
            //gray.Resize(scalled, Interpolation.Linear);
            Size to_resize = new Size(0, 0);
            Cv2.Resize(gray, scalled, to_resize, RValue, RValue, InterpolationFlags.Linear);
        }

        public static void InvertImage(Mat gray, ref Mat inverted)
        {
            Cv2.BitwiseNot(gray, inverted);
        }

        public static int CountPixelByIntensity(Mat gray, int pixelValue)
        {
            MatOfByte3 mat3 = new MatOfByte3(gray);
            var indexer = mat3.GetIndexer();

            int i = 0;
            for (int y = 0; y < gray.Height; y++)
            {
                for (int x = 0; x < gray.Width; x++)
                {
                    Vec3b color = indexer[y, x];
                    //if (m.At<Vec3b>(y, x).Item0 >= pixelValue)
                    if (color.Item0 > pixelValue)
                    {
                        i++;
                    }
                }
            }
            return i;
        }
    }
}
