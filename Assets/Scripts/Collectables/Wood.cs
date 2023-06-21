using UnityEngine;

namespace Collectables
{
    public class Wood : CollectingScript
    {
        protected override void Awake()
        {
            base.Awake();
            resource.ID = (int) CollectableName.Wood;
        }
    }
}
