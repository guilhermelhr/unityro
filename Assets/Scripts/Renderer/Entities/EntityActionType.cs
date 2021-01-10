using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public enum EntityActionType : byte {
        ATTACK,
        PICKUP,
        SIT,
        STAND,
        CONTINUOUS_ATTACK = 7,
        TOUCH_SKILL = 12
    }
