using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.PostProcessing.SubpixelMorphologicalAntialiasing;

public class BottomPanel : MonoBehaviour
{
    public Button btn_SCB1;
    public Button btn_SCB2;
    public Button btn_SpawnCargo;
    public InputField inputField_Quantity;

    int maxInputQuantity = 100;

    private void Awake()
    {
        btn_SCB1.onClick.AddListener(OnClickButton_SCB1);
        btn_SCB2.onClick.AddListener(OnClickButton_SCB2);
        btn_SpawnCargo.onClick.AddListener(OnClickButton_SpawnCargo);
    }

    void OnClickButton_SCB1()
    {
        int num = 0;
        Cacher.uldManager.ChangeULD(num);
    }

    void OnClickButton_SCB2()
    {
        int num = 1;
        Cacher.uldManager.ChangeULD(num);
    }

    void OnClickButton_SpawnCargo()
    {
        Cacher.cargoManager.GenerateCargo(ParseQuantity(inputField_Quantity));
    }

    int ParseQuantity(InputField inputQuantity)
    {
        if(inputQuantity != null)
        {
            int quantity = int.Parse(inputQuantity.text);
            if (quantity > maxInputQuantity)
            {
                quantity = maxInputQuantity;
            }
            else if (quantity < 0)
            {
                quantity = 0;
            }
            inputField_Quantity.GetComponent<InputField>().text = quantity.ToString();
            return quantity;
        }
        else
        {
            return 0;
        }
    }
}
