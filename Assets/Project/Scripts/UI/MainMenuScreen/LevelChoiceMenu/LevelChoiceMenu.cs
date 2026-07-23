using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Core;
using Project.Core.Game;
using UnityEngine;
using VContainer;

namespace Project.UI
{
    public class LevelChoiceMenu : MonoBehaviour
    {
        private GameLevelChoiceHolder _levelChoiceHolder;
        [SerializeField] public List<LevelChoiceView> levelChoiceViews;

        [Inject]
        public void Construct(GameLevelChoiceHolder levelChoiceHolder, ISceneService sceneService)
        {
            _levelChoiceHolder = levelChoiceHolder;
            sceneService.OpenGameFromMenu().Forget();
        }
        
        private void Awake()
        {
            foreach (LevelChoiceView levelChoiceView in levelChoiceViews)
            {
                levelChoiceView.OnLevelPicked += HandleLevelChoice;
            }
        }

        private void HandleLevelChoice(string levelName)
        {
            _levelChoiceHolder.currentLevelName = levelName;
        }

        private void OnDestroy()
        {
            foreach (LevelChoiceView levelChoiceView in levelChoiceViews)
            {
                levelChoiceView.OnLevelPicked -= HandleLevelChoice;
            }
        }
    }
}