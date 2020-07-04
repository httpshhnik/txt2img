using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.IO;

namespace ImageCreator
{
    static class Program
    {

        public static void Convert(string file) {
            try
            {
                //Program.log(file);
                var text = System.IO.File.ReadAllText(file, Encoding.Default);
                if (text=="") return;
                if (text == null) text = string.Empty;
                
                string directory = Path.GetDirectoryName(file);
               
                string targetDirectory = directory.ToString() + "/" + "img";
                string targetDirectory2 = directory.ToString() + "/" + "txt";
                if (!Directory.Exists(targetDirectory))
                    Directory.CreateDirectory(targetDirectory);
                if (!Directory.Exists(targetDirectory2))
                    Directory.CreateDirectory(targetDirectory2);
                string fname = Path.GetFileName(file);//Program.log(fname);
                var image = CreateBitmapImage(text);

                image.Save(targetDirectory+"/"+fname.Replace(".txt", ".png").Replace(".TXT",".png"), ImageFormat.Png);
                try { File.Copy(file, targetDirectory2 + "/" + fname); } catch { }
                try { File.Delete(file); }catch { }

            }
            catch(Exception ex) {
                Program.log(ex.Message,"Error while reading file."+file);
            }
        }

        public static void log(string a, string b="") {
            try {
                File.AppendAllText("errors.log",DateTime.Now.ToString()+" "+b+"\n"+a+"\n\n");
            } catch { }
        }

        private static Bitmap CreateBitmapImage(string sImageText)
        {
            try
            {
                Bitmap objBmpImage = new Bitmap(1, 1);

                int intWidth = 0;
                int intHeight = 0;

                // Create the Font object for the image text drawing.
                Font objFont = new Font("Arial", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

                // Create a graphics object to measure the text's width and height.
                Graphics objGraphics = Graphics.FromImage(objBmpImage);

                // This is where the bitmap size is determined.
                intWidth = (int)objGraphics.MeasureString(sImageText, objFont).Width;
                intHeight = (int)objGraphics.MeasureString(sImageText, objFont).Height;

                // Create the bmpImage again with the correct size for the text and font.
                objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));

                // Add the colors to the new bitmap.
                objGraphics = Graphics.FromImage(objBmpImage);

                // Set Background color
                objGraphics.Clear(Color.White);
                //objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                //objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                objGraphics.DrawString(sImageText, objFont, new SolidBrush(Color.FromArgb(102, 102, 102)), 0, 0);
                objGraphics.Flush();
                return (objBmpImage);
            }
            catch {
                Program.log("Bitmap generation error");
                return null;
            }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
