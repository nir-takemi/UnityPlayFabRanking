using System.Collections.Generic;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

namespace ylib.Services
{
    public class PlayFabRanking
    {
        public struct RankingData
        {
            public int rank;
            public string displayName;
            public string playFabId;
            public int score;

            public RankingData(int rank, string displayName, int score)
            {
                this.rank = rank;
                this.displayName = displayName;
                this.playFabId = "";
                this.score = score;
            }
            public RankingData(int rank, string displayName, string playFabId, int score)
            {
                this.rank = rank;
                this.displayName = displayName;
                this.playFabId = playFabId;
                this.score = score;
            }
        }

        /// <summary>
        /// スコア送信
        /// </summary>
        /// <param name="rankingName">ランキング名</param>
        /// <param name="score">スコア</param>
        /// <param name="onSuccess">送信成功後に実行するアクション</param>
        /// <param name="onFailed">送信失敗後に実行するアクション</param>
        public static void SendPlayScore(string rankingName, int score, System.Action onSuccess = null, System.Action<PlayFabError> onFailed = null)
        {
            var statisticUpdate = new StatisticUpdate
            {
                StatisticName = rankingName,
                Value = score,
            };

            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
            {
                statisticUpdate
            }
            };

            PlayFabClientAPI.UpdatePlayerStatistics(request,
                // success
                (updatePlayerStatisticsResult) =>
                {
                    Debug.Log("PlayFabRanking.SendPlayScore() success!");

                    if (onSuccess != null)
                    {
                        onSuccess();
                    }
                },
                // error
                (error) =>
                {
                    Debug.Log($"{error.Error}");

                    if (onFailed != null)
                    {
                        onFailed(error);
                    }
                });
        }

        /// <summary>
        /// ランキング一覧の取得
        /// </summary>
        /// <param name="rankingName">ランキング名</param>
        /// <param name="afterGetAction">取得後に実行するアクション</param>
        /// <param name="startRank">取得開始rank(default=1)</param>
        /// <param name="rankingSize">ランク数(default=10/max=100)</param>
        /// <param name="onFailed">送信失敗後に実行するアクション</param>
        public static void GetRankingData(string rankingName, System.Action<List<RankingData>> afterGetAction, int startRank = 1, int rankingSize = 10, System.Action<PlayFabError> onFailed = null)
        {
            var request = new GetLeaderboardRequest
            {
                StatisticName = rankingName,
                StartPosition = startRank - 1, //wrapper側ではrank単位で扱いたいため-1してあげる
                MaxResultsCount = rankingSize
            };

            PlayFabClientAPI.GetLeaderboard(request,
                (leaderboardResult) =>
                {
                    Debug.Log("PlayFabRanking.GetRankingData() success!");

                    List<RankingData> rankingDataList = new List<RankingData>();

                    foreach (var item in leaderboardResult.Leaderboard)
                    {
                        RankingData rankingData = new RankingData();

                        rankingData.rank = item.Position + 1;
                        rankingData.displayName = (item.DisplayName != null) ? item.DisplayName : PlayFabPlayerData.cEmptyDisplayName;
                        rankingData.playFabId = item.PlayFabId;
                        rankingData.score = item.StatValue;

                        rankingDataList.Add(rankingData);
                    }

                    afterGetAction(rankingDataList);
                },
                // error
                (error) =>
                {
                    Debug.Log($"{error.Error}");

                    if (onFailed != null)
                    {
                        onFailed(error);
                    }
                });
        }
    }
}
