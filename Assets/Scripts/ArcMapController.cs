
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcMapController : MonoBehaviour {

    public static ArcMapController instance;

    public GameObject arcCircleMain;
    public GameObject arcCircleUserOrigin;

    public GameObject symbolsContainer;
    public GameObject miniSymbolTemplate;
    

    bool isMiniSymbolsCreated = false;

    float angleDiff;

    List<GameObject> allMiniSymbols = new List<GameObject>();

    public void MiniSymbolCreator()
    {

        termimateChildIcons();
        foreach (Transform _symbols in symbolsContainer.transform)
        {
            if(_symbols.gameObject.name != "Quad" && !_symbols.gameObject.name.Contains("Fırtına"))
            {
                GameObject go = Instantiate(miniSymbolTemplate, arcCircleMain.transform) as GameObject;
                go.name = "mini_" + _symbols.name;
                Image _img = go.transform.GetChild(0).transform.GetComponent<Image>();

                Texture2D _tex = _symbols.GetComponent<Renderer>().material.mainTexture as Texture2D;
                Sprite _sprite = Sprite.Create(_tex, new Rect(0, 0, _tex.width, _tex.height), _img.transform.position);
                Debug.Log("Sprite Name: " + _sprite);
                _img.sprite = _sprite;

                allMiniSymbols.Add(go);

                Vector3 target = _symbols.transform.position - Camera.main.transform.position;
                Vector3 camera = Camera.main.transform.forward;

                angleDiff = Vector3.SignedAngle(camera, target, -Vector3.up);
                go.transform.localEulerAngles = new Vector3(0, 0, angleDiff);
            }
        }
         isMiniSymbolsCreated = true;
    }

    public void termimateChildIcons()
    {
        allMiniSymbols.Clear();
        for (int i = 2; i < arcCircleMain.transform.childCount; i++)
        {
            Destroy(arcCircleMain.transform.GetChild(i).gameObject);
        }
    }

  


    public void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start () {

        Invoke("MiniSymbolCreator", 10);
	}
	
	// Update is called once per frame
	void Update () {

        if (isMiniSymbolsCreated)
        {
            //foreach(Transform t in symbolsContainer.transform)
            
                //if(!t.gameObject.name.Contains("Quad"))
                
                    //Vector3 target =  t.transform.position - Camera.main.transform.position;
                    //float _diff = Vector3.Angle(Camera.main.transform.forward, target);


                    //GameObject miniObj = allMiniSymbols.Find(x => x.name.Contains(t.gameObject.name));
                    /*
                    if(Camera.main.transform.hasChanged)
                    {
                        //Vector3 _target = t.transform.position - Camera.main.transform.position;
                        Vector3 _camera = Camera.main.transform.forward;
                        Vector3 _arcUserOrg = arcCircleUserOrigin.transform.forward;
                        Vector3 _miniObj = miniObj.transform.forward;


                        float _diff = Vector3.SignedAngle(_arcUserOrg, _miniObj, Vector3.up);

                        Vector3 _vecAngle = new Vector3(0, 0, _diff - Camera.main.transform.eulerAngles.y);

                        miniObj.transform.localEulerAngles = _vecAngle;

                        Camera.main.transform.hasChanged = false;
                    }
                    */

                    foreach (Transform _symbols in symbolsContainer.transform)
                    {
                        if (_symbols.gameObject.name != "Quad" && !_symbols.gameObject.name.Contains("Fırtına"))
                        {
                            //GameObject go = Instantiate(miniSymbolTemplate, arcCircleMain.transform) as GameObject;
                            //go.name = "mini_" + _symbols.name;
                            //Image _img = go.transform.GetChild(0).transform.GetComponent<Image>();

                            //Texture2D _tex = _symbols.GetComponent<Renderer>().material.mainTexture as Texture2D;
                            //Sprite _sprite = Sprite.Create(_tex, new Rect(0, 0, _tex.width, _tex.height), _img.transform.position);
                            //Debug.Log("Sprite Name: " + _sprite);
                            //_img.sprite = _sprite;

                            //allMiniSymbols.Add(go);

                            Vector3 target = _symbols.transform.position - Camera.main.transform.position;
                            Vector3 camera = Camera.main.transform.forward;

                            angleDiff = Vector3.SignedAngle(camera, target, -Vector3.up);

                            GameObject miniObj = allMiniSymbols.Find(x => x.name.Contains(_symbols.gameObject.name));
                            miniObj.transform.localEulerAngles = new Vector3(0, 0, angleDiff);

                            }
                    }

            //isMiniSymbolsCreated = true;

            //float camDiff = _camY - oldCamerDegre;
            //miniObj.transform.localEulerAngles = new Vector3(0, 0, miniObj.transform.localEulerAngles.z +camDiff );


            //miniObj.transform.localEulerAngles = new Vector3(0, 0, 360 - _diff);


            //miniObj.transform.localEulerAngles = new Vector3(0, 0, _diff);
            /*
            if (Camera.main.transform.eulerAngles.y >= 180 &&  Camera.main.transform.eulerAngles.y <= 360)
            {
                miniObj.transform.localEulerAngles = new Vector3(0, 0, 360 - Mathf.Abs(Camera.main.transform.eulerAngles.y));
            }
            else
            {
               //miniObj.transform.localEulerAngles = new Vector3(0, 0, _diff - );

            }
                      if (t.transform.gameObject.name == "Anıtkabir")
                Debug.Log(
                    "cam angle Y: " + Camera.main.transform.eulerAngles.y +
                    " mini :"+miniObj.transform.localEulerAngles.z.ToString()+ 
                    " diff: " + _diff);
            */

        }
    }
}
