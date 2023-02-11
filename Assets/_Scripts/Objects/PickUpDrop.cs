using Droppables;
public class PickUpDrop : Droppable
{
    protected override void Start()
    {
        _probabilityDrop = .5f;
        base.Start();
    }
    protected override void DropObject()
    {
        FRY_PickUps.Instance.pool.GetObject().SetPosition(_dropPosition.position);
    }
}
