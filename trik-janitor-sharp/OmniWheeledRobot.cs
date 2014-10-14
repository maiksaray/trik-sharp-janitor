using System;
using Trik;

namespace trik_janitor_sharp
{
    class OmniWheeledRobot : Model
    {
        public readonly PowerMotor _frontRightMotor;
        public readonly PowerMotor _frontLeftMotor;
        public readonly PowerMotor _backRightMotor;
        public readonly PowerMotor _backLeftMotor;

        public OmniWheeledRobot()
            : base()
        {
            this._frontLeftMotor = Motor["M1"];
            //hacked
            this._backLeftMotor = Motor["M3"];
            this._backRightMotor = Motor["M4"];
            //------
            this._frontRightMotor = Motor["M2"];
        }


        public void MoveForward()
        {
            MoveStraight();
        }

        public void MoveBackward()
        {
            _frontRightMotor.SetPower(50);
            _frontLeftMotor.SetPower(-50);
            _backLeftMotor.SetPower(50);
            _backRightMotor.SetPower(-50);
        }

        public void MoveRightward()
        {
            _frontRightMotor.SetPower(50);
            _frontLeftMotor.SetPower(50);
            _backLeftMotor.SetPower(-50);
            _backRightMotor.SetPower(-50);
        }

        public void MoveLeftWard()
        {
            _frontRightMotor.SetPower(-50);
            _frontLeftMotor.SetPower(-50);
            _backLeftMotor.SetPower(50);
            _backRightMotor.SetPower(50);
        }

        public void StopRotation()
        {
            Stop();
        }

        /// <summary>
        /// Rotates the robot 
        /// </summary>
        /// <param name="clockwise">clockwise if true, default is true</param>
        public void StartRotate(bool clockwise = true, int power = 100)
        {
            power = clockwise ? power : -power;
            _backLeftMotor.SetPower(power);
            _backRightMotor.SetPower(power);
            _frontLeftMotor.SetPower(power);
            _frontRightMotor.SetPower(power);
        }
        /// <summary>
        /// Moves robot by straight line, rotating
        /// </summary>
        /// <param name="distance">distance to cover</param>
        /// <param name="angle">angle relative to front, stating direction to move (0 < x < 360)</param>
        /// <param name="rotation">rotation to perform againgst center of robot</param>
        public void MoveStraight(double distance = Double.PositiveInfinity, double angle = 0, double rotation = 0)
        {
            if (Double.IsInfinity(distance) || distance == 0)
                DoStartMoving(angle);
            else
            {
                var fraction = rotation / distance;
                DoStartMoving(angle + fraction);
            }
        }

        private double _currentAngle;

        private void DoStartMoving(double angle)
        {
            if (angle > 180) angle = 180 - angle;
            if (angle < -180) return;
            double rPower = 0;
            double lPower = 0;
            if (angle <= 45 && angle >= -45)
            {
                Console.WriteLine("[-45;45]");
                if (angle <= 45 && angle >= 0)
                {
                    rPower = 100;
                    //full-metal kostyl
                    lPower = 100 - (angle / 45) * 100;
                }
                if (angle < 0 && angle >= -45)
                {
                    lPower = 100;
                    rPower = 100 - (-angle / 45) * 100;
                }
                Console.WriteLine("rpower:" + rPower);
                Console.WriteLine("lpower:" + lPower);
                _frontLeftMotor.SetPower((int)rPower);
                Console.Write("FL:" + rPower);
                _backRightMotor.SetPower(-(int)rPower);
                Console.Write("BR:-" + rPower);
                _frontRightMotor.SetPower(-(int)lPower);
                Console.Write("FR:-" + lPower);
                _backLeftMotor.SetPower((int)lPower);
                Console.Write("BL:" + lPower);
                Console.WriteLine();
                return;
            }
            if (angle < -45 && angle >= -135)
            {
                Console.WriteLine("[-135;-45]");
                angle = angle + 90;
                Console.WriteLine("now " + angle);
                if (angle <= 45 && angle >= 0)
                {
                    rPower = 100;
                    //full-metal kostyl
                    lPower = 100 - (angle / 45) * 100;
                }
                if (angle < 0 && angle >= -45)
                {
                    lPower = 100;
                    rPower = 100 - (-angle / 45) * 100;
                }
                Console.WriteLine("rpower:" + rPower);
                Console.WriteLine("lpower:" + lPower);
                _backLeftMotor.SetPower((int)rPower);
                Console.Write("FL:" + rPower);
                _frontRightMotor.SetPower(-(int)rPower);
                Console.Write("BR:-" + rPower);
                _frontLeftMotor.SetPower(-(int)lPower);
                Console.Write("FR:-" + lPower);
                _backRightMotor.SetPower((int)lPower);
                Console.Write("BL:" + lPower);
                Console.WriteLine();
                return;
            }
            if (angle <= 135 && angle >= 45)
            {
                Console.WriteLine("[45;135]");
                angle = angle - 90;
                Console.WriteLine("now " + angle);
                if (angle <= 45 && angle >= 0)
                {
                    rPower = 100;
                    //full-metal kostyl
                    lPower = 100 - (angle / 45) * 100;
                }
                if (angle < 0 && angle >= -45)
                {
                    lPower = 100;
                    rPower = 100 - (-angle / 45) * 100;
                }
                Console.WriteLine("rpower:" + rPower);
                Console.WriteLine("lpower:" + lPower);
                _frontRightMotor.SetPower((int)rPower);
                Console.Write("FL:" + rPower);
                _backLeftMotor.SetPower(-(int)rPower);
                Console.Write("BR    :-" + rPower);
                _backRightMotor.SetPower(-(int)lPower);
                Console.Write("FR:-" + lPower);
                _frontLeftMotor.SetPower((int)lPower);
                Console.Write("BL:" + lPower);
                Console.WriteLine();
                return;
            }
            //from 135 to 225 or (from -135 to -180 and from 135 to 180)
            Console.WriteLine("[135; -135]");
            if (angle > 0) angle = angle - 180;
            else angle = angle + 180;
            Console.WriteLine("now:" + angle);

            if (angle <= 45 && angle >= 0)
            {
                rPower = 100;
                //full-metal kostyl
                lPower = 100 - (angle / 45) * 100;
            }
            if (angle < 0 && angle >= -45)
            {
                lPower = 100;
                rPower = 100 - (-angle / 45) * 100;
            }
            Console.WriteLine("rpower:" + rPower);
            Console.WriteLine("lpower:" + lPower);
            _backLeftMotor.SetPower(-(int)lPower);
            Console.Write("FL:" + rPower);
            _frontRightMotor.SetPower((int)lPower);
            Console.Write("BR:-" + rPower);
            _frontLeftMotor.SetPower(-(int)rPower);
            Console.Write("FR:-" + lPower);
            _backRightMotor.SetPower((int)rPower);
            Console.Write("BL:" + lPower);
            Console.WriteLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="angle"></param>
        /// <param name="radius">curvature radius</param>
        /// <param name="rotation">rotation angle in degrees</param>
        public void MoveCurve(double distance, double angle, double radius, double rotation = 0)
        {

        }

        /// <summary>
        /// Should be deprecated, now just exists
        /// </summary>
        /// <param name="rotation">In degrees</param>
        public void JustRotate(double rotation)
        {

        }

        public void Stop()
        {
            _frontRightMotor.SetPower(0);
            _frontLeftMotor.SetPower(0);
            _backLeftMotor.SetPower(0);
            _backRightMotor.SetPower(0);
        }
    }
}
