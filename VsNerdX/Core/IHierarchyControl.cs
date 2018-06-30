namespace VsNerdX.Core
{
    public interface IHierarchyControl
    {
        void GoUp();
        void GoDown();
        void GoToTop();
        void GoToBottom();
        void GoToParent();
        void CloseParentNode();
        void OpenOrCloseNode();
    }
}
