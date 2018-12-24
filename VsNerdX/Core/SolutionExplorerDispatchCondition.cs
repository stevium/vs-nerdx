using VsNerdX.Dispatcher;
using VsNerdX.Util;

namespace VsNerdX.Core
{
    public class SolutionExplorerDispatchCondition : IDispatchCondition
    {
        private readonly HierarchyControl hierarchyControl;
        private readonly ILogger logger;
        private bool canDispatch;

        public SolutionExplorerDispatchCondition(HierarchyControl hierarchyControl, ILogger logger)
        {
            this.hierarchyControl = hierarchyControl;
            this.logger = logger;
        }

        public bool ShouldDispatch
        {
            get
            {
                var listBox = hierarchyControl.GetHierarchyListBox();
                if (listBox == null) return false;
                var isInRenameMode = (bool?) listBox.GetType().GetProperty("IsInRenameMode")?.GetValue(listBox) == true;
                return listBox.IsKeyboardFocusWithin && !isInRenameMode || hierarchyControl.ContentGrid.IsKeyboardFocusWithin;
            }
        }
    }
}
