using ROIO.Utils;

public class FriendListItem {

    public const int BLOCK_SIZE = 28;

    public uint AID;
    public uint CID;
    public string Name;

    public FriendListItem(MemoryStreamReader br) {

        AID = br.ReadUInt();
        CID = br.ReadUInt();
        Name = br.ReadBinaryString(24);
    }
}
