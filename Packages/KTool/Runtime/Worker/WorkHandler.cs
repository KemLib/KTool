using Unity.Mathematics;
using UnityEngine;
using KLibStandard.Concurrent;

namespace KTool.Worker
{
    public class WorkHandler
    {
        #region Properties
        internal const int STATE_PENDING = (int)StateType.Pending,
            STATE_SUCCESS = (int)StateType.Success,
            STATE_CANCELED = (int)StateType.Canceled,
            STATE_FAILED = (int)StateType.Failed,
            STATE_TIMEOUT = (int)StateType.Timeout;

        private InterValueFloat _progress;
        private InterValueInt _state;

        public float Progress
        {
            get => IsDone ? 1 : _progress.Value;
            set
            {
                if(IsDone)
                    return;
                _progress.Value = math.clamp(value, 0f, 1f);
            }
        }
        public bool IsDone => _state.Value != STATE_PENDING;
        public bool IsSuccess => _state.Value == STATE_SUCCESS;
        public bool IsCanceled => _state.Value == STATE_CANCELED;
        public bool IsFailed => _state.Value == STATE_FAILED;
        public bool IsTimeout => _state.Value == STATE_TIMEOUT;
        #endregion

        #region Constructors
        internal WorkHandler()
        {
            _progress = new(0);
            _state = new(STATE_PENDING);
        }
        #endregion

        #region Methods
        public void Success()
        {
            _state.CompareExchange(STATE_SUCCESS, STATE_PENDING);
        }
        public void Fail()
        {
            _state.CompareExchange(STATE_FAILED, STATE_PENDING);
        }
        internal void Cancel()
        {
            _state.CompareExchange(STATE_CANCELED, STATE_PENDING);
        }
        internal void Timeout()
        {
            _state.CompareExchange(STATE_TIMEOUT, STATE_PENDING);
        }
        #endregion
    }
}
