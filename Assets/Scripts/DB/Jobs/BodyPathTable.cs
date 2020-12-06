using System;
using System.Collections.Generic;

public class BodyPathTable {

    public static Dictionary<Job, String> JobNames = new Dictionary<Job, string>();

    public static Dictionary<Job, String> init() {
		JobNames[Job.NOVICE] = "\xC3\xCA\xBA\xB8\xC0\xDA";

		JobNames[Job.SWORDMAN] = "\xB0\xCB\xBB\xE7";
		JobNames[Job.MAGICIAN] = "\xB8\xB6\xB9\xFD\xBB\xE7";
		JobNames[Job.ARCHER] = "\xB1\xC3\xBC\xF6";
		JobNames[Job.ACOLYTE] = "\xBC\xBA\xC1\xF7\xC0\xDA";
		JobNames[Job.MERCHANT] = "\xBB\xF3\xC0\xCE";
		JobNames[Job.THIEF] = "\xB5\xB5\xB5\xCF";

		JobNames[Job.KNIGHT] = "\xB1\xE2\xBB\xE7";
		JobNames[Job.PRIEST] = "\xC7\xC1\xB8\xAE\xBD\xBA\xC6\xAE";
		JobNames[Job.WIZARD] = "\xC0\xA7\xC0\xFA\xB5\xE5";
		JobNames[Job.BLACKSMITH] = "\xC1\xA6\xC3\xB6\xB0\xF8";
		JobNames[Job.HUNTER] = "\xC7\xE5\xC5\xCD";
		JobNames[Job.ASSASSIN] = "\xBE\xEE\xBC\xBC\xBD\xC5";
		JobNames[Job.KNIGHT2] = "\xC6\xE4\xC4\xDA\xC6\xE4\xC4\xDA_\xB1\xE2\xBB\xE7";

		JobNames[Job.CRUSADER] = "\xC5\xA9\xB7\xE7\xBC\xBC\xC0\xCC\xB4\xF5";
		JobNames[Job.MONK] = "\xB8\xF9\xC5\xA9";
		JobNames[Job.SAGE] = "\xBC\xBC\xC0\xCC\xC1\xF6";
		JobNames[Job.ROGUE] = "\xB7\xCE\xB1\xD7";
		JobNames[Job.ALCHEMIST] = "\xBF\xAC\xB1\xDD\xBC\xFA\xBB\xE7";
		JobNames[Job.BARD] = "\xB9\xD9\xB5\xE5";
		JobNames[Job.DANCER] = "\xB9\xAB\xC8\xF1";
		JobNames[Job.CRUSADER2] = "\xBD\xC5\xC6\xE4\xC4\xDA\xC5\xA9\xB7\xE7\xBC\xBC\xC0\xCC\xB4\xF5";

		JobNames[Job.SUPERNOVICE] = "\xBD\xB4\xC6\xDB\xB3\xEB\xBA\xF1\xBD\xBA";
		JobNames[Job.GUNSLINGER] = "\xB0\xC7\xB3\xCA";
		JobNames[Job.NINJA] = "\xB4\xD1\xC0\xDA";
		JobNames[Job.TAEKWON] = "\xc5\xc2\xb1\xc7\xbc\xd2\xb3\xe2";
		JobNames[Job.STAR] = "\xb1\xc7\xbc\xba";
		JobNames[Job.STAR2] = "\xb1\xc7\xbc\xba\xc0\xb6\xc7\xd5";
		JobNames[Job.LINKER] = "\xbc\xd2\xbf\xef\xb8\xb5\xc4\xbf";

		JobNames[Job.MARRIED] = "\xB0\xE1\xC8\xA5";
		JobNames[Job.XMAS] = "\xBB\xEA\xC5\xB8";
		JobNames[Job.SUMMER] = "\xBF\xA9\xB8\xA7";

		JobNames[Job.KNIGHT_H] = "\xB7\xCE\xB5\xE5\xB3\xAA\xC0\xCC\xC6\xAE";
		JobNames[Job.PRIEST_H] = "\xC7\xCF\xC0\xCC\xC7\xC1\xB8\xAE";
		JobNames[Job.WIZARD_H] = "\xC7\xCF\xC0\xCC\xC0\xA7\xC0\xFA\xB5\xE5";
		JobNames[Job.BLACKSMITH_H] = "\xC8\xAD\xC0\xCC\xC6\xAE\xBD\xBA\xB9\xCC\xBD\xBA";
		JobNames[Job.HUNTER_H] = "\xBD\xBA\xB3\xAA\xC0\xCC\xC6\xDB";
		JobNames[Job.ASSASSIN_H] = "\xBE\xEE\xBD\xD8\xBD\xC5\xC5\xA9\xB7\xCE\xBD\xBA";
		JobNames[Job.KNIGHT2_H] = "\xB7\xCE\xB5\xE5\xC6\xE4\xC4\xDA";
		JobNames[Job.CRUSADER_H] = "\xC6\xC8\xB6\xF3\xB5\xF2";
		JobNames[Job.MONK_H] = "\xC3\xA8\xC7\xC7\xBF\xC2";
		JobNames[Job.SAGE_H] = "\xC7\xC1\xB7\xCE\xC6\xE4\xBC\xAD";
		JobNames[Job.ROGUE_H] = "\xBD\xBA\xC5\xE4\xC4\xBF";
		JobNames[Job.ALCHEMIST_H] = "\xC5\xA9\xB8\xAE\xBF\xA1\xC0\xCC\xC5\xCD";
		JobNames[Job.BARD_H] = "\xC5\xAC\xB6\xF3\xBF\xEE";
		JobNames[Job.DANCER_H] = "\xC1\xFD\xBD\xC3";
		JobNames[Job.CRUSADER2_H] = "\xC6\xE4\xC4\xDA\xC6\xC8\xB6\xF3\xB5\xF2";

		JobNames[Job.RUNE_KNIGHT] = "\xB7\xE9\xB3\xAA\xC0\xCC\xC6\xAE";
		JobNames[Job.WARLOCK] = "\xBF\xF6\xB7\xCF";
		JobNames[Job.RANGER] = "\xB7\xB9\xC0\xCE\xC1\xAE";
		JobNames[Job.ARCHBISHOP] = "\xBE\xC6\xC5\xA9\xBA\xF1\xBC\xF3";
		JobNames[Job.MECHANIC] = "\xB9\xCC\xC4\xC9\xB4\xD0";
		JobNames[Job.GUILLOTINE_CROSS] = "\xB1\xE6\xB7\xCE\xC6\xBE\xC5\xA9\xB7\xCE\xBD\xBA";

		JobNames[Job.ROYAL_GUARD] = "\xB0\xA1\xB5\xE5";
		JobNames[Job.SORCERER] = "\xBC\xD2\xBC\xAD\xB7\xAF";
		JobNames[Job.MINSTREL] = "\xB9\xCE\xBD\xBA\xC6\xAE\xB7\xB2";
		JobNames[Job.WANDERER] = "\xBF\xF8\xB4\xF5\xB7\xAF";
		JobNames[Job.SURA] = "\xBD\xB4\xB6\xF3";
		JobNames[Job.GENETIC] = "\xC1\xA6\xB3\xD7\xB8\xAF";
		JobNames[Job.SHADOW_CHASER] = "\xBD\xA6\xB5\xB5\xBF\xEC\xC3\xBC\xC0\xCC\xBC\xAD";

		JobNames[Job.RUNE_KNIGHT2] = "\xB7\xE9\xB3\xAA\xC0\xCC\xC6\xAE\xBB\xDA\xB6\xEC";
		JobNames[Job.ROYAL_GUARD2] = "\xB1\xD7\xB8\xAE\xC6\xF9\xB0\xA1\xB5\xE5";
		JobNames[Job.RANGER2] = "\xB7\xB9\xC0\xCE\xC1\xAE\xB4\xC1\xB4\xEB";
		JobNames[Job.MECHANIC2] = "\xB8\xB6\xB5\xB5\xB1\xE2\xBE\xEE";

		/**
		 * Inherit
		 */
		JobNames[Job.NOVICE_B]		= JobNames[Job.NOVICE_H]		= JobNames[Job.NOVICE];
		JobNames[Job.SWORDMAN_B]	= JobNames[Job.SWORDMAN_H]		= JobNames[Job.SWORDMAN];
		JobNames[Job.MAGICIAN_B]	= JobNames[Job.MAGICIAN_H]		= JobNames[Job.MAGICIAN];
		JobNames[Job.ARCHER_B]		= JobNames[Job.ARCHER_H]		= JobNames[Job.ARCHER];
		JobNames[Job.ACOLYTE_B]		= JobNames[Job.ACOLYTE_H]		= JobNames[Job.ACOLYTE];
		JobNames[Job.MERCHANT_B]	= JobNames[Job.MERCHANT_H]		= JobNames[Job.MERCHANT];
		JobNames[Job.THIEF_B]		= JobNames[Job.THIEF_H]			= JobNames[Job.THIEF];

		JobNames[Job.KNIGHT_B]		= JobNames[Job.KNIGHT];
		JobNames[Job.KNIGHT2_B]		= JobNames[Job.KNIGHT2];
		JobNames[Job.PRIEST_B]		= JobNames[Job.PRIEST];
		JobNames[Job.WIZARD_B]		= JobNames[Job.WIZARD];
		JobNames[Job.BLACKSMITH_B]	= JobNames[Job.BLACKSMITH];
		JobNames[Job.HUNTER_B]		= JobNames[Job.HUNTER];
		JobNames[Job.ASSASSIN_B]	= JobNames[Job.ASSASSIN];
		JobNames[Job.CRUSADER_B]	= JobNames[Job.CRUSADER];
		JobNames[Job.CRUSADER2_B]	= JobNames[Job.CRUSADER2];
		JobNames[Job.MONK_B]		= JobNames[Job.MONK];
		JobNames[Job.SAGE_B]		= JobNames[Job.SAGE];
		JobNames[Job.ROGUE_B]		= JobNames[Job.ROGUE];
		JobNames[Job.ALCHEMIST_B]	= JobNames[Job.ALCHEMIST];
		JobNames[Job.BARD_B]		= JobNames[Job.BARD];
		JobNames[Job.DANCER_B]		= JobNames[Job.DANCER];

		JobNames[Job.RUNE_KNIGHT_H]			= JobNames[Job.RUNE_KNIGHT_B]		= JobNames[Job.RUNE_KNIGHT];
		JobNames[Job.RUNE_KNIGHT2_H]		= JobNames[Job.RUNE_KNIGHT2_B]		= JobNames[Job.RUNE_KNIGHT2];
		JobNames[Job.WARLOCK_H]				= JobNames[Job.WARLOCK_B]			= JobNames[Job.WARLOCK];
		JobNames[Job.RANGER_H]				= JobNames[Job.RANGER_B]			= JobNames[Job.RANGER];
		JobNames[Job.RANGER2_H]				= JobNames[Job.RANGER2_B]			= JobNames[Job.RANGER2];
		JobNames[Job.ARCHBISHOP_H]			= JobNames[Job.ARCHBISHOP_B]		= JobNames[Job.ARCHBISHOP];
		JobNames[Job.MECHANIC_H]			= JobNames[Job.MECHANIC_B]			= JobNames[Job.MECHANIC];
		JobNames[Job.MECHANIC2_H]			= JobNames[Job.MECHANIC2_B]			= JobNames[Job.MECHANIC2];
		JobNames[Job.GUILLOTINE_CROSS_H]	= JobNames[Job.GUILLOTINE_CROSS_B]	= JobNames[Job.GUILLOTINE_CROSS];
		JobNames[Job.ROYAL_GUARD_H]			= JobNames[Job.ROYAL_GUARD_B]		= JobNames[Job.ROYAL_GUARD];
		JobNames[Job.ROYAL_GUARD2_H]		= JobNames[Job.ROYAL_GUARD2_B]		= JobNames[Job.ROYAL_GUARD2];
		JobNames[Job.SORCERER_H]			= JobNames[Job.SORCERER_B]			= JobNames[Job.SORCERER];
		JobNames[Job.MINSTREL_H]			= JobNames[Job.MINSTREL_B]			= JobNames[Job.MINSTREL];
		JobNames[Job.WANDERER_H]			= JobNames[Job.WANDERER_B]			= JobNames[Job.WANDERER];
		JobNames[Job.SURA_H]				= JobNames[Job.SURA_B]				= JobNames[Job.SURA];
		JobNames[Job.GENETIC_H]				= JobNames[Job.GENETIC_B]			= JobNames[Job.GENETIC];
		JobNames[Job.SHADOW_CHASER_H]		= JobNames[Job.SHADOW_CHASER_B]		= JobNames[Job.SHADOW_CHASER];

		return JobNames;
	}
}