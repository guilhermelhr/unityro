﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SkillInfo {
    public short SkillID;
    public int SkillType;
    public short Level;
    public short SpCost;
    public short AttackRange;
    public string SkillName;
    public bool CanUpgrade;

    public Skill data;
    public Texture2D texture;
}
