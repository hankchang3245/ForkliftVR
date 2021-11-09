namespace edu.tnu.dgd.vehicle
{
    public enum VehicleDrivetrainType
    {
        FWD = 1, // Front Wheel Drive
        RWD = 2, // Rear Wheel Drive
        AWD = 3 // All Wheels Drive
    }

    public enum VehicleSteeringMode
    {
        FrontWheelsSteering = 1,
        RearWheelsSteering = 2
        //ArticulatedSteering
    }

    public enum VehicleSpeedUnit
    {
        MPH = 1,
        KPH = 2
    }

    public enum VehicleTransmissionType
    {
        Automatic = 1,
        Manual = 2
    }

    public enum VehicleCameraType
    {
        TPS = 1,
        FPS = 2,
        TopDown = 3
    }

    public enum VehicleCameraLookDirection
    {
        Center = 1,
        Forward = 2,
        Backward = 3,
        RightTop = 4,
        RightBottom = 5,
        RightBackward = 6,
        LeftTop = 7,
        LeftBottom = 8,
        LeftBackward = 9,
        Up = 10,
        Down = 11
    }

    public enum VehicleGearPosition
    {
        Drive = 1,
        Neutral = 2,
        Reverse = 3,
        Park = 4
    }

    public enum VehicleTranslation
    {
        Forward = 1,
        Reverse = 2,
        Idle = 3
    }
}