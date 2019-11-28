using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ylib.Services
{
    using ylib.Services.UI;

    public class SampleRankingData : MonoBehaviour
	{
		[SerializeField]
		private string rankingName = null;

        [SerializeField]
        private ScrollRect scrollRect = null;

		[SerializeField]
		private Text txtScore = null;
		[SerializeField]
		private Text txtRanking = null;

        [SerializeField]
        private RankingView rankingView = null;

        public void OnSend()
		{
			int score = 0;

			try
			{
				score = int.Parse(txtScore.text);
			}
            catch(System.Exception err)
			{
				Debug.LogError(err);
			}

			ylib.Services.PlayFabRanking.SendPlayScore(rankingName, score, () =>
            {
                LoadRanking();
            });
		}

        public void LoadRanking()
        {
            ylib.Services.PlayFabRanking.GetRankingData(rankingName, (rankingList) =>
            {
                string rankingText = "";

                foreach (PlayFabRanking.RankingData rankingData in rankingList)
                {
                    string text = string.Format("{0}位 {1} {2}", rankingData.rank, rankingData.displayName, rankingData.score);

                    rankingText += text + "\n";
                }

                txtRanking.text = rankingText;

                scrollRect.verticalNormalizedPosition = 1f;
            },
            1, 100);
        }
	}
}

