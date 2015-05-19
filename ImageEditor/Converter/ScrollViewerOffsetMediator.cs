using System.Windows;
using System.Windows.Controls;

namespace ImageEditor.Converter
{
    public class ScrollViewerOffsetMediator : FrameworkElement
    {
        public ScrollViewer ScrollViewer
        {
            get { return (ScrollViewer) GetValue(ScrollViewerProperty); }
            set { SetValue(ScrollViewerProperty, value); }
        }

        public static readonly DependencyProperty ScrollViewerProperty =
            DependencyProperty.Register(
                "ScrollViewer",
                typeof (ScrollViewer),
                typeof (ScrollViewerOffsetMediator),
                new PropertyMetadata(OnScrollViewerChanged));

        private static void OnScrollViewerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var mediator = (ScrollViewerOffsetMediator) o;
            var scrollViewer = (ScrollViewer) (e.NewValue);
            if (null != scrollViewer)
            {
                scrollViewer.ScrollToVerticalOffset(mediator.VerticalOffset);
            }
        }

        public double VerticalOffset
        {
            get { return (double) GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register(
                "VerticalOffset",
                typeof (double),
                typeof (ScrollViewerOffsetMediator),
                new PropertyMetadata(OnVerticalOffsetChanged));

        public static void OnVerticalOffsetChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var mediator = (ScrollViewerOffsetMediator) o;
            if (null != mediator.ScrollViewer)
            {
                mediator.ScrollViewer.ScrollToVerticalOffset((double) (e.NewValue));
                mediator.ScrollViewer.UpdateLayout();
            }
        }

        public double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.Register(
                "HorizontalOffset",
                typeof(double),
                typeof(ScrollViewerOffsetMediator),
                new PropertyMetadata(OnHorizontalOffsetChanged));

        public static void OnHorizontalOffsetChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var mediator = (ScrollViewerOffsetMediator)o;
            if (null != mediator.ScrollViewer)
            {
                mediator.ScrollViewer.ScrollToHorizontalOffset((double)(e.NewValue));
            }
        }
    }
}