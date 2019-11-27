using UnityEngine;

namespace ylib.Services.UI
{
    public class PlayFabViewBase : MonoBehaviour
    {
        protected enum State
        {
            State = 0,
            Login,
            Load,
            Idle,
        }
        private State state;

        protected virtual void OnInitialize() { }

        protected virtual void OnUpdateLogin() { }
        protected virtual void OnUpdateLoad() { }
        protected virtual void OnUpdateIdle() { }

        protected virtual void OnChangeLogin() { }
        protected virtual void OnChangeLoad() { }
        protected virtual void OnChangeIdle() { }

        public void Awake()
        {
            state = State.Login;

            OnInitialize();
        }

        public void Update()
        {
            switch (state)
            {
                case State.Login:
                    {
                        OnUpdateLogin();

                        if (ylib.Services.PlayFabPlayerData.IsAlreadyLogin())
                        {
                            ChangeState(State.Load);
                        }
                        break;
                    }
                case State.Load:
                    {
                        OnUpdateLoad();
                        break;
                    }
                case State.Idle:
                    {
                        OnUpdateIdle();
                        break;
                    }
            }
        }

        protected void ChangeState(State state)
        {
            this.state = state;

            switch (state)
            {
                case State.Login:
                    {
                        OnChangeLogin();
                        break;
                    }
                case State.Load:
                    {
                        OnChangeLoad();
                        break;
                    }
                case State.Idle:
                    {
                        OnChangeIdle();
                        break;
                    }
            }
        }
    }
}
