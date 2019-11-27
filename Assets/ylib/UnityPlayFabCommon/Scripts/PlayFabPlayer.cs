using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace ylib.Services
{
	public class PlayFabPlayer
	{
        /// <summary>
        /// 表示名の更新
        /// </summary>
        /// <param name="name">表示名</param>
        /// <param name="onSuccess">更新成功後に実行するアクション</param>
        /// <param name="onFailed">更新失敗後に実行するアクション</param>
		public static void UpdateName(string name, System.Action<string> onSuccess = null, System.Action<PlayFabError> onFailed = null)
		{
			var request = new UpdateUserTitleDisplayNameRequest
			{
				DisplayName = name
			};

			PlayFabClientAPI.UpdateUserTitleDisplayName(request,
                // success
                (result) =>
                {
                    Debug.Log("PlayFabPlayer.SetName() success!");

                    PlayFabPlayerData.Instance.UpdateDisplayName(result);

                    if(onSuccess != null)
                    {
                        onSuccess(result.DisplayName);
                    }
                },
                // error
                (error) =>
                {
                    Debug.LogError($"{error.Error}");

                    if (onFailed != null)
                    {
                        onFailed(error);
                    }
                });
		}
	}
}
