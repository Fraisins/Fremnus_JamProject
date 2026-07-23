using Cysharp.Threading.Tasks;

namespace Project.Core
{
    public interface ISceneService
    {
        public UniTask OpenGameFromMenu();
        public UniTask OpenMenuFromGame();
    }
}