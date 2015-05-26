public interface IEquipable
{
    private EquipmentSlot _slot;
    private EquipmentSlot _secondarySlot;
    
    public EquipmentSlot Slot; //the primary slot the item is equiped into
    public EquipmentSlot SecondarySlot; //the secondary slot the item is equiped into
}

public enum EquipmentSlot
{
    Head, //helms, hats, hoods
    Head2, //wigs, fake ears, headphones
    LeftEye, //monicles, lenses
    RightEye,
    Ears, //earrings, ear plugs
    Nose, //rings, plugs
    Face, //mask, 
    UpperFace, //glasses
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