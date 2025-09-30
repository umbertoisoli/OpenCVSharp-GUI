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

        public static void Full_Edge(Mat gray, ref Mat enhancedImage)
        {
            //Copia Immagine
            enhancedImage = gray.Clone();
            Mat robertImage = new Mat();
            robertImage= gray.Clone();
            Mat dst = new Mat();
            dst= gray.Clone();

            sbyte[] kernelValues = { +1,  0, +1,
                                      0,  0,  0,
                                     -1,  0, -1 };

            Mat kernelx = new Mat(3, 3, MatType.CV_8SC1, kernelValues);


            Mat kernelw = new Mat(3, 3, MatType.CV_8SC1, new Scalar(0));
            kernelw.Set<sbyte>(0, 0, 0);
            kernelw.Set<sbyte>(0, 1, 0);
            kernelw.Set<sbyte>(0, 2, 0);

            //Utility
            //Riempie tutta l'immagine con valore 0
            //enhancedImage.SetTo(Scalar.All(0));

            //Copia Immagine
            //enhancedImage = dst.Clone();

            Cv2.Normalize(gray, enhancedImage, 0, 255, NormTypes.MinMax);

            //gray.ConvertTo(gray, MatType.CV_32FC1,1,-10); // or CV_32F works too
            //Cv2.Log(gray, enhancedImage);
            //enhancedImage.ConvertTo(enhancedImage, MatType.CV_8UC1,46,-10); //-50 or CV_32F works too

            //Applica un Roberts per edge obliqui e somma un sobel x edge X-Y
            //Cv2.Filter2D(gray, robertImage, MatType.CV_8UC1, kernelx);
            //Cv2.Sobel(gray,dst, MatType.CV_8UC1,1,1,3,1,0, BorderTypes.Default);
            //Cv2.BitwiseOr(dst, robertImage, enhancedImage, null);

            //Applica un filtro laplaciano con offset 128
            //Cv2.Laplacian(gray, enhancedImage, MatType.CV_8UC1, 3, 1, 128);




            //Cv2.Scharr(gray,enhancedImage, MatType.CV_8UC1,0,1,1,128);
            //Cv2.Threshold(enhancedImage, enhancedImage, 48, 255, ThresholdTypes.Binary);
            return;

            //Nuovo




            //Cv2.EqualizeHist(gray, enhancedImage);

            //Point centro = new Point(-1, -1);
            //Cv2.Filter2D(gray, enhancedImage,-1, kernel, centro,0,BorderTypes.Default);
        }


        public static void EdgeEnhancement(Mat gray, ref Mat enhancedImage)
        {
            //Copia Immagine
            enhancedImage = gray.Clone();
            Mat dst = new Mat();

            sbyte[] kernelValues = { -1, -1, -1,
                                     -1,  8, -1,
                                     -1, -1, -1 };

            Mat kernelx = new Mat(3, 3, MatType.CV_8SC1, kernelValues);


            Mat kernelw = new Mat(3, 3, MatType.CV_8SC1, new Scalar(0));
            kernelw.Set<sbyte>(0,0,  0);
            kernelw.Set<sbyte>(0,1,  0);
            kernelw.Set<sbyte>(0,2,  0);


            //Cv2.Normalize(gray, enhancedImage, 0, 255, NormTypes.MinMax);

            //gray.ConvertTo(gray, MatType.CV_32FC1,1,-10); // or CV_32F works too
            //Cv2.Log(gray, enhancedImage);
            //enhancedImage.ConvertTo(enhancedImage, MatType.CV_8UC1,46,-10); //-50 or CV_32F works too

            //Cv2.Filter2D(gray, enhancedImage, MatType.CV_8UC1, kernelx);
            Cv2.Laplacian(gray, enhancedImage, MatType.CV_8UC1,3,1,128);

            //Copia Immagine
            //enhancedImage = gray.Clone();


            //Cv2.Scharr(gray,enhancedImage, MatType.CV_8UC1,0,1,1,128);
            //Cv2.Threshold(enhancedImage, enhancedImage, 48, 255, ThresholdTypes.Binary);
            return;

            //Nuovo




            //Cv2.EqualizeHist(gray, enhancedImage);

            //Point centro = new Point(-1, -1);
            //Cv2.Filter2D(gray, enhancedImage,-1, kernel, centro,0,BorderTypes.Default);
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
            //Cv2.BitwiseNot(gray, inverted);


            //Quantize
            //https://github.com/shimat/opencvsharp/wiki/Accessing-Pixel

            inverted = gray.Clone();

            MatOfByte3 mat3 = new MatOfByte3(gray);
            var indexer = mat3.GetIndexer();

            MatOfByte3 mat4 = new MatOfByte3(inverted);
            var indexer2 = mat4.GetIndexer();



            for (int y = 0; y < gray.Height; y++)
            {
                for (int x = 0; x < gray.Width; x++)
                {
                    byte max_pixel = 0;
                    byte min_pixel = 255;


                    Vec3b color = indexer[y, x];
                    byte uno = color.Item0;
                    if (uno > max_pixel) max_pixel = uno;
                    if (uno < min_pixel) min_pixel = uno;

                    byte due = color.Item1;
                    if (due > max_pixel) max_pixel = due;
                    if (due < min_pixel) min_pixel = due;

                    byte delta = (byte)(max_pixel - min_pixel);
                    byte media = (byte)((max_pixel + min_pixel) >> 1);

                    if (delta < 40 && delta >= 4)
                    {
                        Vec3b color1 = indexer2[y, x];
                        color.Item0 = media;
                        color.Item1 = media;
                        indexer2[y, x]=color;
                        Vec3b color2 = indexer2[y, x];
                    }
                }
            }
        }

        public static int CountPixelByIntensity(Mat gray, int pixelValue)
        {
            //https://github.com/shimat/opencvsharp/wiki/Accessing-Pixel
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
