using Unity.VisualScripting;
using UnityEngine;

namespace Collectables
{
    public class Ressource : Collectable
    {
        private Inventory inventory;

        //player can walk over collectable
        //collectible should be destroyed if collected
        //should be dropable

        private void Start()
        {
            inventory = FindObjectOfType<Inventory>();
            maxAmount = 10;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.parent.CompareTag("Player"))
            {
                var obj = Instantiate(this);
                var rest = inventory.Add(obj);
                Destroy(obj.gameObject);
                if (rest <= 0)
                {
                    Destroy(gameObject);
                }
                Debug.Log(inventory.ToString());
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}