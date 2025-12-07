using Paymongo.Sharp.Features.QrPh.Contracts;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Markup;

namespace kiosk
{
    public class kioskQR
    {
        public async Task<string> GenerateQRCode(string url)
        {


            //File Path
            await Task.Delay(100); // Simulate async work
            try
            {
                Image img;
                var urlData = new PayloadGenerator.Url(url);
                var qrData = QRCodeGenerator.GenerateQrCode(urlData, QRCodeGenerator.ECCLevel.Q);
                PngByteQRCode qrCode = new PngByteQRCode(qrData);
                byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(20);

                using (var ms = new MemoryStream(qrCodeAsPngByteArr))
                {
                    img = new Bitmap(ms);
                }


                string folder = Path.Combine(Application.StartupPath, "QrCode");

                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                string filePath = Path.Combine(folder, "QrLink.png");

                if (File.Exists(filePath)) File.Delete(filePath);

                img.Save(filePath, ImageFormat.Png);

                return filePath;
            }
            catch(Exception e)
            {
                MessageBox.Show("Error generating QR Code: " + e.Message);
                return "";
            }
        }
    }
}
