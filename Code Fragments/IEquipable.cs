public interface IEquipable
{
    private EquipmentSlot _slot;
    private EquipmentSlot _secondarySlot;
    
    public EquipmentSlot Slot;
    public EquipmentSlot SecondarySlot;
}

public enum EquipmentSlot
{
    Head, //helm
    LeftEye,
    RightEye,
    Ears,
    Nose,
    Face,
    Neck,
    Body,
    LeftWrist,
    RightWrist,
    LeftUpperArm,
    RightUpperArm,
    LeftForearm,
    RightForearm,
    LeftHand,
    RightHand,
    LeftFingers,
    RightFingers,
    UnderclothesTop,
    UnderclothesBottom,
    Legs,
    LeftAnkle,
    RightAnkle,
    Feet
}