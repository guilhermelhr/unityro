using UnityEngine;

public class Entity : MonoBehaviour
{
    public const int TYPE_EFFECT    = -3;
	public const int TYPE_UNKNOWN   = -2;
	public const int TYPE_WARP      = -1;
	public const int TYPE_PC        =  0;
	public const int TYPE_DISGUISED =  1;
	public const int TYPE_MOB       =  5;
	public const int TYPE_NPC       =  6;
	public const int TYPE_PET       =  7;
	public const int TYPE_HOM       =  8;
	public const int TYPE_MERC      =  9;
	public const int TYPE_ELEM      = 10;
	public const int TYPE_ITEM      = 11;

    public int type;
    internal object weapon;
    internal object _job;
    internal object _sex;
}
