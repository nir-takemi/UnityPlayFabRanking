using UnityEngine;
using UnityEngine.UI;

namespace ylib.Services
{
    using UI;

    public class SampleRanking : MonoBehaviour
    {
        private enum State
        {
            None = 0,
            Login,
            Idle,
        }
        private State state;

        [SerializeField]
        private PlayFabLogin playFabLogin = null;

        [SerializeField]
        private Text txtHeaderID = null; // PlayFabID\n(UseID)

        [SerializeField]
        private Text txtDisplayName = null;

        [SerializeField]
        private SampleRankingData[] rankings = null;

        [SerializeField]
        private GameObject goNameEdit = null;
        [SerializeField]
        private Text txtInputDisplayName = null;

        [SerializeField]
        private RankingView rankingView = null;

        // Start is called before the first frame update
        void Awake()
        {
            state = State.Login;

            goNameEdit.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            switch (state)
            {
                case State.Login:
                    {
                        if (playFabLogin.IsLoadComplete)
                        {
                            AfterLogin();

                            state = State.Idle;
                        }
                        break;
                    }
                case State.Idle:
                    {
                        break;
                    }
            }
        }

        private void AfterLogin()
        {
            PlayFabPlayerData playFabPlayerData = PlayFabPlayerData.Instance;

            txtHeaderID.text = string.Format("{0}\n({1})", playFabPlayerData.PlayerID, playFabLogin.UsedID);
            if (txtDisplayName != null)
            {
                txtDisplayName.text = playFabPlayerData.DisplayName;
            }

            foreach (SampleRankingData ranking in rankings)
            {
                ranking.LoadRanking();
            }

            if (rankingView != null)
            {
                rankingView.UpdateScore(3000);
            }

            PrintGetPlayerData("TestRankingMax");
            PrintGetPlayerData("TestRankingMin");
            PrintGetPlayerData("TestRankingNew");
        }

        public void OnEdit()
        {
            goNameEdit.SetActive(true);

            txtInputDisplayName.text = txtDisplayName.text;
        }

        public void OnNameEditSend()
        {
            if (txtInputDisplayName.text.Length <= 0)
            {
                Debug.LogError("1文字以上入力してください");
                return;
            }
            ylib.Services.PlayFabPlayer.UpdateName(txtInputDisplayName.text, (displayName) =>
            {
                txtDisplayName.text = displayName;
            });

            goNameEdit.SetActive(false);
        }

        public void PrintGetPlayerData(string rankingName)
        {
            ylib.Services.PlayFabRanking.GetRankPlayer(rankingName, (rankingData) =>
            {
                string text = string.Format("{0}_{1}位 {2} {3}_{4}", rankingName, rankingData.rank, rankingData.displayName, rankingData.score, (PlayFabRanking.IsValidData(rankingData) ? "有効" : "無効"));

                Debug.Log(text);
            });
        }
    }
}
