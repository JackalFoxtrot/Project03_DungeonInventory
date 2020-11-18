using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InventoryObject inventory;
    [SerializeField] GameObject PlayerObject;
    [SerializeField] GameObject _inventoryPanel;
    [SerializeField] AudioClip _itemPickUpClip;
    [SerializeField] AudioClip _playerWalkClip;

    public int moveDistance = 1;
    // Start is called before the first frame update
    public void Update()
    {
        if(!_inventoryPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                AudioHelper.PlayClip2D(_playerWalkClip, 1.0f);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.LeftShift))
            {
                PlayerObject.transform.position = new Vector3(PlayerObject.transform.position.x - moveDistance, PlayerObject.transform.position.y, PlayerObject.transform.position.z);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftShift))
            {
                PlayerObject.transform.position = new Vector3(PlayerObject.transform.position.x + moveDistance, PlayerObject.transform.position.y, PlayerObject.transform.position.z);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftShift))
            {
                PlayerObject.transform.position = new Vector3(PlayerObject.transform.position.x, PlayerObject.transform.position.y, PlayerObject.transform.position.z - moveDistance);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftShift))
            {
                PlayerObject.transform.position = new Vector3(PlayerObject.transform.position.x, PlayerObject.transform.position.y, PlayerObject.transform.position.z + moveDistance);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftShift))
            {
                PlayerObject.transform.position = new Vector3(PlayerObject.transform.position.x - moveDistance, PlayerObject.transform.position.y, PlayerObject.transform.position.z + moveDistance);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftShift))
            {
                PlayerObject.transform.position = new Vector3(PlayerObject.transform.position.x + moveDistance, PlayerObject.transform.position.y, PlayerObject.transform.position.z - moveDistance);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftShift))
            {
                PlayerObject.transform.position = new Vector3(PlayerObject.transform.position.x - moveDistance, PlayerObject.transform.position.y, PlayerObject.transform.position.z - moveDistance);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftShift))
            {
                PlayerObject.transform.position = new Vector3(PlayerObject.transform.position.x + moveDistance, PlayerObject.transform.position.y, PlayerObject.transform.position.z + moveDistance);
            }  
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();
        if(item)
        {
            inventory.AddItem(item.item, 1);
            AudioHelper.PlayClip2D(_itemPickUpClip, 1.0f);
            _inventoryPanel.GetComponent<DisplayInventory>().AddPickUp("You have picked up "+ item.item.itemname);
            _inventoryPanel.GetComponent<DisplayInventory>().clearConsole();
            _inventoryPanel.GetComponent<DisplayInventory>().printToConsole();
            Destroy(other.gameObject);
        }
    }
    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}
