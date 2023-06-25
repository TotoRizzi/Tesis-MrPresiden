using UnityEngine;
public class PlayerSetSkin : MonoBehaviour
{
    [SerializeField] SpriteRenderer _headSprite, _torsoSprite, _rightLegSprite, _leftLegSprite, _rightHandSprite, _leftHandSprite, _tailSprite;
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
        _tailSprite.sprite = cosmetic.tailSprite ? cosmetic.tailSprite : null;
    }
}
