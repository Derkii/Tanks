using Cysharp.Threading.Tasks;

namespace Components.Lose
{
    public interface ILosable
    {
        public UniTaskVoid Lose();
    }
}