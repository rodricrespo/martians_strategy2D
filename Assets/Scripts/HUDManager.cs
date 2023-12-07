using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI playerResourcesText;
    public TextMeshProUGUI AIResourcesText;
    public GameManager gm;

    void Update()
    {
        playerResourcesText.text = gm.playerResources.ToString();
        AIResourcesText.text = gm.AIresources.ToString();
    }
}
