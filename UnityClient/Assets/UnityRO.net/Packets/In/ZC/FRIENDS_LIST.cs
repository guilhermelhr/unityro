using ROIO.Utils;
using System.Collections.Generic;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_FRIENDS_LIST")]
    public class FRIENDS_LIST : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_FRIENDS_LIST;
        public PacketHeader Header => HEADER;

        public List<FriendListItem> FriendList;

        public void Read(MemoryStreamReader br, int size) {
            int n = (int) ((br.Length - br.Position) / FriendListItem.BLOCK_SIZE);
            FriendList = new List<FriendListItem>();

            for (int i = 0; i < n; i++) {
                FriendList.Add(new FriendListItem(br));
            }
        }
    }
}
