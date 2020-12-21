public static class JobHelper {
    //The following system marks a different job ID system used by the map server,
    //which makes a lot more sense than the normal one. [Skotlex]
    //
    //These marks the "level" of the job.
    public const ulong JOBL_2_1 = 0x100; //256
    public const ulong JOBL_2_2 = 0x200; //512
    public const ulong JOBL_2 = 0x300; //768

    public const ulong JOBL_UPPER = 0x1000; //4096
    public const ulong JOBL_BABY = 0x2000;  //8192
    public const ulong JOBL_THIRD = 0x4000; //16384

    //for filtering and quick checking.
    public const ulong MAPID_BASEMASK = 0x00ff;
    public const ulong MAPID_UPPERMASK = 0x0fff;
    public const ulong MAPID_THIRDMASK = (JOBL_THIRD | MAPID_UPPERMASK);

    public static Job ServerIdToJobId(ushort class_, int sex) {
        switch ((ServerJob)class_) {
            //Novice And 1-1 Jobs
            case ServerJob.MAPID_NOVICE: return Job.JOB_NOVICE;
            case ServerJob.MAPID_SWORDMAN: return Job.JOB_SWORDMAN;
            case ServerJob.MAPID_MAGE: return Job.JOB_MAGE;
            case ServerJob.MAPID_ARCHER: return Job.JOB_ARCHER;
            case ServerJob.MAPID_ACOLYTE: return Job.JOB_ACOLYTE;
            case ServerJob.MAPID_MERCHANT: return Job.JOB_MERCHANT;
            case ServerJob.MAPID_THIEF: return Job.JOB_THIEF;
            case ServerJob.MAPID_TAEKWON: return Job.JOB_TAEKWON;
            case ServerJob.MAPID_WEDDING: return Job.JOB_WEDDING;
            case ServerJob.MAPID_GUNSLINGER: return Job.JOB_GUNSLINGER;
            case ServerJob.MAPID_NINJA: return Job.JOB_NINJA;
            case ServerJob.MAPID_XMAS: return Job.JOB_XMAS;
            case ServerJob.MAPID_SUMMER: return Job.JOB_SUMMER;
            case ServerJob.MAPID_HANBOK: return Job.JOB_HANBOK;
            case ServerJob.MAPID_GANGSI: return Job.JOB_GANGSI;
            case ServerJob.MAPID_OKTOBERFEST: return Job.JOB_OKTOBERFEST;
            case ServerJob.MAPID_SUMMER2: return Job.JOB_SUMMER2;
            //2-1 Jobs                                   Job.
            case ServerJob.MAPID_SUPER_NOVICE: return Job.JOB_SUPER_NOVICE;
            case ServerJob.MAPID_KNIGHT: return Job.JOB_KNIGHT;
            case ServerJob.MAPID_WIZARD: return Job.JOB_WIZARD;
            case ServerJob.MAPID_HUNTER: return Job.JOB_HUNTER;
            case ServerJob.MAPID_PRIEST: return Job.JOB_PRIEST;
            case ServerJob.MAPID_BLACKSMITH: return Job.JOB_BLACKSMITH;
            case ServerJob.MAPID_ASSASSIN: return Job.JOB_ASSASSIN;
            case ServerJob.MAPID_STAR_GLADIATOR: return Job.JOB_STAR_GLADIATOR;
            case ServerJob.MAPID_KAGEROUOBORO: return sex == 0 ? Job.JOB_KAGEROU : Job.JOB_OBORO;
            case ServerJob.MAPID_REBELLION: return Job.JOB_REBELLION;
            case ServerJob.MAPID_DEATH_KNIGHT: return Job.JOB_DEATH_KNIGHT;
            //2-2 Jobs                                   Job.
            case ServerJob.MAPID_CRUSADER: return Job.JOB_CRUSADER;
            case ServerJob.MAPID_SAGE: return Job.JOB_SAGE;
            case ServerJob.MAPID_BARDDANCER: return sex == 0 ? Job.JOB_BARD : Job.JOB_DANCER;
            case ServerJob.MAPID_MONK: return Job.JOB_MONK;
            case ServerJob.MAPID_ALCHEMIST: return Job.JOB_ALCHEMIST;
            case ServerJob.MAPID_ROGUE: return Job.JOB_ROGUE;
            case ServerJob.MAPID_SOUL_LINKER: return Job.JOB_SOUL_LINKER;
            case ServerJob.MAPID_DARK_COLLECTOR: return Job.JOB_DARK_COLLECTOR;
            //Trans Novice And Trans 2-1 Jobs            Job.
            case ServerJob.MAPID_NOVICE_HIGH: return Job.JOB_NOVICE_HIGH;
            case ServerJob.MAPID_SWORDMAN_HIGH: return Job.JOB_SWORDMAN_HIGH;
            case ServerJob.MAPID_MAGE_HIGH: return Job.JOB_MAGE_HIGH;
            case ServerJob.MAPID_ARCHER_HIGH: return Job.JOB_ARCHER_HIGH;
            case ServerJob.MAPID_ACOLYTE_HIGH: return Job.JOB_ACOLYTE_HIGH;
            case ServerJob.MAPID_MERCHANT_HIGH: return Job.JOB_MERCHANT_HIGH;
            case ServerJob.MAPID_THIEF_HIGH: return Job.JOB_THIEF_HIGH;
            //Trans 2-1 Jobs                             Job.
            case ServerJob.MAPID_LORD_KNIGHT: return Job.JOB_LORD_KNIGHT;
            case ServerJob.MAPID_HIGH_WIZARD: return Job.JOB_HIGH_WIZARD;
            case ServerJob.MAPID_SNIPER: return Job.JOB_SNIPER;
            case ServerJob.MAPID_HIGH_PRIEST: return Job.JOB_HIGH_PRIEST;
            case ServerJob.MAPID_WHITESMITH: return Job.JOB_WHITESMITH;
            case ServerJob.MAPID_ASSASSIN_CROSS: return Job.JOB_ASSASSIN_CROSS;
            //Trans 2-2 Jobs                             Job.
            case ServerJob.MAPID_PALADIN: return Job.JOB_PALADIN;
            case ServerJob.MAPID_PROFESSOR: return Job.JOB_PROFESSOR;
            case ServerJob.MAPID_CLOWNGYPSY: return sex == 0 ? Job.JOB_CLOWN : Job.JOB_GYPSY;
            case ServerJob.MAPID_CHAMPION: return Job.JOB_CHAMPION;
            case ServerJob.MAPID_CREATOR: return Job.JOB_CREATOR;
            case ServerJob.MAPID_STALKER: return Job.JOB_STALKER;
            //Baby Novice And Baby 1-1 Jobs              Job.
            case ServerJob.MAPID_BABY: return Job.JOB_BABY;
            case ServerJob.MAPID_BABY_SWORDMAN: return Job.JOB_BABY_SWORDMAN;
            case ServerJob.MAPID_BABY_MAGE: return Job.JOB_BABY_MAGE;
            case ServerJob.MAPID_BABY_ARCHER: return Job.JOB_BABY_ARCHER;
            case ServerJob.MAPID_BABY_ACOLYTE: return Job.JOB_BABY_ACOLYTE;
            case ServerJob.MAPID_BABY_MERCHANT: return Job.JOB_BABY_MERCHANT;
            case ServerJob.MAPID_BABY_THIEF: return Job.JOB_BABY_THIEF;
            case ServerJob.MAPID_BABY_TAEKWON: return Job.JOB_BABY_TAEKWON;
            case ServerJob.MAPID_BABY_GUNSLINGER: return Job.JOB_BABY_GUNSLINGER;
            case ServerJob.MAPID_BABY_NINJA: return Job.JOB_BABY_NINJA;
            case ServerJob.MAPID_BABY_SUMMONER: return Job.JOB_BABY_SUMMONER;
            //Baby 2-1 Jobs                              Job.
            case ServerJob.MAPID_SUPER_BABY: return Job.JOB_SUPER_BABY;
            case ServerJob.MAPID_BABY_KNIGHT: return Job.JOB_BABY_KNIGHT;
            case ServerJob.MAPID_BABY_WIZARD: return Job.JOB_BABY_WIZARD;
            case ServerJob.MAPID_BABY_HUNTER: return Job.JOB_BABY_HUNTER;
            case ServerJob.MAPID_BABY_PRIEST: return Job.JOB_BABY_PRIEST;
            case ServerJob.MAPID_BABY_BLACKSMITH: return Job.JOB_BABY_BLACKSMITH;
            case ServerJob.MAPID_BABY_ASSASSIN: return Job.JOB_BABY_ASSASSIN;
            case ServerJob.MAPID_BABY_STAR_GLADIATOR: return Job.JOB_BABY_STAR_GLADIATOR;
            case ServerJob.MAPID_BABY_REBELLION: return Job.JOB_BABY_REBELLION;
            case ServerJob.MAPID_BABY_KAGEROUOBORO: return sex == 0 ? Job.JOB_BABY_KAGEROU : Job.JOB_BABY_OBORO;
            //Baby 2-2 Jobs                              Job.
            case ServerJob.MAPID_BABY_CRUSADER: return Job.JOB_BABY_CRUSADER;
            case ServerJob.MAPID_BABY_SAGE: return Job.JOB_BABY_SAGE;
            case ServerJob.MAPID_BABY_BARDDANCER: return sex == 0 ? Job.JOB_BABY_BARD : Job.JOB_BABY_DANCER;
            case ServerJob.MAPID_BABY_MONK: return Job.JOB_BABY_MONK;
            case ServerJob.MAPID_BABY_ALCHEMIST: return Job.JOB_BABY_ALCHEMIST;
            case ServerJob.MAPID_BABY_ROGUE: return Job.JOB_BABY_ROGUE;
            case ServerJob.MAPID_BABY_SOUL_LINKER: return Job.JOB_BABY_SOUL_LINKER;
            //3-1 Jobs                                   Job.
            case ServerJob.MAPID_SUPER_NOVICE_E: return Job.JOB_SUPER_NOVICE_E;
            case ServerJob.MAPID_RUNE_KNIGHT: return Job.JOB_RUNE_KNIGHT;
            case ServerJob.MAPID_WARLOCK: return Job.JOB_WARLOCK;
            case ServerJob.MAPID_RANGER: return Job.JOB_RANGER;
            case ServerJob.MAPID_ARCH_BISHOP: return Job.JOB_ARCH_BISHOP;
            case ServerJob.MAPID_MECHANIC: return Job.JOB_MECHANIC;
            case ServerJob.MAPID_GUILLOTINE_CROSS: return Job.JOB_GUILLOTINE_CROSS;
            case ServerJob.MAPID_STAR_EMPEROR: return Job.JOB_STAR_EMPEROR;
            //3-2 Jobs                                   Job.
            case ServerJob.MAPID_ROYAL_GUARD: return Job.JOB_ROYAL_GUARD;
            case ServerJob.MAPID_SORCERER: return Job.JOB_SORCERER;
            case ServerJob.MAPID_MINSTRELWANDERER: return sex == 0 ? Job.JOB_MINSTREL : Job.JOB_WANDERER;
            case ServerJob.MAPID_SURA: return Job.JOB_SURA;
            case ServerJob.MAPID_GENETIC: return Job.JOB_GENETIC;
            case ServerJob.MAPID_SHADOW_CHASER: return Job.JOB_SHADOW_CHASER;
            case ServerJob.MAPID_SOUL_REAPER: return Job.JOB_SOUL_REAPER;
            //Trans 3-1 Jobs                             Job.
            case ServerJob.MAPID_RUNE_KNIGHT_T: return Job.JOB_RUNE_KNIGHT_T;
            case ServerJob.MAPID_WARLOCK_T: return Job.JOB_WARLOCK_T;
            case ServerJob.MAPID_RANGER_T: return Job.JOB_RANGER_T;
            case ServerJob.MAPID_ARCH_BISHOP_T: return Job.JOB_ARCH_BISHOP_T;
            case ServerJob.MAPID_MECHANIC_T: return Job.JOB_MECHANIC_T;
            case ServerJob.MAPID_GUILLOTINE_CROSS_T: return Job.JOB_GUILLOTINE_CROSS_T;
            //Trans 3-2 Jobs                             Job.
            case ServerJob.MAPID_ROYAL_GUARD_T: return Job.JOB_ROYAL_GUARD_T;
            case ServerJob.MAPID_SORCERER_T: return Job.JOB_SORCERER_T;
            case ServerJob.MAPID_MINSTRELWANDERER_T: return sex == 0 ? Job.JOB_MINSTREL_T : Job.JOB_WANDERER_T;
            case ServerJob.MAPID_SURA_T: return Job.JOB_SURA_T;
            case ServerJob.MAPID_GENETIC_T: return Job.JOB_GENETIC_T;
            case ServerJob.MAPID_SHADOW_CHASER_T: return Job.JOB_SHADOW_CHASER_T;
            //Baby 3-1 Jobs                              Job.
            case ServerJob.MAPID_SUPER_BABY_E: return Job.JOB_SUPER_BABY_E;
            case ServerJob.MAPID_BABY_RUNE_KNIGHT: return Job.JOB_BABY_RUNE_KNIGHT;
            case ServerJob.MAPID_BABY_WARLOCK: return Job.JOB_BABY_WARLOCK;
            case ServerJob.MAPID_BABY_RANGER: return Job.JOB_BABY_RANGER;
            case ServerJob.MAPID_BABY_ARCH_BISHOP: return Job.JOB_BABY_ARCH_BISHOP;
            case ServerJob.MAPID_BABY_MECHANIC: return Job.JOB_BABY_MECHANIC;
            case ServerJob.MAPID_BABY_GUILLOTINE_CROSS: return Job.JOB_BABY_GUILLOTINE_CROSS;
            case ServerJob.MAPID_BABY_STAR_EMPEROR: return Job.JOB_BABY_STAR_EMPEROR;
            //Baby 3-2 Jobs                              Job.
            case ServerJob.MAPID_BABY_ROYAL_GUARD: return Job.JOB_BABY_ROYAL_GUARD;
            case ServerJob.MAPID_BABY_SORCERER: return Job.JOB_BABY_SORCERER;
            case ServerJob.MAPID_BABY_MINSTRELWANDERER: return sex == 0 ? Job.JOB_BABY_MINSTREL : Job.JOB_BABY_WANDERER;
            case ServerJob.MAPID_BABY_SURA: return Job.JOB_BABY_SURA;
            case ServerJob.MAPID_BABY_GENETIC: return Job.JOB_BABY_GENETIC;
            case ServerJob.MAPID_BABY_SHADOW_CHASER: return Job.JOB_BABY_SHADOW_CHASER;
            case ServerJob.MAPID_BABY_SOUL_REAPER: return Job.JOB_BABY_SOUL_REAPER;
            //Doram Jobs                                 Job.
            case ServerJob.MAPID_SUMMONER: return Job.JOB_SUMMONER;
            default:
                return Job.JOB_NOVICE;
        }
    }

    public static ServerJob JobIdToServerId(ushort class_) {
        switch ((Job)class_) {
            case Job.JOB_NOVICE: return ServerJob.MAPID_NOVICE;
            case Job.JOB_SWORDMAN: return ServerJob.MAPID_SWORDMAN;
            case Job.JOB_MAGE: return ServerJob.MAPID_MAGE;
            case Job.JOB_ARCHER: return ServerJob.MAPID_ARCHER;
            case Job.JOB_ACOLYTE: return ServerJob.MAPID_ACOLYTE;
            case Job.JOB_MERCHANT: return ServerJob.MAPID_MERCHANT;
            case Job.JOB_THIEF: return ServerJob.MAPID_THIEF;
            case Job.JOB_TAEKWON: return ServerJob.MAPID_TAEKWON;
            case Job.JOB_WEDDING: return ServerJob.MAPID_WEDDING;
            case Job.JOB_GUNSLINGER: return ServerJob.MAPID_GUNSLINGER;
            case Job.JOB_NINJA: return ServerJob.MAPID_NINJA;
            case Job.JOB_XMAS: return ServerJob.MAPID_XMAS;
            case Job.JOB_SUMMER: return ServerJob.MAPID_SUMMER;
            case Job.JOB_HANBOK: return ServerJob.MAPID_HANBOK;
            case Job.JOB_GANGSI: return ServerJob.MAPID_GANGSI;
            case Job.JOB_OKTOBERFEST: return ServerJob.MAPID_OKTOBERFEST;
            case Job.JOB_SUMMER2: return ServerJob.MAPID_SUMMER2;
            //2-1 Job.JOBs
            case Job.JOB_SUPER_NOVICE: return ServerJob.MAPID_SUPER_NOVICE;
            case Job.JOB_KNIGHT: return ServerJob.MAPID_KNIGHT;
            case Job.JOB_WIZARD: return ServerJob.MAPID_WIZARD;
            case Job.JOB_HUNTER: return ServerJob.MAPID_HUNTER;
            case Job.JOB_PRIEST: return ServerJob.MAPID_PRIEST;
            case Job.JOB_BLACKSMITH: return ServerJob.MAPID_BLACKSMITH;
            case Job.JOB_ASSASSIN: return ServerJob.MAPID_ASSASSIN;
            case Job.JOB_STAR_GLADIATOR: return ServerJob.MAPID_STAR_GLADIATOR;
            case Job.JOB_KAGEROU:
            case Job.JOB_OBORO: return ServerJob.MAPID_KAGEROUOBORO;
            case Job.JOB_REBELLION: return ServerJob.MAPID_REBELLION;
            case Job.JOB_DEATH_KNIGHT: return ServerJob.MAPID_DEATH_KNIGHT;
            //2-2 Job.JOBs
            case Job.JOB_CRUSADER: return ServerJob.MAPID_CRUSADER;
            case Job.JOB_SAGE: return ServerJob.MAPID_SAGE;
            case Job.JOB_BARD:
            case Job.JOB_DANCER: return ServerJob.MAPID_BARDDANCER;
            case Job.JOB_MONK: return ServerJob.MAPID_MONK;
            case Job.JOB_ALCHEMIST: return ServerJob.MAPID_ALCHEMIST;
            case Job.JOB_ROGUE: return ServerJob.MAPID_ROGUE;
            case Job.JOB_SOUL_LINKER: return ServerJob.MAPID_SOUL_LINKER;
            case Job.JOB_DARK_COLLECTOR: return ServerJob.MAPID_DARK_COLLECTOR;
            //Trans Novice And Trans 1-1 Job.JOBs
            case Job.JOB_NOVICE_HIGH: return ServerJob.MAPID_NOVICE_HIGH;
            case Job.JOB_SWORDMAN_HIGH: return ServerJob.MAPID_SWORDMAN_HIGH;
            case Job.JOB_MAGE_HIGH: return ServerJob.MAPID_MAGE_HIGH;
            case Job.JOB_ARCHER_HIGH: return ServerJob.MAPID_ARCHER_HIGH;
            case Job.JOB_ACOLYTE_HIGH: return ServerJob.MAPID_ACOLYTE_HIGH;
            case Job.JOB_MERCHANT_HIGH: return ServerJob.MAPID_MERCHANT_HIGH;
            case Job.JOB_THIEF_HIGH: return ServerJob.MAPID_THIEF_HIGH;
            //Trans 2-1 Job.JOBs
            case Job.JOB_LORD_KNIGHT: return ServerJob.MAPID_LORD_KNIGHT;
            case Job.JOB_HIGH_WIZARD: return ServerJob.MAPID_HIGH_WIZARD;
            case Job.JOB_SNIPER: return ServerJob.MAPID_SNIPER;
            case Job.JOB_HIGH_PRIEST: return ServerJob.MAPID_HIGH_PRIEST;
            case Job.JOB_WHITESMITH: return ServerJob.MAPID_WHITESMITH;
            case Job.JOB_ASSASSIN_CROSS: return ServerJob.MAPID_ASSASSIN_CROSS;
            //Trans 2-2 Job.JOBs
            case Job.JOB_PALADIN: return ServerJob.MAPID_PALADIN;
            case Job.JOB_PROFESSOR: return ServerJob.MAPID_PROFESSOR;
            case Job.JOB_CLOWN:
            case Job.JOB_GYPSY: return ServerJob.MAPID_CLOWNGYPSY;
            case Job.JOB_CHAMPION: return ServerJob.MAPID_CHAMPION;
            case Job.JOB_CREATOR: return ServerJob.MAPID_CREATOR;
            case Job.JOB_STALKER: return ServerJob.MAPID_STALKER;
            //Baby Novice And Baby 1-1 Job.JOBs
            case Job.JOB_BABY: return ServerJob.MAPID_BABY;
            case Job.JOB_BABY_SWORDMAN: return ServerJob.MAPID_BABY_SWORDMAN;
            case Job.JOB_BABY_MAGE: return ServerJob.MAPID_BABY_MAGE;
            case Job.JOB_BABY_ARCHER: return ServerJob.MAPID_BABY_ARCHER;
            case Job.JOB_BABY_ACOLYTE: return ServerJob.MAPID_BABY_ACOLYTE;
            case Job.JOB_BABY_MERCHANT: return ServerJob.MAPID_BABY_MERCHANT;
            case Job.JOB_BABY_THIEF: return ServerJob.MAPID_BABY_THIEF;
            case Job.JOB_BABY_TAEKWON: return ServerJob.MAPID_BABY_TAEKWON;
            case Job.JOB_BABY_GUNSLINGER: return ServerJob.MAPID_BABY_GUNSLINGER;
            case Job.JOB_BABY_NINJA: return ServerJob.MAPID_BABY_NINJA;
            case Job.JOB_BABY_SUMMONER: return ServerJob.MAPID_BABY_SUMMONER;
            //Baby 2-1 Job.JOBs
            case Job.JOB_SUPER_BABY: return ServerJob.MAPID_SUPER_BABY;
            case Job.JOB_BABY_KNIGHT: return ServerJob.MAPID_BABY_KNIGHT;
            case Job.JOB_BABY_WIZARD: return ServerJob.MAPID_BABY_WIZARD;
            case Job.JOB_BABY_HUNTER: return ServerJob.MAPID_BABY_HUNTER;
            case Job.JOB_BABY_PRIEST: return ServerJob.MAPID_BABY_PRIEST;
            case Job.JOB_BABY_BLACKSMITH: return ServerJob.MAPID_BABY_BLACKSMITH;
            case Job.JOB_BABY_ASSASSIN: return ServerJob.MAPID_BABY_ASSASSIN;
            case Job.JOB_BABY_STAR_GLADIATOR: return ServerJob.MAPID_BABY_STAR_GLADIATOR;
            case Job.JOB_BABY_REBELLION: return ServerJob.MAPID_BABY_REBELLION;
            case Job.JOB_BABY_KAGEROU:
            case Job.JOB_BABY_OBORO: return ServerJob.MAPID_BABY_KAGEROUOBORO;
            //Baby 2-2 Job.JOBs
            case Job.JOB_BABY_CRUSADER: return ServerJob.MAPID_BABY_CRUSADER;
            case Job.JOB_BABY_SAGE: return ServerJob.MAPID_BABY_SAGE;
            case Job.JOB_BABY_BARD:
            case Job.JOB_BABY_DANCER: return ServerJob.MAPID_BABY_BARDDANCER;
            case Job.JOB_BABY_MONK: return ServerJob.MAPID_BABY_MONK;
            case Job.JOB_BABY_ALCHEMIST: return ServerJob.MAPID_BABY_ALCHEMIST;
            case Job.JOB_BABY_ROGUE: return ServerJob.MAPID_BABY_ROGUE;
            case Job.JOB_BABY_SOUL_LINKER: return ServerJob.MAPID_BABY_SOUL_LINKER;
            //3-1 Job.JOBs
            case Job.JOB_SUPER_NOVICE_E: return ServerJob.MAPID_SUPER_NOVICE_E;
            case Job.JOB_RUNE_KNIGHT: return ServerJob.MAPID_RUNE_KNIGHT;
            case Job.JOB_WARLOCK: return ServerJob.MAPID_WARLOCK;
            case Job.JOB_RANGER: return ServerJob.MAPID_RANGER;
            case Job.JOB_ARCH_BISHOP: return ServerJob.MAPID_ARCH_BISHOP;
            case Job.JOB_MECHANIC: return ServerJob.MAPID_MECHANIC;
            case Job.JOB_GUILLOTINE_CROSS: return ServerJob.MAPID_GUILLOTINE_CROSS;
            case Job.JOB_STAR_EMPEROR: return ServerJob.MAPID_STAR_EMPEROR;
            //3-2 Job.JOBs
            case Job.JOB_ROYAL_GUARD: return ServerJob.MAPID_ROYAL_GUARD;
            case Job.JOB_SORCERER: return ServerJob.MAPID_SORCERER;
            case Job.JOB_MINSTREL:
            case Job.JOB_WANDERER: return ServerJob.MAPID_MINSTRELWANDERER;
            case Job.JOB_SURA: return ServerJob.MAPID_SURA;
            case Job.JOB_GENETIC: return ServerJob.MAPID_GENETIC;
            case Job.JOB_SHADOW_CHASER: return ServerJob.MAPID_SHADOW_CHASER;
            case Job.JOB_SOUL_REAPER: return ServerJob.MAPID_SOUL_REAPER;
            //Trans 3-1 Job.JOBs
            case Job.JOB_RUNE_KNIGHT_T: return ServerJob.MAPID_RUNE_KNIGHT_T;
            case Job.JOB_WARLOCK_T: return ServerJob.MAPID_WARLOCK_T;
            case Job.JOB_RANGER_T: return ServerJob.MAPID_RANGER_T;
            case Job.JOB_ARCH_BISHOP_T: return ServerJob.MAPID_ARCH_BISHOP_T;
            case Job.JOB_MECHANIC_T: return ServerJob.MAPID_MECHANIC_T;
            case Job.JOB_GUILLOTINE_CROSS_T: return ServerJob.MAPID_GUILLOTINE_CROSS_T;
            //Trans 3-2 Job.JOBs
            case Job.JOB_ROYAL_GUARD_T: return ServerJob.MAPID_ROYAL_GUARD_T;
            case Job.JOB_SORCERER_T: return ServerJob.MAPID_SORCERER_T;
            case Job.JOB_MINSTREL_T:
            case Job.JOB_WANDERER_T: return ServerJob.MAPID_MINSTRELWANDERER_T;
            case Job.JOB_SURA_T: return ServerJob.MAPID_SURA_T;
            case Job.JOB_GENETIC_T: return ServerJob.MAPID_GENETIC_T;
            case Job.JOB_SHADOW_CHASER_T: return ServerJob.MAPID_SHADOW_CHASER_T;
            //Baby 3-1 Job.JOBs
            case Job.JOB_SUPER_BABY_E: return ServerJob.MAPID_SUPER_BABY_E;
            case Job.JOB_BABY_RUNE_KNIGHT: return ServerJob.MAPID_BABY_RUNE_KNIGHT;
            case Job.JOB_BABY_WARLOCK: return ServerJob.MAPID_BABY_WARLOCK;
            case Job.JOB_BABY_RANGER: return ServerJob.MAPID_BABY_RANGER;
            case Job.JOB_BABY_ARCH_BISHOP: return ServerJob.MAPID_BABY_ARCH_BISHOP;
            case Job.JOB_BABY_MECHANIC: return ServerJob.MAPID_BABY_MECHANIC;
            case Job.JOB_BABY_GUILLOTINE_CROSS: return ServerJob.MAPID_BABY_GUILLOTINE_CROSS;
            case Job.JOB_BABY_STAR_EMPEROR: return ServerJob.MAPID_BABY_STAR_EMPEROR;
            //Baby 3-2 Job.JOBs
            case Job.JOB_BABY_ROYAL_GUARD: return ServerJob.MAPID_BABY_ROYAL_GUARD;
            case Job.JOB_BABY_SORCERER: return ServerJob.MAPID_BABY_SORCERER;
            case Job.JOB_BABY_MINSTREL:
            case Job.JOB_BABY_WANDERER: return ServerJob.MAPID_BABY_MINSTRELWANDERER;
            case Job.JOB_BABY_SURA: return ServerJob.MAPID_BABY_SURA;
            case Job.JOB_BABY_GENETIC: return ServerJob.MAPID_BABY_GENETIC;
            case Job.JOB_BABY_SHADOW_CHASER: return ServerJob.MAPID_BABY_SHADOW_CHASER;
            case Job.JOB_BABY_SOUL_REAPER: return ServerJob.MAPID_BABY_SOUL_REAPER;
            //Doram Job.JOBs
            case Job.JOB_SUMMONER: return ServerJob.MAPID_SUMMONER;
            default:
                return ServerJob.MAPID_NOVICE;
        }
    }

    public static Job GetBaseClass(ushort job, int sex) {
        var serverJob = (ulong)JobIdToServerId(job);

        return ServerIdToJobId((ushort)(serverJob & MAPID_BASEMASK), sex);
    }
}

public enum Job : int {
    JOB_NOVICE,
    JOB_SWORDMAN,
    JOB_MAGE,
    JOB_ARCHER,
    JOB_ACOLYTE,
    JOB_MERCHANT,
    JOB_THIEF,
    JOB_KNIGHT,
    JOB_PRIEST,
    JOB_WIZARD,
    JOB_BLACKSMITH,
    JOB_HUNTER,
    JOB_ASSASSIN,
    JOB_KNIGHT2,
    JOB_CRUSADER,
    JOB_MONK,
    JOB_SAGE,
    JOB_ROGUE,
    JOB_ALCHEMIST,
    JOB_BARD,
    JOB_DANCER,
    JOB_CRUSADER2,
    JOB_WEDDING,
    JOB_SUPER_NOVICE,
    JOB_GUNSLINGER,
    JOB_NINJA,
    JOB_XMAS,
    JOB_SUMMER,
    JOB_HANBOK,
    JOB_OKTOBERFEST,
    JOB_SUMMER2,
    JOB_MAX_BASIC,

    JOB_NOVICE_HIGH = 4001,
    JOB_SWORDMAN_HIGH,
    JOB_MAGE_HIGH,
    JOB_ARCHER_HIGH,
    JOB_ACOLYTE_HIGH,
    JOB_MERCHANT_HIGH,
    JOB_THIEF_HIGH,
    JOB_LORD_KNIGHT,
    JOB_HIGH_PRIEST,
    JOB_HIGH_WIZARD,
    JOB_WHITESMITH,
    JOB_SNIPER,
    JOB_ASSASSIN_CROSS,
    JOB_LORD_KNIGHT2,
    JOB_PALADIN,
    JOB_CHAMPION,
    JOB_PROFESSOR,
    JOB_STALKER,
    JOB_CREATOR,
    JOB_CLOWN,
    JOB_GYPSY,
    JOB_PALADIN2,

    JOB_BABY,
    JOB_BABY_SWORDMAN,
    JOB_BABY_MAGE,
    JOB_BABY_ARCHER,
    JOB_BABY_ACOLYTE,
    JOB_BABY_MERCHANT,
    JOB_BABY_THIEF,
    JOB_BABY_KNIGHT,
    JOB_BABY_PRIEST,
    JOB_BABY_WIZARD,
    JOB_BABY_BLACKSMITH,
    JOB_BABY_HUNTER,
    JOB_BABY_ASSASSIN,
    JOB_BABY_KNIGHT2,
    JOB_BABY_CRUSADER,
    JOB_BABY_MONK,
    JOB_BABY_SAGE,
    JOB_BABY_ROGUE,
    JOB_BABY_ALCHEMIST,
    JOB_BABY_BARD,
    JOB_BABY_DANCER,
    JOB_BABY_CRUSADER2,
    JOB_SUPER_BABY,

    JOB_TAEKWON,
    JOB_STAR_GLADIATOR,
    JOB_STAR_GLADIATOR2,
    JOB_SOUL_LINKER,

    JOB_GANGSI,
    JOB_DEATH_KNIGHT,
    JOB_DARK_COLLECTOR,

    JOB_RUNE_KNIGHT = 4054,
    JOB_WARLOCK,
    JOB_RANGER,
    JOB_ARCH_BISHOP,
    JOB_MECHANIC,
    JOB_GUILLOTINE_CROSS,

    JOB_RUNE_KNIGHT_T,
    JOB_WARLOCK_T,
    JOB_RANGER_T,
    JOB_ARCH_BISHOP_T,
    JOB_MECHANIC_T,
    JOB_GUILLOTINE_CROSS_T,

    JOB_ROYAL_GUARD,
    JOB_SORCERER,
    JOB_MINSTREL,
    JOB_WANDERER,
    JOB_SURA,
    JOB_GENETIC,
    JOB_SHADOW_CHASER,

    JOB_ROYAL_GUARD_T,
    JOB_SORCERER_T,
    JOB_MINSTREL_T,
    JOB_WANDERER_T,
    JOB_SURA_T,
    JOB_GENETIC_T,
    JOB_SHADOW_CHASER_T,

    JOB_RUNE_KNIGHT2,
    JOB_RUNE_KNIGHT_T2,
    JOB_ROYAL_GUARD2,
    JOB_ROYAL_GUARD_T2,
    JOB_RANGER2,
    JOB_RANGER_T2,
    JOB_MECHANIC2,
    JOB_MECHANIC_T2,

    JOB_BABY_RUNE_KNIGHT = 4096,
    JOB_BABY_WARLOCK,
    JOB_BABY_RANGER,
    JOB_BABY_ARCH_BISHOP,
    JOB_BABY_MECHANIC,
    JOB_BABY_GUILLOTINE_CROSS,
    JOB_BABY_ROYAL_GUARD,
    JOB_BABY_SORCERER,
    JOB_BABY_MINSTREL,
    JOB_BABY_WANDERER,
    JOB_BABY_SURA,
    JOB_BABY_GENETIC,
    JOB_BABY_SHADOW_CHASER,

    JOB_BABY_RUNE_KNIGHT2,
    JOB_BABY_ROYAL_GUARD2,
    JOB_BABY_RANGER2,
    JOB_BABY_MECHANIC2,

    JOB_SUPER_NOVICE_E = 4190,
    JOB_SUPER_BABY_E,

    JOB_KAGEROU = 4211,
    JOB_OBORO,

    JOB_REBELLION = 4215,

    JOB_SUMMONER = 4218,

    JOB_BABY_SUMMONER = 4220,

    JOB_BABY_NINJA = 4222,
    JOB_BABY_KAGEROU,
    JOB_BABY_OBORO,
    JOB_BABY_TAEKWON,
    JOB_BABY_STAR_GLADIATOR,
    JOB_BABY_SOUL_LINKER,
    JOB_BABY_GUNSLINGER,
    JOB_BABY_REBELLION,

    JOB_BABY_STAR_GLADIATOR2 = 4238,

    JOB_STAR_EMPEROR,
    JOB_SOUL_REAPER,
    JOB_BABY_STAR_EMPEROR,
    JOB_BABY_SOUL_REAPER,
    JOB_STAR_EMPEROR2,
    JOB_BABY_STAR_EMPEROR2,

    JOB_MAX
}

//First Jobs
//Note the oddity of the novice:
//Super Novices are considered the 2-1 version of the novice! Novices are considered a first class type, too...
public enum ServerJob : ulong {
    //Novice And 1-1 Jobs
    MAPID_NOVICE = 0x0,
    MAPID_SWORDMAN,
    MAPID_MAGE,
    MAPID_ARCHER,
    MAPID_ACOLYTE,
    MAPID_MERCHANT,
    MAPID_THIEF,
    MAPID_TAEKWON,
    MAPID_WEDDING,
    MAPID_GUNSLINGER,
    MAPID_NINJA,
    MAPID_XMAS,
    MAPID_SUMMER,
    MAPID_HANBOK,
    MAPID_GANGSI,
    MAPID_OKTOBERFEST,
    MAPID_SUMMONER,
    MAPID_SUMMER2,
    //2-1 Jobs
    MAPID_SUPER_NOVICE = JobHelper.JOBL_2_1 | MAPID_NOVICE,
    MAPID_KNIGHT,
    MAPID_WIZARD,
    MAPID_HUNTER,
    MAPID_PRIEST,
    MAPID_BLACKSMITH,
    MAPID_ASSASSIN,
    MAPID_STAR_GLADIATOR,
    MAPID_REBELLION = JobHelper.JOBL_2_1 | MAPID_GUNSLINGER,
    MAPID_KAGEROUOBORO,
    MAPID_DEATH_KNIGHT = JobHelper.JOBL_2_1 | MAPID_GANGSI,
    //2-2 Jobs
    MAPID_CRUSADER = JobHelper.JOBL_2_2 | MAPID_SWORDMAN,
    MAPID_SAGE,
    MAPID_BARDDANCER,
    MAPID_MONK,
    MAPID_ALCHEMIST,
    MAPID_ROGUE,
    MAPID_SOUL_LINKER,
    MAPID_DARK_COLLECTOR = JobHelper.JOBL_2_2 | MAPID_GANGSI,
    //Trans Novice And Trans 1-1 Jobs
    MAPID_NOVICE_HIGH = JobHelper.JOBL_UPPER | MAPID_NOVICE,
    MAPID_SWORDMAN_HIGH,
    MAPID_MAGE_HIGH,
    MAPID_ARCHER_HIGH,
    MAPID_ACOLYTE_HIGH,
    MAPID_MERCHANT_HIGH,
    MAPID_THIEF_HIGH,
    //Trans 2-1 Jobs
    MAPID_LORD_KNIGHT = JobHelper.JOBL_UPPER | MAPID_KNIGHT,
    MAPID_HIGH_WIZARD,
    MAPID_SNIPER,
    MAPID_HIGH_PRIEST,
    MAPID_WHITESMITH,
    MAPID_ASSASSIN_CROSS,
    //Trans 2-2 Jobs
    MAPID_PALADIN = JobHelper.JOBL_UPPER | MAPID_CRUSADER,
    MAPID_PROFESSOR,
    MAPID_CLOWNGYPSY,
    MAPID_CHAMPION,
    MAPID_CREATOR,
    MAPID_STALKER,
    //Baby Novice And Baby 1-1 Jobs
    MAPID_BABY = JobHelper.JOBL_BABY | MAPID_NOVICE,
    MAPID_BABY_SWORDMAN,
    MAPID_BABY_MAGE,
    MAPID_BABY_ARCHER,
    MAPID_BABY_ACOLYTE,
    MAPID_BABY_MERCHANT,
    MAPID_BABY_THIEF,
    MAPID_BABY_TAEKWON,
    MAPID_BABY_GUNSLINGER = JobHelper.JOBL_BABY | MAPID_GUNSLINGER,
    MAPID_BABY_NINJA,
    MAPID_BABY_SUMMONER = JobHelper.JOBL_BABY | MAPID_SUMMONER,
    //Baby 2-1 Jobs
    MAPID_SUPER_BABY = JobHelper.JOBL_BABY | MAPID_SUPER_NOVICE,
    MAPID_BABY_KNIGHT,
    MAPID_BABY_WIZARD,
    MAPID_BABY_HUNTER,
    MAPID_BABY_PRIEST,
    MAPID_BABY_BLACKSMITH,
    MAPID_BABY_ASSASSIN,
    MAPID_BABY_STAR_GLADIATOR,
    MAPID_BABY_REBELLION = JobHelper.JOBL_BABY | MAPID_REBELLION,
    MAPID_BABY_KAGEROUOBORO,
    //Baby 2-2 Jobs
    MAPID_BABY_CRUSADER = JobHelper.JOBL_BABY | MAPID_CRUSADER,
    MAPID_BABY_SAGE,
    MAPID_BABY_BARDDANCER,
    MAPID_BABY_MONK,
    MAPID_BABY_ALCHEMIST,
    MAPID_BABY_ROGUE,
    MAPID_BABY_SOUL_LINKER,
    //3-1 Jobs
    MAPID_SUPER_NOVICE_E = JobHelper.JOBL_THIRD | MAPID_SUPER_NOVICE,
    MAPID_RUNE_KNIGHT,
    MAPID_WARLOCK,
    MAPID_RANGER,
    MAPID_ARCH_BISHOP,
    MAPID_MECHANIC,
    MAPID_GUILLOTINE_CROSS,
    MAPID_STAR_EMPEROR,
    //3-2 Jobs
    MAPID_ROYAL_GUARD = JobHelper.JOBL_THIRD | MAPID_CRUSADER,
    MAPID_SORCERER,
    MAPID_MINSTRELWANDERER,
    MAPID_SURA,
    MAPID_GENETIC,
    MAPID_SHADOW_CHASER,
    MAPID_SOUL_REAPER,
    //Trans 3-1 Jobs
    MAPID_RUNE_KNIGHT_T = JobHelper.JOBL_THIRD | MAPID_LORD_KNIGHT,
    MAPID_WARLOCK_T,
    MAPID_RANGER_T,
    MAPID_ARCH_BISHOP_T,
    MAPID_MECHANIC_T,
    MAPID_GUILLOTINE_CROSS_T,
    //Trans 3-2 Jobs
    MAPID_ROYAL_GUARD_T = JobHelper.JOBL_THIRD | MAPID_PALADIN,
    MAPID_SORCERER_T,
    MAPID_MINSTRELWANDERER_T,
    MAPID_SURA_T,
    MAPID_GENETIC_T,
    MAPID_SHADOW_CHASER_T,
    //Baby 3-1 Jobs
    MAPID_SUPER_BABY_E = JobHelper.JOBL_THIRD | MAPID_SUPER_BABY,
    MAPID_BABY_RUNE_KNIGHT,
    MAPID_BABY_WARLOCK,
    MAPID_BABY_RANGER,
    MAPID_BABY_ARCH_BISHOP,
    MAPID_BABY_MECHANIC,
    MAPID_BABY_GUILLOTINE_CROSS,
    MAPID_BABY_STAR_EMPEROR,
    //Baby 3-2 Jobs
    MAPID_BABY_ROYAL_GUARD = JobHelper.JOBL_THIRD | MAPID_BABY_CRUSADER,
    MAPID_BABY_SORCERER,
    MAPID_BABY_MINSTRELWANDERER,
    MAPID_BABY_SURA,
    MAPID_BABY_GENETIC,
    MAPID_BABY_SHADOW_CHASER,
    MAPID_BABY_SOUL_REAPER,
    // Additional constants
    MAPID_ALL = ulong.MaxValue
};