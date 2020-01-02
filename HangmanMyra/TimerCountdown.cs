using System;
using System.Timers;

namespace HangmanMyra
{
    internal class TimerCountdown
    {
        private System.Timers.Timer timer;
        public int i;

        public Action OnTimeOver { get; set; }

        public void Stop()
        {
            timer.Stop();
            timer.Close();
            timer.Dispose();
        }

        public void SetTimer()
        {
            i = 60;
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();
        }

        public void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            i--;

            if (i == 0)
            {
                OnTimeOver?.Invoke();
                Stop();
            }
        }
    }
}