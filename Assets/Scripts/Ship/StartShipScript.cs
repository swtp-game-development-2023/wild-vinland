using Cinemachine;
using Pathfinding;
using UnityEngine;

public class StartShipScript : MonoBehaviour
{
    private GameObject ship;

    public GameObject Ship
    {
        get => ship;
        set => ship = value; // is set from ship build script
    }

    public void startShip()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var virtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera");
        player.SetActive(false);
        virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = ship.transform;
        ship.GetComponent<AIPath>().canMove = true;
        ship.GetComponent<AudioSource>().PlayDelayed(1);
    }
}
