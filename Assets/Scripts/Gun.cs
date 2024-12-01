using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Pickups/Gun")]
public class Gun : ScriptableObject
{
    [SerializeField]
    public string gunName = "TEST";
    [SerializeField]
    public string gunDescription = "";
    [SerializeField]
    public Sprite gunSprite = null;

    [SerializeField]
    public MonoScript gunBehavior;

    [SerializeField]
    public float rof = 0.5f;
    [SerializeField]
    public float velocity = 10;
    [SerializeField]
    public GameObject bullet;
    [SerializeField]
    public int damage = 1;
    [SerializeField]
    public float accuracy = 1;
    [SerializeField]
    public float spread = 0;
    [SerializeField]
    public int arc = 0;
    [SerializeField]
    public int shots = 1;

    [SerializeField]
    public AudioClip sound;
}
