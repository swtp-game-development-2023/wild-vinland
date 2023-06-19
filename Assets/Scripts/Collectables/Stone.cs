namespace Collectables
{
    public class Stone : CollectingScript
    {
        protected override void Awake() {
            base.Awake();
            resource.ID = (int) CollectableName.Stone;
        }
    }
}
