using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IconsVisiblty : MonoBehaviour {


    IconSelectTable changeIcon;
    CursorPosition cursorSetIcon;
    GameObject cursorVisible, cameraOrigin;
    int visibltyCounter;
    Ray rayCast;
    RaycastHit hit;
    public Camera camera;
    
    void Start()
    {

        cameraOrigin = GameObject.Find("CameraOrigin");
        cursorVisible = GameObject.Find("changeIcon");
        cursorSetIcon = GameObject.Find("targetCursor").GetComponent<CursorPosition>();
        changeIcon = GameObject.Find("icons").GetComponent<IconSelectTable>();
       
        iconVisblty();
    }

    void Update()
    {
       // Debug.DrawLine(Input.mousePosition, Camera.main.transform.TransformDirection(Vector3.forward),Color.red, Mathf.Infinity);
        
            rayCast = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayCast,out hit,30000000))
            {
                
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                iconVisblty();
                SelectedPanelManager.instance.infoPanaleVisibilty(false,"");
                }
            }
        
    }
    
    public void iconVisblty()
    {
        visibltyCounter++;
        if (Input.GetMouseButtonDown(0))
        {
            if (visibltyCounter % 2 == 0)
            {
                cursorVisible.SetActive(false);
                cameraOrigin.SetActive(false);
            }
            else
            {
                cursorVisible.SetActive(true);
                cameraOrigin.SetActive(true);
            }
        }
    }

     
}
