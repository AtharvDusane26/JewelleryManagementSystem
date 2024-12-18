using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JewelleryManagementSystem.RecieptManager.View
{
    /// <summary>
    /// Interaction logic for RecieptWindow.xaml
    /// </summary>
    public partial class RecieptWindow : Window
    {
        private IReciept _reciept;
        public RecieptWindow(IReciept reciept)
        {
            InitializeComponent();
            _reciept = reciept;
            webControl.NavigateToString(RecieptGenerator.CreateReciept(reciept));
        }

        private void btnPrintPdf_Click(object sender, RoutedEventArgs e)
        {
            CreateReciept();
        }
        private void CreateReciept()
        {
            var grid = webControl;
            var filePath = System.IO.Path.Combine(GeneralSettings.Default.BaseDirectory, $"{_reciept.CustomerName}{_reciept.ReceiptID}.png"); 
            if(File.Exists(filePath))
                File.Delete(filePath);
            // Measure and arrange the grid
            grid.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            grid.Arrange(new Rect(0, 0, grid.ActualWidth, grid.ActualHeight));

            // Create a RenderTargetBitmap to capture the visual
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                (int)grid.ActualWidth,
                (int)grid.ActualHeight,
                96, 96,
                PixelFormats.Pbgra32
            );
            renderBitmap.Render(grid);

            // Create a PngBitmapEncoder to save the image as a PNG
            PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            // Save the PNG to the specified file path
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                pngEncoder.Save(stream);
            }
        }
    }
}
