using UnityEngine;

namespace KTool.Cron
{
    public class ConditionResourceRequest : Condition
    {
        #region Properties
        private ResourceRequest request;
        #endregion

        #region Construction
        internal ConditionResourceRequest(ResourceRequest request) : base()
        {
            this.request = request;
        }
        #endregion

        #region Methods
        protected override void OnCheck()
        {
            if(request == null)
            {
                SetComplete();
                return;
            }
            //
            if (request.isDone)
            {
                SetComplete();
                request = null;
            }
        }
        #endregion

        #region Build
        public static Condition Create(ResourceRequest request)
        {
            return new ConditionResourceRequest(request);
        }
        #endregion
    }
}
