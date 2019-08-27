using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolController : MonoBehaviour {

    public static SymbolController Instance { set; get; }

    public float userStaticLat;
    public float userStaticLon;
    public List<Symbol> symbols;
    public List<TemplateSymbol> symbolsInfo;

  

    public void Awake()
    {
        Instance = this;
    }

    [System.Serializable]
    public class TemplateSymbol
    {
        public string name;
        public Sprite icon;

        public Sprite getIcons()
        {
            return icon;
        }

        public string getName()
        {
            return name;
        }

    }



    [System.Serializable]
    public class Symbol
    {
        public string name;
        public Sprite icon;
        public float latitude;
        public float longitude;
        public string category;
        public int index;
  
      public  Symbol(string name, Sprite icon, float latitude, float longitude, string category)
        {
            this.name = name;
            this.icon = icon;
            this.latitude = latitude;
            this.longitude = longitude;
            this.category = category;
         }
        


        public Sprite getIcons()
        {
            return icon;
        }

        public string getName()
        {
            return name;
        }

        public float getlatitude()
        {
            return latitude;
        }

        public float getLongitude()
        {
            return longitude;
        }

        public string getCategory()
        {
            return category;
        }

        public int getIndex()
        {
            return index;
        }

    }
   

}