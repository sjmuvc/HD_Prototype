using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private void Start()
    {
        List<Dictionary<string, object>> csv_Data = CSVReader.Read("LibraryExmaple");

        for (int i = 0; i < csv_Data.Count; i++)
        {
            Cacher.cargoManager.cargos[i].GetComponent<CargoInfo>().width = float.Parse(csv_Data[i]["width"].ToString());
        }
    }

    public void Excel()
    {
        List<string> strings = new List<string>();

        //Cacher.cargoManager.GenerateCargo(strings);


    }
    /*
    public CargoInfo[] ParseCargoDB()
    {
        return 
    }
    */
}
