using System;
using Unity.VisualScripting;
using UnityEngine;



//This interface Collectable  guarantees that a player can collect an object.

namespace Collectables
{
    public abstract class Collectable : MonoBehaviour
    {
        private int _id = (int) CollectableName.Empty;
        [SerializeField]
        protected int maxAmount = 1;
        [SerializeField]
        protected int amount = 0;
    
        [SerializeField]
        private Sprite sprite;

        public int MaxAmount => maxAmount;
        public Sprite Sprite
        {
            get => sprite;
        }

        public int Amount
        {
            get => amount;
        }

        public int Add(int amount)
        {
            Debug.Log("c:"+ amount);
            var freeSpace= maxAmount - this.amount;
            freeSpace = freeSpace < 0 ? 0 : freeSpace;
            int addValue = freeSpace < amount ? freeSpace : amount;
            this.amount += addValue;
            return amount - addValue;
        }
    
        public int ID {
            get => _id;
            set
            {
                if (_id == -1) {
                    _id = value;
                }
            }
        }
    
        ///<summary>
        /// Generates a String representation of the Collectable in Format: CollectableName [Collectable Amount]
        ///</summary>
        public override string ToString()
        {
            return (CollectableName) _id +"["+amount+"]";
        }
    }
}