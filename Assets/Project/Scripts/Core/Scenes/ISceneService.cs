using Cysharp.Threading.Tasks;

namespace Project.Core
{
    public interface ISceneService
    {
        public UniTask OpenGame();
        public UniTask OpenMenu();
    }
}