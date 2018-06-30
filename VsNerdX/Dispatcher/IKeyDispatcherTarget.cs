namespace VsNerdX.Dispatcher
{
    using System.Windows.Forms;
    public interface IKeyDispatcherTarget
    {
        bool OnKey(Keys key);
    }
}