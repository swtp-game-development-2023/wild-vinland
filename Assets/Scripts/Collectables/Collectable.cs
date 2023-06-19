using System;
using Unity.VisualScripting;
using UnityEngine;


//This interface Collectable  guarantees that a player can collect an object.

namespace Collectables
{
    public abstract class Collectable : ScriptableObject
    {
        private int _id = (int)CollectableName.Empty;
        [SerializeField] protected int maxAmount = 1;
        [SerializeField] protected int amount;

        [SerializeField] private Sprite sprite;

        public int MaxAmount => maxAmount;

        public Sprite Sprite
        {
            get => sprite;
        }

        public int Amount
        {
            set { amount = value; }
            get => amount;
        }

        public int Add(int otherAmount)
        {
            var freeSpace = maxAmount - amount;
            freeSpace = freeSpace < 0 ? 0 : freeSpace;
            int addValue = freeSpace < otherAmount ? freeSpace : otherAmount;
            this.amount += addValue;
            return otherAmount - addValue;
        }

        public int ID
        {
            get => _id;
            set
            {
                if (_id == -1)
                {
                    _id = value;
                }
            }
        }

        public bool IsMaxAmount()
        {
            return maxAmount <= amount;
        }

        ///<summary>
        /// Generates a String representation of the Collectable in Format: CollectableName [Collectable Amount]
        ///</summary>
        public override string ToString()
        {
            return (CollectableName)_id + "[" + amount + "]";
        }

        public Collectable copy()
        {
            var obj = Instantiate(this);
            obj.amount = amount;
            obj._id = _id;
            obj.maxAmount = maxAmount;
            return obj;
        }
    }
}