using System;

[Flags]
public enum ChatMessageType {
    SELF = 1 << 0,
    PUBLIC = 1 << 1,
    PRIVATE = 1 << 2,
    PARTY = 1 << 3,
    GUILD = 1 << 4,
    ANNOUNCE = 1 << 5,
    ERROR = 1 << 6,
    INFO = 1 << 7,
    BLUE = 1 << 8,
    ADMIN = 1 << 9,
}
