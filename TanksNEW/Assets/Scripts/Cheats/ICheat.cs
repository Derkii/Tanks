namespace Cheats.Abstraction
{
    public interface ITurnableCheat
    {
        public void Turn(bool turn);
    }

    public interface IInitializableCheat<T>
    {
        public void Init(T initParams);
    }
}