namespace Runtime.UI.Base
{
    public interface IScreenProvider<T> where T : IViewModel
    {
        public ScreenHolder<T> GetScreen();
    }
}