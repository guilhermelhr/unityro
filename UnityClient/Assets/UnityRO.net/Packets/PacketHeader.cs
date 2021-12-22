﻿public enum PacketHeader : ushort {

    #region ANNOTATIONS
    //0x = Indicating that it is a hexadecimal number
    //Ex: 0x69 = 0x[Hexadecimal] 6[Binary: 0110] 9[Binary: 1001]
    //Binary to decimal => [0110, 1001] = [105]
    #endregion

    #region MISC
    PING = 0x187,
    #endregion

    #region CA
    CA_LOGIN = 0x64,
    #endregion

    #region AC
    AC_ACCEPT_LOGIN = 0x69,
    AC_ACCEPT_LOGIN2 = 0x276,
    AC_ACCEPT_LOGIN3 = 0xac4,
    AC_REFUSE_LOGIN = 0x6a,
    #endregion

    #region CH
    CH_ENTER = 0x65,
    CH_ENTER2 = 0x275,
    CH_SELECT_CHAR = 0x66,
    CH_MAKE_CHAR = 0x67,
    CH_MAKE_CHAR2 = 0x970,
    CH_DELETE_CHAR = 0x68,
    #endregion

    #region HC
    HC_ACK_ACCOUNT_ID = 0x8480,
    HC_ACCEPT_ENTER = 0x6b,
    HC_ACCEPT_ENTER2 = 0x82d,
    HC_NOTIFY_CHARLIST = 0x09a0,
    HC_SECOND_PASSWD_LOGIN = 0x8b9,
    HC_NOTIFY_ZONESVR2 = 0xAC5,
    HC_REFUSE_ENTER = 0x6c,
    HC_ACCEPT_MAKECHAR = 0x6d,
    HC_REFUSE_MAKECHAR = 0x6e,
    HC_ACCEPT_DELETECHAR = 0x6f,
    HC_REFUSE_DELETECHAR = 0x70,
    HC_BLOCK_CHARACTER = 0x20d,
    #endregion

    #region CZ
    CZ_ENTER = 0x72,
    CZ_ENTER2 = 0x436,
    ZC_REFUSE_ENTER = 0x74,
    CZ_NOTIFY_ACTORINIT = 0x7d,
    CZ_REQUEST_TIME = 0x7e,
    CZ_REQUEST_QUIT = 0x82,
    CZ_REQUEST_MOVE = 0x85,
    CZ_REQUEST_ACT = 0x89,
    CZ_REQUEST_CHAT = 0xf3,
    CZ_CONTACTNPC = 0x90,
    CZ_REQNAME = 0x94,
    CZ_WHISPER = 0x96,
    CZ_BROADCAST = 0x99,
    CZ_CHANGE_DIRECTION = 0x9b,
    CZ_ITEM_THROW = 0xa2,
    CZ_USE_ITEM = 0xa7,
    CZ_REQ_WEAR_EQUIP = 0xa9,
    CZ_REQ_WEAR_EQUIP_V5 = 0x998,
    CZ_REQ_TAKEOFF_EQUIP = 0xab,
    CZ_REQ_ITEM_EXPLANATION_BYNAME = 0xad,
    CZ_RESTART = 0xb2,
    CZ_CHOOSE_MENU = 0xb8,
    CZ_REQ_NEXT_SCRIPT = 0xb9,
    CZ_REQ_STATUS = 0xba,
    CZ_STATUS_CHANGE = 0xbb,
    CZ_PC_PURCHASE_ITEMLIST = 0xc8,
    CZ_PC_SELL_ITEMLIST = 0xc9,
    CZ_REQ_EMOTION = 0xbf,
    CZ_ACK_SELECT_DEALTYPE = 0xc5,
    CZ_REQ_USER_COUNT = 0xc1,
    CZ_ITEM_PICKUP = 0x9f,
    CZ_IRMAIL_LIST = 0x2f6,
    CZ_OPEN_SIMPLE_CASHSHOP_ITEMLIST = 0x35c,
    CZ_CLOSE_WINDOW = 0x35e,
    CZ_REQUEST_MOVENPC = 0x232,
    CZ_REQUEST_MOVETOOWNER = 0x234,
    CZ_REQUEST_MOVE_NEW_JAPEN = 0x2e5,
    CZ_REQUEST_MOVE2 = 0x35f,
    CZ_REQUEST_TIME2 = 0x360,
    CZ_CHANGE_DIRECTION2 = 0x361,
    CZ_ITEM_PICKUP2 = 0x362,
    CZ_ITEM_THROW2 = 0x363,
    CZ_MOVE_ITEM_FROM_BODY_TO_STORE2 = 0x364,
    CZ_MOVE_ITEM_FROM_STORE_TO_BODY2 = 0x365,
    CZ_USE_SKILL_TOGROUND2 = 0x366,
    CZ_USE_SKILL_TOGROUND_WITHTALKBOX2 = 0x367,
    CZ_REQNAME2 = 0x368,
    CZ_REQNAME_BYGID2 = 0x369,
    CZ_CLOSE_DIALOG = 0x146,
    CZ_USE_ITEM2 = 0x439,
    CZ_REQUEST_ACT2 = 0x437,
    CZ_UPGRADE_SKILLLEVEL = 0x112,
    CZ_USE_SKILL2 = 0x438,
    #endregion

    #region ZC
    ZC_NOTIFY_STANDENTRY11 = 0x9fe,
    ZC_NOTIFY_NEWENTRY11 = 0x9ff,
    ZC_NOTIFY_MOVEENTRY11 = 0x9fd,
    ZC_ACH_UPDATE = 0xa24,
    ZC_ACCEPT_ENTER = 0x73,
    ZC_ACCEPT_ENTER2 = 0x2eb,
    ZC_AID = 0x283,
    ZC_NOTIFY_INITCHAR = 0x75,
    ZC_NOTIFY_UPDATECHAR = 0x76,
    ZC_NOTIFY_UPDATEPLAYER = 0x77,
    ZC_NOTIFY_STANDENTRY = 0x78,
    ZC_NOTIFY_NEWENTRY = 0x79,
    ZC_NOTIFY_ACTENTRY = 0x7a,
    ZC_NOTIFY_MOVEENTRY = 0x7b,
    ZC_NOTIFY_STANDENTRY_NPC = 0x7c,
    ZC_NOTIFY_VANISH = 0x80,
    ZC_NOTIFY_TIME = 0x7f,
    ZC_ACCEPT_QUIT = 0x83,
    ZC_NOTIFY_CHAT = 0x8d,
    ZC_NOTIFY_PLAYERCHAT = 0x8e,
    ZC_REFUSE_QUIT = 0x84,
    ZC_NOTIFY_MOVE = 0x86,
    ZC_NOTIFY_PLAYERMOVE = 0x87,
    ZC_STOPMOVE = 0x88,
    ZC_NOTIFY_ACT = 0x8a,
    ZC_NOTIFY_ACT_POSITION = 0x8b,
    ZC_NPCACK_MAPMOVE = 0x91,
    ZC_NPCACK_SERVERMOVE = 0x92,
    ZC_NPCACK_ENABLE = 0x93,
    ZC_ACK_REQNAME = 0x95,
    ZC_ACK_WHISPER = 0x98,
    ZC_BROADCAST = 0x9a,
    ZC_WHISPER = 0x97,
    ZC_CHANGE_DIRECTION = 0x9c,
    ZC_ITEM_ENTRY = 0x9d,
    ZC_ITEM_FALL_ENTRY = 0x9e,
    ZC_ITEM_FALL_ENTRY4 = 0x84b,
    ZC_ITEM_FALL_ENTRY5 = 0xadd,
    ZC_ITEM_PICKUP_ACK = 0xa0,
    ZC_ITEM_PICKUP_ACK_V7 = 0xa37,
    ZC_NORMAL_ITEMLIST = 0xa3,
    ZC_ITEM_DISAPPEAR = 0xa1,
    ZC_EQUIPMENT_ITEMLIST = 0xa4,
    ZC_STORE_NORMAL_ITEMLIST = 0xa5,
    ZC_STORE_EQUIPMENT_ITEMLIST = 0xa6,
    ZC_USE_ITEM_ACK = 0xa8,
    ZC_REQ_WEAR_EQUIP_ACK = 0xaa,
    ZC_ACK_WEAR_EQUIP_V5 = 0x999,
    ZC_ACK_TAKEOFF_EQUIP_V5 = 0x99a,
    ZC_REQ_TAKEOFF_EQUIP_ACK = 0xac,
    ZC_REQ_ITEM_EXPLANATION_ACK = 0xae,
    ZC_ITEM_THROW_ACK = 0xaf,
    ZC_ALL_ACH_LIST = 0xa23,
    ZC_PAR_CHANGE = 0xb0,
    ZC_LONGPAR_CHANGE = 0xb1,
    ZC_LONGPAR_CHANGE2 = 0xacb,
    ZC_RESTART_ACK = 0xb3,
    ZC_SAY_DIALOG = 0xb4,
    ZC_WAIT_DIALOG = 0xb5,
    ZC_CLOSE_DIALOG = 0xb6,
    ZC_CLOSE_SCRIPT = 0x8d6,
    ZC_MENU_LIST = 0xb7,
    ZC_STATUS_CHANGE_ACK = 0xbc,
    ZC_STATUS = 0xbd,
    ZC_STATUS_CHANGE = 0xbe,
    ZC_EMOTION = 0xc0,
    ZC_INVENTORY_ITEMLIST_EQUIP = 0xb0a,
    ZC_INVENTORY_ITEMLIST_NORMAL = 0xb09,
    ZC_INVENTORY_SWITCH = 0xa9b,
    ZC_HP_INFO = 0x977,
    ZC_INVENTORY_START = 0xb08,
    ZC_INVENTORY_END = 0xb0b,
    ZC_USE_ITEM_ACK2 = 0x1c8,
    ZC_USESKILL_ACK2 = 0x7fb,
    ZC_SPRITE_CHANGE2 = 0x1d7,
    ZC_SKILLINFO_LIST = 0x10f,
    ZC_SKILLINFO_UPDATE = 0x10e,
    ZC_ATTACK_RANGE = 0x13a,
    ZC_SHORTCUT_KEY_LIST_V4 = 0xb20,
    ZC_CONFIG = 0x2d9,
    ZC_CONFIG_NOTIFY = 0x2da,
    ZC_COUPLESTATUS = 0x141,
    ZC_MSG = 0x291,
    ZC_MSG_STATE_CHANGE = 0x196,
    ZC_MSG_STATE_CHANGE3 = 0x983,
    ZC_NOTIFY_ACT3 = 0x8c8,
    ZC_NOTIFY_WEIGHT_PERCENTAGE = 0xade,
    ZC_NOTIFY_EXP2 = 0xacc,
    ZC_NOTIFY_MAPPROPERTY2 = 0x99b,
    ZC_NOTIFY_UNREAD_MAIL = 0x9e7,
    ZC_NPCSPRITE_CHANGE = 0x1b0,
    ZC_PC_PURCHASE_ITEMLIST_FROMMC2 = 0x800,
    ZC_OPEN_EDITDLG = 0x142,
    ZC_PARTY_CONFIG = 0x2c9,
    ZC_NOTIFY_EFFECT2 = 0x1f3,
    ZC_RESURRECTION = 0x148,
    ZC_DELETE_ITEM_FROM_BODY = 0x7fa,
    ZC_EQUIP_ARROW = 0x13c,
    ZC_ACTION_FAILURE = 0x13b,
    ZC_NOTIFY_RANKING = 0x19a,
    ZC_ACK_TOUSESKILL = 0x110,
    ZC_NOTIFY_SKILL2 = 0x1de,
    ZC_SELECT_DEALTYPE = 0xc4,
    ZC_PC_PURCHASE_ITEMLIST = 0xc6,
    ZC_PC_SELL_ITEMLIST = 0xc7,
    ZC_PC_PURCHASE_RESULT = 0xca,
    ZC_PC_SELL_RESULT = 0xcb,
    ZC_QUEST_NOTIFY_EFFECT = 0x446,
    #endregion

    //ZC_INVENTORY_ITEMLIST_EQUIP = 0xa0d,
    //ZC_INVENTORY_ITEMLIST_NORMAL = 0x991,
    //ZC_USER_COUNT = 0xc2,
    //ZC_SPRITE_CHANGE = 0xc3,
    //SERVER_ENTRY_ACK = 0x8f,
    //SC_NOTIFY_BAN = 0x81,
    //ZC_ACK_DISCONNECT_CHARACTER = 0xcd,
    //CZ_DISCONNECT_CHARACTER = 0xcc,
    //CZ_DISCONNECT_ALL_CHARACTER = 0xce,
    //CZ_SETTING_WHISPER_PC = 0xcf,
    //CZ_SETTING_WHISPER_STATE = 0xd0,
    //ZC_SETTING_WHISPER_PC = 0xd1,
    //ZC_SETTING_WHISPER_STATE = 0xd2,
    //CZ_REQ_WHISPER_LIST = 0xd3,
    //ZC_WHISPER_LIST = 0xd4,
    //CZ_CREATE_CHATROOM = 0xd5,
    //ZC_ACK_CREATE_CHATROOM = 0xd6,
    //ZC_ROOM_NEWENTRY = 0xd7,
    //ZC_DESTROY_ROOM = 0xd8,
    //CZ_REQ_ENTER_ROOM = 0xd9,
    //ZC_REFUSE_ENTER_ROOM = 0xda,
    //ZC_ENTER_ROOM = 0xdb,
    //ZC_MEMBER_NEWENTRY = 0xdc,
    //ZC_MEMBER_EXIT = 0xdd,
    //CZ_CHANGE_CHATROOM = 0xde,
    //ZC_CHANGE_CHATROOM = 0xdf,
    //CZ_REQ_ROLE_CHANGE = 0xe0,
    //ZC_ROLE_CHANGE = 0xe1,
    //CZ_REQ_EXPEL_MEMBER = 0xe2,
    //CZ_EXIT_ROOM = 0xe3,
    //CZ_REQ_EXCHANGE_ITEM = 0xe4,
    //ZC_REQ_EXCHANGE_ITEM = 0xe5,
    //CZ_ACK_EXCHANGE_ITEM = 0xe6,
    //ZC_ACK_EXCHANGE_ITEM = 0xe7,
    //CZ_ADD_EXCHANGE_ITEM = 0xe8,
    //ZC_ADD_EXCHANGE_ITEM = 0xe9,
    //ZC_ACK_ADD_EXCHANGE_ITEM = 0xea,
    //CZ_CONCLUDE_EXCHANGE_ITEM = 0xeb,
    //ZC_CONCLUDE_EXCHANGE_ITEM = 0xec,
    //CZ_CANCEL_EXCHANGE_ITEM = 0xed,
    //ZC_CANCEL_EXCHANGE_ITEM = 0xee,
    //CZ_EXEC_EXCHANGE_ITEM = 0xef,
    //ZC_EXEC_EXCHANGE_ITEM = 0xf0,
    //ZC_EXCHANGEITEM_UNDO = 0xf1,
    //ZC_NOTIFY_STOREITEM_COUNTINFO = 0xf2,
    //CZ_MOVE_ITEM_FROM_BODY_TO_STORE = 0xf3,
    //ZC_ADD_ITEM_TO_STORE = 0xf4,
    //CZ_MOVE_ITEM_FROM_STORE_TO_BODY = 0xf5,
    //ZC_DELETE_ITEM_FROM_STORE = 0xf6,
    //CZ_CLOSE_STORE = 0xf7,
    //ZC_CLOSE_STORE = 0xf8,
    //CZ_MAKE_GROUP = 0xf9,
    //ZC_ACK_MAKE_GROUP = 0xfa,
    //ZC_GROUP_LIST = 0xfb,
    //CZ_REQ_JOIN_GROUP = 0xfc,
    //ZC_ACK_REQ_JOIN_GROUP = 0xfd,
    //ZC_REQ_JOIN_GROUP = 0xfe,
    //CZ_JOIN_GROUP = 0xff,
    //CZ_REQ_LEAVE_GROUP = 0x100,
    //ZC_GROUPINFO_CHANGE = 0x101,
    //CZ_CHANGE_GROUPEXPOPTION = 0x102,
    //CZ_REQ_EXPEL_GROUP_MEMBER = 0x103,
    //ZC_ADD_MEMBER_TO_GROUP = 0x104,
    //ZC_DELETE_MEMBER_FROM_GROUP = 0x105,
    //ZC_NOTIFY_HP_TO_GROUPM = 0x106,
    //ZC_NOTIFY_POSITION_TO_GROUPM = 0x107,
    //CZ_REQUEST_CHAT_PARTY = 0x108,
    //ZC_NOTIFY_CHAT_PARTY = 0x109,
    //ZC_MVP_GETTING_ITEM = 0x10a,
    //ZC_MVP_GETTING_SPECIAL_EXP = 0x10b,
    //ZC_MVP = 0x10c,
    //ZC_THROW_MVPITEM = 0x10d,
    //ZC_ADD_SKILL = 0x111,
    //CZ_USE_SKILL = 0x113,
    //ZC_NOTIFY_SKILL = 0x114,
    //ZC_NOTIFY_SKILL_POSITION = 0x115,
    //CZ_USE_SKILL_TOGROUND = 0x116,
    //ZC_NOTIFY_GROUNDSKILL = 0x117,
    //CZ_CANCEL_LOCKON = 0x118,
    //ZC_STATE_CHANGE = 0x119,
    //ZC_USE_SKILL = 0x11a,
    //CZ_SELECT_WARPPOINT = 0x11b,
    //ZC_WARPLIST = 0x11c,
    //CZ_REMEMBER_WARPPOINT = 0x11d,
    //ZC_ACK_REMEMBER_WARPPOINT = 0x11e,
    //ZC_SKILL_ENTRY = 0x11f,
    //ZC_SKILL_DISAPPEAR = 0x120,
    //ZC_NOTIFY_CARTITEM_COUNTINFO = 0x121,
    //ZC_CART_EQUIPMENT_ITEMLIST = 0x122,
    //ZC_CART_NORMAL_ITEMLIST = 0x123,
    //ZC_ADD_ITEM_TO_CART = 0x124,
    //ZC_DELETE_ITEM_FROM_CART = 0x125,
    //CZ_MOVE_ITEM_FROM_BODY_TO_CART = 0x126,
    //CZ_MOVE_ITEM_FROM_CART_TO_BODY = 0x127,
    //CZ_MOVE_ITEM_FROM_STORE_TO_CART = 0x128,
    //CZ_MOVE_ITEM_FROM_CART_TO_STORE = 0x129,
    //CZ_REQ_CARTOFF = 0x12a,
    //ZC_CARTOFF = 0x12b,
    //ZC_ACK_ADDITEM_TO_CART = 0x12c,
    //ZC_OPENSTORE = 0x12d,
    //CZ_REQ_CLOSESTORE = 0x12e,
    //CZ_REQ_OPENSTORE = 0x12f,
    //CZ_REQ_BUY_FROMMC = 0x130,
    //ZC_STORE_ENTRY = 0x131,
    //ZC_DISAPPEAR_ENTRY = 0x132,
    //ZC_PC_PURCHASE_ITEMLIST_FROMMC = 0x133,
    //CZ_PC_PURCHASE_ITEMLIST_FROMMC = 0x134,
    //ZC_PC_PURCHASE_RESULT_FROMMC = 0x135,
    //ZC_PC_PURCHASE_MYITEMLIST = 0x136,
    //ZC_DELETEITEM_FROM_MCSTORE = 0x137,
    //CZ_PKMODE_CHANGE = 0x138,
    //ZC_ATTACK_FAILURE_FOR_DISTANCE = 0x139,

    //ZC_RECOVERY = 0x13d,
    //ZC_USESKILL_ACK = 0x13e,
    //CZ_ITEM_CREATE = 0x13f,
    //CZ_MOVETO_MAP = 0x140,
    //CZ_INPUT_EDITDLG = 0x143,
    //ZC_COMPASS = 0x144,
    //ZC_SHOW_IMAGE = 0x145,
    //ZC_AUTORUN_SKILL = 0x147,
    //CZ_REQ_GIVE_MANNER_POINT = 0x149,
    //ZC_ACK_GIVE_MANNER_POINT = 0x14a,
    //ZC_NOTIFY_MANNER_POINT_GIVEN = 0x14b,
    //ZC_MYGUILD_BASIC_INFO = 0x14c,
    //CZ_REQ_GUILD_MENUINTERFACE = 0x14d,
    //ZC_ACK_GUILD_MENUINTERFACE = 0x14e,
    //CZ_REQ_GUILD_MENU = 0x14f,
    //ZC_GUILD_INFO = 0x150,
    //CZ_REQ_GUILD_EMBLEM_IMG = 0x151,
    //ZC_GUILD_EMBLEM_IMG = 0x152,
    //CZ_REGISTER_GUILD_EMBLEM_IMG = 0x153,
    //ZC_MEMBERMGR_INFO = 0x154,
    //CZ_REQ_CHANGE_MEMBERPOS = 0x155,
    //ZC_ACK_REQ_CHANGE_MEMBERS = 0x156,
    //CZ_REQ_OPEN_MEMBER_INFO = 0x157,
    //XXXXX_ZC_ACK_OPEN_MEMBER_INFO = 0x158,
    //CZ_REQ_LEAVE_GUILD = 0x159,
    //ZC_ACK_LEAVE_GUILD = 0x15a,
    //CZ_REQ_BAN_GUILD = 0x15b,
    //ZC_ACK_BAN_GUILD = 0x15c,
    //CZ_REQ_DISORGANIZE_GUILD = 0x15d,
    //ZC_ACK_DISORGANIZE_GUILD_RESULT = 0x15e,
    //ZC_ACK_DISORGANIZE_GUILD = 0x15f,
    //ZC_POSITION_INFO = 0x160,
    //CZ_REG_CHANGE_GUILD_POSITIONINFO = 0x161,
    //ZC_GUILD_SKILLINFO = 0x162,
    //ZC_BAN_LIST = 0x163,
    //ZC_OTHER_GUILD_LIST = 0x164,
    //CZ_REQ_MAKE_GUILD = 0x165,
    //ZC_POSITION_ID_NAME_INFO = 0x166,
    //ZC_RESULT_MAKE_GUILD = 0x167,
    //CZ_REQ_JOIN_GUILD = 0x168,
    //ZC_ACK_REQ_JOIN_GUILD = 0x169,
    //ZC_REQ_JOIN_GUILD = 0x16a,
    //CZ_JOIN_GUILD = 0x16b,
    //ZC_UPDATE_GDID = 0x16c,
    //ZC_UPDATE_CHARSTAT = 0x16d,
    //CZ_GUILD_NOTICE = 0x16e,
    //ZC_GUILD_NOTICE = 0x16f,
    //CZ_REQ_ALLY_GUILD = 0x170,
    //ZC_REQ_ALLY_GUILD = 0x171,
    //CZ_ALLY_GUILD = 0x172,
    //ZC_ACK_REQ_ALLY_GUILD = 0x173,
    //ZC_ACK_CHANGE_GUILD_POSITIONINFO = 0x174,
    //CZ_REQ_GUILD_MEMBER_INFO = 0x175,
    //ZC_ACK_GUILD_MEMBER_INFO = 0x176,
    //ZC_ITEMIDENTIFY_LIST = 0x177,
    //CZ_REQ_ITEMIDENTIFY = 0x178,
    //ZC_ACK_ITEMIDENTIFY = 0x179,
    //CZ_REQ_ITEMCOMPOSITION_LIST = 0x17a,
    //ZC_ITEMCOMPOSITION_LIST = 0x17b,
    //CZ_REQ_ITEMCOMPOSITION = 0x17c,
    //ZC_ACK_ITEMCOMPOSITION = 0x17d,
    //CZ_GUILD_CHAT = 0x17e,
    //ZC_GUILD_CHAT = 0x17f,
    //CZ_REQ_HOSTILE_GUILD = 0x180,
    //ZC_ACK_REQ_HOSTILE_GUILD = 0x181,
    //ZC_MEMBER_ADD = 0x182,
    //CZ_REQ_DELETE_RELATED_GUILD = 0x183,
    //ZC_DELETE_RELATED_GUILD = 0x184,
    //ZC_ADD_RELATED_GUILD = 0x185,
    //COLLECTORDEAD = 0x186,
    //ZC_ACK_ITEMREFINING = 0x188,
    //ZC_NOTIFY_MAPINFO = 0x189,
    //CZ_REQ_DISCONNECT = 0x18a,
    //ZC_ACK_REQ_DISCONNECT = 0x18b,
    //ZC_MONSTER_INFO = 0x18c,
    //ZC_MAKABLEITEMLIST = 0x18d,
    //CZ_REQMAKINGITEM = 0x18e,
    //ZC_ACK_REQMAKINGITEM = 0x18f,
    //CZ_USE_SKILL_TOGROUND_WITHTALKBOX = 0x190,
    //ZC_TALKBOX_CHATCONTENTS = 0x191,
    //ZC_UPDATE_MAPINFO = 0x192,
    //CZ_REQNAME_BYGID = 0x193,
    //ZC_ACK_REQNAME_BYGID = 0x194,
    //ZC_ACK_REQNAMEALL = 0x195,
    //CZ_RESET = 0x197,
    //CZ_CHANGE_MAPTYPE = 0x198,
    //ZC_NOTIFY_MAPPROPERTY = 0x199,
    //ZC_NOTIFY_EFFECT = 0x19b,
    //CZ_LOCALBROADCAST = 0x19c,
    //CZ_CHANGE_EFFECTSTATE = 0x19d,
    //ZC_START_CAPTURE = 0x19e,
    //CZ_TRYCAPTURE_MONSTER = 0x19f,
    //ZC_TRYCAPTURE_MONSTER = 0x1a0,
    //CZ_COMMAND_PET = 0x1a1,
    //ZC_PROPERTY_PET = 0x1a2,
    //ZC_FEED_PET = 0x1a3,
    //ZC_CHANGESTATE_PET = 0x1a4,
    //CZ_RENAME_PET = 0x1a5,
    //ZC_PETEGG_LIST = 0x1a6,
    //CZ_SELECT_PETEGG = 0x1a7,
    //CZ_PETEGG_INFO = 0x1a8,
    //CZ_PET_ACT = 0x1a9,
    //ZC_PET_ACT = 0x1aa,
    //ZC_PAR_CHANGE_USER = 0x1ab,
    //ZC_SKILL_UPDATE = 0x1ac,
    //ZC_MAKINGARROW_LIST = 0x1ad,
    //CZ_REQ_MAKINGARROW = 0x1ae,
    //CZ_REQ_CHANGECART = 0x1af,
    //ZC_SHOWDIGIT = 0x1b1,
    //CZ_REQ_OPENSTORE2 = 0x1b2,
    //ZC_SHOW_IMAGE2 = 0x1b3,
    //ZC_CHANGE_GUILD = 0x1b4,
    //SC_BILLING_INFO = 0x1b5,
    //ZC_GUILD_INFO2 = 0x1b6,
    //CZ_GUILD_ZENY = 0x1b7,
    //ZC_GUILD_ZENY_ACK = 0x1b8,
    //ZC_DISPEL = 0x1b9,
    //CZ_REMOVE_AID = 0x1ba,
    //CZ_SHIFT = 0x1bb,
    //CZ_RECALL = 0x1bc,
    //CZ_RECALL_GID = 0x1bd,
    //AC_ASK_PNGAMEROOM = 0x1be,
    //CA_REPLY_PNGAMEROOM = 0x1bf,
    //CZ_REQ_REMAINTIME = 0x1c0,
    //ZC_REPLY_REMAINTIME = 0x1c1,
    //ZC_INFO_REMAINTIME = 0x1c2,
    //ZC_BROADCAST2 = 0x1c3,
    //ZC_ADD_ITEM_TO_STORE2 = 0x1c4,
    //ZC_ADD_ITEM_TO_CART2 = 0x1c5,
    //CS_REQ_ENCRYPTION = 0x1c6,
    //SC_ACK_ENCRYPTION = 0x1c7,
    //ZC_SKILL_ENTRY2 = 0x1c9,
    //CZ_REQMAKINGHOMUN = 0x1ca,
    //CZ_MONSTER_TALK = 0x1cb,
    //ZC_MONSTER_TALK = 0x1cc,
    //ZC_AUTOSPELLLIST = 0x1cd,
    //CZ_SELECTAUTOSPELL = 0x1ce,
    //ZC_DEVOTIONLIST = 0x1cf,
    //ZC_SPIRITS = 0x1d0,
    //ZC_BLADESTOP = 0x1d1,
    //ZC_COMBODELAY = 0x1d2,
    //ZC_SOUND = 0x1d3,
    //ZC_OPEN_EDITDLGSTR = 0x1d4,
    //CZ_INPUT_EDITDLGSTR = 0x1d5,
    //ZC_NOTIFY_STANDENTRY2 = 0x1d8,
    //ZC_NOTIFY_NEWENTRY2 = 0x1d9,
    //ZC_NOTIFY_MOVEENTRY2 = 0x1da,
    //CA_REQ_HASH = 0x1db,
    //AC_ACK_HASH = 0x1dc,
    //CA_LOGIN2 = 0x1dd,
    //CZ_REQ_ACCOUNTNAME = 0x1df,
    //ZC_ACK_ACCOUNTNAME = 0x1e0,
    //ZC_SPIRITS2 = 0x1e1,
    //ZC_REQ_COUPLE = 0x1e2,
    //CZ_JOIN_COUPLE = 0x1e3,
    //ZC_START_COUPLE = 0x1e4,
    //CZ_REQ_JOIN_COUPLE = 0x1e5,
    //ZC_COUPLENAME = 0x1e6,
    //CZ_DORIDORI = 0x1e7,
    //CZ_MAKE_GROUP2 = 0x1e8,
    //ZC_ADD_MEMBER_TO_GROUP2 = 0x1e9,
    //ZC_CONGRATULATION = 0x1ea,
    //ZC_NOTIFY_POSITION_TO_GUILDM = 0x1eb,
    //ZC_GUILD_MEMBER_MAP_CHANGE = 0x1ec,
    //CZ_CHOPOKGI = 0x1ed,
    //ZC_NORMAL_ITEMLIST2 = 0x1ee,
    //ZC_CART_NORMAL_ITEMLIST2 = 0x1ef,
    //ZC_STORE_NORMAL_ITEMLIST2 = 0x1f0,
    //AC_NOTIFY_ERROR = 0x1f1,
    //ZC_UPDATE_CHARSTAT2 = 0x1f2,
    //ZC_REQ_EXCHANGE_ITEM2 = 0x1f4,
    //ZC_ACK_EXCHANGE_ITEM2 = 0x1f5,
    //ZC_REQ_BABY = 0x1f6,
    //CZ_JOIN_BABY = 0x1f7,
    //ZC_START_BABY = 0x1f8,
    //CZ_REQ_JOIN_BABY = 0x1f9,
    //CA_LOGIN3 = 0x1fa,
    //CH_DELETE_CHAR2 = 0x1fb,
    //ZC_REPAIRITEMLIST = 0x1fc,
    //CZ_REQ_ITEMREPAIR = 0x1fd,
    //ZC_ACK_ITEMREPAIR = 0x1fe,
    //ZC_HIGHJUMP = 0x1ff,
    //CA_CONNECT_INFO_CHANGED = 0x200,
    //ZC_FRIENDS_LIST = 0x201,
    //CZ_ADD_FRIENDS = 0x202,
    //CZ_DELETE_FRIENDS = 0x203,
    //CA_EXE_HASHCHECK = 0x204,
    //ZC_DIVORCE = 0x205,
    //ZC_FRIENDS_STATE = 0x206,
    //ZC_REQ_ADD_FRIENDS = 0x207,
    //CZ_ACK_REQ_ADD_FRIENDS = 0x208,
    //ZC_ADD_FRIENDS_LIST = 0x209,
    //ZC_DELETE_FRIENDS = 0x20a,
    //CH_EXE_HASHCHECK = 0x20b,
    //CZ_EXE_HASHCHECK = 0x20c,
    //ZC_STARSKILL = 0x20e,
    //CZ_REQ_PVPPOINT = 0x20f,
    //ZC_ACK_PVPPOINT = 0x210,
    //ZH_MOVE_PVPWORLD = 0x211,
    //CZ_REQ_GIVE_MANNER_BYNAME = 0x212,
    //CZ_REQ_STATUS_GM = 0x213,
    //ZC_ACK_STATUS_GM = 0x214,
    //ZC_SKILLMSG = 0x215,
    //ZC_BABYMSG = 0x216,
    //CZ_BLACKSMITH_RANK = 0x217,
    //CZ_ALCHEMIST_RANK = 0x218,
    //ZC_BLACKSMITH_RANK = 0x219,
    //ZC_ALCHEMIST_RANK = 0x21a,
    //ZC_BLACKSMITH_POINT = 0x21b,
    //ZC_ALCHEMIST_POINT = 0x21c,
    //CZ_LESSEFFECT = 0x21d,
    //ZC_LESSEFFECT = 0x21e,
    //ZC_NOTIFY_PKINFO = 0x21f,
    //ZC_NOTIFY_CRAZYKILLER = 0x220,
    //ZC_NOTIFY_WEAPONITEMLIST = 0x221,
    //CZ_REQ_WEAPONREFINE = 0x222,
    //ZC_ACK_WEAPONREFINE = 0x223,
    //ZC_TAEKWON_POINT = 0x224,
    //CZ_TAEKWON_RANK = 0x225,
    //ZC_TAEKWON_RANK = 0x226,
    //ZC_GAME_GUARD = 0x227,
    //CZ_ACK_GAME_GUARD = 0x228,
    //ZC_STATE_CHANGE3 = 0x229,
    //ZC_NOTIFY_STANDENTRY3 = 0x22a,
    //ZC_NOTIFY_NEWENTRY3 = 0x22b,
    //ZC_NOTIFY_MOVEENTRY3 = 0x22c,
    //CZ_COMMAND_MER = 0x22d,
    //ZC_PROPERTY_HOMUN = 0x22e,
    //ZC_FEED_MER = 0x22f,
    //ZC_CHANGESTATE_MER = 0x230,
    //CZ_RENAME_MER = 0x231,
    //CZ_REQUEST_ACTNPC = 0x233,
    //ZC_HOSKILLINFO_LIST = 0x235,
    //ZC_KILLER_POINT = 0x236,
    //CZ_KILLER_RANK = 0x237,
    //ZC_KILLER_RANK = 0x238,
    //ZC_HOSKILLINFO_UPDATE = 0x239,
    //ZC_REQ_STORE_PASSWORD = 0x23a,
    //CZ_ACK_STORE_PASSWORD = 0x23b,
    //ZC_RESULT_STORE_PASSWORD = 0x23c,
    //AC_EVENT_RESULT = 0x23d,
    //HC_REQUEST_CHARACTER_PASSWORD = 0x23e,
    //CZ_MAIL_GET_LIST = 0x23f,
    //ZC_MAIL_REQ_GET_LIST = 0x240,
    //CZ_MAIL_OPEN = 0x241,
    //ZC_MAIL_REQ_OPEN = 0x242,
    //CZ_MAIL_DELETE = 0x243,
    //CZ_MAIL_GET_ITEM = 0x244,
    //ZC_MAIL_REQ_GET_ITEM = 0x245,
    //CZ_MAIL_RESET_ITEM = 0x246,
    //CZ_MAIL_ADD_ITEM = 0x247,
    //CZ_MAIL_SEND = 0x248,
    //ZC_MAIL_REQ_SEND = 0x249,
    //ZC_MAIL_RECEIVE = 0x24a,
    //CZ_AUCTION_CREATE = 0x24b,
    //CZ_AUCTION_ADD_ITEM = 0x24c,
    //CZ_AUCTION_ADD = 0x24d,
    //CZ_AUCTION_ADD_CANCEL = 0x24e,
    //CZ_AUCTION_BUY = 0x24f,
    //ZC_AUCTION_RESULT = 0x250,
    //CZ_AUCTION_ITEM_SEARCH = 0x251,
    //ZC_AUCTION_ITEM_REQ_SEARCH = 0x252,
    //ZC_STARPLACE = 0x253,
    //CZ_AGREE_STARPLACE = 0x254,
    //ZC_ACK_MAIL_ADD_ITEM = 0x255,
    //ZC_ACK_AUCTION_ADD_ITEM = 0x256,
    //ZC_ACK_MAIL_DELETE = 0x257,
    //CA_REQ_GAME_GUARD_CHECK = 0x258,
    //AC_ACK_GAME_GUARD = 0x259,
    //ZC_MAKINGITEM_LIST = 0x25a,
    //CZ_REQ_MAKINGITEM = 0x25b,
    //CZ_AUCTION_REQ_MY_INFO = 0x25c,
    //CZ_AUCTION_REQ_MY_SELL_STOP = 0x25d,
    //ZC_AUCTION_ACK_MY_SELL_STOP = 0x25e,
    //ZC_AUCTION_WINDOWS = 0x25f,
    //ZC_MAIL_WINDOWS = 0x260,
    //AC_REQ_LOGIN_OLDEKEY = 0x261,
    //AC_REQ_LOGIN_NEWEKEY = 0x262,
    //AC_REQ_LOGIN_CARDPASS = 0x263,
    //CA_ACK_LOGIN_OLDEKEY = 0x264,
    //CA_ACK_LOGIN_NEWEKEY = 0x265,
    //CA_ACK_LOGIN_CARDPASS = 0x266,
    //AC_ACK_EKEY_FAIL_NOTEXIST = 0x267,
    //AC_ACK_EKEY_FAIL_NOTUSESEKEY = 0x268,
    //AC_ACK_EKEY_FAIL_NOTUSEDEKEY = 0x269,
    //AC_ACK_EKEY_FAIL_AUTHREFUSE = 0x26a,
    //AC_ACK_EKEY_FAIL_INPUTEKEY = 0x26b,
    //AC_ACK_EKEY_FAIL_NOTICE = 0x26c,
    //AC_ACK_EKEY_FAIL_NEEDCARDPASS = 0x26d,
    //AC_ACK_AUTHEKEY_FAIL_NOTMATCHCARDPASS = 0x26e,
    //AC_ACK_FIRST_LOGIN = 0x26f,
    //AC_REQ_LOGIN_ACCOUNT_INFO = 0x270,
    //CA_ACK_LOGIN_ACCOUNT_INFO = 0x271,
    //AC_ACK_PT_ID_INFO = 0x272,
    //CZ_REQ_MAIL_RETURN = 0x273,
    //ZC_ACK_MAIL_RETURN = 0x274,

    //CA_LOGIN_PCBANG = 0x277,
    //ZC_NOTIFY_PCBANG = 0x278,
    //CZ_HUNTINGLIST = 0x279,
    //ZC_HUNTINGLIST = 0x27a,
    //ZC_PCBANG_EFFECT = 0x27b,
    //CA_LOGIN4 = 0x27c,
    //ZC_PROPERTY_MERCE = 0x27d,
    //ZC_SHANDA_PROTECT = 0x27e,
    //CA_CLIENT_TYPE = 0x27f,
    //ZC_GANGSI_POINT = 0x280,
    //CZ_GANGSI_RANK = 0x281,
    //ZC_GANGSI_RANK = 0x282,
    //ZC_NOTIFY_EFFECT3 = 0x284,
    //ZC_DEATH_QUESTION = 0x285,
    //CZ_DEATH_QUESTION = 0x286,
    //ZC_PC_CASH_POINT_ITEMLIST = 0x287,
    //CZ_PC_BUY_CASH_POINT_ITEM = 0x288,
    //ZC_PC_CASH_POINT_UPDATE = 0x289,
    //ZC_NPC_SHOWEFST_UPDATE = 0x28a,
    //HC_CHARNOTBEENSELECTED = 0x28b,
    //CH_SELECT_CHAR_GOINGTOBEUSED = 0x28c,
    //CH_REQ_IS_VALID_CHARNAME = 0x28d,
    //HC_ACK_IS_VALID_CHARNAME = 0x28e,
    //CH_REQ_CHANGE_CHARNAME = 0x28f,
    //HC_ACK_CHANGE_CHARNAME = 0x290,
    //CZ_STANDING_RESURRECTION = 0x292,
    //ZC_BOSS_INFO = 0x293,
    //ZC_READ_BOOK = 0x294,
    //ZC_EQUIPMENT_ITEMLIST2 = 0x295,
    //ZC_STORE_EQUIPMENT_ITEMLIST2 = 0x296,
    //ZC_CART_EQUIPMENT_ITEMLIST2 = 0x297,
    //ZC_CASH_TIME_COUNTER = 0x298,
    //ZC_CASH_ITEM_DELETE = 0x299,
    //ZC_ITEM_PICKUP_ACK2 = 0x29a,
    //ZC_MER_INIT = 0x29b,
    //ZC_MER_PROPERTY = 0x29c,
    //ZC_MER_SKILLINFO_LIST = 0x29d,
    //ZC_MER_SKILLINFO_UPDATE = 0x29e,
    //CZ_MER_COMMAND = 0x29f,
    //UNUSED_CZ_MER_USE_SKILL = 0x2a0,
    //UNUSED_CZ_MER_UPGRADE_SKILLLEVEL = 0x2a1,
    //ZC_MER_PAR_CHANGE = 0x2a2,
    //ZC_GAMEGUARD_LINGO_KEY = 0x2a3,
    //CZ_GAMEGUARD_LINGO_READY = 0x2a4,
    //CZ_KSY_EVENT = 0x2a5,
    //ZC_HACKSH_CPX_MSG = 0x2a6,
    //CZ_HACKSH_CPX_MSG = 0x2a7,
    //ZC_HACKSHIELD_CRC_MSG = 0x2a8,
    //CZ_HACKSHIELD_CRC_MSG = 0x2a9,
    //ZC_REQ_CASH_PASSWORD = 0x2aa,
    //CZ_ACK_CASH_PASSWORD = 0x2ab,
    //ZC_RESULT_CASH_PASSWORD = 0x2ac,
    //AC_REQUEST_SECOND_PASSWORD = 0x2ad,
    //ZC_SRPACKET_INIT = 0x2ae,
    //CZ_SRPACKET_START = 0x2af,
    //CA_LOGIN_CHANNEL = 0x2b0,
    //ZC_ALL_QUEST_LIST = 0x2b1,
    //ZC_ALL_QUEST_MISSION = 0x2b2,
    //ZC_ADD_QUEST = 0x2b3,
    //ZC_DEL_QUEST = 0x2b4,
    //ZC_UPDATE_MISSION_HUNT = 0x2b5,
    //CZ_ACTIVE_QUEST = 0x2b6,
    //ZC_ACTIVE_QUEST = 0x2b7,
    //ZC_ITEM_PICKUP_PARTY = 0x2b8,
    //ZC_SHORTCUT_KEY_LIST = 0x2b9,
    //CZ_SHORTCUT_KEY_CHANGE = 0x2ba,
    //ZC_EQUIPITEM_DAMAGED = 0x2bb,
    //ZC_NOTIFY_PCBANG_PLAYING_TIME = 0x2bc,
    //ZC_SRCRYPTOR2_INIT = 0x2bd,
    //CZ_SRCRYPTOR2_START = 0x2be,
    //ZC_SRPACKETR2_INIT = 0x2bf,
    //CZ_SRPACKETR2_START = 0x2c0,
    //ZC_NPC_CHAT = 0x2c1,
    //ZC_FORMATSTRING_MSG = 0x2c2,
    //UNUSED_CZ_FORMATSTRING_MSG_RES = 0x2c3,
    //CZ_PARTY_JOIN_REQ = 0x2c4,
    //ZC_PARTY_JOIN_REQ_ACK = 0x2c5,
    //ZC_PARTY_JOIN_REQ = 0x2c6,
    //CZ_PARTY_JOIN_REQ_ACK = 0x2c7,
    //CZ_PARTY_CONFIG = 0x2c8,
    //HC_REFUSE_SELECTCHAR = 0x2ca,
    //ZC_MEMORIALDUNGEON_SUBSCRIPTION_INFO = 0x2cb,
    //ZC_MEMORIALDUNGEON_SUBSCRIPTION_NOTIFY = 0x2cc,
    //ZC_MEMORIALDUNGEON_INFO = 0x2cd,
    //ZC_MEMORIALDUNGEON_NOTIFY = 0x2ce,
    //CZ_MEMORIALDUNGEON_COMMAND = 0x2cf,
    //ZC_EQUIPMENT_ITEMLIST3 = 0x2d0,
    //ZC_STORE_EQUIPMENT_ITEMLIST3 = 0x2d1,
    //ZC_CART_EQUIPMENT_ITEMLIST3 = 0x2d2,
    //ZC_NOTIFY_BIND_ON_EQUIP = 0x2d3,
    //ZC_ITEM_PICKUP_ACK3 = 0x2d4,
    //ZC_ISVR_DISCONNECT = 0x2d5,
    //CZ_EQUIPWIN_MICROSCOPE = 0x2d6,
    //ZC_EQUIPWIN_MICROSCOPE = 0x2d7,
    //CZ_CONFIG = 0x2d8,
    //CZ_BATTLEFIELD_CHAT = 0x2db,
    //ZC_BATTLEFIELD_CHAT = 0x2dc,
    //ZC_BATTLEFIELD_NOTIFY_CAMPINFO = 0x2dd,
    //ZC_BATTLEFIELD_NOTIFY_POINT = 0x2de,
    //ZC_BATTLEFIELD_NOTIFY_POSITION = 0x2df,
    //ZC_BATTLEFIELD_NOTIFY_HP = 0x2e0,
    //ZC_NOTIFY_ACT2 = 0x2e1,
    //CZ_USE_ITEM_NEW_JAPEN = 0x2e2,
    //CZ_USE_SKILL_NEW_JAPEN = 0x2e3,
    //CZ_ITEM_PICKUP_NEW_JAPEN = 0x2e4,

    //CZ_BOT_CHECK = 0x2e6,
    //ZC_MAPPROPERTY = 0x2e7,
    //ZC_NORMAL_ITEMLIST3 = 0x2e8,
    //ZC_CART_NORMAL_ITEMLIST3 = 0x2e9,
    //ZC_STORE_NORMAL_ITEMLIST3 = 0x2ea,
    //ZC_NOTIFY_MOVEENTRY4 = 0x2ec,
    //ZC_NOTIFY_NEWENTRY4 = 0x2ed,
    //ZC_NOTIFY_STANDENTRY4 = 0x2ee,
    //ZC_NOTIFY_FONT = 0x2ef,
    //ZC_PROGRESS = 0x2f0,
    //CZ_PROGRESS = 0x2f1,
    //ZC_PROGRESS_CANCEL = 0x2f2,
    //CZ_IRMAIL_SEND = 0x2f3,
    //ZC_IRMAIL_SEND_RES = 0x2f4,
    //ZC_IRMAIL_NOTIFY = 0x2f5,

    //ZC_SIMPLE_CASHSHOP_POINT_ITEMLIST = 0x35d,

    //AHC_GAME_GUARD = 0x3dd,
    //CAH_ACK_GAME_GUARD = 0x3de,
    //ZC_WAITINGROOM_PARTYPLAY_JOIN = 0x3df,
    //CZ_WAITINGROOM_PARTYPLAY_JOIN_RESULT = 0x3e0,
    //ZC_WAITINGROOM_SUBSCRIPTION_RESULT = 0x3e1,
    //CZ_USE_SKILL2 = 0x438,
    //ZC_REQ_CRACKPROOF = 0x43a,
    //CZ_ACK_CRACKPROOF = 0x43b,
    //ZC_CRACKPROOF_ERRCODE = 0x43c,
    //ZC_SKILL_POSTDELAY = 0x43d,
    //ZC_SKILL_POSTDELAY_LIST = 0x43e,
    //ZC_MSG_STATE_CHANGE2 = 0x43f,
    //ZC_MILLENNIUMSHIELD = 0x440,
    //ZC_SKILLINFO_DELETE = 0x441,
    //ZC_SKILL_SELECT_REQUEST = 0x442,
    //CZ_SKILL_SELECT_RESPONSE = 0x443,
    //ZC_SIMPLE_CASH_POINT_ITEMLIST = 0x444,
    //CZ_SIMPLE_BUY_CASH_POINT_ITEM = 0x445,
    //CZ_BLOCKING_PLAY_CANCEL = 0x447,
    //HC_CHARACTER_LIST = 0x448,
    //ZC_HACKSH_ERROR_MSG = 0x449,
    //CZ_CLIENT_VERSION = 0x44a,
    //CZ_CLOSE_SIMPLECASH_SHOP = 0x44b,
    //ZC_ES_RESULT = 0x7d0,
    //CZ_ES_GET_LIST = 0x7d1,
    //ZC_ES_LIST = 0x7d2,
    //CZ_ES_CHOOSE = 0x7d3,
    //CZ_ES_CANCEL = 0x7d4,
    //ZC_ES_READY = 0x7d5,
    //ZC_ES_GOTO = 0x7d6,
    //CZ_GROUPINFO_CHANGE_V2 = 0x7d7,
    //ZC_REQ_GROUPINFO_CHANGE_V2 = 0x7d8,
    //ZC_SHORTCUT_KEY_LIST_V2 = 0x7d9,
    //ZC_SHORTCUT_KEY_LIST_V3 = 0xa00,
    //CZ_CHANGE_GROUP_MASTER = 0x7da,
    //ZC_HO_PAR_CHANGE = 0x7db,
    //CZ_SEEK_PARTY = 0x7dc,
    //ZC_SEEK_PARTY = 0x7dd,
    //CZ_SEEK_PARTY_MEMBER = 0x7de,
    //ZC_SEEK_PARTY_MEMBER = 0x7df,
    //ZC_ES_NOTI_MYINFO = 0x7e0,
    //ZC_SKILLINFO_UPDATE2 = 0x7e1,
    //ZC_MSG_VALUE = 0x7e2,
    //ZC_ITEMLISTWIN_OPEN = 0x7e3,
    //CZ_ITEMLISTWIN_RES = 0x7e4,
    //CH_ENTER_CHECKBOT = 0x7e5,
    //ZC_MSG_SKILL = 0x7e6,
    //CH_CHECKBOT = 0x7e7,
    //HC_CHECKBOT = 0x7e8,
    //HC_CHECKBOT_RESULT = 0x7e9,
    //CZ_BATTLE_FIELD_LIST = 0x7ea,
    //ZC_BATTLE_FIELD_LIST = 0x7eb,
    //CZ_JOIN_BATTLE_FIELD = 0x7ec,
    //ZC_JOIN_BATTLE_FIELD = 0x7ed,
    //CZ_CANCEL_BATTLE_FIELD = 0x7ee,
    //ZC_CANCEL_BATTLE_FIELD = 0x7ef,
    //CZ_REQ_BATTLE_STATE_MONITOR = 0x7f0,
    //ZC_ACK_BATTLE_STATE_MONITOR = 0x7f1,
    //ZC_BATTLE_NOTI_START_STEP = 0x7f2,
    //ZC_BATTLE_JOIN_NOTI_DEFER = 0x7f3,
    //ZC_BATTLE_JOIN_DISABLE_STATE = 0x7f4,
    //CZ_GM_FULLSTRIP = 0x7f5,
    //ZC_NOTIFY_EXP = 0x7f6,
    //ZC_NOTIFY_MOVEENTRY7 = 0x7f7,
    //ZC_NOTIFY_NEWENTRY5 = 0x7f8,
    //ZC_NOTIFY_STANDENTRY5 = 0x7f9,
    //ZC_CHANGE_GROUP_MASTER = 0x7fc,
    //ZC_BROADCASTING_SPECIAL_ITEM_OBTAIN = 0x7fd,
    //ZC_PLAY_NPC_BGM = 0x7fe,
    //ZC_DEFINE_CHECK = 0x7ff,
    //CZ_PC_PURCHASE_ITEMLIST_FROMMC2 = 0x801,
    //CZ_PARTY_BOOKING_REQ_REGISTER = 0x802,
    //ZC_PARTY_BOOKING_ACK_REGISTER = 0x803,
    //CZ_PARTY_BOOKING_REQ_SEARCH = 0x804,
    //ZC_PARTY_BOOKING_ACK_SEARCH = 0x805,
    //CZ_PARTY_BOOKING_REQ_DELETE = 0x806,
    //ZC_PARTY_BOOKING_ACK_DELETE = 0x807,
    //CZ_PARTY_BOOKING_REQ_UPDATE = 0x808,
    //ZC_PARTY_BOOKING_NOTIFY_INSERT = 0x809,
    //ZC_PARTY_BOOKING_NOTIFY_UPDATE = 0x80a,
    //ZC_PARTY_BOOKING_NOTIFY_DELETE = 0x80b,
    //CZ_SIMPLE_CASH_BTNSHOW = 0x80c,
    //ZC_SIMPLE_CASH_BTNSHOW = 0x80d,
    //ZC_NOTIFY_HP_TO_GROUPM_R2 = 0x80e,
    //ZC_ADD_EXCHANGE_ITEM2 = 0x80f,
    //ZC_OPEN_BUYING_STORE = 0x810,
    //CZ_REQ_OPEN_BUYING_STORE = 0x811,
    //ZC_FAILED_OPEN_BUYING_STORE_TO_BUYER = 0x812,
    //ZC_MYITEMLIST_BUYING_STORE = 0x813,
    //ZC_BUYING_STORE_ENTRY = 0x814,
    //CZ_REQ_CLOSE_BUYING_STORE = 0x815,
    //ZC_DISAPPEAR_BUYING_STORE_ENTRY = 0x816,
    //CZ_REQ_CLICK_TO_BUYING_STORE = 0x817,
    //ZC_ACK_ITEMLIST_BUYING_STORE = 0x818,
    //CZ_REQ_TRADE_BUYING_STORE = 0x819,
    //ZC_FAILED_TRADE_BUYING_STORE_TO_BUYER = 0x81a,
    //ZC_UPDATE_ITEM_FROM_BUYING_STORE = 0x81b,
    //ZC_ITEM_DELETE_BUYING_STORE = 0x81c,
    //ZC_EL_INIT = 0x81d,
    //ZC_EL_PAR_CHANGE = 0x81e,
    //ZC_BROADCAST4 = 0x81f,
    //ZC_COSTUME_SPRITE_CHANGE = 0x820,
    //AC_OTP_USER = 0x821,
    //CA_OTP_AUTH_REQ = 0x822,
    //AC_OTP_AUTH_ACK = 0x823,
    //ZC_FAILED_TRADE_BUYING_STORE_TO_SELLER = 0x824,
    //CA_SSO_LOGIN_REQ = 0x825,
    //AC_SSO_LOGIN_ACK = 0x826,
    //CH_DELETE_CHAR3_RESERVED = 0x827,
    //HC_DELETE_CHAR3_RESERVED = 0x828,
    //CH_DELETE_CHAR3 = 0x829,
    //HC_DELETE_CHAR3 = 0x82a,
    //CH_DELETE_CHAR3_CANCEL = 0x82b,
    //HC_DELETE_CHAR3_CANCEL = 0x82c,
    //CZ_SEARCH_STORE_INFO = 0x835,
    //ZC_SEARCH_STORE_INFO_ACK = 0x836,
    //ZC_SEARCH_STORE_INFO_FAILED = 0x837,
    //CZ_SEARCH_STORE_INFO_NEXT_PAGE = 0x838,
    //ZC_ACK_BAN_GUILD_SSO = 0x839,
    //ZC_OPEN_SEARCH_STORE_INFO = 0x83a,
    //CZ_CLOSE_SEARCH_STORE_INFO = 0x83b,
    //CZ_SSILIST_ITEM_CLICK = 0x83c,
    //ZC_SSILIST_ITEM_CLICK_ACK = 0x83d,
    //AC_REFUSE_LOGIN_R2 = 0x83e,
    //ZC_SEARCH_STORE_OPEN_INFO = 0x83f,
    //HC_NOTIFY_ACCESSIBLE_MAPNAME = 0x840,
    //CH_SELECT_ACCESSIBLE_MAPNAME = 0x841,
    //CZ_RECALL_SSO = 0x842,
    //CZ_REMOVE_AID_SSO = 0x843,
    //CZ_SE_CASHSHOP_OPEN = 0x844,
    //ZC_SE_CASHSHOP_OPEN = 0x845,
    //CZ_REQ_SE_CASH_TAB_CODE = 0x846,
    //ZC_ACK_SE_CASH_ITEM_LIST = 0x847,
    //CZ_SE_PC_BUY_CASHITEM_LIST = 0x848,
    //ZC_SE_PC_BUY_CASHITEM_RESULT = 0x849,
    //CZ_SE_CASHSHOP_CLOSE = 0x84a,
    //CZ_MACRO_USE_SKILL = 0x84c,
    //CZ_MACRO_USE_SKILL_TOGROUND = 0x84d,
    //CZ_MACRO_REQUEST_MOVE = 0x84e,
    //CZ_MACRO_ITEM_PICKUP = 0x84f,
    //CZ_MACRO_REQUEST_ACT = 0x850,
    //ZC_GPK_DYNCODE = 0x851,
    //CZ_GPK_DYNCODE_RELOAD = 0x852,
    //ZC_GPK_AUTH = 0x853,
    //CZ_GPK_AUTH = 0x854,
    //ZC_MACRO_ITEMPICKUP_FAIL = 0x855,
    //ZC_NOTIFY_MOVEENTRY8 = 0x856,
    //ZC_NOTIFY_STANDENTRY7 = 0x857,
    //ZC_NOTIFY_NEWENTRY6 = 0x858,
    //ZC_EQUIPWIN_MICROSCOPE2 = 0x859,
    //ZC_REASSEMBLY_AUTH01 = 0x85a,
    //ZC_REASSEMBLY_AUTH02 = 0x85b,
    //ZC_REASSEMBLY_AUTH03 = 0x85c,
    //ZC_REASSEMBLY_AUTH04 = 0x85d,
    //ZC_REASSEMBLY_AUTH05 = 0x85e,
    //ZC_REASSEMBLY_AUTH06 = 0x85f,
    //ZC_REASSEMBLY_AUTH07 = 0x860,
    //ZC_REASSEMBLY_AUTH08 = 0x861,
    //ZC_REASSEMBLY_AUTH09 = 0x862,
    //ZC_REASSEMBLY_AUTH10 = 0x863,
    //ZC_REASSEMBLY_AUTH11 = 0x864,
    //ZC_REASSEMBLY_AUTH12 = 0x865,
    //ZC_REASSEMBLY_AUTH13 = 0x866,
    //ZC_REASSEMBLY_AUTH14 = 0x867,
    //ZC_REASSEMBLY_AUTH15 = 0x868,
    //ZC_REASSEMBLY_AUTH16 = 0x869,
    //ZC_REASSEMBLY_AUTH17 = 0x86a,
    //ZC_REASSEMBLY_AUTH18 = 0x86b,
    //ZC_REASSEMBLY_AUTH19 = 0x86c,
    //ZC_REASSEMBLY_AUTH20 = 0x86d,
    //ZC_REASSEMBLY_AUTH21 = 0x86e,
    //ZC_REASSEMBLY_AUTH22 = 0x86f,
    //ZC_REASSEMBLY_AUTH23 = 0x870,
    //ZC_REASSEMBLY_AUTH24 = 0x871,
    //ZC_REASSEMBLY_AUTH25 = 0x872,
    //ZC_REASSEMBLY_AUTH26 = 0x873,
    //ZC_REASSEMBLY_AUTH27 = 0x874,
    //ZC_REASSEMBLY_AUTH28 = 0x875,
    //ZC_REASSEMBLY_AUTH29 = 0x876,
    //ZC_REASSEMBLY_AUTH30 = 0x877,
    //ZC_REASSEMBLY_AUTH31 = 0x878,
    //ZC_REASSEMBLY_AUTH32 = 0x879,
    //ZC_REASSEMBLY_AUTH33 = 0x87a,
    //ZC_REASSEMBLY_AUTH34 = 0x87b,
    //ZC_REASSEMBLY_AUTH35 = 0x87c,
    //ZC_REASSEMBLY_AUTH36 = 0x87d,
    //ZC_REASSEMBLY_AUTH37 = 0x87e,
    //ZC_REASSEMBLY_AUTH38 = 0x87f,
    //ZC_REASSEMBLY_AUTH39 = 0x880,
    //ZC_REASSEMBLY_AUTH40 = 0x881,
    //ZC_REASSEMBLY_AUTH41 = 0x882,
    //ZC_REASSEMBLY_AUTH42 = 0x883,
    //CZ_REASSEMBLY_AUTH01 = 0x884,
    //CZ_REASSEMBLY_AUTH02 = 0x885,
    //CZ_REASSEMBLY_AUTH03 = 0x886,
    //CZ_REASSEMBLY_AUTH04 = 0x887,
    //CZ_REASSEMBLY_AUTH05 = 0x888,
    //CZ_REASSEMBLY_AUTH06 = 0x889,
    //CZ_REASSEMBLY_AUTH07 = 0x88a,
    //CZ_REASSEMBLY_AUTH08 = 0x88b,
    //CZ_REASSEMBLY_AUTH09 = 0x88c,
    //CZ_REASSEMBLY_AUTH10 = 0x88d,
    //CZ_REASSEMBLY_AUTH11 = 0x88e,
    //CZ_REASSEMBLY_AUTH12 = 0x88f,
    //CZ_REASSEMBLY_AUTH13 = 0x890,
    //CZ_REASSEMBLY_AUTH14 = 0x891,
    //CZ_REASSEMBLY_AUTH15 = 0x892,
    //CZ_REASSEMBLY_AUTH16 = 0x893,
    //CZ_REASSEMBLY_AUTH17 = 0x894,
    //CZ_REASSEMBLY_AUTH18 = 0x895,
    //CZ_REASSEMBLY_AUTH19 = 0x896,
    //CZ_REASSEMBLY_AUTH20 = 0x897,
    //CZ_REASSEMBLY_AUTH21 = 0x898,
    //CZ_REASSEMBLY_AUTH22 = 0x899,
    //CZ_REASSEMBLY_AUTH23 = 0x89a,
    //CZ_REASSEMBLY_AUTH24 = 0x89b,
    //CZ_REASSEMBLY_AUTH25 = 0x89c,
    //CZ_REASSEMBLY_AUTH26 = 0x89d,
    //CZ_REASSEMBLY_AUTH27 = 0x89e,
    //CZ_REASSEMBLY_AUTH28 = 0x89f,
    //CZ_REASSEMBLY_AUTH29 = 0x8a0,
    //CZ_REASSEMBLY_AUTH30 = 0x8a1,
    //CZ_REASSEMBLY_AUTH31 = 0x8a2,
    //CZ_REASSEMBLY_AUTH32 = 0x8a3,
    //CZ_REASSEMBLY_AUTH33 = 0x8a4,
    //CZ_REASSEMBLY_AUTH34 = 0x8a5,
    //CZ_REASSEMBLY_AUTH35 = 0x8a6,
    //CZ_REASSEMBLY_AUTH36 = 0x8a7,
    //CZ_REASSEMBLY_AUTH37 = 0x8a8,
    //CZ_REASSEMBLY_AUTH38 = 0x8a9,
    //CZ_REASSEMBLY_AUTH39 = 0x8aa,
    //CZ_REASSEMBLY_AUTH40 = 0x8ab,
    //CZ_REASSEMBLY_AUTH41 = 0x8ac,
    //CZ_REASSEMBLY_AUTH42 = 0x8ad,
    //CC_REPLAYPACKET = 0x8ae,
    //HC_WAITING_LOGIN = 0x8af,
    //CH_WAITING_LOGIN = 0x8b0,
    //ZC_MCSTORE_NOTMOVEITEM_LIST = 0x8b1,
    //AC_REALNAME_AUTH = 0x8b2,
    //ZC_SHOWSCRIPT = 0x8b3,
    //ZC_START_COLLECTION = 0x8b4,
    //CZ_TRYCOLLECTION = 0x8b5,
    //ZC_TRYCOLLECTION = 0x8b6,
    //HC_SECOND_PASSWD_REQ = 0x8b7,
    //CH_SECOND_PASSWD_ACK = 0x8b8,
    //CH_MAKE_SECOND_PASSWD = 0x8ba,
    //HC_MAKE_SECOND_PASSWD = 0x8bb,
    //CH_DELETE_SECOND_PASSWD = 0x8bc,
    //HC_DELETE_SECOND_PASSWD = 0x8bd,
    //CH_EDIT_SECOND_PASSWD = 0x8be,
    //HC_EDIT_SECOND_PASSWD = 0x8bf,
    //ZC_ACK_SE_CASH_ITEM_LIST2 = 0x8c0,
    //CZ_MACRO_START = 0x8c1,
    //CZ_MACRO_STOP = 0x8c2,
    //CH_NOT_AVAILABLE_SECOND_PASSWD = 0x8c3,
    //HC_NOT_AVAILABLE_SECOND_PASSWD = 0x8c4,
    //CH_AVAILABLE_SECOND_PASSWD = 0x8c5,
    //HC_AVAILABLE_SECOND_PASSWD = 0x8c6,
    //ZC_SKILL_ENTRY3 = 0x8c7,
    LAST = 0x8c9,
}