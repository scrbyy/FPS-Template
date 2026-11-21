using UnityEngine;
using UnityEngine.UI;
using Zenject;

using scrbyy.FpsTemplate.GameCycle;

namespace scrbyy.FpsTemplate.UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public class RestartSceneButton : UIButton
    {
        [Inject] private SceneLoader _sceneLoader;

        protected override void OnButtonClicked()
        {
            _sceneLoader.RestartCurrent();
        }
    }
}