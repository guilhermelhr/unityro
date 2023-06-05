using System.Collections.Generic;

namespace Core.Path {
    public class PathNodeComparer : IComparer<PathNode> {
        public int Compare(PathNode first, PathNode second) {
            if (first!.Total == second!.Total) {
                return 0;
            } else if (first.Total > second.Total) {
                return -1;
            }

            return 1;
        }
    }
}