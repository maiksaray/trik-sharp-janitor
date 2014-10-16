namespace trik_janitor_sharp
{
    public class OmniConfig
    {
        //Facing of a controller, wheel ports depend on this param
        public Facing Facing { get; set; }

        //Length of the 'leg' from center of robot to the wheel in cm
        public int LegLength { get; set; }

        //Radius of the OmniWheel in mm
        public int WheelSize { get; set; }
    }
}
