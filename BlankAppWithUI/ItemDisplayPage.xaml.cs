using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Foundation;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BlankAppWithUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ItemDisplayPage : Page
    {
        public ItemDisplayPage()
        {
            this.InitializeComponent();
            // 1. Fetch data
            AppCards = FetchAppCardsData();
            // 2. Handle Responsive Layout
            this.SizeChanged += Page_SizeChanged;

        }

        private ObservableCollection<AppCard> FetchAppCardsData()
        {
            return new ObservableCollection<AppCard>
            {
                new AppCard { AppName = "TikTok", ImageSource = "Assets/TikTok.jpg", Rating = "4.1 бя", Status = "Owned", BackgroundColor = new SolidColorBrush(Colors.Black) },
                new AppCard { AppName = "Adobe Photoshop", ImageSource = "Assets/PS.png", Rating = "3.9 бя", Status = "Paid", BackgroundColor = new SolidColorBrush(Colors.DarkBlue) },
                new AppCard { AppName = "LinkedIn", ImageSource = "Assets/LinkedIn.jpg", Rating = "4.2 бя", Status = "Free", BackgroundColor = new SolidColorBrush(Colors.Blue) },
                new AppCard { AppName = "Adobe Acrobat Reader DC", ImageSource = "Assets/Adobe.png", Rating = "3.5 бя", Status = "Free", BackgroundColor = new SolidColorBrush(Colors.Red) },
                new AppCard { AppName = "Dolby", ImageSource = "Assets/Dolby.jpg", Rating = "3.5 бя", Status = "Free", BackgroundColor = new SolidColorBrush(Colors.Red) },
            };
        }

        public ObservableCollection<AppCard> AppCards;
        private Stack<AppCard> HiddenCards = new Stack<AppCard>();


        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int maxItemsToShow = CalculateMaxItemsToShow(e.NewSize);
            UpdateCardsCollection(maxItemsToShow);
            Debug.WriteLine($"WindowSize changed: {e.NewSize.Width}x{e.NewSize.Height}, MaxItemsToShow: {maxItemsToShow}");
        }

        private int CalculateMaxItemsToShow(Size windowSize)
        {
            int baseItemCount = 0;
            int widthFactor = (int)((windowSize.Width - 200) / 200);
            if (windowSize.Width < 300)
            {
                widthFactor = 0;
            }
            return baseItemCount + widthFactor;
        }

        private void UpdateCardsCollection(int maxItemsToShow)
        {
            while (AppCards.Count < maxItemsToShow && HiddenCards.Count > 0)
            {
                var cardToShow = HiddenCards.Pop();
                AppCardPush(cardToShow);
            }

            while (AppCards.Count > maxItemsToShow)
            {
                var cardToHide = AppCardPop();
                HiddenCards.Push(cardToHide);
            }
            Debug.WriteLine($"AppCardsDisplay: {AppCards.Count}");

        }

        public void AppCardPush(AppCard item)
        {
            AppCards.Add(item);
        }

        public AppCard AppCardPop()
        {
            if (AppCards.Count > 0)
            {
                int lastIndex = AppCards.Count - 1;
                AppCard item = AppCards[lastIndex];
                AppCards.RemoveAt(lastIndex);
                return item;
            }
            else
            {
                throw new InvalidOperationException("The collection is empty.");
            }
        }

        private void AppCardControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"sender: {sender}");

            var appCardControl = sender as AppCardControl;
            if (appCardControl != null)
            {
                System.Diagnostics.Debug.WriteLine($"appCardControl: {appCardControl}");

                Frame frame = FindParentFrame(this);
                if (frame != null)
                {
                    System.Diagnostics.Debug.WriteLine($"frame: {frame}");

                    frame.Navigate(typeof(DetailPage), appCardControl);
                }
            }
        }

        private Frame FindParentFrame(DependencyObject obj)
        {
            while (obj != null)
            {
                if (obj is Frame frame)
                {
                    return frame;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
            return null;
        }
    }


}
