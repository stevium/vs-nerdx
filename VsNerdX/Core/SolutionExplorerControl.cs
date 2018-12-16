using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VsNerdX.Util;

namespace VsNerdX.Core
{
    public class HierarchyControl : IHierarchyControl
    {
        private const string SolutionPivotNavigator = "Microsoft.VisualStudio.PlatformUI.SolutionPivotNavigator";
        private const string SolutionPivotTreeView = "Microsoft.VisualStudio.PlatformUI.SolutionPivotTreeView";
        private const string WorkspaceTreeView = "Microsoft.VisualStudio.Workspace.VSIntegration.UI.WorkspaceTreeViewControl";

        private readonly ILogger logger;
        private readonly VsNerdXPackage vsNerdXPackage;

        private Panel  ContentGrid = null;
        private ContentPresenter ContentPresenter;
        private ToolWindowPane SolutionPane;
        private Grid NavigatiorFrame;
        private StackPanel HelpStackPanel;

        public IVsUIHierarchyWindow SolutionHierarchy { get; set; }

        internal HierarchyControl(VsNerdXPackage vsNerdXPackage, ILogger logger)
        {
            this.logger = logger;
            this.vsNerdXPackage = vsNerdXPackage;
        }

        public void GoDown()
        {
            var listBox = GetHierarchyListBox();
            if (listBox == null) return;
            var index = listBox.Items.IndexOf(listBox.SelectedItem) + 1;

            if (index < listBox.Items.Count)
            {
                listBox.SelectedItem = listBox.Items.GetItemAt(index);
                EnsureSelection(listBox);
            }

        }

        public void GoToFirstChild()
        {
            var listBox = GetHierarchyListBox();
            var item = listBox.SelectedItem;

            if (item == null) return;

            var parent = item.GetType().GetProperty("Parent")?.GetValue(item);
            if (parent == null) return;

            var childNodes = parent.GetType().GetProperty("ChildNodes").GetValue(parent);
            if (childNodes  == null) return;

            var first = listBox.SelectedItem = ((IEnumerable) childNodes).Cast<Object>().ToList().First();
            EnsureSelection(listBox);
        }

        public void GoToLastChild()
        {
            var listBox = GetHierarchyListBox();
            var item = listBox.SelectedItem;

            if (item == null) return;

            var parent = item.GetType().GetProperty("Parent")?.GetValue(item);
            if (parent == null) return;

            var childNodes = parent.GetType().GetProperty("ChildNodes").GetValue(parent);
            if (childNodes  == null) return;

            var last = listBox.SelectedItem = ((IEnumerable) childNodes).Cast<Object>().ToList().Last();
            EnsureSelection(listBox);
        }

        public void GoToParent()
        {
            var listBox = GetHierarchyListBox();
            var item = listBox.SelectedItem;

            if (item == null) return;

            var parent = item.GetType().GetProperty("Parent")?.GetValue(item);
            if (parent == null) return;

            listBox.SelectedItem = parent;
            EnsureSelection(listBox);
        }

        public void CloseParentNode()
        {
            GoToParent();
            var parent = GetHierarchyListBox().SelectedItem;
            parent.GetType().GetProperty("IsExpanded")
                ?.SetValue(parent, false);
        }

        public void OpenOrCloseNode()
        {
            var listBox = GetHierarchyListBox();
            var item = listBox.SelectedItem;
            var expandable = (bool?)item.GetType().GetProperty("IsExpandable")?.GetValue(item);
            var expanded = (bool?)item.GetType().GetProperty("IsExpanded")?.GetValue(item);

            if (expandable != true) return;

            item.GetType().GetProperty("IsExpanded")?.SetValue(item, expanded != true);
        }

        public Object GetSelectedItem()
        {
            var listBox = GetHierarchyListBox();
            return listBox.SelectedItem;
        }

        public void GoUp()
        {
            var listBox = GetHierarchyListBox();
            if (listBox == null) return;
            var index = listBox.Items.IndexOf(listBox.SelectedItem) - 1;
            if (index >= 0)
            {
                listBox.SelectedItem = listBox.Items.GetItemAt(index);
                EnsureSelection(listBox);
            }
        }

        public void GoToTop()
        {
            var listBox = GetHierarchyListBox();
            if (listBox == null) return;
            listBox.SelectedItem = listBox.Items.GetItemAt(0);
            EnsureSelection(listBox);
        }

        public void GoToBottom()
        {
            var listBox = GetHierarchyListBox();
            if (listBox == null) return;
            listBox.SelectedItem = listBox.Items.GetItemAt(listBox.Items.Count - 1);
            EnsureSelection(listBox);
        }

        public void EnsureSelection(object listBox)
        {
            if (listBox == null) return;
            listBox.GetType().GetMethod("FocusSelectedItem")
                ?.Invoke(listBox, new Object[] { });
        }

        public ListBox GetHierarchyListBox()
        {
            SolutionHierarchy = VsShellUtilities.GetUIHierarchyWindow(vsNerdXPackage, VSConstants.StandardToolWindows.SolutionExplorer);

            SolutionPane = SolutionHierarchy as ToolWindowPane;
            if (!(SolutionPane != null))
            {
                return null;
            }
                
            ContentGrid = SolutionPane.Content as Panel;
            if (!(ContentGrid != null) || ContentGrid.Children.Count == 0)
            {
                return null;
            }

            ContentPresenter = ContentGrid.Children[0] as ContentPresenter;
            if (!(ContentPresenter != null))
            {
                return null;
            }

            ListBox listBox = null;

            switch (ContentPresenter.Content.GetType().FullName)
            {
                case SolutionPivotNavigator:
                    listBox = ContentPresenter.Content.GetType().GetProperties()
                        .Single(p => p.Name == "TreeView" && p.PropertyType.FullName == SolutionPivotTreeView)
                        .GetValue(ContentPresenter.Content) as ListBox;
                    break;
                case WorkspaceTreeView:
                    listBox = ContentPresenter.Content as ListBox;
                    break;
            }

            return listBox;
        }

        public void ToggleHelp()
        {
            GetHierarchyListBox();
            if (HelpStackPanel == null)
            {
                this.ShowHelp();
                NavigatiorFrame = (Grid) SolutionHierarchy.GetValue("Frame").GetValue("FrameView").GetValue("Content");
                NavigatiorFrame.SizeChanged += HierarchyControl_SizeChanged;
                HierarchyControl_SizeChanged(null, null);
            }
            else
            {
                this.HideHelp();
                var navigatiorFrame = (Grid) SolutionHierarchy.GetValue("Frame").GetValue("FrameView").GetValue("Content");
                navigatiorFrame.SizeChanged -= HierarchyControl_SizeChanged;
            }
        }

        private void HierarchyControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var navigationContent = (ContentPresenter) NavigatiorFrame.GetValue("Content");
            HelpStackPanel.MaxHeight = navigationContent.ActualHeight;
            HelpStackPanel.MaxWidth = navigationContent.ActualWidth;
        }

        private void HideHelp()
        {
            ContentGrid.Children.Remove(HelpStackPanel);
            HelpStackPanel = null;
        }

        private void ShowHelp()
        {
            var scrollView = new ScrollViewer();
            var textBlock = new TextBlock();

            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Background = new SolidColorBrush(Colors.WhiteSmoke);
            textBlock.FontSize = 15;
            textBlock.FontFamily = new FontFamily("Consolas");

            textBlock.Inlines.AddRange(new Inline [] {
                new Bold(new Run(" NerdX (2.0.0) Quick Help")), new LineBreak(),
                new Bold(new Run("=========================")), new LineBreak(),
                new Bold(new Run(" Directory node mappings")), new LineBreak(),
                new Run("-------------------------"), new LineBreak(),
                new Bold(new Run(" o")), new Run(": open & close node"), new LineBreak(),
                new Bold(new Run(" O")), new Run(": recursively open node"), new LineBreak(),
                new Bold(new Run(" x")), new Run(": close parent of node"), new LineBreak(),
                new Bold(new Run(" X")), new Run(": recursively close child nodes"), new LineBreak(), new LineBreak(),

                new Bold(new Run(" Tree navigation mappings")), new LineBreak(),
                new Run("-------------------------"), new LineBreak(),
                new Bold(new Run(" P")), new Run(": go to root"), new LineBreak(),
                new Bold(new Run(" p")), new Run(": go to parent"), new LineBreak(),
                new Bold(new Run(" j")), new Run(": go to next sibling"), new LineBreak(),
                new Bold(new Run(" J")), new Run(": go to last child"), new LineBreak(),
                new Bold(new Run(" k")), new Run(": go to prev sibling"), new LineBreak(),
                new Bold(new Run(" K")), new Run(": go to first child"), new LineBreak(),
                new Bold(new Run(" gg")), new Run(": go to top"), new LineBreak(),
                new Bold(new Run(" G")), new Run(": go to bottom"), new LineBreak(), new LineBreak(),

                new Bold(new Run(" Other mappings")), new LineBreak(),
                new Run("-------------------------"), new LineBreak(), 
                new Bold(new Run(" /")), new Run(": Enter Find Mode"), new LineBreak(),
                new Bold(new Run(" Esc")), new Run(": Exit Find Mode"), new LineBreak(),
                new Bold(new Run(" ?")), new Run(": Toggle Help")
            });

            scrollView.Content = textBlock;
            scrollView.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

            HelpStackPanel = new StackPanel();
            var separator = new Separator();
            HelpStackPanel.Children.Add(scrollView);
            HelpStackPanel.Children.Add(separator);

            Grid.SetColumn(HelpStackPanel, 0);
            Grid.SetRow(HelpStackPanel, 0);
            ContentGrid.Children.Add(HelpStackPanel);
        }
    }
}