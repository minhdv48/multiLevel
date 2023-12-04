using System.Text;
using System.Security.Cryptography;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Multi_level.Helper
{
    public static class Utility
    {
        public static string Sha256Hash(string text)
        {
            using (var sha256 = new SHA256Managed())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "");
            }
        }
        public static DateTime FirstDayOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        public static int DaysInMonth(this DateTime value)
        {
            return DateTime.DaysInMonth(value.Year, value.Month);
        }

        public static DateTime LastDayOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.DaysInMonth());
        }
        public static decimal Round(decimal d, int decimals)
        {
            if (decimals >= 0) return decimal.Round(d, decimals);

            decimal n = (decimal)Math.Pow(10, -decimals);
            return decimal.Round(d / n, 0) * n;
        }
        const string Letters = "012346789ABCDEFGHiJKLMNOPQRTUVWXYZ-";
        public static string GenerateCode()
        {
            Random rand = new Random();
            int maxRand = Letters.Length - 1;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                int index = rand.Next(maxRand);
                sb.Append(Letters[index]);
            }
            return sb.ToString();
        }
    }
    public static class Captcha
    {
        const string Letters = "2346789ABCDEFGHJKLMNPRTUVWXYZ";
        public static string GenerateCaptchaCode()
        {
            Random rand = new Random();
            int maxRand = Letters.Length - 1;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                int index = rand.Next(maxRand);
                sb.Append(Letters[index]);
            }

            return sb.ToString();
        }
        public static bool ValidateCaptchaCode(string userInputCaptcha, HttpContext context)
        {
            var isValid = userInputCaptcha.ToLower() == context.Session.GetString("CaptchaCode").ToLower();
            context.Session.Remove("CaptchaCode");
            return isValid;
        }

        public static CaptchaResult GenerateCaptchaImage(int width, int height, string captchaCode)
        {
            using (Bitmap baseMap = new Bitmap(width, height))
            {
                using (Graphics graph = Graphics.FromImage(baseMap))
                {
                    Random rand = new Random();

                    graph.Clear(GetRandomLightColor());

                    DrawCaptchaCode();
                    AdjustRippleEffect();

                    MemoryStream ms = new MemoryStream();

                    baseMap.Save(ms, ImageFormat.Png);

                    return new CaptchaResult { CaptchaCode = captchaCode, CaptchaByteData = ms.ToArray(), Timestamp = DateTime.Now };

                    int GetFontSize(int imageWidth, int captchCodeCount)
                    {
                        var averageSize = imageWidth / captchCodeCount;

                        return Convert.ToInt32(averageSize);
                    }

                    Color GetRandomDeepColor()
                    {
                        int redlow = 160, greenLow = 100, blueLow = 160;
                        return Color.FromArgb(rand.Next(redlow), rand.Next(greenLow), rand.Next(blueLow));
                    }

                    Color GetRandomLightColor()
                    {
                        int low = 180, high = 255;

                        int nRend = rand.Next(high) % (high - low) + low;
                        int nGreen = rand.Next(high) % (high - low) + low;
                        int nBlue = rand.Next(high) % (high - low) + low;

                        return Color.FromArgb(nRend, nGreen, nBlue);
                    }
                    void DrawCaptchaCode()
                    {
                        SolidBrush fontBrush = new SolidBrush(Color.Black);
                        int fontSize = GetFontSize(width, captchaCode.Length);
                        Font font = new Font(FontFamily.GenericSerif, fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
                        for (int i = 0; i < captchaCode.Length; i++)
                        {
                            fontBrush.Color = GetRandomDeepColor();

                            int shiftPx = fontSize / 6;

                            float x = i * fontSize + rand.Next(-shiftPx, shiftPx) + rand.Next(-shiftPx, shiftPx);
                            int maxY = height - fontSize;
                            if (maxY < 0) maxY = 0;
                            float y = rand.Next(0, maxY);

                            graph.DrawString(captchaCode[i].ToString(), font, fontBrush, x, y);
                        }
                    }
                    void AdjustRippleEffect()
                    {
                        short nWave = 6;
                        int nWidth = baseMap.Width;
                        int nHeight = baseMap.Height;

                        Point[,] pt = new Point[nWidth, nHeight];

                        for (int x = 0; x < nWidth; ++x)
                        {
                            for (int y = 0; y < nHeight; ++y)
                            {
                                var xo = nWave * Math.Sin(2.0 * 3.1415 * y / 128.0);
                                var yo = nWave * Math.Cos(2.0 * 3.1415 * x / 128.0);

                                var newX = x + xo;
                                var newY = y + yo;

                                if (newX > 0 && newX < nWidth)
                                {
                                    pt[x, y].X = (int)newX;
                                }
                                else
                                {
                                    pt[x, y].X = 0;
                                }


                                if (newY > 0 && newY < nHeight)
                                {
                                    pt[x, y].Y = (int)newY;
                                }
                                else
                                {
                                    pt[x, y].Y = 0;
                                }
                            }
                        }

                        Bitmap bSrc = (Bitmap)baseMap.Clone();

                        BitmapData bitmapData = baseMap.LockBits(new Rectangle(0, 0, baseMap.Width, baseMap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                        BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                        int scanline = bitmapData.Stride;

                        IntPtr scan0 = bitmapData.Scan0;
                        IntPtr srcScan0 = bmSrc.Scan0;
                        baseMap.UnlockBits(bitmapData);
                        bSrc.UnlockBits(bmSrc);
                        bSrc.Dispose();
                    }
                }
            }

        }
    }
    public static class ExtendMethod
    {
        public static string ToMonth(this int input)
        {
            try
            {
                string[] months = {"Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5",
    "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"};
                return months[input].ToString();
            }
            catch (Exception)
            {
                return input.ToString();
            }
        }
        public static string ToDay(this DateTime input)
        {
            var day = input.DayOfWeek.ToString();
            string retval = day;
            try
            {
                switch (day.ToLower())
                {
                    case "monday":
                        retval = "Thứ hai";
                        break;
                    case "tuesday":
                        retval = "Thứ ba";
                        break;
                    case "wednesday":
                        retval = "Thứ thư";
                        break;
                    case "thursday":
                        retval = "Thứ năm";
                        break;
                    case "friday":
                        retval = "Thứ sáu";
                        break;
                    case "saturday":
                        retval = "Thứ bảy";
                        break;
                    case "sunday":
                        retval = "Chủ nhật";
                        break;
                    default:
                        break;
                }
                return retval + "," + input.ToString("dd/MM/yyyy");
            }
            catch (Exception)
            {
                return day;
            }
        }
        public static string City(this string input)
        {
            try
            {
                switch (input.ToLower())
                {
                    case "hanoi":
                        return "Hà Nội";
                    case "hochiminh":
                        return "Hồ Chí Minh";
                    default:
                        break;
                }
                return input;
            }
            catch (Exception)
            {
                return input;
            }
        }
        public static string thumbImg(string orginalImgPath, string thumbPath, string fileName, int tbwidth = 0, int tbheight = 0, bool autoSize = false)
        {
            try
            {
                int width = 0;
                int height = 0;
                string thumbName = "";
                string returnPath = "";
                // Thumb image
                using (System.Drawing.Image image = System.Drawing.Image.FromFile(orginalImgPath))
                {
                    Size thumbSize = GetThumbSize(image, tbwidth);
                    if (autoSize)
                    {
                        width = thumbSize.Width;
                        height = thumbSize.Height;
                    }
                    else
                    {
                        width = tbwidth;
                        height = tbheight;
                    }
                    if (!Directory.Exists(thumbPath))
                    {
                        Directory.CreateDirectory(thumbPath);
                    }
                    thumbName = width + "_" + height + "_" + fileName;
                    var thumbnailImg = new Bitmap(width, height);
                    var thumbGraph = Graphics.FromImage(thumbnailImg);
                    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    var imageRectangle = new Rectangle(0, 0, width, height);
                    thumbGraph.DrawImage(image, imageRectangle);
                    // System.Drawing.Image thumb = image.GetThumbnailImage(width, height, null, IntPtr.Zero);
                    var _thumbPath = System.IO.Path.Combine(thumbPath, thumbName);
                    thumbnailImg.Save(_thumbPath, image.RawFormat);
                    //thumb.Save(_thumbPath);
                    //thumb.Dispose();
                    image.Dispose();
                }
                // End thumb
                return thumbName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

        }
        public static Size GetThumbSize(System.Drawing.Image original, int maxPixels)
        {
            int originalWidth = original.Width;
            int originalHeight = original.Height;
            double factor;
            if (originalWidth > originalHeight)
            {
                factor = (double)maxPixels / originalWidth;
            }
            else
            {
                factor = (double)maxPixels / originalHeight;
            }
            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }
        public static string StreamToFile(Stream fileStream, string filePath)
        {
            using (FileStream writeStream = new FileStream(filePath,
                                            FileMode.Create,
                                            FileAccess.Write))
            {
                int length = 1024;
                Byte[] buffer = new Byte[length];
                int bytesRead = fileStream.Read(buffer, 0, length);
                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = fileStream.Read(buffer, 0, length);
                }
                fileStream.Close();
                writeStream.Close();
            }
            return filePath;
        }
    }
}
