using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchCubeBuffer : MonoBehaviour 
{

    public void OnMouseDown()
    {
        SelectedPanelManager.instance.infoPanaleVisibilty(true, this.gameObject.name);
    }
}
