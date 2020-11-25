//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//class PathFinder {

//    private GAT _GAT { get; set; }

//    private const short MAX_HEAP = 150;
//    private const short MAX_WALKPATH = 32;

//    private uint[] _heap = new uint[MAX_HEAP];
//    private uint[] _heapClean = new uint[MAX_HEAP];

//    private ushort[] shortClean = new ushort[MAX_WALKPATH * MAX_WALKPATH];
//    private byte[] charClean = new byte[MAX_WALKPATH * MAX_WALKPATH];
//    private byte[] dc = new byte[4];

//    private ushort[] _x = new ushort[MAX_WALKPATH * MAX_WALKPATH];
//    private ushort[] _y = new ushort[MAX_WALKPATH * MAX_WALKPATH];
//    private ushort[] _dist = new ushort[MAX_WALKPATH * MAX_WALKPATH];
//    private ushort[] _cost = new ushort[MAX_WALKPATH * MAX_WALKPATH];
//    private ushort[] _before = new ushort[MAX_WALKPATH * MAX_WALKPATH];
//    private byte[] _flag = new byte[MAX_WALKPATH * MAX_WALKPATH];

//    private uint CalcIndex(int x, int y) => (uint)((x + y * MAX_WALKPATH) & (MAX_WALKPATH * MAX_WALKPATH - 1));
//    private uint CalcCost(int i, int x1, int y1) => (uint)((Math.Abs(x1 - _x[i]) + Math.Abs(y1 - _y[i])) * 10 + _dist[i]);

//    /**
//	 * Heap push (helper function)
//	 *
//	 * @param {Uint32Array} heap
//	 * @param {number} index
//	 */
//    private void PushHeapPath(uint[] heap, uint index) {
//        uint i, h = heap[0]++;

//        for (i = (h - 1) >> 1; h > 0 && _cost[index] < _cost[heap[i + 1]]; i = (h - 1) >> 1) {
//            heap[h + 1] = heap[i + 1];
//            h = i;
//        }

//        heap[h + 1] = index;
//    }

//    /**
//	 * Heap update (helper function)
//	 * Move toward the root because cost has decreased.
//	 *
//	 * @param {Uint32Array} heap
//	 * @param {number} index
//	 */
//    private void UpdateHeapPath(uint[] heap, uint index) {
//        uint i, h, cost;

//        for (h = 0; h < heap[0] && heap[h + 1] != index; ++h) ;

//        if (h == heap[0]) {
//            throw new Exception("PathFinding::update_heap_path() - Error updating head path");
//        }

//        cost = _cost[index];

//        for (i = (h - 1) >> 1; h > 0 && cost < _cost[heap[i + 1]]; i = (h - 1) >> 1) {
//            heap[h + 1] = heap[i + 1];
//            h = i;
//        }

//        heap[h + 1] = index;
//    }

//    /**
//	 * Heap pop (helper function)
//	 *
//	 * @param {Uint32Array} heap
//	 * @return {number}
//	 */
//    private uint PopHeapPath(uint[] heap) {
//        uint i, h, k, ret, last, cost;

//        if (heap[0] <= 0)
//            return unchecked((uint)-1);

//        ret = +heap[1];
//        last = +heap[heap[0]--];
//        cost = _cost[last];

//        for (h = 0, k = 2; k < heap[0]; k = k * 2 + 2) {
//            if (_cost[heap[k + 1]] > _cost[heap[k]]) {
//                k--;
//            }
//            heap[h + 1] = +heap[k + 1];
//            h = k;
//        }

//        if (k == heap[0]) {
//            heap[h + 1] = +heap[k];
//            h = k - 1;
//        }

//        for (i = (h - 1) >> 1; h > 0 && _cost[heap[i + 1]] > cost; i = (h - 1) >> 1) {
//            heap[h + 1] = heap[i + 1];
//            h = i;
//        }

//        heap[h + 1] = last;

//        return ret;
//    }

//    /**
//	 * calculate cost for the specified position
//	 *
//	 * @param {Uint32Array} heap
//	 * @param {number} x
//	 * @param {number} y
//	 * @param {number} dist
//	 * @param {number} before
//	 * @param {number} cost
//	 */
//    private uint AddPath(uint[] heap, ushort x, ushort y, ushort dist, ushort before, ushort cost) {
//        var i = CalcIndex(x, y);

//        if (_x[i] == x && _y[i] == y) {

//            if (_dist[i] > dist) {
//                _dist[i] = dist;
//                _before[i] = before;
//                _cost[i] = cost;

//                if (_flag[i] != null) {
//                    PushHeapPath(heap, i);
//                } else {
//                    UpdateHeapPath(heap, i);
//                }

//                _flag[i] = 0;
//            }

//            return 0;
//        }

//        if (_x[i] != null || _y[i] != null) {
//            return 1;
//        }

//        _x[i] = x;
//        _y[i] = y;
//        _dist[i] = dist;
//        _before[i] = before;
//        _cost[i] = cost;
//        _flag[i] = 0;

//        PushHeapPath(heap, i);
//        return 0;
//    }

//    /**
//	 * Find the direct patch between two points
//	 *
//	 * @param {number} x0
//	 * @param {number} y0
//	 * @param {number} x1
//	 * @param {number} y1
//	 * @param {Array} out
//	 * @param {number} range
//	 * @param {type} type - see Altitude.TYPE.* consts
//	 */
//    private int searchLong(int x0, int y0, int x1, int y1, int range, int[] outPath, GAT.TYPE type) {
//        //int i, dx, dy, x, y;
//        //var types = _GAT.cells.ToArray();
//        //var width = _GAT.width;

//        //dx = ((dx = x1 - x0) != null) ? ((dx < 0) ? -1 : 1) : 0;
//        //dy = ((dy = y1 - y0) != null) ? ((dy < 0) ? -1 : 1) : 0;
//        //x = x0;
//        //y = y0;
//        //i = 0;

//        //outPath[0] = (ushort)x0;
//        //outPath[1] = (ushort)y0;

//        //while ((i++) < MAX_WALKPATH) {
//        //    x += dx;
//        //    y += dy;

//        //    outPath[i * 2 + 0] = (ushort)x;
//        //    outPath[i * 2 + 1] = (ushort)y;

//        //    if (x == x1) dx = 0;
//        //    if (y == y1) dy = 0;

//        //    if ((dx == 0 && dy == 0) || (types[x + y * width] & type) == 0) {
//        //        break;
//        //    }
//        //}

//        //if (x == x1 && y == y1) {
//        //    // Range feature
//        //    if (range > 0) {
//        //        for (x = 0; x < i; ++x) {
//        //            if (Math.Abs(outPath[x * 2 + 0] - x1) <= range && Math.Abs(outPath[x * 2 + 1] - y1) <= range) {
//        //                return x + 1;
//        //            }
//        //        }
//        //    }

//        //    return i + 1;
//        //}

//        //// Range feature
//        //if (range > 0) {
//        //    x = x1 - x0;
//        //    y = y1 - y0;
//        //    if (Math.Sqrt(x * x + y * y) <= range) {
//        //        return searchLong(x0, y0, x1, y1, 0, outPath, GAT.TYPE.SNIPABLE);
//        //    }
//        //}

//        return 0;
//    }

//	/**
//	 * Find the path between two points.
//	 *
//	 * @param {vec2} from
//	 * @param {vec2} to
//	 * @param {number} range
//	 * @param {Array} out
//	 */
//	private int search(int x0, int y0, int x1, int y1, int range, int[] outPath) {
//		//uint[] heap;
//		//int x, y, i, j, rp, xs, ys;
//		//short e, f, len, dist, cost;

//		//// Import world
//		//var width = _GAT.width;
//		//var height = _GAT.height;
//		//var types = _GAT.cells;
//		//var TYPE = GAT.TYPE_TABLE;

//		//// Direct search
//		//i = searchLong(x0, y0, x1, y1, range, outPath, GAT.TYPE.WALKABLE);
//		//if (i != null) {
//		//	return i;
//		//}

//		//// Clean variables (avoid garbage collection problem)
//		//_heap = _heapClean;
//		//_x = shortClean;
//		//_y= shortClean;
//		//_dist = shortClean;
//		//_cost = shortClean;
//		//_before = shortClean;
//		//_flag = charClean;

//		//heap = _heap;
//		//outPath[0]   = x0;
//		//outPath[1]   = y0;

//		//i = (int)CalcIndex(x0, y0);
//		//_x[i] = (ushort)x0;
//		//_y[i] = (ushort)y0;
//		//_cost[i] = CalcCost(i, x1, y1);

//		//heap[0] = 0;
//		//push_heap_path(heap, i);


//		//xs = width - 1;
//		//ys = height - 1;

//		//while (true) {

//		//	// Clean up variables
//		//	e = 0;
//		//	f = 0;

//		//	dc[0] = 0;
//		//	dc[1] = 0;
//		//	dc[2] = 0;
//		//	dc[3] = 0;

//		//	rp = pop_heap_path(heap);

//		//	// No path found.
//		//	if (rp < 0) {
//		//		return 0;
//		//	}

//		//	x = _x[rp];
//		//	y = _y[rp];
//		//	dist = _dist[rp] + 10;
//		//	cost = _cost[rp];


//		//	// Finished
//		//	if (x == x1 && y == y1) {
//		//		break;
//		//	}

//		//	if (y < ys && types[(x + 0) + (y + 1) * width] & TYPE.WALKABLE) {
//		//		dc[0] = (y >= y1 ? 20 : 0);
//		//		f |= 1;
//		//		e += add_path(heap, x + 0, y + 1, dist, rp, cost + dc[0]);
//		//	}

//		//	if (x > 0 && types[(x - 1) + (y + 0) * width] & TYPE.WALKABLE) {
//		//		dc[1] = (x <= x1 ? 20 : 0);
//		//		f |= 2;
//		//		e += add_path(heap, x - 1, y + 0, dist, rp, cost + dc[1]);
//		//	}

//		//	if (y > 0 && types[(x + 0) + (y - 1) * width] & TYPE.WALKABLE) {
//		//		dc[2] = (y <= y1 ? 20 : 0);
//		//		f |= 4;
//		//		e += add_path(heap, x + 0, y - 1, dist, rp, cost + dc[2]);
//		//	}

//		//	if (x < xs && types[(x + 1) + (y + 0) * width] & TYPE.WALKABLE) {
//		//		dc[3] = (x >= x1 ? 20 : 0);
//		//		f |= 8;
//		//		e += add_path(heap, x + 1, y + 0, dist, rp, cost + dc[3]);
//		//	}

//		//	// Diagonals
//		//	if ((f & (2 + 1)) == 2 + 1 && types[(x - 1) + (y + 1) * width] & TYPE.WALKABLE) {
//		//		e += add_path(heap, x - 1, y + 1, dist + 4, rp, cost + dc[1] + dc[0] - 6);
//		//	}

//		//	if ((f & (2 + 4)) == 2 + 4 && types[(x - 1) + (y - 1) * width] & TYPE.WALKABLE) {
//		//		e += add_path(heap, x - 1, y - 1, dist + 4, rp, cost + dc[1] + dc[2] - 6);
//		//	}

//		//	if ((f & (8 + 4)) == 8 + 4 && types[(x + 1) + (y - 1) * width] & TYPE.WALKABLE) {
//		//		e += add_path(heap, x + 1, y - 1, dist + 4, rp, cost + dc[3] + dc[2] - 6);
//		//	}

//		//	if ((f & (8 + 1)) == 8 + 1 && types[(x + 1) + (y + 1) * width] & TYPE.WALKABLE) {
//		//		e += add_path(heap, x + 1, y + 1, dist + 4, rp, cost + dc[3] + dc[0] - 6);
//		//	}

//		//	_flag[rp] = 1;

//		//	// Too much... ending.
//		//	if (e || heap[0] >= MAX_HEAP - 5) {
//		//		return 0;
//		//	}
//		//}


//		//// Reorganize Path
//		//for (len = 0, i = rp; len < 100 && i !== calc_index(x0, y0); i = _before[i], len++) ;


//		//for (i = rp, j = len - 1; j >= 0; i = _before[i], j--) {
//		//	out[(j + 1)*2 + 0] = _x[i];
//		//	out[(j + 1)*2 + 1] = _y[i];
//		//}

//		//return len + 1;
//		return 0;
//	}


//}