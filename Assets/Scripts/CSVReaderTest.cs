using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReaderTest : MonoBehaviour
{
    private void Start()
    {
        List<Dictionary<string, object>> data_Dialog = CSVReader.Read("LibraryExmaple");

        for (int i = 0; i < data_Dialog.Count; i++)
        {
            print(data_Dialog[i]["weight"].ToString());
        }
    }
}

