using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;

namespace SquareCalc
{
    public partial class frmMain : Form
    {
        Point m_CircleCenter;
        int m_CircleRadius;
        Point[] m_Pnts;
        const int m_MaxNumOfPoints = 10000;
        Thread m_UpdateThread;

        public frmMain()
        {
            InitializeComponent();
            LoadSettingsFile("input.txt");
        }


        /// <summary>
        /// This method loads data from a text file named "input.txt"
        /// in this file the folowing data is supplied each in a new line
        /// 1. Circle center X coordinate
        /// 2. Circle center Y coordinate
        /// 3. Circle radius
        /// each one is an double number
        /// </summary>
        /// <param name="sPathOfInputFile">Path of the input file</param>
        /// <returns></returns>
        private bool LoadSettingsFile(string sPathOfInputFile)
        {
            bool bResult = false;
            //Todo: implement this function

            return bResult;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Draw the circle in the coordinates read from file on the picturebox
            DrawCircle();
            m_Pnts = GenerateRandomPoints();
            DrawPoints(m_Pnts);
            // This method should count how many points are inside the circle

            int InsidePoints = FindIfInside();
            if (!CreateBitmap(m_Pnts))
            {
                throw new Exception("File not created");
            }

            StartUpdateInNewThread(InsidePoints);
        }

        private void StartUpdateInNewThread(int insidePoints)
        {
            m_UpdateThread = new Thread(new ParameterizedThreadStart(UpdateData));
            m_UpdateThread.Start(insidePoints);
        }


        private void DrawCircle()
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            Pen pen = new Pen(Color.Red);
            Rectangle rect = new Rectangle(m_CircleCenter.X - m_CircleRadius, m_CircleCenter.Y - m_CircleRadius,
                m_CircleRadius * 2, m_CircleRadius * 2);
            using (Graphics grphx = Graphics.FromImage(bmp))
            {
                grphx.DrawEllipse(pen, rect);
                pictureBox1.Image = bmp;
            }
        }

        /// <summary>
        /// Update label with the result
        /// </summary>
        /// <param name="numOfInsidePoints">Muber of points inside the circle</param>
        private void UpdateData(object numOfInsidePoints)
        {
            double dNumOfPoints = (double) ((int) numOfInsidePoints);
            double result = (dNumOfPoints * pictureBox1.Width * pictureBox1.Height) /
                            (m_Pnts.Length * m_CircleRadius * m_CircleRadius);
            label1.Text = result.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Find how many points are inside the circle
        /// </summary>
        /// <returns>the number of points inside the circle</returns>
        private int FindIfInside()
        {
            int inside = 0;
            //Todo: implement this function

            return inside;
        }

        /// <summary>
        /// Draw all the generated points onto the picturebox
        /// </summary>
        /// <param name="pnts">array of Point</param>
        private void DrawPoints(Point[] pnts)
        {
            using (Graphics grphx = Graphics.FromImage(pictureBox1.Image))
            {
                SolidBrush brush = new SolidBrush(Color.Black);
                for (int ii = 0; ii < m_MaxNumOfPoints; ii++)
                {
                    grphx.FillRectangle(brush, pnts[ii].X, pnts[ii].Y, 1, 1);
                }
            }
        }


        /// <summary>
        /// Function that creates a bitmap file of the point distribution
        /// </summary>
        /// <param name="pnts">Points</param>
        /// <returns>success status of creating bitmap file</returns>
        private bool CreateBitmap(Point[] pnts)
        {
            int width = pictureBox1.Width, height = pictureBox1.Height;

            bool bResult = false;
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            Stopwatch sw = Stopwatch.StartNew();

            #region Create bitmapfile here

            try
            {
                BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                    PixelFormat.Format24bppRgb);

                byte[] byImage = new byte[width*3*height];
             
                for (int i = 0; i < pnts.Length; i++)
                {
                    int x = pnts[i].X;
                    int y = pnts[i].Y;
                    byImage[y*width*3+(3*x-1)] = byte.MaxValue;
                }
                Marshal.Copy(byImage, 0, data.Scan0, byImage.Length);
                
                bmp.UnlockBits(data);
                bmp.Save("result.jpg");
                bResult = true;
            }
            catch (Exception e)
            {
                bResult = false;
            }

            #endregion Create bitmapfile here

            return bResult;
        }


        /// <summary>
        /// Generates random points in the range of picturebox
        /// </summary>
        /// <returns>array of random points</returns>
        private Point[] GenerateRandomPoints()
        {
            Point[] pnts = new Point[m_MaxNumOfPoints];
            Random rnd = new Random(42);
            for (int ii = 0; ii < m_MaxNumOfPoints; ii++)
            {
                pnts[ii].X = rnd.Next(pictureBox1.Width);
                pnts[ii].Y = rnd.Next(pictureBox1.Height);
            }

            return pnts;
        }
    }
}