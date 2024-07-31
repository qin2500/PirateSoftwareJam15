using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PentagramDisplay : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject fireOrb;
    [SerializeField] private GameObject waterOrb;
    [SerializeField] private GameObject grassOrb;
    [SerializeField] private GameObject fireFireOrb;
    [SerializeField] private GameObject fireWaterOrb;
    [SerializeField] private GameObject fireGrassOrb;
    [SerializeField] private GameObject waterWaterOrb;
    [SerializeField] private GameObject waterGrassOrb;
    [SerializeField] private GameObject grassGrassOrb;

    void Start()
    {
        List<AlchemyUpgrade> upgrades = GlobalReferences.PLAYER.Pentagram.getUpgrades();
        GameObject loadingOrb = new GameObject();
        for (int i = 0; i < 5; i++)
        {
            //hide all current orbs

            GameObject img = GameObject.Find($@"Orb {i + 1}");

            if (upgrades.Count > i)
            {
                Element e1 = upgrades[i].getFirstElement();
                Element? e2 = upgrades[i].getOtherElement(e1);


                if (e1 == Element.Fire)
                {
                    if (!e2.HasValue)
                        loadingOrb = fireOrb;
                    else if (e2.Value == Element.Fire) loadingOrb = fireFireOrb;
                    else if (e2.Value == Element.Water) loadingOrb = fireWaterOrb;
                    else if (e2.Value == Element.Grass) loadingOrb = fireGrassOrb;
                }

                if (e1 == Element.Water)
                {
                    if (!e2.HasValue)
                        loadingOrb = waterOrb;
                    else if (e2.Value == Element.Fire) loadingOrb = fireWaterOrb;
                    else if (e2.Value == Element.Water) loadingOrb = waterWaterOrb;
                    else if (e2.Value == Element.Grass) loadingOrb = waterGrassOrb;
                }
                if (e1 == Element.Grass)
                {
                    if (!e2.HasValue)
                        loadingOrb = grassOrb;
                    else if (e2.Value == Element.Fire) loadingOrb = fireGrassOrb;
                    else if (e2.Value == Element.Water) loadingOrb = waterGrassOrb;
                    else if (e2.Value == Element.Grass) loadingOrb = grassGrassOrb;
                }

               if (loadingOrb) Instantiate(loadingOrb, img.transform.position, Quaternion.identity);
                Destroy(img);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
