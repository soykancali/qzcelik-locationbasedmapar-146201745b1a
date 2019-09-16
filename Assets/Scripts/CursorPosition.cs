using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CursorPosition : MonoBehaviour {

    GameObject invokeObject, cursor, createdObject, cursortmp;
    Vector3 endPoint;
    IconSelectTable changeIcon;
    public List<SymbolController.Symbol> createdSymbol;
    int counter;
    float km;
    int scaleRate = 3500;
    


    void Start()
    {
        invokeObject = GameObject.Find("ContentObject");
        cursor = GameObject.Find("targetCursor");
        cursortmp = GameObject.FindGameObjectWithTag("cursortmp");
        changeIcon = GameObject.Find("icons").GetComponent<IconSelectTable>();
        setIcon();
        defaultIcons();
    }

    void LateUpdate()
    {
        endPoint = cursor.transform.position;
        if (Input.GetKey(KeyCode.UpArrow) && km <= 20)
        {
            cursor.transform.Translate(new Vector3(0, 0, Time.deltaTime * 5));
            km += Time.deltaTime*5;
            this.transform.localScale = new Vector3(this.transform.localScale.x + (km/ scaleRate), this.transform.localScale.y + (km/ scaleRate), 1);
        }

        if (Input.GetKey(KeyCode.DownArrow) && km >= 0.03f)
        {
            cursor.transform.Translate(new Vector3(0, 0, -Time.deltaTime * 5));
            km -= Time.deltaTime * 5;
            this.transform.localScale = new Vector3(this.transform.localScale.x - (km / scaleRate), this.transform.localScale.y - (km / scaleRate), 1);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            changeIcon.createIcons(true);
            setIcon();
        }


        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            changeIcon.createIcons(false);
            setIcon();

        }


        if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Space) )
        {
            counter++;
            if(counter> 1)
            {
                objectCreater();
            }

        }

        //cursortmp.transform.GetComponent<TextMeshProUGUI>().text = km.ToString("0.00") + "KM";
        //cursor.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = km.ToString("0.00") + " KM";
        cursor.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = km.ToString("0.00") + " KM";
        
    }



    public void setIcon()
    {
       cursor.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = SymbolController.Instance.symbolsInfo[PlayerPrefs.GetInt("iconIndex")].getIcons();
    }



    void defaultIcons()
    {
        for (int i = 0; i < SymbolController.Instance.symbols.Capacity; i++)
        {
            StartCoroutine(PackageSender(SymbolController.Instance.symbols[i].getName(), SymbolController.Instance.symbols[i].getLongitude(),
                SymbolController.Instance.symbols[i].getlatitude(),0, SymbolController.Instance.symbols[i].getCategory()));
        }
    }

    IEnumerator PackageSender(string name, float longi, float lati, float alti, string category)
    {
        yield return new WaitForSeconds(5.0f); 
        SymbolManager.instance.AddSymbol(name, longi, lati, alti, category);
    }



    public void objectCreater()
    {
        createdObject = Instantiate(invokeObject.transform.GetChild(0), endPoint, Quaternion.identity).gameObject;
        createdObject.name = SymbolController.Instance.symbolsInfo[PlayerPrefs.GetInt("iconIndex")].getName() + counter.ToString();
        createdObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = km.ToString("0.00") + " KM";
        createdObject.GetComponent<Renderer>().material.mainTexture = SymbolController.Instance.symbolsInfo[PlayerPrefs.GetInt("iconIndex")].getIcons().texture;
        createdObject.transform.SetParent(invokeObject.transform);
        ArcMapController.instance.MiniSymbolCreator();
        
        SymbolController.Instance.symbols.Add(new SymbolController.Symbol(SymbolController.Instance.symbolsInfo[PlayerPrefs.GetInt("iconIndex")].getName() + counter.ToString(),
            SymbolController.Instance.symbolsInfo[PlayerPrefs.GetInt("iconIndex")].getIcons(), 0, 0, km.ToString()));
        ClientControllerUDP.loc =System.Convert.ToString(km);   //ssc


        /*createdSymbol.Add (new SymbolController.Symbol(SymbolController.Instance.symbolsInfo[PlayerPrefs.GetInt("iconIndex")].getName() + counter.ToString(), 
            SymbolController.Instance.symbolsInfo[PlayerPrefs.GetInt("iconIndex")].getIcons(), 0, 0,km.ToString()));*/
        //**************bulunduğu sınıfta oluşturur----------------

        /*  createdObject.transform.GetChild(1).GetComponent<TextMeshPro>().text = SymbolController.Instance.symbols[PlayerPrefs.GetInt("iconIndex")].getName() + counter.ToString() +"#"+
          SymbolController.Instance.symbols[PlayerPrefs.GetInt("iconIndex")].getLongitude()+"#"+ SymbolController.Instance.symbols[PlayerPrefs.GetInt("iconIndex")].getlatitude()+"#"
          + SymbolController.Instance.symbols[PlayerPrefs.GetInt("iconIndex")].getCategory();
          ArcMapController.instance.MiniSymbolCreator();

          SymbolController.Instance.symbolsInfo[Player]

          /*SymbolController.Instance.symbols[PlayerPrefs.GetInt("iconIndex")].addTarget(SymbolController.Instance.symbols[PlayerPrefs.GetInt("iconIndex")].getName() + counter.ToString(),
              SymbolController.Instance.symbols[PlayerPrefs.GetInt("iconIndex")].getIcons(),
              SymbolController.Instance.symbols[PlayerPrefs.GetInt("iconIndex")].getLongitude(),
               SymbolController.Instance.symbols[PlayerPrefs.GetInt("iconIndex")].getlatitude(),
               SymbolController.Instance.symbols[PlayerPrefs.GetInt("iconIndex")].getCategory());

         /* SymbolManager.instance.AddSymbol(SymbolController.Instance.symbols[PlayerPrefs.GetInt("iconIndex")].getName() + counter.ToString(), 
              SymbolController.Instance.symbols[PlayerPrefs.GetInt("iconIndex")].getLongitude(), 
              SymbolController.Instance.symbols[PlayerPrefs.GetInt("iconIndex")].getlatitude(),0,
              SymbolController.Instance.symbols[PlayerPrefs.GetInt("iconIndex")].getCategory());*/
    }

    
}
