namespace Runtime.Application
{
    public interface IStateProvider<T> where T : IState
    {
        public int StateType { get; }
        public T GetState();
    }
}