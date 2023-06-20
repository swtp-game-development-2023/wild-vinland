using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Collectables
{
    public abstract class CollectingScript : MonoBehaviour
    {
        [SerializeField]
        [Range(1, 12)] 
        private int amount = 1;
        private Inventory inventory;
        protected Resource resource;
        protected virtual void Awake()
        {
            
            resource = ScriptableObject.CreateInstance<Resource>();
            resource.Amount = amount;
            resource.Sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            inventory = FindObjectOfType<Inventory>();
        }
        
        //player can walk over collectable
        //collectible should be destroyed if collected
        //should be dropable
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other is BoxCollider2D)
            {
                if (other.transform.parent.CompareTag("Player"))
                {
                    resource.Amount = inventory.Add(resource.copy());
                    if (resource.Amount <= 0)
                    {
                        Destroy(gameObject);
                    }

                    Debug.Log(inventory.ToString());
                }
            }
        }
    }
}