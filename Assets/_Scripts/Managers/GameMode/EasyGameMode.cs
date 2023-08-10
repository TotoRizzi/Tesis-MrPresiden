public class EasyGameMode : GameModeManager
{
    public override void PlayerDead()
    {
        playerHealth.EffectsOnDeath();
        playerHealth.RestartPosition();
    }
}
