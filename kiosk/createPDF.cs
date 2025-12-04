using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF;
using QuestPDF.Companion;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Windows.Forms;
using System.IO;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;


namespace kiosk
{
    
    public class createPDF
    {
        public void generate(pdfTemplate pdf)
        {
            string folder = Path.Combine(Application.StartupPath, "Receipts");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            //full file path
            string filePath = Path.Combine(folder, "receipt.pdf");
            // Generate pdf
            pdf.GeneratePdf(filePath);
        }
    }

    //View pdf in panel
    public static class Web {

        public async static Task<WebView2> viewPDF()
        {
            WebView2 web = new WebView2();
            string folder = Path.Combine(Application.StartupPath, "Receipts");
            string filePath = Path.Combine(folder, "receipt.pdf");
            string cacheBuster = DateTime.Now.Ticks.ToString();
            web.Dock = DockStyle.Fill;
            await web.EnsureCoreWebView2Async();
            web.CoreWebView2.Settings.HiddenPdfToolbarItems = CoreWebView2PdfToolbarItems.Bookmarks
                    | CoreWebView2PdfToolbarItems.FitPage
                    | CoreWebView2PdfToolbarItems.PageLayout
                    | CoreWebView2PdfToolbarItems.PageSelector
                    | CoreWebView2PdfToolbarItems.Print
                    | CoreWebView2PdfToolbarItems.Rotate
                    | CoreWebView2PdfToolbarItems.Save
                    | CoreWebView2PdfToolbarItems.SaveAs
                    | CoreWebView2PdfToolbarItems.Search
                    | CoreWebView2PdfToolbarItems.ZoomIn
                    | CoreWebView2PdfToolbarItems.ZoomOut;

            web.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            web.CoreWebView2.Settings.AreDevToolsEnabled = false;


            web.CoreWebView2.Navigate(new Uri(filePath).AbsoluteUri);
            //web.CoreWebView2.Navigate($"{new Uri(filePath).AbsoluteUri}?v={cacheBuster}");
            return web;
        }

    }

    //pdf layout
    public class pdfTemplate : IDocument
    {

        public receiptTemplate receipt { get; }
        public pdfTemplate(receiptTemplate receiptModel)
        {
            receipt = receiptModel;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;


        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {

                page.Size(PageSizes.A5);
                page.PageColor(Colors.White);
                page.Margin(1, Unit.Centimetre);

                page.Header().Height(25, Unit.Millimetre).Background(Colors.Grey.Lighten2)
                    .BorderBottom(1)//.BorderTop(2).BorderVertical(2)
                    .Element(ComposerHeader);

                page.Content().Background(Colors.Grey.Lighten3).BorderTop(1) //.BorderVertical(2)
                    .Element(Body);


                page.Footer().Height(25, Unit.Millimetre).Background(Colors.Grey.Lighten2)//.BorderBottom(2).BorderVertical(2)
                .Element(ComposerFooter);
            });
        }
        //Header
        private void ComposerHeader(IContainer container)
        {
            string logo = Path.Combine(Application.StartupPath, "Pictures/LSPU-Logo.png");

            container.Row(row =>
            {
                row.RelativeItem(1).PaddingLeft(16, Unit.Millimetre).Width(15, Unit.Millimetre).AlignMiddle();//.Image(logo);
                row.RelativeItem(2).Column(col =>
                {
                    col.Item().AlignCenter().PaddingTop(5, Unit.Millimetre).Text("Republic of the Philippines").FontSize(7).Bold().FontColor(Colors.Black);
                    col.Item().AlignCenter().PaddingTop(1, Unit.Millimetre).Text("Laguna State Polytechnic University").FontSize(8).Bold().FontColor(Colors.Black);
                    col.Item().AlignCenter().Text("San Pablo City Campus").FontSize(6).Bold().FontColor(Colors.Black);
                    col.Item().AlignCenter().PaddingTop(5, Unit.Millimetre).Text("Student Receipt").FontSize(7).Bold().FontColor(Colors.Black);
                });
                row.RelativeItem(1);
            });

        }
        //Body
        private void Body(IContainer container)
        {
            container.Column(Column =>
            {
                Column.Item().Padding(2, Unit.Millimetre).Row(r =>
                {
                    r.RelativeItem().Column(col => {

                        col.Item().Text(text => {
                            text.Span("Receipt ID: ").Bold().FontSize(10);
                            text.Span(receipt.receiptID).FontSize(10);
                        });

                        col.Item().Text(text => {
                            text.Span(" ").Bold().FontSize(10);
                            //text.Span(receipt.Student.Name).FontSize(10);
                        });

                        //col.Item().Text(text => {
                        //    text.Span(" ").Bold().FontSize(10);
                        //    text.Span(receipt.Student.Course).FontSize(10);
                        //});

                    });

                    r.RelativeItem().Column(col => {

                        col.Item().AlignRight().Text(text => {
                            text.Span("Date: ").Bold().FontSize(10);
                            text.Span(receipt.receiptDate.ToString()).FontSize(10);
                        });
                        col.Item().AlignRight().Text(text => {
                            text.Span(" ").Bold().FontSize(10);
                            //text.Span(receipt.receiptDate.ToString()).FontSize(10);
                        });

                    });
                });

                Column.Item().Padding(2, Unit.Millimetre).Border(2).Row(row =>
                {
                    row.RelativeItem(3).Background(Colors.Grey.Darken2).Border(1)
                        .AlignCenter().Padding(1, Unit.Millimetre)
                        .Text("Item").Bold().FontSize(10).FontColor(Colors.White);

                    row.RelativeItem(3).Background(Colors.Grey.Darken2).Border(1)
                        .AlignCenter().Padding(1, Unit.Millimetre)
                        .Text("Type").Bold().FontSize(10).FontColor(Colors.White);

                    row.RelativeItem(1).Background(Colors.Grey.Darken2).Border(1)
                        .AlignCenter().Padding(1, Unit.Millimetre)
                        .Text("Size").Bold().FontSize(10).FontColor(Colors.White);

                    row.RelativeItem(1).Background(Colors.Grey.Darken2).Border(1)
                        .AlignCenter().Padding(1, Unit.Millimetre)
                        .Text("QTY").Bold().FontSize(10).FontColor(Colors.White);

                    row.RelativeItem(2).Background(Colors.Grey.Darken2).Border(1)
                        .AlignCenter().Padding(1, Unit.Millimetre)
                        .Text("Unit Price").Bold().FontSize(10).FontColor(Colors.White);

                    row.RelativeItem(2).Background(Colors.Grey.Darken2).Border(1)
                        .AlignCenter().Padding(1, Unit.Millimetre)
                        .Text("Amount").Bold().FontSize(10).FontColor(Colors.White);
                });


                //temp
                int totalPrice = 0;

                for (int i = 0; i < receipt.Items.Count; i++)
                {
                    Column.Item().PaddingTop(3, Unit.Millimetre).PaddingHorizontal(2, Unit.Millimetre).Row(row =>
                    {
                        row.RelativeItem(3).AlignCenter().Padding(1, Unit.Millimetre).Text(receipt.Items[i].Name).FontSize(10);
                        row.RelativeItem(3).AlignCenter().Padding(1, Unit.Millimetre).Text(receipt.Items[i].Type).FontSize(10);
                        row.RelativeItem(1).AlignCenter().Padding(1, Unit.Millimetre).Text(receipt.Items[i].Size).FontSize(10);
                        row.RelativeItem(1).AlignCenter().Padding(1, Unit.Millimetre).Text(receipt.Items[i].Quantity.ToString()).FontSize(10);
                        row.RelativeItem(2).AlignCenter().Padding(1, Unit.Millimetre).Text(receipt.Items[i].Price.ToString()).FontSize(10);
                        row.RelativeItem(2).AlignCenter().Padding(1, Unit.Millimetre).Text((receipt.Items[i].Price * receipt.Items[i].Quantity).ToString()).FontSize(10);
                    });
                    totalPrice += (int)(receipt.Items[i].Price * receipt.Items[i].Quantity);

                }

                Column.Item().PaddingTop(5, Unit.Millimetre).PaddingHorizontal(2, Unit.Millimetre).Row(row =>
                {
                    row.RelativeItem(3).AlignCenter().Padding(1, Unit.Millimetre).Text("");
                    row.RelativeItem(3).AlignCenter().Padding(1, Unit.Millimetre).Text("");
                    row.RelativeItem(1).AlignCenter().Padding(1, Unit.Millimetre).Text("");
                    row.RelativeItem(1).AlignCenter().Padding(1, Unit.Millimetre).Text("");
                    row.RelativeItem(2).AlignCenter().Padding(1, Unit.Millimetre).Text("Total: ").Bold().FontSize(10);
                    row.RelativeItem(2).AlignCenter().Padding(1, Unit.Millimetre).Text(totalPrice.ToString()).FontSize(10);
                });
            });
        }
        //Footer
        private void ComposerFooter(IContainer container)
        {
            container.AlignCenter().Text("Thank you for your purchase!").FontSize(7).Bold().FontColor(Colors.Black);
        }
    }

}
