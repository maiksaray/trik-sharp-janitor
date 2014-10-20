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
                                robot.FollowDetection();
                                break;
                            }
                        case "orient":
                            {
                                robot.FollowDetection(true);
                                break;
                            }
                        case "stop":
                            {
                                robot.StopFollowing();
                                break;
                            }
                        case "sense":
                            {
                                robot.GetDetection();
                                break;
                            }
                        case "kickass":
                            {
                                robot.StartSensing();
                                break;
                            }
                        case "stops":
                            {
                                robot.StopSensing();
                                break;
                            }
                        case "radial":
                            {
                                robot.MoveRadial();
                                break;
                            }
                        case "start":
                            {
                                robot.StartJob();
                            }
                        default:
                            {
                                try
                                {
                                    if (cmd.StartsWith("m"))
                                    {
                                        var angle = int.Parse(cmd.Substring(1));
                                        robot.MoveStraight(angle: angle);
                                    }                                   
                                    break;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("invalid input");
                                    break;
                                }
                            }
                    }
                    cmd = Console.ReadLine();
                }
                Console.WriteLine("Bye");
            }
            Console.WriteLine("Now, bye for sure");
        }
    }
}
