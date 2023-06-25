using UnityEngine;
public class PlayerSetSkin : MonoBehaviour
{
    [SerializeField] SpriteRenderer _headSprite, _torsoSprite, _rightLegSprite, _leftLegSprite, _rightHandSprite, _leftHandSprite;
    void Start()
    {
        SetSkin();
        Helpers.GameManager.SetPlayerSkin += SetSkin;
    }

    public void SetSkin()
    {
        if (!Helpers.PersistantData.persistantDataSaved.playerCosmeticEquiped) return;

        CosmeticData cosmetic = Helpers.PersistantData.persistantDataSaved.playerCosmeticEquiped;
        _headSprite.sprite = cosmetic.headSprite;
        _torsoSprite.sprite = cosmetic.torsoSprite;
        _rightLegSprite.sprite = cosmetic.rightLegSprite;
        _leftLegSprite.sprite = cosmetic.leftLegSprite;
        _rightHandSprite.sprite = cosmetic.rightHandSprite;
        _leftHandSprite.sprite = cosmetic.leftHandSprite;
    }
}
