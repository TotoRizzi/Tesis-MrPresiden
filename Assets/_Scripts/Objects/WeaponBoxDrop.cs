using Droppables;
public class WeaponBoxDrop : Droppable
{
    protected override void Start()
    {
        _probabilityDrop = .8f;
        base.Start();
    }
    protected override void DropObject()
    {
        Instantiate(GameManager.instance.DropManager.GetWeaponDrop()).SetPosition(_dropPosition.position);
    }
}
