using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ItemEntry : MonoBehaviour
{
    public Rarity rarity;

    public Image rarityHighlight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color color = ItemData.GetRarityColor(rarity);
        rarityHighlight.color = color;

    }
}
