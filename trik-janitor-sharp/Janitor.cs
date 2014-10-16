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
        private readonly AnalogSensor _frontSensor;
        public Janitor()
            : base()
        {
            _kicker = new ServoMotor(Trik.Ports.Servo.E1.Path, new Collections.ServoMotor.Kind(590000, 2000000, 1295000, 0, 20000000));
            ObjectSensorConfig = new Tuple<string, string, string>("/etc/init.d/object-sensor.sh",
                                                                    "/run/object-sensor.in.fifo",
                                                                    "/run/object-sensor.out.fifo");
            _objectSensor = ObjectSensor;
            _objectSensor.Start();
            _frontSensor = AnalogSensor["A1"];
        }


        public void RotateToCenter()
        {
            var loc = _objectSensor.Read();
            if (loc.Mass <= 5)
            {
                StartRotate();
                while (loc.Mass <= 5)
                {
                    loc = _objectSensor.Read();
                }
            }
            Stop();
            FollowDetection(true);
        }

        public void LearnCenter()
        {
            _objectSensor.Detect(new Collections.HSV(0, 20, 70, 30, 80, 20));
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
            var worker = new Thread(DoFollowDetection);
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
                    if (Math.Abs(__rotationCounter) > 15) _stopDetectingFlag = true;
                }
                lock (_locker)
                {
                    if (!_stopDetectingFlag) continue;
                    StopRotation();
                    break;
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
            const int millisecondsTimeout = 100;
            _kicker.SetPower(-100);
            Thread.Sleep(millisecondsTimeout);
            _kicker.Release();
            Thread.Sleep(millisecondsTimeout);
            _kicker.SetPower(100);
            Thread.Sleep(millisecondsTimeout);
            _kicker.Release();
        }

        private const int __centerSensorValue = 500;
        private void MoveToNextObstacle()
        {
            var val = _frontSensor.Read();
            while (val > __centerSensorValue)
            {
                MoveRadial();
                val = _frontSensor.Read();
            }
        }

        private void DetectNextObstacle()
        {

        }

        public void GetDetection()
        {
            Console.WriteLine(_frontSensor.Read());
        }

        public void Kicker(int power)
        {
            _kicker.SetPower(power);
        }

        public void MoveRadial()
        {
            _frontRightMotor.SetPower(50);
            _frontLeftMotor.SetPower(50);
            _backLeftMotor.SetPower(-100);
            _backRightMotor.SetPower(-100);
        }
    }
}
