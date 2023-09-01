public class PS_EnemyExplode : Particle
{
    public override void ReturnObject()
    {
        FRY_EnemyBloodExplodeParticle.Instance.ReturnObject(this);
    }
}
