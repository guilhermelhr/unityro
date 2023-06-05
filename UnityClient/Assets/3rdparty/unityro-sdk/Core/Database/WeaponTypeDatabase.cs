namespace UnityRO.Core.Database {
    public static class WeaponTypeDatabase {
        private const int SEX_MALE = 1;
        private const int SEX_FEMALE = 0;

        public static WeaponType MakeWeaponType(int left, int right) {
            WeaponType type = WeaponType.NONE;
            if (left <= 0 && right > 0) {
                left = right;
                right = 0;
            }

            if ((left >= 1100 && left < 1150) || (left >= 13400 && left < 13500) || (left >= 500000 && left < 509999)) { // 한손검
                type = WeaponType.SWORD;
                if ((right >= 1100 && right < 1150) || (right >= 13400 && right < 13500) || (right >= 500000 && right < 509999)) // 한손검
                    return WeaponType.SWORD_SWORD;

                if ((right < 1200 || right >= 1250)
                    && (right < 13000 || right >= 13100)
                    && (right < 28700 || right >= 28900)
                    && (right >= 510000 && right < 519999)) {
                    if (right >= 1300 && right < 1350 || right >= 520000 && right < 529999)
                        return WeaponType.SWORD_AXE;
                    return type;
                }

                if (right >= 1300 && right < 1350 || (right >= 520000 && right < 529999)) // 한손도끼
                    return WeaponType.SWORD_AXE;

                return type;
            }

            if ((left >= 1200 && left < 1250) || (left >= 13000 && left < 13100) || (left >= 28700 && left < 28900) ||
                (left >= 510000 && left < 519999)) { // 단검
                type = WeaponType.SHORTSWORD;
                if ((right < 1100 || right >= 1150) && (right < 13400 || right >= 13500) && right >= 500000 && right < 509999) {
                    if ((right >= 1200 && right < 1250) || (right >= 13000 && right < 13100) || (right >= 28700 && right < 28900) ||
                        (right >= 510000 && right < 519999)) // 단검
                        return WeaponType.SHORTSWORD_SHORTSWORD;

                    if ((right >= 1300 && right < 1350) || (right >= 520000 && right < 529999)) // 한손도끼
                        return WeaponType.SHORTSWORD_AXE;

                    return type;
                }

                return WeaponType.SHORTSWORD_SWORD;
            }

            if (left >= 1300 && left < 1350 || left >= 520000 && left < 529999) {
                type = WeaponType.AXE;
                if (right >= 1300 && right < 1350)
                    return WeaponType.AXE_AXE;
                if (right >= 520000 && right < 529999)
                    return WeaponType.AXE_AXE;
            }

            return type;
        }

        public static WeaponType GetWeaponType(int itemID) {
            if (itemID <= 0) return WeaponType.NONE;

            var type = itemID switch {
                >= 1100 and < 1150 => 2,
                >= 13400 and < 13500 => 2,
                >= 1150 and < 1200 => 3,
                >= 1150 and < 1250 => 1,
                >= 1150 and < 1300 => 16,
                >= 1150 and < 1350 => 6,
                >= 1150 and < 1400 => 7,
                >= 1150 and < 1450 => 4,
                >= 1150 and < 1500 when itemID != 1472 && itemID != 1473 => 5,
                >= 1150 and < 1500 => 10,
                >= 1150 and < 1550 => 8,
                >= 1150 and < 1600 => 15,
                >= 1150 and < 1700 => 10,
                >= 1150 and < 1750 => 11,
                >= 1800 and < 1900 => 12,
                >= 1800 and < 1950 => 13,
                >= 1800 and < 2000 => 14,
                >= 1800 and < 2100 => 23,
                >= 13150 and < 13200 => 18,
                < 13000 => itemID,
                < 13100 => 1,
                >= 13150 and < 13300 or >= 13400 => -1,
                >= 13150 => 22,
                _ => 17
            };

            return (WeaponType)type;
        }

        public static WeaponType GetRealWeaponId(int weapon) {
            switch ((WeaponType)weapon) {
                case WeaponType.Main_Gauche: return WeaponType.SHORTSWORD;
                case WeaponType.Stiletto: return WeaponType.SHORTSWORD;
                case WeaponType.Gladius: return WeaponType.SHORTSWORD;
                case WeaponType.Zeny_Knife: return WeaponType.SHORTSWORD;
                case WeaponType.Poison_Knife: return WeaponType.SHORTSWORD;
                case WeaponType.Princess_Knife: return WeaponType.SHORTSWORD;
                case WeaponType.Sasimi: return WeaponType.SHORTSWORD;
                case WeaponType.Lacma: return WeaponType.SHORTSWORD;
                case WeaponType.Tsurugi: return WeaponType.SWORD;
                case WeaponType.Ring_Pommel_Saber: return WeaponType.SWORD;
                case WeaponType.Haedonggum: return WeaponType.SWORD;
                case WeaponType.Saber: return WeaponType.SWORD;
                case WeaponType.Jewel_Sword: return WeaponType.SWORD;
                case WeaponType.Gaia_Sword: return WeaponType.SWORD;
                case WeaponType.Twin_Edge_B: return WeaponType.SWORD;
                case WeaponType.Twin_Edge_R: return WeaponType.SWORD;
                case WeaponType.Priest_Sword: return WeaponType.SWORD;
                case WeaponType.Katana: return WeaponType.TWOHANDSWORD;
                case WeaponType.Bastard_Sword: return WeaponType.TWOHANDSWORD;
                case WeaponType.Broad_Sword: return WeaponType.TWOHANDSWORD;
                case WeaponType.Violet_Fear: return WeaponType.TWOHANDSWORD;
                case WeaponType.Lance: return WeaponType.SPEAR;
                case WeaponType.Partizan: return WeaponType.SPEAR;
                case WeaponType.Trident: return WeaponType.SPEAR;
                case WeaponType.Halberd: return WeaponType.SPEAR;
                case WeaponType.Crescent_Scythe: return WeaponType.SPEAR;
                case WeaponType.Zephyrus: return WeaponType.SPEAR;
                case WeaponType.Hammer: return WeaponType.AXE;
                case WeaponType.Buster: return WeaponType.AXE;
                case WeaponType.Brood_Axe: return WeaponType.AXE;
                case WeaponType.Right_Epsilon: return WeaponType.AXE;
                case WeaponType.Mace: return WeaponType.MACE;
                case WeaponType.Sword_Mace: return WeaponType.MACE;
                case WeaponType.Chain: return WeaponType.MACE;
                case WeaponType.Stunner: return WeaponType.MACE;
                case WeaponType.Golden_Mace: return WeaponType.MACE;
                case WeaponType.Iron_Driver: return WeaponType.MACE;
                case WeaponType.Spanner: return WeaponType.MACE;
                case WeaponType.Spoon: return WeaponType.MACE;
                case WeaponType.Arc_Wand: return WeaponType.ROD;
                case WeaponType.Mighty_Staff: return WeaponType.ROD;
                case WeaponType.Blessed_Wand: return WeaponType.ROD;
                case WeaponType.Bone_Wand: return WeaponType.ROD;
                case WeaponType.CrossBow: return WeaponType.BOW;
                case WeaponType.Arbalest: return WeaponType.BOW;
                case WeaponType.Kakkung: return WeaponType.BOW;
                case WeaponType.Hunter_Bow: return WeaponType.BOW;
                case WeaponType.Bow_Of_Rudra: return WeaponType.BOW;
                case WeaponType.Waghnakh: return WeaponType.KNUKLE;
                case WeaponType.Knuckle_Duster: return WeaponType.KNUKLE;
                case WeaponType.Hora: return WeaponType.KNUKLE;
                case WeaponType.Fist: return WeaponType.KNUKLE;
                case WeaponType.Claw: return WeaponType.KNUKLE;
                case WeaponType.Finger: return WeaponType.KNUKLE;
                case WeaponType.Kaiser_Knuckle: return WeaponType.KNUKLE;
                case WeaponType.Berserk: return WeaponType.KNUKLE;
                case WeaponType.Rante: return WeaponType.WHIP;
                case WeaponType.Tail: return WeaponType.WHIP;
                case WeaponType.Whip: return WeaponType.WHIP;
                case WeaponType.Bible: return WeaponType.BOOK;
                case WeaponType.Book_Of_Billows: return WeaponType.BOOK;
                case WeaponType.Book_Of_Mother_Earth: return WeaponType.BOOK;
                case WeaponType.Book_Of_Blazing_Sun: return WeaponType.BOOK;
                case WeaponType.Book_Of_Gust_Of_Wind: return WeaponType.BOOK;
                case WeaponType.Book_Of_The_Apocalypse: return WeaponType.BOOK;
                case WeaponType.Girls_Diary: return WeaponType.BOOK;
                case WeaponType.Staff_Of_Soul: return WeaponType.TWOHANDROD;
                case WeaponType.Wizardy_Staff: return WeaponType.TWOHANDROD;
                case WeaponType.FOXTAIL_BROWN: return WeaponType.ROD;
                case WeaponType.FOXTAIL_GREEN: return WeaponType.ROD;
                case WeaponType.CandyCaneRod: return WeaponType.ROD;
                case WeaponType.FOXTAIL_METAL: return WeaponType.ROD;
                default:
                    return (WeaponType)weapon;
            }
        }

        public static bool IsSecondAttack(int job, int sex, int weapon, int shield) {
            var isSecondAttack = false;

            if (weapon >= (int)WeaponType.WEAPONTYPE_LAST) {
                if (job is (int)JobType.JT_ASSASSIN or (int)JobType.JT_ASSASSIN_H or (int)JobType.JT_ASSASSIN_B) {
                    int right, left;
                    left = weapon & 0xFFFF;
                    right = (weapon >> 16) & 0xFFFF;
                    if (GetWeaponType(right) > 0) {
                        weapon = (int)MakeWeaponType(left, right);
                    } else {
                        weapon = (int)GetWeaponType((weapon & 0xffff));
                    }
                } else {
                    weapon = (int)GetWeaponType((weapon & 0xffff));
                }
            }

            var realWeapon = GetRealWeaponId(weapon);
            switch ((JobType)job) {
                case JobType.JT_SWORDMAN:
                case JobType.JT_KNIGHT:
                case JobType.JT_CHICKEN:
                case JobType.JT_CRUSADER:
                case JobType.JT_CHICKEN2:
                case JobType.JT_SWORDMAN_H:
                case JobType.JT_KNIGHT_H:
                case JobType.JT_CHICKEN_H:
                case JobType.JT_CRUSADER_H:
                case JobType.JT_CHICKEN2_H:
                case JobType.JT_SWORDMAN_B:
                case JobType.JT_KNIGHT_B:
                case JobType.JT_CHICKEN_B:
                case JobType.JT_CRUSADER_B:
                case JobType.JT_CHICKEN2_B:
                case JobType.JT_RUNE_KNIGHT:
                case JobType.JT_RUNE_KNIGHT_H:
                case JobType.JT_ROYAL_GUARD:
                case JobType.JT_ROYAL_GUARD_H:
                case JobType.JT_RUNE_CHICKEN:
                case JobType.JT_RUNE_CHICKEN_H:
                case JobType.JT_ROYAL_CHICKEN:
                case JobType.JT_ROYAL_CHICKEN_H:
                case JobType.JT_RUNE_CHICKEN2:
                case JobType.JT_RUNE_CHICKEN2_H:
                case JobType.JT_RUNE_CHICKEN3:
                case JobType.JT_RUNE_CHICKEN3_H:
                case JobType.JT_RUNE_CHICKEN4:
                case JobType.JT_RUNE_CHICKEN4_H:
                case JobType.JT_RUNE_CHICKEN5:
                case JobType.JT_RUNE_CHICKEN5_H:
                case JobType.JT_RUNE_KNIGHT_B:
                case JobType.JT_ROYAL_GUARD_B:
                case JobType.JT_RUNE_CHICKEN_B:
                case JobType.JT_ROYAL_CHICKEN_B:
                case JobType.JT_DRAGON_KNIGHT:
                case JobType.JT_IMPERIAL_GUARD:
                case JobType.JT_DRAGON_KNIGHT_CHICKEN:
                case JobType.JT_IMPERIAL_GUARD_CHICKEN: {
                    switch (realWeapon) {
                        case WeaponType.SPEAR:
                        case WeaponType.TWOHANDSPEAR:
                            isSecondAttack = true;
                            break;
                    }

                    break;
                }

                case JobType.JT_MAGICIAN:
                case JobType.JT_MERCHANT:
                case JobType.JT_MAGICIAN_H:
                case JobType.JT_MERCHANT_H:
                case JobType.JT_MAGICIAN_B:
                case JobType.JT_MERCHANT_B: {
                    switch (realWeapon) {
                        case WeaponType.SHORTSWORD:
                            isSecondAttack = true;
                            break;
                    }

                    break;
                }

                case JobType.JT_ARCHER:
                case JobType.JT_ARCHER_H:
                case JobType.JT_ARCHER_B: {
                    switch (realWeapon) {
                        case WeaponType.BOW:
                            break;
                        default:
                            isSecondAttack = true;
                            break;
                    }

                    break;
                }

                case JobType.JT_THIEF:
                case JobType.JT_HUNTER:
                case JobType.JT_ROGUE:
                case JobType.JT_BARD:
                case JobType.JT_DANCER:
                case JobType.JT_THIEF_H:
                case JobType.JT_HUNTER_H:
                case JobType.JT_ROGUE_H:
                case JobType.JT_BARD_H:
                case JobType.JT_DANCER_H:
                case JobType.JT_THIEF_B:
                case JobType.JT_HUNTER_B:
                case JobType.JT_ROGUE_B:
                case JobType.JT_BARD_B:
                case JobType.JT_DANCER_B:
                case JobType.JT_RANGER:
                case JobType.JT_RANGER_H:
                case JobType.JT_MINSTREL:
                case JobType.JT_WANDERER:
                case JobType.JT_SHADOW_CHASER:
                case JobType.JT_MINSTREL_H:
                case JobType.JT_WANDERER_H:
                case JobType.JT_SHADOW_CHASER_H:
                case JobType.JT_WOLF_RANGER:
                case JobType.JT_WOLF_RANGER_H:
                case JobType.JT_RANGER_B:
                case JobType.JT_MINSTREL_B:
                case JobType.JT_WANDERER_B:
                case JobType.JT_SHADOW_CHASER_B:
                case JobType.JT_WOLF_RANGER_B:
                case JobType.JT_WINDHAWK:
                case JobType.JT_ABYSS_CHASER:
                case JobType.JT_TROUBADOUR:
                case JobType.JT_TROUVERE:
                case JobType.JT_WOLF_WINDHAWK: {
                    switch (realWeapon) {
                        case WeaponType.BOW:
                            isSecondAttack = true;
                            break;
                    }

                    break;
                }

                case JobType.JT_PRIEST:
                case JobType.JT_PRIEST_H:
                case JobType.JT_PRIEST_B:
                case JobType.JT_ARCHBISHOP:
                case JobType.JT_ARCHBISHOP_H:
                case JobType.JT_ARCHBISHOP_B:
                case JobType.JT_CARDINAL: {
                    switch (realWeapon) {
                        case WeaponType.BOOK:
                            isSecondAttack = true;
                            break;
                    }

                    break;
                }

                case JobType.JT_WIZARD:
                case JobType.JT_WIZARD_H:
                case JobType.JT_WIZARD_B:
                case JobType.JT_LINKER:
                case JobType.JT_WARLOCK:
                case JobType.JT_WARLOCK_H:
                case JobType.JT_WARLOCK_B:
                case JobType.JT_LINKER_B:
                case JobType.JT_SOUL_REAPER:
                case JobType.JT_SOUL_REAPER_B:
                case JobType.JT_ARCH_MAGE:
                case JobType.JT_SOUL_ASCETIC: {
                    switch (realWeapon) {
                        case WeaponType.SHORTSWORD:
                            if (sex == SEX_MALE) isSecondAttack = true;
                            break;
                        case WeaponType.ROD:
                        case WeaponType.TWOHANDROD:
                            if (sex == SEX_FEMALE) isSecondAttack = true;
                            break;
                    }

                    break;
                }

                case JobType.JT_BLACKSMITH:
                case JobType.JT_ALCHEMIST:
                case JobType.JT_BLACKSMITH_H:
                case JobType.JT_ALCHEMIST_H:
                case JobType.JT_BLACKSMITH_B:
                case JobType.JT_ALCHEMIST_B:
                case JobType.JT_MECHANIC:
                case JobType.JT_MECHANIC_H:
                case JobType.JT_GENETIC:
                case JobType.JT_GENETIC_H:
                case JobType.JT_MADOGEAR:
                case JobType.JT_MADOGEAR_H:
                case JobType.JT_MECHANIC_B:
                case JobType.JT_GENETIC_B:
                case JobType.JT_MADOGEAR_B:
                case JobType.JT_MEISTER:
                case JobType.JT_BIOLO:
                case JobType.JT_MEISTER_MADOGEAR: {
                    switch (realWeapon) {
                        case WeaponType.SWORD:
                        case WeaponType.AXE:
                        case WeaponType.TWOHANDAXE:
                        case WeaponType.MACE:
                            isSecondAttack = true;
                            break;
                    }

                    break;
                }

                case JobType.JT_ASSASSIN:
                case JobType.JT_ASSASSIN_H:
                case JobType.JT_ASSASSIN_B:
                case JobType.JT_GUILLOTINE_CROSS:
                case JobType.JT_GUILLOTINE_CROSS_H:
                case JobType.JT_GUILLOTINE_CROSS_B:
                case JobType.JT_SHADOW_CROSS: {
                    switch (realWeapon) {
                        case WeaponType.CATARRH:
                        case WeaponType.SHORTSWORD_SHORTSWORD:
                        case WeaponType.SHORTSWORD_SWORD:
                        case WeaponType.SHORTSWORD_AXE:
                        case WeaponType.SWORD_SWORD:
                        case WeaponType.SWORD_AXE:
                        case WeaponType.AXE_AXE:
                            isSecondAttack = true;
                            break;
                    }

                    break;
                }

                case JobType.JT_MONK:
                case JobType.JT_MONK_H:
                case JobType.JT_MONK_B:
                case JobType.JT_SURA:
                case JobType.JT_SURA_H:
                case JobType.JT_SURA_B:
                case JobType.JT_INQUISITOR: {
                    switch (realWeapon) {
                        case WeaponType.KNUKLE:
                        case WeaponType.NONE:
                            isSecondAttack = true;
                            break;
                    }

                    break;
                }

                case JobType.JT_SAGE:
                case JobType.JT_SAGE_H:
                case JobType.JT_SAGE_B:
                case JobType.JT_SORCERER:
                case JobType.JT_SORCERER_H:
                case JobType.JT_SORCERER_B:
                case JobType.JT_ELEMENTAL_MASTER: {
                    switch (realWeapon) {
                        case WeaponType.BOOK:
                        case WeaponType.ROD:
                        case WeaponType.TWOHANDROD:
                        case WeaponType.TWOHANDSPEAR: // 양손스태프의 무기 아이디가 제대로 수정되면 지울것.
                            isSecondAttack = true;
                            break;
                    }

                    break;
                }

                case JobType.JT_NOVICE:
                case JobType.JT_NOVICE_H:
                case JobType.JT_SUPERNOVICE:
                case JobType.JT_NOVICE_B:
                case JobType.JT_SUPERNOVICE_B:
                case JobType.JT_SUPERNOVICE2:
                case JobType.JT_SUPERNOVICE2_B:
                case JobType.JT_HYPER_NOVICE: {
                    switch (sex) {
                        case SEX_FEMALE:
                            switch (realWeapon) {
                                case WeaponType.SWORD:
                                case WeaponType.TWOHANDSWORD:
                                case WeaponType.AXE:
                                case WeaponType.TWOHANDAXE:
                                case WeaponType.ROD:
                                case WeaponType.TWOHANDROD:
                                case WeaponType.MACE:
                                case WeaponType.TWOHANDMACE:
                                    break;
                                case WeaponType.SHORTSWORD:
                                    isSecondAttack = true;
                                    break;
                            }

                            break;
                        case SEX_MALE:
                            switch (realWeapon) {
                                case WeaponType.SWORD:
                                case WeaponType.TWOHANDSWORD:
                                case WeaponType.AXE:
                                case WeaponType.TWOHANDAXE:
                                case WeaponType.ROD:
                                case WeaponType.TWOHANDROD:
                                case WeaponType.MACE:
                                case WeaponType.TWOHANDMACE:
                                    isSecondAttack = true;
                                    break;
                                case WeaponType.SHORTSWORD:
                                    break;
                            }

                            break;
                    }

                    break;
                }

                case JobType.JT_NINJA:
                case JobType.JT_KAGEROU:
                case JobType.JT_OBORO:
                case JobType.JT_NINJA_B:
                case JobType.JT_KAGEROU_B:
                case JobType.JT_OBORO_B:
                case JobType.JT_SHINKIRO:
                case JobType.JT_SHIRANUI: {
                    switch (realWeapon) {
                        case WeaponType.SYURIKEN:
                            isSecondAttack = true;
                            break;
                    }

                    break;
                }

                case JobType.JT_GUNSLINGER:
                case JobType.JT_REBELLION:
                case JobType.JT_GUNSLINGER_B:
                case JobType.JT_REBELLION_B:
                case JobType.JT_NIGHT_WATCH: {
                    switch (realWeapon) {
                        case WeaponType.GUN_RIFLE:
                        case WeaponType.GUN_GATLING:
                        case WeaponType.GUN_SHOTGUN:
                        case WeaponType.GUN_GRANADE:
                            isSecondAttack = true;
                            break;
                    }

                    break;
                }

                case JobType.JT_GANGSI:
                case JobType.JT_DEATHKNIGHT:
                case JobType.JT_COLLECTOR: {
                    // 임시
                    break;
                }

                case JobType.JT_TAEKWON:
                case JobType.JT_STAR:
                case JobType.JT_STAR2: {
                    break;
                }

                case JobType.JT_ACOLYTE:
                case JobType.JT_ACOLYTE_H:
                case JobType.JT_ACOLYTE_B: {
                    break;
                }
            }

            return isSecondAttack;
        }

        public static bool IsWeaponUsingArrow(int weapon) {
            var weaponType = weapon > (int)WeaponType.WEAPONTYPE_LAST ? GetWeaponType(weapon) : (WeaponType)weapon;
            return weaponType is WeaponType.BOW or
                WeaponType.CrossBow or
                WeaponType.Arbalest or
                WeaponType.Kakkung or
                WeaponType.Hunter_Bow or
                WeaponType.Bow_Of_Rudra;
        }
    }
}