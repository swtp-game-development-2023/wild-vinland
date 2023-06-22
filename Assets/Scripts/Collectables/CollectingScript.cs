using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Collectables
{
    public abstract class CollectingScript : MonoBehaviour
    {
        [SerializeField] [Range(1, 12)] private int amount = 1;
        private Inventory inventory;
        protected Resource resource;

        protected virtual void Awake()
        {
            resource = ScriptableObject.CreateInstance<Resource>();
            resource.Amount = amount;
            resource.Sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            inventory = FindObjectOfType<Inventory>();
            StartCoroutine(CollectableAfterTime(2));
        }

        public int Amount
        {
            get => amount;
            set  {
            amount = value;
            resource.Amount = value;
        }
    }

    //player can walk over collectable
    //collectible should be destroyed if collected
    //should be dropable
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Body"))
        {
            {
                resource.Amount = inventory.Add(resource.copy());
                if (resource.Amount <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    IEnumerator CollectableAfterTime(int seconds)
    {
        int counter = seconds;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
        }

        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}

}