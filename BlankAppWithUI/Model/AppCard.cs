using Microsoft.UI.Xaml.Media;

namespace BlankAppWithUI
{
    public class AppCard
    {
        public string AppName { get; set; }
        public string ImageSource { get; set; }
        public string Rating { get; set; }
        public string Status { get; set; } // e.g., Owned, Paid, Free
        public SolidColorBrush BackgroundColor { get; set; }
    }
}
