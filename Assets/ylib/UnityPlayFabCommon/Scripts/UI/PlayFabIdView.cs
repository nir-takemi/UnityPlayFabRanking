using UnityEngine;
using UnityEngine.UI;

namespace ylib.Services.UI
{
    public class PlayFabIdView : PlayFabViewBase
    {
        [SerializeField]
        private string formatViewId = "{0}";

        [SerializeField]
        private Text txtPlayFabId = null;

        protected override void OnInitialize()
        {
            txtPlayFabId.text = "Now Loading...";
        }

        protected override void OnChangeLoad()
        {

            txtPlayFabId.text = string.Format(formatViewId, PlayFabPlayerData.Instance.PlayerID);

            ChangeState(State.Idle);
        }
    }
}
