using System;

[Serializable]
public class EntityEquipInfo {
    public EquipInfo RightHand; 
    public EquipInfo LeftHand; 
    public EquipInfo HeadTop; 
    public EquipInfo HeadBottom; 
    public EquipInfo HeadMid; 
    public EquipInfo Gargment;
}

public class EquipInfo {
    public short ViewID;
    public EquipmentLocation Location;
}
