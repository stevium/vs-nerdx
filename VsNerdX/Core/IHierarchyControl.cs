namespace VsNerdX.Core
{
    public interface IHierarchyControl
    {
        void GoUp();
        void GoDown();
        void GoToTop();
        void GoToBottom();
        void GoToParent();
        void GoToFirstChild();
        void GoToLastChild();
        void CloseParentNode();
        void OpenOrCloseNode();
    }
}
