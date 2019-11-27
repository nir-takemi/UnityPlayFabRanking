using UnityEngine;
using UnityEngine.UI;

namespace ylib.Services.UI
{
    public class RankingViewData : MonoBehaviour
    {
        [SerializeField]
        private Text txtRank = null;
        [SerializeField]
        private Text txtDisplayName = null;
        [SerializeField]
        private Text txtScore = null;

        [SerializeField]
        private string FormatRank = "{0}";
        [SerializeField]
        private string FormatDisplayName = "{0}";
        [SerializeField]
        private string FormatScore = "{0}";

        private PlayFabRanking.RankingData rankingData;

        public void Initialize(PlayFabRanking.RankingData rankingData)
        {
            txtRank.text = string.Format(FormatRank, rankingData.rank);
            txtDisplayName.text = string.Format(FormatDisplayName, rankingData.displayName);
            txtScore.text = string.Format(FormatScore, rankingData.score);

            this.rankingData = rankingData;
        }

        public void Clear()
        {
            txtRank.text = "";
            txtDisplayName.text = "";
            txtScore.text = "";
        }

        public bool IsMyRankingData(string playFabId = null)
        {
            if (playFabId == null)
            {
                playFabId = PlayFabPlayerData.Instance.PlayerID;
            }

            return (playFabId == rankingData.playFabId);
        }

        public int GetScore()
        {
            return rankingData.score;
        }
    }
}
