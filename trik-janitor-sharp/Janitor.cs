using System;
using System.Threading;
using System.Threading.Tasks;
using Trik;
using Trik.Sensors;

namespace trik_janitor_sharp
{
    class Janitor : OmniWheeledRobot
    {
        private readonly ServoMotor _kicker;
        private readonly ObjectSensor _objectSensor;
        private readonly object _locker = new object();
        private bool _stopDetectingFlag;

        public Janitor()
            : base()
        {
            _kicker = new ServoMotor(Trik.Ports.Servo.E1.Path, new Collections.ServoMotor.Kind(590000, 2000000, 1295000, 0, 20000000));
            ObjectSensorConfig = new Tuple<string, string, string>("/etc/init.d/object-sensor.sh",
                                                                    "/run/object-sensor.in.fifo",
                                                                    "/run/object-sensor.out.fifo");
            _objectSensor = ObjectSensor;
            _objectSensor.Start();
        }


        public void RotateToCenter()
        {
            var loc = _objectSensor.Read();
            if (loc.Mass > 0) return;
            while (loc.Mass == 0)
            {
                StartRotate();
                loc = _objectSensor.Read();
            }
            Stop();
            FollowDetection();
        }

        public void LearnCenter()
        {
            _objectSensor.Detect(new Collections.HSV(0, 20, 70, 30, 60, 40));
        }

        public void TraceDetection()
        {
            var loc = _objectSensor.Read();
            Console.WriteLine(loc.X + "," + loc.Y + ":" + loc.Mass);
        }

        public void FollowDetection(bool continual = false)
        {
            lock (_locker)
            {
                _stopDetectingFlag = false;
            }
            var worker = new Thread(new ParameterizedThreadStart(DoFollowDetection));
            worker.Start(continual);
        }

        private int __rotationCounter;
        private void DoFollowDetection(object continual)
        {
            var cont = (bool)continual;
            __rotationCounter = 0;
            while (true)
            {
                var loc = _objectSensor.Read();
                Console.WriteLine(loc.X + "," + loc.Y + ":" + loc.Mass);
                const int safeZone = 10;
                var diff = Math.Abs(loc.X);
                var power = diff > 50 ? 100 : (int)(diff / 1.5);
                if (loc.X > safeZone)
                    StartRotate(power: power);
                if (loc.X < -safeZone)
                    StartRotate(false, power);
                if (loc.X >= -safeZone && loc.X <= safeZone)
                    StopRotation();
                if (cont)
                {
                    if (loc.X * __rotationCounter >= 0)
                        __rotationCounter += Math.Sign(loc.X);
                    else
                        __rotationCounter = 0;
                    if (Math.Abs(__rotationCounter) > 10) _stopDetectingFlag = true;
                }
                lock (_locker)
                {
                    if (_stopDetectingFlag)
                    {
                        break;
                    }
                }
            }
        }

        public void StopFollowing()
        {
            lock (_locker)
            {
                _stopDetectingFlag = true;
            }
        }

        public void StartJob()
        {
            while (true)
            {
                RotateToCenter();
                DetectNextObstacle();
                MoveToNextObstacle();
                Kick();
            }
        }

        public void Kick(int power = 100)
        {
            const int millisecondsTimeout = 150;
            _kicker.SetPower(-100);
            Thread.Sleep(millisecondsTimeout);
            _kicker.Release();
            Thread.Sleep(millisecondsTimeout);
            _kicker.SetPower(100);
            Thread.Sleep(millisecondsTimeout);
            _kicker.Release();
        }

        private void MoveToNextObstacle()
        {

        }

        private void DetectNextObstacle()
        {

        }

        //        public void Dispose()
        //        {
        //            _objectSensor.Dispose();
        //        }

        public void Kicker(int power)
        {
            _kicker.SetPower(power);
        }
    }
}
