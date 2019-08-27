using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectedPanelManager : MonoBehaviour {
    GameObject infoPanel;
    public static SelectedPanelManager instance;
    SymbolController symCont;
    private void Start()
    {
        instance = this;
        infoPanel = GameObject.Find("SelectedLocation2");
        infoPanel.SetActive(false);
        symCont = SymbolController.Instance;

    }

    public void infoPanaleVisibilty(bool cont,string id)
    {
        infoPanel.SetActive(cont);
        for (int i = 0; i < symCont.symbols.Capacity; i++)
        {
            if(symCont.symbols[i].getName().Equals(id))
            {
                infoPanel.transform.GetChild(0).GetComponent<Text>().text = "\n" + symCont.symbols[i].getName() + "\n" + "Enlem " + symCont.symbols[i].getlatitude()
                    + "  Boylam " + symCont.symbols[i].getLongitude() + "\n" + "Yükseklik " + "0";
                break;
            }
        }
    }
}
