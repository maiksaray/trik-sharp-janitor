using System;
namespace trik_janitor_sharp
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            Console.WriteLine("Hello world!");
            using (var robot = new Janitor())
            {
                Console.WriteLine("Waiting...");
                var cmd = Console.ReadLine();
                while (cmd != "e")
                {
                    switch (cmd)
                    {
                        case "fr":
                            {
                                robot._frontRightMotor.SetPower(100);
                                break;
                            }
                        case "br":
                            {
                                robot._backRightMotor.SetPower(100);
                                break;
                            }
                        case "bl":
                            {
                                robot._backLeftMotor.SetPower(100);
                                break;
                            }
                        case "fl":
                            {
                                robot._frontLeftMotor.SetPower(100);
                                break;
                            }
                        case "f":
                            {
                                robot.MoveForward();
                                break;
                            }
                        case "b":
                            {
                                robot.MoveBackward();
                                break;
                            }
                        case "r":
                            {
                                robot.MoveRightward();
                                break;
                            }
                        case "l":
                            {
                                robot.MoveLeftWard();
                                break;
                            }
                        case "c":
                            {
                                robot.StartRotate();
                                break;
                            }
                        case "cc":
                            {
                                robot.StartRotate(false);
                                break;
                            }
                        case "s":
                            {
                                robot.Stop();
                                break;
                            }
                        case "k":
                            {
                                robot.Kick();
                                break;
                            }
                        case "t":
                            {
                                robot.TraceDetection();
                                break;
                            }
                        case "begin":
                            {
                                robot.RotateToCenter();
                                break;
                            }
                        case "detect":
                            {
                                robot.LearnCenter();
                                break;
                            }
                        case "follow":
                            {
                                robot.FollowDetection(true);
                                break;
                            }
                        case "stop":
                            {
                                robot.StopFollowing();
                                break;
                            }
                        default:
                            {
                                if (cmd.StartsWith("m"))
                                {
                                    var angle = int.Parse(cmd.Substring(1));
                                    robot.MoveStraight(angle: angle);
                                }
                                if (cmd.StartsWith("k"))
                                {
                                    var power = int.Parse(cmd.Substring(1));
                                    robot.Kicker(power);
                                }
                                break;
                            }
                    }
                    cmd = Console.ReadLine();
                }
            }
        }
    }
}
