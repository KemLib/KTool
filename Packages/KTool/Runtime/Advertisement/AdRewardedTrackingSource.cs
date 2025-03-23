using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTool.Advertisement
{
    public class AdRewardedTrackingSource : AdRewardedTracking
    {
        #region Properties

        #endregion

        #region Event
        public void Displayed(bool isSuccess)
        {
            PushEvent_Displayed(isSuccess);
        }
        public void ShowComplete(bool isSuccess)
        {
            PushEvent_ShowComplete(isSuccess);
        }
        public void Hidden()
        {
            PushEvent_Hidden();
        }
        public void Clicked()
        {
            PushEvent_Clicked();
        }
        protected void ReceivedReward(AdRewardReceived adRewardReceived)
        {
            PushEvent_ReceivedReward(adRewardReceived);
        }
        public void RevenuePaid(AdRevenuePaid adRevenuePaid)
        {
            PushEvent_RevenuePaid(adRevenuePaid);
        }
        #endregion
    }
}
