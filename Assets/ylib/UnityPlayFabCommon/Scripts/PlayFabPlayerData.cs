using UnityEngine;

using PlayFab.ClientModels;

namespace ylib.Services
{
    public class PlayFabPlayerData
    {
        public const string cEmptyDisplayName = "<no name>";

        public string PlayerID { get; private set; }
        public string DisplayName { get; private set; }

        private static PlayFabPlayerData instance;

        public static PlayFabPlayerData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayFabPlayerData();
                    instance.Initialize();
                }

                return instance;
            }
        }

        private void Initialize()
        {
            PlayerID = null;
            DisplayName = null;
        }

        static public bool IsValid()
        {
            return (instance != null);
        }

        static public bool IsAlreadyLogin()
        {
            return ( IsValid() && (instance.PlayerID != null) );
        }

        public void SetData(PlayerProfileModel model)
        {
            PlayerID = model.PlayerId;
            DisplayName = (model.DisplayName != null) ? model.DisplayName : cEmptyDisplayName;
        }

        public void UpdateDisplayName(UpdateUserTitleDisplayNameResult result)
        {
            DisplayName = result.DisplayName;
        }

        public override string ToString()
        {
            return string.Format("PlayerID={0}, DisplayName={1}", PlayerID, DisplayName);
        }
    }
}
