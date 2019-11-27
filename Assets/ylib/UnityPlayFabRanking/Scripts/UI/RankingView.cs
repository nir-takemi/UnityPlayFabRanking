using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ylib.Services.UI
{
    public class RankingView : PlayFabViewBase
    {
        [SerializeField]
        private bool loadOnAwake = true;

        [SerializeField]
        private bool rankingViewOnly = false;

        [SerializeField]
        private string rankingName = "";

        [SerializeField]
        private int rankingDataSize = 10;

        [SerializeField]
        private string formatScore = "{0}";

        [SerializeField]
        private GameObject prefabRankingData = null;

        [SerializeField]
        private UnityEvent eventOnRankingLoad = null;

        [SerializeField]
        private UnityEvent eventOnClose = null;

        [SerializeField]
        private ScrollRect scrollRect = null;

        [SerializeField]
        private InputField txtInputDisplayName = null;

        [SerializeField]
        private Text txtScore = null;

        [SerializeField]
        private GameObject goOtherRanking = null;

        private List<RankingViewData> rankingDataList;


        protected override void OnInitialize()
        {
            rankingDataList = null;

            if(rankingViewOnly)
            {
                goOtherRanking.SetActive(false);
            }

            CreateRankingData();
        }

        protected override void OnChangeLoad() {
            if (txtInputDisplayName != null)
            {
                txtInputDisplayName.text = PlayFabPlayerData.Instance.DisplayName;
            }
            
            if (loadOnAwake)
            {
                LoadRanking();
            }
        }

        private void CreateRankingData()
        {
            rankingDataList = new List<RankingViewData>(rankingDataSize);

            for (int i = 0; i < rankingDataSize; ++i)
            {
                GameObject goRankingData = Instantiate(prefabRankingData, scrollRect.content);
                RankingViewData rankingViewData = goRankingData.GetComponent<RankingViewData>();

                rankingDataList.Add(rankingViewData);
            }

            scrollRect.verticalNormalizedPosition = 1f;
        }

        public void SetData(PlayFabRanking.RankingData[] rankingDatas)
        {
            for (int i = 0; i < rankingDataList.Count; ++i)
            {
                if (i < rankingDatas.Length)
                {
                    rankingDataList[i].Initialize(rankingDatas[i]);
                }
                else
                {
                    rankingDataList[i].Clear();
                }
            }

            scrollRect.verticalNormalizedPosition = 1f;
        }

        public void LoadRanking()
        {
            PlayFabRanking.GetRankingData(rankingName, (rankingList) =>
            {
                SetData(rankingList.ToArray());

                eventOnRankingLoad.Invoke();

                ChangeState(State.Idle);
            },
            1, rankingDataSize);
        }

        public void UpdateScore(int score)
        {
            txtScore.text = string.Format(formatScore, score);

            PlayFabRanking.SendPlayScore(rankingName, score, () =>
            {
                ChangeState(State.Login);
            });
        }

        public void OnNameUpdate()
        {
            if (txtInputDisplayName.text.Length <= 0)
            {
                Debug.LogError("1文字以上入力してください");
                return;
            }

            PlayFabPlayer.UpdateName(txtInputDisplayName.text, (displayName) =>
            {
                txtInputDisplayName.text = displayName;

                LoadRanking();
            },
            (error) =>
            {
                if(error.Error == PlayFab.PlayFabErrorCode.NameNotAvailable)
                {
                    Debug.LogError("重複する名前が存在しそうです");
                }
            });
        }

        public bool IsRankInWithHighScore()
        {
            PlayFabPlayerData playFabPlayerData = PlayFabPlayerData.Instance;
            bool isRankIn = false;
            int rankedScore = 0;

            foreach (RankingViewData rankingViewData in rankingDataList)
            {
                if (rankingViewData.IsMyRankingData(playFabPlayerData.PlayerID))
                {
                    isRankIn = true;
                    rankedScore = rankingViewData.GetScore();
                    break;
                }
            }

            return (isRankIn && (int.Parse(txtScore.text) == rankedScore));
        }

        public void OnClose()
        {
            eventOnClose.Invoke();

            gameObject.SetActive(false);
        }
    }
}
