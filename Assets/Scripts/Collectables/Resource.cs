using Unity.VisualScripting;
using UnityEngine;

namespace Collectables
{
    public class Resource : Collectable
    {
        private void Awake()
        {
            maxAmount = 10;
        }
    }
}