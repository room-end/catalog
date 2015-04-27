namespace catalog.viewer
{
    using BitMiracle.LibTiff.Classic;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    /// <seealso cref="https://bitmiracle.github.io/libtiff.net"/>
    internal static class TIFF
    {
        internal static void Resize(string srcpath, string dstpath, string filename, int width, int height)
        {
            if (Path.GetFileName(filename) != filename)
            {
                throw new InvalidOperationException("'filename' is invalid!");
            }
            string combined_src = Path.Combine(srcpath, filename);
            string filename_only = Path.GetFileNameWithoutExtension(filename);
            string combined_dst = Path.Combine(dstpath, filename_only + ".jpeg");
            using (Tiff image = Tiff.Open(combined_src, "r"))
            {
                if (image == null)
                {
                    throw new InvalidOperationException("TIFF File not found");
                }

                //FieldValue[] exifIFDTag = image.GetField(TiffTag.EXIFIFD);
                //if (exifIFDTag == null)
                //{
                //    throw new InvalidOperationException("Exif metadata not found");
                //}

                //int exifIFDOffset = exifIFDTag[0].ToInt();
                //if (!image.ReadEXIFDirectory(exifIFDOffset))
                //{
                //    throw new InvalidOperationException("Could not read EXIF IFD");
                //}

                //image.SetDirectory(0);
                FieldValue[] value = image.GetField(TiffTag.IMAGEDESCRIPTION);
                value = image.GetField(TiffTag.IMAGEWIDTH);
                int _width = value[0].ToInt();

                value = image.GetField(TiffTag.IMAGELENGTH);
                int _height = value[0].ToInt();

                // Read the image into the memory buffer 
                int[] raster = new int[_height * _width];
                if (!image.ReadRGBAImage(_width, _height, raster))
                {
                    throw new InvalidOperationException("Could not read Image");
                }
                using (Bitmap bmp = new Bitmap(_width, _height, PixelFormat.Format24bppRgb))
                {
                    Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

                    BitmapData bmpdata = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    byte[] bits = new byte[bmpdata.Stride * bmpdata.Height];

                    for (int y = 0; y < bmp.Height; y++)
                    {
                        int rasterOffset = y * bmp.Width;
                        int bitsOffset = (bmp.Height - y - 1) * bmpdata.Stride;

                        for (int x = 0; x < bmp.Width; x++)
                        {
                            int rgba = raster[rasterOffset++];
                            bits[bitsOffset++] = (byte)((rgba >> 16) & 0xff);
                            bits[bitsOffset++] = (byte)((rgba >> 8) & 0xff);
                            bits[bitsOffset++] = (byte)(rgba & 0xff);
                        }
                    }

                    System.Runtime.InteropServices.Marshal.Copy(bits, 0, bmpdata.Scan0, bits.Length);
                    bmp.UnlockBits(bmpdata);

                    int newWidth, newHeight;

                    if (bmp.Width > bmp.Height)
                    {
                        newWidth = width;
                        newHeight = (int)(height * bmp.Height / bmp.Width);
                    }
                    else
                    {
                        newHeight = height;
                        newWidth = (int)(width * bmp.Width / bmp.Height);
                    }

                    Rectangle destRect = new Rectangle(0, 0, newWidth, newHeight);
                    Bitmap destImage = new Bitmap(newWidth, newHeight);

                    destImage.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);
                    using (var graphics = Graphics.FromImage(destImage))
                    {
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                        using (var wrapMode = new ImageAttributes())
                        {
                            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                            graphics.DrawImage(bmp, destRect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, wrapMode);
                        }
                    }
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);

                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    destImage.Save(combined_dst, GetEncoder(ImageFormat.Jpeg), myEncoderParameters);

                }
            }
        }

        internal static string description(string srcpath, string filename)
        {
            if (Path.GetFileName(filename) != filename)
            {
                throw new InvalidOperationException("'filename' is invalid!");
            }
            string combined = Path.Combine(srcpath, filename);
            using (Tiff image = Tiff.Open(combined, "r"))
            {
                if (image == null)
                {
                    throw new InvalidOperationException("TIFF File not found");
                }
                FieldValue[] value = image.GetField(TiffTag.IMAGEDESCRIPTION);
                return (value != null) ? value[0].ToString() : string.Empty;
            }
        }

        internal static string copyright(string srcpath, string filename)
        {
            if (Path.GetFileName(filename) != filename)
            {
                throw new InvalidOperationException("'filename' is invalid!");
            }
            string combined = Path.Combine(srcpath, filename);
            using (Tiff image = Tiff.Open(combined, "r"))
            {
                if (image == null)
                {
                    throw new InvalidOperationException("TIFF File not found");
                }
                FieldValue[] value = image.GetField(TiffTag.COPYRIGHT);
                return (value != null) ? value[0].ToString() : string.Empty;
            }
        }

        internal static EXIF exif(string srcpath, string filename)
        {
            if (Path.GetFileName(filename) != filename)
            {
                throw new InvalidOperationException("'filename' is invalid!");
            }
            string combined = Path.Combine(srcpath, filename);
            using (Tiff image = Tiff.Open(combined, "r"))
            {
                if (image == null)
                {
                    throw new InvalidOperationException("TIFF File not found");
                }
                FieldValue[] _copyright = image.GetField(TiffTag.COPYRIGHT);
                FieldValue[] _description = image.GetField(TiffTag.IMAGEDESCRIPTION);
                FieldValue[] _height = image.GetField(TiffTag.IMAGELENGTH);
                FieldValue[] _photometric = image.GetField(TiffTag.PHOTOMETRIC);
                FieldValue[] _resolutionX = image.GetField(TiffTag.XRESOLUTION);
                FieldValue[] _resolutionY = image.GetField(TiffTag.YRESOLUTION);
                FieldValue[] _unit = image.GetField(TiffTag.RESOLUTIONUNIT);
                FieldValue[] _width = image.GetField(TiffTag.IMAGEWIDTH);
                FieldValue[] _software = image.GetField(TiffTag.SOFTWARE);
                FieldValue[] _make = image.GetField(TiffTag.MAKE);
                FieldValue[] _model = image.GetField(TiffTag.MODEL);
                EXIF _exif = new EXIF()
                {
                    copyright = (_copyright != null) ? _copyright[0].ToString() : string.Empty,
                    description = (_description != null) ? _description[0].ToString() : string.Empty,
                    height = (_height != null) ? _height[0].ToInt() : 0,
                    photometric = (_photometric != null) ? (Photometric)_photometric[0].ToInt() : Photometric.RGB,
                    resolutionX = (_resolutionX != null) ? _resolutionX[0].ToFloat() : 0,
                    resolutionY = (_resolutionY != null) ? _resolutionY[0].ToFloat() : 0,
                    unit = (_unit != null) ? (ResUnit)_unit[0].ToInt() : ResUnit.NONE,
                    width = (_width != null) ? _width[0].ToInt() : 0,
                    software = (_software != null) ? _software[0].ToString() : string.Empty,
                    make = (_make != null) ? _make[0].ToString() : string.Empty,
                    model = (_model != null) ? _model[0].ToString() : string.Empty,
                };
                return _exif;
            }
        }

        
        internal static string all_EXIF(string srcpath, string filename)
        {
            if (Path.GetFileName(filename) != filename)
            {
                throw new InvalidOperationException("'filename' is invalid!");
            }
            string combined = Path.Combine(srcpath, filename);
            StringBuilder soba = new StringBuilder();
            using (Tiff image = Tiff.Open(combined, "r"))
            {
                if (image == null)
                {
                    throw new InvalidOperationException("TIFF File not found");
                }
                for (ushort t = ushort.MinValue; t < ushort.MaxValue; ++t)
                {
                    TiffTag tag = (TiffTag)t;
                    FieldValue[] value = image.GetField(tag);
                    if (value != null)
                    {
                        for (int j = 0; j < value.Length; j++)
                        {
                            soba.AppendFormat("{0}\n", tag.ToString());
                            soba.AppendFormat("{0} : {1}\n", value[j].Value.GetType().ToString(), value[j].ToString());
                        }

                    }
                }
            }
            return soba.ToString();
        }


        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}