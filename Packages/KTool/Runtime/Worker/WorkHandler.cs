using Unity.Mathematics;
using UnityEngine;
using KLibStandard.Concurrent;

namespace KTool.Worker
{
    public class WorkHandler : IWorker
    {
        private enum State
        {
            Pending,
            Success,
            Canceled,
            Failed,
            Timeout,
        }
        #region Properties
        private const int STATE_PENDING = (int)State.Pending;
        private const int STATE_SUCCESS = (int)State.Success;
        private const int STATE_CANCELED = (int)State.Canceled;
        private const int STATE_FAILED = (int)State.Failed;
        private const int STATE_TIMEOUT = (int)State.Timeout;

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
        public void Cancel()
        {
            _state.CompareExchange(STATE_CANCELED, STATE_PENDING);
        }
        public void Fail()
        {
            _state.CompareExchange(STATE_FAILED, STATE_PENDING);
        }
        internal void Timeout()
        {
            _state.CompareExchange(STATE_TIMEOUT, STATE_PENDING);
        }
        #endregion
    }
}
