using System;

[Serializable]
public class EntityEquipInfo {
    public EquipmentInfo Weapon; 
    public EquipmentInfo Shield; 
    public EquipmentInfo HeadTop; 
    public EquipmentInfo HeadBottom; 
    public EquipmentInfo HeadMid; 
    public EquipmentInfo Robe; 
} 

public class EquipmentInfo {
    public short ViewID;
    public EquipmentLocation EquipmentLocation;
}
