namespace Collectables
{
    public class Ore : CollectingScript
    {
        protected override void Awake() {
            base.Awake();
            resource.ID = (int) CollectableName.Ore;
        }
    }
}
