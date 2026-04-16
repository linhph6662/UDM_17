using System;
using System.Windows.Forms;

namespace UDM_17.Client
{
    public class TimerManager : IDisposable
    {
        private System.Windows.Forms.Timer? _timer;
        public event EventHandler? Tick;

        public int Interval
        {
            get => _timer != null ? _timer.Interval : 0;
            set { if (_timer != null) _timer.Interval = value; }
        }

        public bool IsRunning => _timer != null && _timer.Enabled;

        public TimerManager(int interval = 1000)
        {
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = interval;
            _timer.Tick += (s, e) => Tick?.Invoke(this, EventArgs.Empty);
        }

        public void Start()
        {
            _timer?.Start();
        }

        public void Stop()
        {
            _timer?.Stop();
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }
    }
}
