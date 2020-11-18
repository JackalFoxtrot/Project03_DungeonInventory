using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] GameObject _inventoryPanel;
    [SerializeField] GameObject _ShiftArrowImages;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        { 
            _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);
            if(_inventoryPanel.activeSelf)
            {
                _inventoryPanel.GetComponent<DisplayInventory>().CheckInventory();
            }
        }
        if(Input.GetKey(KeyCode.LeftShift) && !_inventoryPanel.activeSelf)
        {
            _ShiftArrowImages.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) && !_inventoryPanel.activeSelf)
        {
            _ShiftArrowImages.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow) && _inventoryPanel.activeSelf)
        {
            _inventoryPanel.GetComponent<DisplayInventory>().UpdateCategory(-1);
            _inventoryPanel.GetComponent<DisplayInventory>().CheckInventory();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && _inventoryPanel.activeSelf)
        {
            _inventoryPanel.GetComponent<DisplayInventory>().UpdateCategory(1);
            _inventoryPanel.GetComponent<DisplayInventory>().CheckInventory();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            EndApplication();
        }
        if (Input.GetKeyDown(KeyCode.U) && _inventoryPanel.activeSelf)
        {
            _inventoryPanel.GetComponent<DisplayInventory>().ClearDisplay();
        }
    }
    public void EndApplication()
    {
        Application.Quit();
    }
}
