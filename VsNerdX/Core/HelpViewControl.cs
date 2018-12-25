using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.VisualStudio.Shell.Interop;
using VsNerdX.Util;

namespace VsNerdX.Core
{
    public class HelpViewControl
    {
        
        private readonly HierarchyControl _hierarchyControl;
        private StackPanel _helpStackPanel;
        private ScrollViewer _scrollView;
        private Grid NavigatorFrame { get; set; }

        public HelpViewControl(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = (HierarchyControl) hierarchyControl;
        }

        public void ToggleHelp()
        {
            if (_helpStackPanel == null)
            {
                this.ShowHelp();
                NavigatorFrame = (Grid) this._hierarchyControl.SolutionHierarchy.GetValue("Frame").GetValue("FrameView").GetValue("Content");
                NavigatorFrame.SizeChanged += HierarchyControl_SizeChanged;
                HierarchyControl_SizeChanged(null, null);
            }
            else
            {
                this.HideHelp();
                var navigatiorFrame = (Grid) this._hierarchyControl.SolutionHierarchy.GetValue("Frame").GetValue("FrameView").GetValue("Content");
                navigatiorFrame.SizeChanged -= HierarchyControl_SizeChanged;
            }
        }

        private void HierarchyControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var navigationContent = (ContentPresenter) NavigatorFrame.GetValue("Content");
            _scrollView.MaxHeight = navigationContent.ActualHeight;
            _scrollView.MaxWidth = navigationContent.ActualWidth;
        }

        private void HideHelp()
        {
            _hierarchyControl.ContentGrid.Children.Remove(_helpStackPanel);
            _helpStackPanel = null;
        }


        private void ShowHelp()
        {
            this._scrollView = new ScrollViewer();
            var textBlock = new TextBlock();

            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Background = new SolidColorBrush(Color.FromRgb(201,203,215));
            textBlock.FontSize = 15;
            textBlock.FontFamily = new FontFamily("Consolas");

            textBlock.Inlines.AddRange(new Inline [] {
                new Bold(new Run("VsNerdX (2.2.0) Quick Help")), new LineBreak(),
                new Bold(new Run("==========================")), new LineBreak(),
                new Bold(new Run(" Directory node mappings")), new LineBreak(),
                new Run("--------------------------"), new LineBreak(),
                new Bold(new Run(" o")), new Run(": open & close node"), new LineBreak(),
                new Bold(new Run(" O")), new Run(": recursively open node"), new LineBreak(),
                new Bold(new Run(" x")), new Run(": close parent of node"), new LineBreak(),
                new Bold(new Run(" X")), new Run(": recursively close child nodes"), new LineBreak(), new LineBreak(),

                new Bold(new Run(" File node mappings")), new LineBreak(),
                new Run("--------------------------"), new LineBreak(),
                new Bold(new Run(" <Enter>")), new Run(": open file"), new LineBreak(),
                new Bold(new Run(" go")), new Run(": preview file"), new LineBreak(),
                new Bold(new Run(" i")), new Run(": open split"), new LineBreak(),
                new Bold(new Run(" s")), new Run(": open vertical split"), new LineBreak(),
                
                
                new Bold(new Run(" Tree navigation mappings")), new LineBreak(),
                new Run("--------------------------"), new LineBreak(),
                new Bold(new Run(" P")), new Run(": go to parent"), new LineBreak(),
                new Bold(new Run(" j")), new Run(": go to next sibling"), new LineBreak(),
                new Bold(new Run(" J")), new Run(": go to last child"), new LineBreak(),
                new Bold(new Run(" k")), new Run(": go to prev sibling"), new LineBreak(),
                new Bold(new Run(" K")), new Run(": go to first child"), new LineBreak(),
                new Bold(new Run(" gg")), new Run(": go to top"), new LineBreak(),
                new Bold(new Run(" G")), new Run(": go to bottom"), new LineBreak(), new LineBreak(),

                new Bold(new Run(" Node editing mappings")), new LineBreak(),
                new Run("--------------------------"), new LineBreak(),
                new Bold(new Run(" dd")), new Run(": delete"), new LineBreak(),
                new Bold(new Run(" cc")), new Run(": cut"), new LineBreak(),
                new Bold(new Run(" yy")), new Run(": copy"), new LineBreak(),
                new Bold(new Run(" yp")), new Run(": copy full path"), new LineBreak(),
                new Bold(new Run(" yw")), new Run(": copy visible text"), new LineBreak(),
                new Bold(new Run(" p")), new Run(": paste"), new LineBreak(),
                new Bold(new Run(" r")), new Run(": rename"), new LineBreak(), new LineBreak(),

                new Bold(new Run(" Tree filtering mappings")), new LineBreak(),
                new Run("--------------------------"), new LineBreak(),
                new Bold(new Run(" I")), new Run(": Show all files"), new LineBreak(), new LineBreak(),
                
                new Bold(new Run(" Other mappings")), new LineBreak(),
                new Run("--------------------------"), new LineBreak(), 
                new Bold(new Run(" /")), new Run(": Enter Find Mode"), new LineBreak(),
                new Bold(new Run(" Esc")), new Run(": Exit Find Mode"), new LineBreak(),
                new Bold(new Run(" ?")), new Run(": Toggle Help")
            });

            _scrollView.Content = textBlock;
            _scrollView.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

            _helpStackPanel = new StackPanel();
            var separator = new Separator();
            _helpStackPanel.Children.Add(_scrollView);
            _helpStackPanel.Children.Add(separator);

            Grid.SetColumn(_helpStackPanel, 0);
            Grid.SetRow(_helpStackPanel, 0);
            _hierarchyControl.ContentGrid.Children.Add(_helpStackPanel);
        }
    }
}