using System;
using System.Drawing;
using System.IO;

namespace MatroxLDS
{
    /// <summary>
    /// Provides utility functions to the client.
    /// </summary>
    public static class ClientUtils
    {
        /// <summary>
        /// Converts a Bitmap to a MemoryStream for display in Windows Forms.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static MemoryStream GetImageStream(Bitmap bitmap)
        {
            if (bitmap == null)
                return null;

            var stream = new MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Converts byte array to Bitmap.
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <returns></returns>
        public static Bitmap BytesToBitmap(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                return null;

            using (var ms = new MemoryStream(imageBytes))
            {
                return new Bitmap(ms);
            }
        }
    }
}