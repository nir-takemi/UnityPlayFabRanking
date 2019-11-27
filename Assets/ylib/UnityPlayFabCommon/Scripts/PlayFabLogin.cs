using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace ylib.Services
{
    public class PlayFabLogin : MonoBehaviour
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        private const string cLoginCustomIdKey = "UNITY_PLAYFAB_WEBGL_LOGIN_CUSTOM_ID";
        private bool isCreateId;
#endif

        public bool IsLoadComplete { get; private set; }
        public bool HasError { get; private set; }
        public PlayFabError LastError { get; private set; }

        public string UsedID { get; private set; }

        public void Awake()
        {
            IsLoadComplete = false;
            HasError = false;
            LastError = null;

#if UNITY_EDITOR
            LoginWithEditor();
#elif UNITY_IOS
            LoginWithIOS();
#elif UNITY_ANDROID
            LoginWithAndroid();
#elif UNITY_WEBGL
            LoginWithWebGL();
#endif
        }

#if UNITY_EDITOR
        public void LoginWithEditor()
        {
            string id = SystemInfo.deviceUniqueIdentifier;
            UsedID = id;

            var request = new LoginWithCustomIDRequest { CustomId = id, CreateAccount = true };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }
#elif UNITY_IOS
        public void LoginWithIOS()
        {
            string uuid = UnityEngine.iOS.Device.vendorIdentifier;
            UsedID = uuid;

            var request = new LoginWithIOSDeviceIDRequest
            {
                DeviceId = uuid,
                CreateAccount = true
            };
            PlayFabClientAPI.LoginWithIOSDeviceID(request, OnLoginSuccess, OnLoginFailure);
        }
#elif UNITY_ANDROID
        public void LoginWithAndroid()
        {
            AndroidJavaClass clsUnity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject objActivity = clsUnity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
            AndroidJavaClass clsSecure = new AndroidJavaClass("android.provider.Settings$Secure");
            string androidId = clsSecure.CallStatic<string>("getString", objResolver, "android_id");
            UsedID = androidId;

            var request = new LoginWithAndroidDeviceIDRequest
            {
                AndroidDevice = SystemInfo.deviceModel,
                AndroidDeviceId = androidId,
                CreateAccount = true
            };
            PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginSuccess, OnLoginFailure);
        }
#elif UNITY_WEBGL
        private string CreateRandomCustomId()
        {
            const int concatNum = 6;

            string result = "";

            for (int i = 0; i < concatNum; ++i)
            {
                result += System.Guid.NewGuid().ToString("N").Substring(0, 5);

                if(i < concatNum-1)
                {
                    result += "-";
                }
            }

            return result;
        }
        public static void DeleteLoginCustomIdWithWebGL()
        {
            PlayerPrefs.DeleteKey(cLoginCustomIdKey);
            PlayerPrefs.Save();
        }
        public void LoginWithWebGL()
        {
            string id = "";

            if (PlayerPrefs.HasKey(cLoginCustomIdKey))
            {
                id = PlayerPrefs.GetString(cLoginCustomIdKey);
            }
            else
            {
                id = CreateRandomCustomId();
                isCreateId = true;
                PlayerPrefs.SetString(cLoginCustomIdKey, id);
                PlayerPrefs.Save();
            }
            UsedID = id;

            var request = new LoginWithCustomIDRequest { CustomId = id, CreateAccount = true };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }
#endif

        private void OnLoginSuccess(LoginResult result)
        {
            var playFabId = result.PlayFabId;

#if !UNITY_EDITOR && UNITY_WEBGL
            if (isCreateId && !result.NewlyCreated)
            {
                PlayFabLogin.DeleteLoginCustomIdWithWebGL();
                LoginWithWebGL();
                Debug.Log(string.Format("Already createID...PlayerID = {0}", playFabId));
                return;
            }
#endif

            Debug.Log(string.Format("PlayFabLogin success!! PlayerID = {0}", playFabId));

            LoadPlayerProfile(playFabId);
        }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogError("PlayFabLogin failed!");
            Debug.LogError(error.GenerateErrorReport());

            HasError = true;
            LastError = error;
        }

        /// <summary>
        /// PlayerProfileのロード
        /// </summary>
        /// <param name="playFabId">PlayFabId</param>
        void LoadPlayerProfile(string playFabId)
        {
            var request = new GetPlayerProfileRequest
            {
                PlayFabId = playFabId,
                ProfileConstraints = new PlayerProfileViewConstraints()
                {
                    ShowDisplayName = true
                }
            };

            PlayFabClientAPI.GetPlayerProfile(request,
                (getPlayerProfileResult) =>
                {
                    Debug.Log("GetPlayerProfile success!!");
                    PlayFabPlayerData playFabPlayerData = PlayFabPlayerData.Instance;

                    playFabPlayerData.SetData(getPlayerProfileResult.PlayerProfile);
                    Debug.Log(playFabPlayerData);

                    IsLoadComplete = true;
                },
                (error) =>
                {
                    Debug.LogError("GetPlayerProfile failed!");
                    Debug.LogError(error.GenerateErrorReport());

                    HasError = true;
                    LastError = error;
                });
        }
    }
}
