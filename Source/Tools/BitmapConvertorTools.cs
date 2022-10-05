using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace BookCat
{
    class BitmapConvertorTools
    {
        // Преобразование BitmapSource в byte[] и обратно для сохранения в БД (взято в MSDN social)
        public static byte[] BitmapSourceToByte(System.Windows.Media.Imaging.BitmapSource source)
        {
            var encoder = new System.Windows.Media.Imaging.JpegBitmapEncoder();
            var frame = System.Windows.Media.Imaging.BitmapFrame.Create(source);
            encoder.Frames.Add(frame);
            var stream = new MemoryStream();

            encoder.Save(stream);
            return stream.ToArray();
        }
        public static byte[] UriJpegToByte(Uri uri) 
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = uri;
            bi.DecodePixelWidth = 200;
            bi.EndInit();
            return BitmapSourceToByte((BitmapSource)bi);
        }
        public static System.Windows.Media.Imaging.BitmapSource ByteToBitmapSource(byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            return System.Windows.Media.Imaging.BitmapFrame.Create(stream);
        }
    }
}
