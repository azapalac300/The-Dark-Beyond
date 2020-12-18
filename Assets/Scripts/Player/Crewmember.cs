using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Crewmember : MonoBehaviour
{
    public CrewData crewData;
    public int level;
    public Sprite icon;
    public Dictionary<AbilityCategory, Ability> abilities;

    [Range(0, 10000)]
    public int XP;
    // Start is called before the first frame update


    void Start()
    {
        abilities = new Dictionary<AbilityCategory, Ability>();
        foreach(Ability a in crewData.abilities)
        {
            abilities.Add(a.category, a);
        }
        icon = crewData.icon;
        
    }

    // Update is called once per frame
    void Update()
    {
        crewData.XP = XP;
        level = crewData.level;
    }



   
   
}
