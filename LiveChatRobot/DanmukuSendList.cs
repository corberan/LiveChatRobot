using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveChatRobot
{
    class DanmukuSendList
    {
        double lastSendTime = 0;

        public void Add(System.Timers.Timer timer)
        {
            double nowTime = DateTime.Now.Subtract(DateTime.Parse("1970-1-1")).TotalMilliseconds;
            // 延时一点，避免网络原因导致的误差
            if (nowTime - lastSendTime > 1100)
            {
                timer.Interval = 100;
                lastSendTime = nowTime + 100;
            }
            else
            {
                timer.Interval = lastSendTime + 1100 - nowTime;
                lastSendTime += 1100;
            }
            timer.Start();
        }
    }
}
