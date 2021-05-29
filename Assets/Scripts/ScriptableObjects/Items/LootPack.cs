using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LootPack : MonoBehaviour
{
    public Rarity _rarity;

    public Color rarityColor;

    //TO DO later
    public ItemData data;

    [Space(10)]
    public float pickupRadius;
    public float basePickupSpeed;
    private float currPickupSpeed;

    public Material thisMaterial;
    public MeshRenderer renderer;

    private Player player;

    private float distAdjust = 0;

    // Start is called before the first frame update
    public void Initialize(Rarity rarity)
    {
        _rarity = rarity;
        renderer.materials[0] = new Material(thisMaterial);

        thisMaterial = renderer.materials[0];
        currPickupSpeed = basePickupSpeed; 
       
    }

    public void Start()
    {
       // player = GameObject.Find("Player").GetComponent<Player>();
    }
    // Update is called once per frame

    void Update()
    {
        rarityColor = ItemData.GetRarityColor(_rarity);

        thisMaterial.SetColor("Color_EE4BC10E", rarityColor);

        LookForPlayer();


        HandleMovement();
    }

    public void LookForPlayer()
    {
        if (player != null) return;


        player = GameObject.Find("Player").GetComponent<Player>();



        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRadius);

        foreach(Collider c in colliders)
        {
            if(c.gameObject.tag == "Player")
            {
                player = c.gameObject.GetComponent<Player>();
                break;
            }
        }
    }


    void HandleMovement()
    {
        if (player == null) return;

           float dist = Vector3.Distance(transform.position, player.transform.position);
           Vector3 diff = (player.transform.position - transform.position).normalized;
            transform.Translate(diff * currPickupSpeed * Time.deltaTime);
            currPickupSpeed++;
            distAdjust += 0.2f;

        if(dist < 1f + distAdjust)
        {
            //Player picks up item
            Destroy(gameObject);
        }
    }
}
