using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class IconSelectTable : MonoBehaviour {

    GameObject iconSelectPanel;
    static int iconsCounter;
    private void Start()
    {
        iconSelectPanel = GameObject.Find("changeIcon");
        createIcons(true);
    }

    public void createIcons(bool direction)
    {

        if(direction)
        {
            if(iconsCounter < SymbolController.Instance.symbolsInfo.Capacity-1)
            {
                iconsCounter++;
            }
        }
        else
        {
            if (iconsCounter > 0)
            {
                iconsCounter--;
            }
        }

        if (iconsCounter < SymbolController.Instance.symbolsInfo.Capacity || iconsCounter > 0) 
        {
            PlayerPrefs.SetInt("iconIndex", iconsCounter);
            iconSelectPanel.GetComponent<Image>().sprite = SymbolController.Instance.symbolsInfo[iconsCounter].getIcons();
            iconSelectPanel.transform.GetChild(0).GetComponent<Text>().text = SymbolController.Instance.symbolsInfo[iconsCounter].getName().ToString();
        }
        else
        {
            iconsCounter = 0;
        }
    }

}
