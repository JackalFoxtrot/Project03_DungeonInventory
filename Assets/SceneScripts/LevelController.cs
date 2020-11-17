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
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            _ShiftArrowImages.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            _ShiftArrowImages.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow) && _inventoryPanel.activeSelf)
        {
            _inventoryPanel.GetComponent<DisplayInventory>().UpdateCategory(-1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && _inventoryPanel.activeSelf)
        {
            _inventoryPanel.GetComponent<DisplayInventory>().UpdateCategory(1);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            EndApplication();
        }
    }
    public void EndApplication()
    {
        Application.Quit();
    }
}
