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

        private CanvasBitmap originalBitmap; // �洢ԭʼͼ��
        private GaussianBlurEffect blurEffect; // ��˹ģ��Ч��
        private Transform2DEffect scaleEffect; // ��˹ģ��Ч��


        // ��ImageSource���Ա仯ʱ����
        private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // ת��Ϊ��ǰ�ؼ�ʵ��
            var control = d as AppCardControl;
            control?.LoadAndBlurImage((string)e.NewValue);
        }
        private async void LoadAndBlurImage(string imagePath)
        {
            // ʹ��ms-appx URI������ָ�����ڵ�ͼ��·��
            string packageImagePath = $"ms-appx:///{imagePath}";

            CanvasDevice device = CanvasDevice.GetSharedDevice();

            // ʹ����ȷ��Uri����ͼ��
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
            // ȷ��ͼ���Ч���Ѽ����Ҳ�Ϊnull
            if (originalBitmap != null && blurEffect != null)
            {
                // ����ģ��ͼ��
                try
                {
                    args.DrawingSession.DrawImage(blurEffect);
                }
                catch (Exception ex)
                {
                    // �ʵ��ش����쳣�������¼��־
                    System.Diagnostics.Debug.WriteLine($"Drawing error: {ex.Message}");
                }
            }
        }



    }


}
