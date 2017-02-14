using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using OpenCvSharp;
using FlyCapture2Managed;
using System.Linq;
using System.ComponentModel;

namespace MAPS_Alignment
{
    public partial class Form1 : Form
    {
        public void AppendTextBoxX(string valuex)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBoxX), new object[] { valuex });
                return;
            }
            label1.Text = valuex;

        }
        public void AppendTextBoxY(string valuey)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBoxY), new object[] { valuey });
                return;
            }
            label2.Text = valuey;

        }
        public void AppendTextBoxE(string valueE)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBoxE), new object[] { valueE });
                return;
            }
            label3.Text = valueE;

        }

        #region flycam
        static void PrintBuildInfo()
        {
            FC2Version version = ManagedUtilities.libraryVersion;

            StringBuilder newStr = new StringBuilder();
            newStr.AppendFormat(
                "FlyCapture2 library version: {0}.{1}.{2}.{3}\n",
                version.major, version.minor, version.type, version.build);

            Console.WriteLine(newStr);
        }

        static void PrintCameraInfo(CameraInfo camInfo)
        {
            StringBuilder newStr = new StringBuilder();
            newStr.Append("\n*** CAMERA INFORMATION ***\n");
            newStr.AppendFormat("Serial number - {0}\n", camInfo.serialNumber);
            newStr.AppendFormat("Camera model - {0}\n", camInfo.modelName);
            newStr.AppendFormat("Camera vendor - {0}\n", camInfo.vendorName);
            newStr.AppendFormat("Sensor - {0}\n", camInfo.sensorInfo);
            newStr.AppendFormat("Resolution - {0}\n", camInfo.sensorResolution);

            Console.WriteLine(newStr);
        }

        static void PrintFormat7Capabilities(Format7Info fmt7Info)
        {
            StringBuilder newStr = new StringBuilder();
            newStr.AppendFormat("Max image pixels: ({0}, {1})\n", fmt7Info.maxWidth, fmt7Info.maxHeight);
            newStr.AppendFormat("Image Unit size: ({0}, {1})\n", fmt7Info.imageHStepSize, fmt7Info.imageVStepSize);
            newStr.AppendFormat("Offset Unit size: ({0}, {1})", fmt7Info.offsetHStepSize, fmt7Info.offsetVStepSize);

            Console.WriteLine(newStr);
        }
        #endregion
        private Thread _cameraThread;

        public Form1()
        {
            InitializeComponent();
        }
        #region Camera Thread
        private void CaptureCamera()
        {
            _cameraThread = new Thread(new ThreadStart(CaptureCameraCallback));


            _cameraThread.Start();
        }

        private void CaptureCameraCallback()
        {
            //initialise camera here
            #region camsetup
            const Mode k_fmt7Mode = Mode.Mode0;
            const PixelFormat k_fmt7PixelFormat = PixelFormat.PixelFormatMono8;
            ManagedBusManager busMgr = new ManagedBusManager();
            uint numCameras = busMgr.GetNumOfCameras();

            Console.WriteLine("Number of cameras detected: {0}", numCameras);

            ManagedPGRGuid guid = busMgr.GetCameraFromIndex(0);

            ManagedCamera cam = new ManagedCamera();

            cam.Connect(guid);

            // Get the camera information
            CameraInfo camInfo = cam.GetCameraInfo();

            PrintCameraInfo(camInfo);

            // Query for available Format 7 modes
            bool supported = false;
            Format7Info fmt7Info = cam.GetFormat7Info(k_fmt7Mode, ref supported);

            PrintFormat7Capabilities(fmt7Info);

            if ((k_fmt7PixelFormat & (PixelFormat)fmt7Info.pixelFormatBitField) == 0)
            {
                // Pixel format not supported!
                Console.WriteLine("Pixel format is not supported");
                return;
            }

            Format7ImageSettings fmt7ImageSettings = new Format7ImageSettings();
            fmt7ImageSettings.mode = k_fmt7Mode;
            fmt7ImageSettings.offsetX = 1124;
            fmt7ImageSettings.offsetY = 924;
            fmt7ImageSettings.width = 200;
            fmt7ImageSettings.height = 200;
            fmt7ImageSettings.pixelFormat = k_fmt7PixelFormat;
           

            // Validate the settings to make sure that they are valid
            bool settingsValid = false;
            Format7PacketInfo fmt7PacketInfo = cam.ValidateFormat7Settings(
                fmt7ImageSettings,
                ref settingsValid);

            if (settingsValid != true)
            {
                // Settings are not valid
                return;
            }

            // Set the settings to the camera
            cam.SetFormat7Configuration(
               fmt7ImageSettings,
               fmt7PacketInfo.recommendedBytesPerPacket);

            // Get embedded image info from camera
            EmbeddedImageInfo embeddedInfo = cam.GetEmbeddedImageInfo();

            // Enable timestamp collection	
            if (embeddedInfo.timestamp.available == true)
            {
                embeddedInfo.timestamp.onOff = true;
            }

            // Set embedded image info to camera
            cam.SetEmbeddedImageInfo(embeddedInfo);
            
            // Start capturing images
            cam.StartCapture();

            // Retrieve frame rate property
            CameraProperty frmRate = cam.GetProperty(PropertyType.FrameRate);
            
            Console.WriteLine("Frame rate is {0:F2} fps", frmRate.absValue);
            #endregion
            Mat image;
            Mat grey = new Mat();
            int differencex = 0;
            int differencey = 0;
            double diffenceeucl = 0;
            OpenCvSharp.Point centre_im = new OpenCvSharp.Point();

            int centrex = Convert.ToInt16(fmt7ImageSettings.width) / 2;
            int centrey = Convert.ToInt16(fmt7ImageSettings.width) / 2;
            double dp_ = 1;            //inverse ratio of array accumulator to image resolution 
            double minDist_ = 100;    // minimum distance between centre of detected circles
            double param1_ = 100;     // Higher threshold of Canny edge detection 
            double param2_ = 20;    // Accumulator threshold, smaller value leads to higher false detection rates
            int minRad_ = 45;      // minimum radius 
            int maxRad_ = 60;     // maximum radius
            centre_im.X = centrex;

            centre_im.Y = centrey;
            int windowsize = 20;
            int[] centresx = new int[windowsize];
            int[] centresy = new int[windowsize];
            int[] radi = new int[windowsize];
            OpenCvSharp.Point centre = new OpenCvSharp.Point();
            int radius = 1;
            int buffpos = 0;
            int flag = 0;
            ManagedImage rawImage = new ManagedImage();
            ManagedImage convertedImage = new ManagedImage();
            //do repeated actions here 
            while (true)
            {







                //Thread.Sleep(1000);
                cam.RetrieveBuffer(rawImage);

                rawImage.Convert(PixelFormat.PixelFormatBgr, convertedImage);
                System.Drawing.Bitmap bitmap = convertedImage.bitmap;
                image = OpenCvSharp.Extensions.BitmapConverter.ToMat(bitmap);
                Cv2.CvtColor(image, grey, ColorConversionCodes.BGR2GRAY);

                // Inner circle
                CircleSegment[] circles = Cv2.HoughCircles(grey, HoughMethods.Gradient, dp_, minDist_, param1_, param2_, minRad_, maxRad_);
                for (int i = 0; i < circles.Length; i++)
                {

                    flag = 1;
                    

                    centre.X = Convert.ToInt16(Math.Round(circles[0].Center.X));
                    centre.Y = Convert.ToInt16(Math.Round(circles[0].Center.Y));
                    radius = Convert.ToInt16(Math.Round(circles[0].Radius));
                    buffpos = (buffpos + 1) % windowsize;
                    centresx[buffpos] = centre.X;
                    centresy[buffpos] = centre.Y;
                    radi[buffpos] = radius;
                    centre.X = Convert.ToInt16(centresx.Average());
                    centre.Y = Convert.ToInt16(centresy.Average());
                    radius = Convert.ToInt16(radi.Average());
                    differencex = centre.X - centrex;
                    differencey = centre.Y - centrey;
                    diffenceeucl = Math.Round((Math.Sqrt(Math.Pow(differencex, 2) + Math.Pow(differencey, 2))),2);
             }
                if (flag == 1)
                {
                    Cv2.Circle(image, centre, 3, Scalar.Red);
                    Cv2.Circle(image, centre, radius, Scalar.Red, 3);

                }
                Cv2.Circle(image, centre_im, 3, Scalar.DeepSkyBlue);
                Cv2.Circle(image, centre_im, 50, Scalar.DeepSkyBlue, 3);

                string xdiff = differencex.ToString();
                string textxparse = "X Offset: " + xdiff + " [pixels]";
                string ydiff = differencey.ToString();
                string textyparse = "Y Offset: " + ydiff + " [pixels]";
                string eucl = diffenceeucl.ToString();
                string texteucle = "Eucl. Dist: " + eucl + " [pixels]";
                AppendTextBoxX(textxparse);
                AppendTextBoxE(texteucle);

                AppendTextBoxY(textyparse);
                Bitmap bm = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
                bm.SetResolution(flydisp.Width, flydisp.Height);
                flydisp.Image = bm;


            }
        }



        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Start_Stop_Click(object sender, EventArgs e)
        {
            if (Start_Stop.Text.Equals("Start"))
            {
                CaptureCamera();
                Start_Stop.Text = "Stop";
            }
            else
            {

                _cameraThread.Abort();
                Start_Stop.Text = "Start";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
