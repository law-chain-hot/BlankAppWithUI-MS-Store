using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Numerics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BlankAppWithUI
{
    public sealed partial class AppCardControl : UserControl
    {
        public AppCardControl()
        {
            this.InitializeComponent();
        }

        // Dependency Properties for Data Binding
        public SolidColorBrush BackgroundColor
        {
            get { return (SolidColorBrush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(SolidColorBrush), typeof(AppCardControl), new PropertyMetadata(null));

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(AppCardControl), new PropertyMetadata(null, OnImageSourceChanged));

        public string AppName
        {
            get { return (string)GetValue(AppNameProperty); }
            set { SetValue(AppNameProperty, value); }
        }

        public static readonly DependencyProperty AppNameProperty =
            DependencyProperty.Register("AppName", typeof(string), typeof(AppCardControl), new PropertyMetadata(string.Empty));

        public string Rating
        {
            get { return (string)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register("Rating", typeof(string), typeof(AppCardControl), new PropertyMetadata(string.Empty));

        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string), typeof(AppCardControl), new PropertyMetadata(string.Empty));

        private void Border_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(CardControl, "PointerOver", true);
        }

        private void Border_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(CardControl, "Normal", true);
        }

        private CanvasBitmap originalBitmap; // 存储原始图像
        private GaussianBlurEffect blurEffect; // 高斯模糊效果
        private Transform2DEffect scaleEffect; // 高斯模糊效果


        // 当ImageSource属性变化时触发
        private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // 转换为当前控件实例
            var control = d as AppCardControl;
            control?.LoadAndBlurImage((string)e.NewValue);
        }
        private async void LoadAndBlurImage(string imagePath)
        {
            // 使用ms-appx URI方案来指定包内的图像路径
            string packageImagePath = $"ms-appx:///{imagePath}";

            CanvasDevice device = CanvasDevice.GetSharedDevice();

            // 使用正确的Uri加载图像
            Uri imageUri = new Uri(packageImagePath);
            originalBitmap = await CanvasBitmap.LoadAsync(device, imageUri);



            scaleEffect = new Transform2DEffect()
            {
                Source = originalBitmap,
                TransformMatrix = Matrix3x2.CreateScale(1.2f)
            };

            blurEffect = new GaussianBlurEffect()
            {
                Source = scaleEffect,
                BlurAmount = 80.0f,
                BorderMode = EffectBorderMode.Soft
            };
            CanvasControl.Invalidate();
        }

        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            // 确保图像和效果已加载且不为null
            if (originalBitmap != null && blurEffect != null)
            {
                // 绘制模糊图像
                try
                {
                    args.DrawingSession.DrawImage(blurEffect);
                }
                catch (Exception ex)
                {
                    // 适当地处理异常，比如记录日志
                    System.Diagnostics.Debug.WriteLine($"Drawing error: {ex.Message}");
                }
            }
        }



    }


}
