using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Introduction.Iot
{
    using System.Threading;
    using Windows.Devices.Gpio;

    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private GpioPin _pin5;
        private GpioPin _pin2;
        private GpioPin _pin3;
        private GpioPin _pin4;
        private Timer _intervalTimer;
        private Timer _turnOffTimer;
        private readonly Random _random = new Random();

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // 
            // TODO: Insert code to perform background work
            //
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral
            //

            _deferral = taskInstance.GetDeferral();
            Start();
        }

        private void Start()
        {
            var controller = GpioController.GetDefault();
            if (controller is null)
                return;
            Initialize(controller);
            _intervalTimer = new Timer(CloseRelay, null, 0, 2000);
            
        }

        // this turns the circuit on
        private void CloseRelay(object state)
        {
            var result = _random.Next(2, 6);
            switch (result)
            {
                
                case 2:
                    _pin2.Write(GpioPinValue.Low);
                    break;
                case 3:
                    _pin3.Write(GpioPinValue.Low);
                    break;
                case 4:
                    _pin4.Write(GpioPinValue.Low);
                    break;
                case 5:
                    _pin5.Write(GpioPinValue.Low);
                    break;
            }

            _turnOffTimer?.Dispose();
            _turnOffTimer = new Timer(OpenRelay, null, 1000, Timeout.Infinite);
        }

        // this turns the circuit off
        private void OpenRelay(object state)
        {
            _pin5.Write(GpioPinValue.High);
            _pin2.Write(GpioPinValue.High);
            _pin3.Write(GpioPinValue.High);
            _pin4.Write(GpioPinValue.High);
        }

        // this method initializes all the pins
        private void Initialize(GpioController controller)
        {
            _pin2 = controller.OpenPin(2, GpioSharingMode.Exclusive);
            _pin2.SetDriveMode(GpioPinDriveMode.Output);
            _pin2.Write(GpioPinValue.High);
            _pin3 = controller.OpenPin(3, GpioSharingMode.Exclusive);
            _pin3.SetDriveMode(GpioPinDriveMode.Output);
            _pin3.Write(GpioPinValue.High);
            _pin4 = controller.OpenPin(4, GpioSharingMode.Exclusive);
            _pin4.SetDriveMode(GpioPinDriveMode.Output);
            _pin4.Write(GpioPinValue.High);
            _pin5 = controller.OpenPin(5, GpioSharingMode.Exclusive);
            _pin5.SetDriveMode(GpioPinDriveMode.Output);
            _pin5.Write(GpioPinValue.High);
        }
    }
}
