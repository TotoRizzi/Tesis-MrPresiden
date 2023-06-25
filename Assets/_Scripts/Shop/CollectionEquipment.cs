using System;
using UnityEngine;
using UnityEngine.UI;
public class CollectionEquipment : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button _playerButton, _presidentButton;
    [SerializeField] Button _playerNextCosmetic, _playerPreviousCosmetic;
    [SerializeField] Button _presidentNextCosmetic, _presidentrPreviousCosmetic;
    [SerializeField] Button _playerEquipButton, _presidentEquipButton;

    [Header("Player Sprites")]
    [SerializeField] Image _playerHead, _playerTorso, _playerRightLeg, _playerLeftLeg, _playerRightHand, _playerLeftHand;

    [Header("President Sprites")]
    [SerializeField] Image _presidentHead, _presidentTorso, _presidentRightLeg, _presidentLeftLeg, _presidentRightHand, _presidentLeftHand;

    int _playerIndex, _presidentIndex;
    Color _buttonsColor;
    void Start()
    {
        _buttonsColor = _playerButton.image.color;
        PersistantDataSaved persistantDataSaved = Helpers.PersistantData.persistantDataSaved;

        if (persistantDataSaved.playerCosmeticEquiped) EquipPlayer(persistantDataSaved.playerCosmeticEquiped);
        if (persistantDataSaved.presidentCosmeticEquiped) EquipPresident(persistantDataSaved.playerCosmeticEquiped);

        _playerNextCosmetic.onClick.AddListener(() =>
        {
            _playerIndex = (_playerIndex + 1) % persistantDataSaved.playerCosmeticCollection.Count;
            EquipPlayer(persistantDataSaved.playerCosmeticCollection[_playerIndex]);
            _playerEquipButton.interactable = persistantDataSaved.playerCosmeticCollection[_playerIndex] == persistantDataSaved.playerCosmeticEquiped ? false : true;
        });
        _playerPreviousCosmetic.onClick.AddListener(() =>
        {
            _playerIndex--;
            if (_playerIndex < 0) _playerIndex += persistantDataSaved.playerCosmeticCollection.Count;
            EquipPlayer(persistantDataSaved.playerCosmeticCollection[_playerIndex]);
            _playerEquipButton.interactable = persistantDataSaved.playerCosmeticCollection[_playerIndex] == persistantDataSaved.playerCosmeticEquiped ? false : true;
        });

        _presidentNextCosmetic.onClick.AddListener(() =>
        {
            _presidentIndex = (_presidentIndex + 1) % persistantDataSaved.presidentCosmeticCollection.Count;
            EquipPresident(persistantDataSaved.presidentCosmeticCollection[_presidentIndex]);
            _presidentEquipButton.interactable = persistantDataSaved.presidentCosmeticCollection[_presidentIndex] == persistantDataSaved.presidentCosmeticEquiped ? false : true;
        });
        _presidentrPreviousCosmetic.onClick.AddListener(() =>
        {
            _presidentIndex--;
            if (_presidentIndex < 0) _presidentIndex += persistantDataSaved.presidentCosmeticCollection.Count;
            EquipPresident(persistantDataSaved.presidentCosmeticCollection[_presidentIndex]);
            _presidentEquipButton.interactable = persistantDataSaved.presidentCosmeticCollection[_presidentIndex] == persistantDataSaved.presidentCosmeticEquiped ? false : true;
        });

        _playerEquipButton.onClick.AddListener(() =>
        {
            persistantDataSaved.playerCosmeticEquiped = persistantDataSaved.playerCosmeticCollection[_playerIndex];
            _playerEquipButton.interactable = false;
            if (Helpers.GameManager) Helpers.GameManager.SetPlayerSkin();
        });

        _presidentEquipButton.onClick.AddListener(() =>
        {
            persistantDataSaved.presidentCosmeticEquiped = persistantDataSaved.presidentCosmeticCollection[_presidentIndex];
            _presidentEquipButton.interactable = false;
            if (Helpers.GameManager) Helpers.GameManager.SetPresidentSkin();
        });

        _playerButton.onClick.AddListener(() =>
        {
            _playerButton.image.color = _buttonsColor * .5f;
            _presidentButton.image.color = _buttonsColor;
        });

        _presidentButton.onClick.AddListener(() =>
        {
            _presidentButton.image.color = _buttonsColor * .5f;
            _playerButton.image.color = _buttonsColor;
        });

        EquipPlayer(persistantDataSaved.playerCosmeticCollection[0]);
        EquipPresident(persistantDataSaved.presidentCosmeticCollection[0]);

        _playerButton.onClick.Invoke();
    }

    void EquipPlayer(CosmeticData cosmeticData)
    {
        _playerHead.sprite = cosmeticData.headSprite;
        _playerTorso.sprite = cosmeticData.torsoSprite;
        _playerRightLeg.sprite = cosmeticData.rightLegSprite;
        _playerLeftLeg.sprite = cosmeticData.leftLegSprite;
        _playerRightHand.sprite = cosmeticData.rightHandSprite;
        _playerLeftHand.sprite = cosmeticData.leftHandSprite;
    }
    void EquipPresident(CosmeticData cosmeticData)
    {
        _presidentHead.sprite = cosmeticData.headSprite;
        _presidentTorso.sprite = cosmeticData.torsoSprite;
        _presidentRightLeg.sprite = cosmeticData.rightLegSprite;
        _presidentLeftLeg.sprite = cosmeticData.leftLegSprite;
        _presidentRightHand.sprite = cosmeticData.rightHandSprite;
        _presidentLeftHand.sprite = cosmeticData.leftHandSprite;
    }
}
