namespace Runtime.Application
{
    public interface IStateProvider<out T> where T : IState
    {
        public int StateType { get; }
        public T GetState();
    }
}