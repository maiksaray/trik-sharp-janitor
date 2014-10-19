using System;
using Trik;

namespace trik_janitor_sharp
{
    class OmniWheeledRobot : Model
    {
        private PowerMotor _frontRightMotor;
        private PowerMotor _frontLeftMotor;
        private PowerMotor _backRightMotor;
        private PowerMotor _backLeftMotor;
        private static readonly OmniConfig DefaultConfig = new OmniConfig { Facing = Facing.Front, LegLength = 15, WheelSize = 30 };

        public OmniWheeledRobot(OmniConfig config = null)
        {
            if (config == null) config = DefaultConfig;

            InitPowerMotors(config.Facing);
        }

        private void InitPowerMotors(Facing facing)
        {
            switch (facing)
            {
                case Facing.Back:
                    {
                        _backLeftMotor = Motor["M2"];
                        _backRightMotor = Motor["M1"];
                        _frontRightMotor = Motor["M3"];
                        _frontLeftMotor = Motor["M4"];
                        break;
                    }
                case Facing.Front:
                    {
                        _backLeftMotor = Motor["M3"];
                        _backRightMotor = Motor["M4"];
                        _frontRightMotor = Motor["M2"];
                        _frontLeftMotor = Motor["M1"];
                        break;
                    }
                case Facing.Custom:
                    {
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException("facing");
            }
        }

        public void MoveForward()
        {
            MoveStraight();
        }

        public void MoveBackward()
        {
            MoveStraight(angle: 180);
        }

        public void MoveRightward()
        {
            MoveStraight(angle: 90);
        }

        public void MoveLeftWard()
        {
            MoveStraight(angle: -90);
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
            power = !clockwise ? power : -power;
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
                //                var fraction = rotation / distance;
                //                DoStartMoving(angle + fraction);
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
                if (angle <= 45 && angle >= 0)
                {
                    rPower = 100;
                    lPower = 100 - (angle / 45) * 100;
                }
                if (angle < 0 && angle >= -45)
                {
                    lPower = 100;
                    rPower = 100 - (-angle / 45) * 100;
                }
                _frontLeftMotor.SetPower((int)rPower);
                _backRightMotor.SetPower(-(int)rPower);
                _frontRightMotor.SetPower(-(int)lPower);
                _backLeftMotor.SetPower((int)lPower);
                return;
            }
            if (angle < -45 && angle >= -135)
            {
                angle = angle + 90;
                if (angle <= 45 && angle >= 0)
                {
                    rPower = 100;
                    lPower = 100 - (angle / 45) * 100;
                }
                if (angle < 0 && angle >= -45)
                {
                    lPower = 100;
                    rPower = 100 - (-angle / 45) * 100;
                }
                _backLeftMotor.SetPower((int)rPower);
                _frontRightMotor.SetPower(-(int)rPower);
                _frontLeftMotor.SetPower(-(int)lPower);
                _backRightMotor.SetPower((int)lPower);
                return;
            }
            if (angle <= 135 && angle >= 45)
            {
                angle = angle - 90;
                if (angle <= 45 && angle >= 0)
                {
                    rPower = 100;
                    lPower = 100 - (angle / 45) * 100;
                }
                if (angle < 0 && angle >= -45)
                {
                    lPower = 100;
                    rPower = 100 - (-angle / 45) * 100;
                }
                _frontRightMotor.SetPower((int)rPower);
                _backLeftMotor.SetPower(-(int)rPower);
                _backRightMotor.SetPower(-(int)lPower);
                _frontLeftMotor.SetPower((int)lPower);
                return;
            }
            if (angle > 0)
                angle = angle - 180;
            else
                angle = angle + 180;
            if (angle <= 45 && angle >= 0)
            {
                rPower = 100;
                lPower = 100 - (angle / 45) * 100;
            }
            if (angle < 0 && angle >= -45)
            {
                lPower = 100;
                rPower = 100 - (-angle / 45) * 100;
            }
            _backLeftMotor.SetPower(-(int)lPower);
            _frontRightMotor.SetPower((int)lPower);
            _frontLeftMotor.SetPower(-(int)rPower);
            _backRightMotor.SetPower((int)rPower);
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

        public void MoveRadial()
        {
            _frontRightMotor.SetPower(100);
            _frontLeftMotor.SetPower(100);
            _backLeftMotor.SetPower(-50);
            _backRightMotor.SetPower(-20);
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
