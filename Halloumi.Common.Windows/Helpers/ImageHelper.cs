using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Halloumi.Common.Windows.Helpers
{
    public static class ImageHelper
    {
        #region Public Methods

        /// <summary>
        /// Resizes the specified image, streching or shrink as neccesary
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="size">The new size.</param>
        /// <returns>A new image, with dimesions specified in 'size'</returns>
        public static Image Resize(Image image, Size size)
        {
            var bitmap = new Bitmap(size.Width, size.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, 0, 0, size.Width, size.Height);
                graphics.Dispose();
            }
            return bitmap;
        }

        /// <summary>
        /// Copies the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>A new image that is an cioy of the original</returns>
        public static Image Copy(Image image)
        {
            var bitmap = new Bitmap(image.Width, image.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(image, 0, 0, image.Width, image.Height);
                graphics.Dispose();
            }
            return bitmap;
        }

        /// <summary>
        /// Scales the specified image so it fits within the specified size
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="size">The maximim size of the new image.</param>
        /// <returns>A new image that fits within the dimesions specifed in 'size'</returns>
        public static Image ScaleImageToFit(Image image, Size size)
        {
            var ratioW = (size.Width / (double)image.Width);
            var ratioH = (size.Height / (double)image.Height);

            double ratio = 0;
            if (ratioH < ratioW)
            {
                ratio = ratioH;
            }
            else
            {
                ratio = ratioW;
            }

            var newSize = new Size(0, 0);
            newSize.Width = (int)(image.Width * ratio);
            newSize.Height = (int)(image.Height * ratio);

            return Resize(image, newSize);
        }

        /// <summary>
        /// Scales an image so is the same size or larger than the specified size
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="size">The size.</param>
        /// <returns>A new image that is at least the dimesions specifed in 'size'</returns>
        public static Image ScaleImageToFill(Image image, Size size)
        {
            var ratioW = (size.Width / (double)image.Width);
            var ratioH = (size.Height / (double)image.Height);

            double ratio = 0;
            if (ratioH > ratioW)
            {
                ratio = ratioH;
            }
            else
            {
                ratio = ratioW;
            }

            var newSize = new Size(0, 0);
            newSize.Width = (int)Math.Round(image.Width * ratio, 0);
            newSize.Height = (int)Math.Round(image.Height * ratio, 0);

            return Resize(image, newSize);
        }

        /// <summary>
        /// Scales and crops an image so it fits the specifed size
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="size">The size.</param>
        /// <returns>A new scaled croped image the fits exactly the dimesions specifed in 'size'</returns>
        public static Image ScaleAndCropImageToFit(Image image, Size size)
        {
            using (var resizedImage = ScaleImageToFill(image, size))
            {
                var cropArea = CalculateCenteredCropArea(resizedImage, size);
                return Crop(resizedImage, cropArea);
            }
        }

        /// <summary>
        /// Crops the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="cropArea">The crop area.</param>
        /// <returns></returns>
        public static Image Crop(Image image, Rectangle cropArea)
        {
            if (cropArea.Height > image.Height || cropArea.Height < 0)
            {
                cropArea.Y = 0;
                cropArea.Height = image.Height;
            }
            if (cropArea.Width > image.Width || cropArea.Width < 0)
            {
                cropArea.X = 0;
                cropArea.Width = image.Width;
            }

            using (var bitmap = new Bitmap(image))
            {
                var croppedBitmap = bitmap.Clone(cropArea, image.PixelFormat);
                return croppedBitmap;
            }
        }

        /// <summary>
        /// Calculates the centered crop area for an image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="cropSize">The size of the crop.</param>
        /// <returns>The crop area of the original image</returns>
        private static Rectangle CalculateCenteredCropArea(Image image, Size cropSize)
        {
            var cropArea = new Rectangle(0, 0, cropSize.Width, cropSize.Height);

            if (image.Height != cropSize.Height)
            {
                cropArea.Y = (image.Height - cropSize.Height) / 2;
            }

            if (image.Width != cropSize.Width)
            {
                cropArea.X = (image.Width - cropSize.Width) / 2;
            }

            return cropArea;
        }

        /// <summary>
        /// Perfoms a median filter on the specified image
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="size">The size (pixels squared) of the area to take the median of.</param>
        /// <returns>The filtered median</returns>
        public static Image MedianFilter(Image image, int size)
        {
            var sourceBitmap = new Bitmap(image);
            var medianBitmap = new Bitmap(image.Width, image.Height);

            var medianFastBitmap = new BitmapAccessor(medianBitmap);
            var sourceFastBitmap = new BitmapAccessor(sourceBitmap);

            var apetureMin = -(size / 2);
            var apetureMax = (size / 2);

            var rValues = new List<int>();
            var gValues = new List<int>();
            var bValues = new List<int>();

            Color medianColor;
            Color currentColor;

            for (var x = 0; x < medianBitmap.Width; x++)
            {
                for (var y = 0; y < medianBitmap.Height; y++)
                {
                    for (var currentX = x + apetureMin; currentX < x + apetureMax; currentX++)
                    {
                        if (currentX >= 0 && currentX < medianBitmap.Width)
                        {
                            for (var currentY = y + apetureMin; currentY < y + apetureMax; currentY++)
                            {
                                if (currentY >= 0 && currentY < medianBitmap.Height)
                                {
                                    currentColor = sourceFastBitmap.GetPixel(currentX, currentY);
                                    rValues.Add(currentColor.R);
                                    gValues.Add(currentColor.G);
                                    bValues.Add(currentColor.B);
                                }
                            }
                        }
                    }

                    rValues.Sort();
                    gValues.Sort();
                    bValues.Sort();

                    medianColor = Color.FromArgb(rValues[rValues.Count / 2],
                        gValues[gValues.Count / 2],
                        bValues[bValues.Count / 2]);

                    medianFastBitmap.SetPixel(x, y, medianColor);

                    rValues.Clear();
                    gValues.Clear();
                    bValues.Clear();
                }
            }

            medianFastBitmap.Dispose();
            sourceFastBitmap.Dispose();
            sourceBitmap.Dispose();

            return medianBitmap;
        }

        /// <summary>
        /// Returns a new color that is the averages of two colors.
        /// </summary>
        /// <param name="color1">The first color.</param>
        /// <param name="color2">The second color.</param>
        /// <returns>A new color that is the averages of two colors.</returns>
        public static Color AverageColor(Color color1, Color color2)
        {
            var a = (color1.A + color2.A) / 2;
            var r = (color1.R + color2.R) / 2;
            var g = (color1.G + color2.G) / 2;
            var b = (color1.B + color2.B) / 2;
            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Perfoms a median filter on the specified image
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="size">The size (pixels squared) of the area to take the median of.</param>
        /// <returns>The filtered median</returns>
        public static Image BlurFilter(Image image, int size)
        {
            var sourceBitmap = new Bitmap(image);
            var medianBitmap = new Bitmap(image.Width, image.Height);

            var apetureMin = -(size / 2);
            var apetureMax = (size / 2);

            Color medianColor;
            Color currentColor;
            var rValue = 0;
            var gValue = 0;
            var bValue = 0;
            var pixelCount = 0;

            var medianFastBitmap = new BitmapAccessor(medianBitmap);
            var sourceFastBitmap = new BitmapAccessor(sourceBitmap);

            for (var x = 0; x < medianBitmap.Width; x++)
            {
                for (var y = 0; y < medianBitmap.Height; y++)
                {
                    rValue = 0;
                    gValue = 0;
                    bValue = 0;
                    pixelCount = 0;

                    for (var currentX = x + apetureMin; currentX < x + apetureMax; currentX++)
                    {
                        if (currentX >= 0 && currentX < medianBitmap.Width)
                        {
                            for (var currentY = y + apetureMin; currentY < y + apetureMax; currentY++)
                            {
                                if (currentY >= 0 && currentY < medianBitmap.Height)
                                {
                                    currentColor = sourceFastBitmap.GetPixel(currentX, currentY);
                                    rValue += currentColor.R;
                                    gValue += currentColor.G;
                                    bValue += currentColor.B;
                                    pixelCount++;
                                }
                            }
                        }
                    }

                    medianColor = Color.FromArgb(rValue / pixelCount,
                        gValue / pixelCount,
                        bValue / pixelCount);

                    medianFastBitmap.SetPixel(x, y, medianColor);
                }
            }

            medianFastBitmap.Dispose();
            sourceFastBitmap.Dispose();
            sourceBitmap.Dispose();

            return medianBitmap;
        }

        /// <summary>
        /// Creates a 'glass table top' reflection of an image
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="height">The height of the reflection.</param>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <returns>An image of the reflection. (Just the reflection, not the original image)</returns>
        public static Image GlassTableTopReflection(Image image, int height, Color backgroundColor)
        {
            using (var sourceBitmap = new Bitmap(image))
            using (var destinationBitmap = new Bitmap(image.Width, height))
            using (var graphics = Graphics.FromImage(destinationBitmap))
            {
                // fill with background color
                var bounds = new Rectangle(0, 0, image.Width, height);
                graphics.FillRectangle(new SolidBrush(backgroundColor), bounds);

                // flip source
                sourceBitmap.RotateFlip(RotateFlipType.Rotate180FlipX);

                // draw at 50% transparency
                DrawTransparentImage(graphics, sourceBitmap, 0.5F, 0, 0);

                // fade out
                Brush brush = new LinearGradientBrush(bounds, Color.Transparent, backgroundColor, LinearGradientMode.Vertical);
                graphics.FillRectangle(brush, bounds);

                // blur and return result
                return BlurFilter(destinationBitmap, 2);
            }
        }

        /// <summary>
        /// Saves an image as a jpeg image, with the given quality
        /// </summary>
        /// <param name="path">Path to which the image would be saved.</param>
        /// <param name="image">The image.</param>
        /// <param name="quality">An integer from 0 to 100, with 100 being the
        /// highest quality</param>
        public static void SaveJpg(string path, Image image, int quality)
        {
            image.Save(path, GetJPGCodec(), GetJPGQualityParameters(quality));
        }

        /// <summary>
        /// Saves an image as a jpeg image, with quality value of 95
        /// </summary>
        /// <param name="path">Path to which the image would be saved.</param>
        /// <param name="image">The image.</param>
        public static void SaveJpg(string path, Image image)
        {
            SaveJpg(path, image, 95);
        }

        /// <summary>
        /// Saves an image as a jpeg image, with the given quality
        /// </summary>
        /// <param name="path">Path to which the image would be saved.</param>
        /// <param name="image">The image.</param>
        /// <param name="quality">An integer from 0 to 100, with 100 being the
        /// highest quality</param>
        public static void SaveJpg(Stream stream, Image image, int quality)
        {
            // save the image using the codec and the parameters
            image.Save(stream, GetJPGCodec(), GetJPGQualityParameters(quality));
        }

        /// <summary>
        /// Saves an image as a jpeg image, with quality value of 95
        /// </summary>
        /// <param name="path">Path to which the image would be saved.</param>
        /// <param name="image">The image.</param>
        public static void SaveJpg(Stream stream, Image image)
        {
            SaveJpg(stream, image, 95);
        }

        public static Image DarkenImage(Image image, double darkenAmount)
        {
            var sourceBitmap = new Bitmap(image);
            var darkenBitmap = new Bitmap(image.Width, image.Height);

            var darkenFastBitmap = new BitmapAccessor(darkenBitmap);
            var sourceFastBitmap = new BitmapAccessor(sourceBitmap);

            for (var x = 0; x < darkenBitmap.Width; x++)
            {
                for (var y = 0; y < darkenBitmap.Height; y++)
                {
                    var color = sourceFastBitmap.GetPixel(x, y);
                    darkenFastBitmap.SetPixel(x, y, DarkenColor(color, darkenAmount));
                }
            }

            darkenFastBitmap.Dispose();
            sourceFastBitmap.Dispose();
            sourceBitmap.Dispose();

            return darkenBitmap;
        }

        public static Color DarkenColor(Color inColor, double darkenAmount)
        {
            return Color.FromArgb(
                inColor.A,
                (int)Math.Max(0, inColor.R - 255 * darkenAmount),
                (int)Math.Max(0, inColor.G - 255 * darkenAmount),
                (int)Math.Max(0, inColor.B - 255 * darkenAmount));
        }

        /// <summary>
        /// Gets the JPG quality encoder parameters.
        /// </summary>
        /// <param name="quality">An integer from 0 to 100, with 100 being the
        /// highest quality</param>
        /// <returns>The JPG quality encoder parameters</returns>
        private static EncoderParameters GetJPGQualityParameters(int quality)
        {
            // ensure the quality is within the correct range
            if (quality < 0 || quality > 100)
            {
                var error = string.Format("Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", quality);
                throw new ArgumentOutOfRangeException(error);
            }

            //creatre the quality parameter for the codec
            var encoderParameters = new EncoderParameters(1);
            var qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParameters.Param[0] = qualityParam;

            return encoderParameters;
        }

        /// <summary>
        /// Gets the JPG image codec.
        /// </summary>
        /// <returns>The JPG image codec.</returns>
        private static ImageCodecInfo GetJPGCodec()
        {
            return ImageCodecInfo.GetImageEncoders().Where(e => e.MimeType.ToLower() == "image/jpeg").FirstOrDefault();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Draws an opaque image.
        /// </summary>
        /// <param name="graphics">The graphics to draw to.</param>
        /// <param name="bitmap">The source bitmap.</param>
        /// <param name="opacity">The opacity amount (0 = transparent, 100 = not transparent).</param>
        /// <param name="x">The x position to draw to.</param>
        /// <param name="y">The y position to draw to.</param>
        private static void DrawTransparentImage(Graphics graphics, Bitmap bitmap, float opacity, int x, int y)
        {
            var bounds = new Rectangle(x, y, bitmap.Width, bitmap.Height);

            float[][] matrixItems =
            {
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, opacity, 0},
                new float[] {0, 0, 0, 0, 1}
            };
            var colorMatrix = new ColorMatrix(matrixItems);
            var imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            graphics.DrawImage(bitmap, bounds, 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, imageAttributes);
        }

        #endregion

        #region Internal Classes

        /// <summary>
        /// Class that speeds up read/write access to a bitmap
        /// </summary>
        private class BitmapAccessor : IDisposable
        {
            private Bitmap _bitmap;
            private BitmapData _bitmapData;
            private IntPtr _bitmapPointer;
            private byte[] _rgbValues;
            private bool _modified = false;

            /// <summary>
            /// Initializes a new instance of the BitmapAccessor class.
            /// </summary>
            /// <param name="bitmpap">The bitmpap to access.</param>
            public BitmapAccessor(Bitmap bitmpap)
            {
                _bitmap = bitmpap;

                var bounds = new Rectangle(Point.Empty, _bitmap.Size);

                _bitmapData = _bitmap.LockBits(bounds, ImageLockMode.ReadWrite, _bitmap.PixelFormat);
                _bitmapPointer = _bitmapData.Scan0;

                var bytes = (_bitmap.Width * _bitmap.Height) * 4;
                _rgbValues = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(_bitmapPointer, _rgbValues, 0, _rgbValues.Length);
            }

            /// <summary>
            /// Gets the colour of pixel at the specified location
            /// </summary>
            /// <param name="x">The x position.</param>
            /// <param name="y">The y position.</param>
            /// <returns>The colour of pixel at the specified location</returns>
            public Color GetPixel(int x, int y)
            {
                var index = ((y * _bitmap.Width + x) * 4);
                int b = _rgbValues[index];
                int g = _rgbValues[index + 1];
                int r = _rgbValues[index + 2];
                int a = _rgbValues[index + 3];

                return Color.FromArgb(a, r, g, b);
            }

            /// <summary>
            /// Sets the colour of pixel at the specified location
            /// </summary>
            /// <param name="x">The x position.</param>
            /// <param name="y">The y position.</param>
            /// <param name="colour">The colour.</param>
            public void SetPixel(int x, int y, Color colour)
            {
                var index = ((y * _bitmap.Width + x) * 4);
                _rgbValues[index] = colour.B;
                _rgbValues[index + 1] = colour.G;
                _rgbValues[index + 2] = colour.R;
                _rgbValues[index + 3] = colour.A;

                _modified = true;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (_modified)
                {
                    System.Runtime.InteropServices.Marshal.Copy(_rgbValues, 0, _bitmapPointer, _rgbValues.Length);
                }
                _bitmap.UnlockBits(_bitmapData);

                _bitmap = null;
                _bitmapData = null;
                _rgbValues = null;
            }
        }

        #endregion
    }
}