using scrbyy.FpsTemplate.GameCycle;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace scrbyy.FpsTemplate.UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public class UnpauseButton : UIButton
    {
        [Inject] private GameFSM _gameFSM;

        protected override void OnButtonClicked()
        {
            _gameFSM.SetState<GameplayState>();
        }
    }
}