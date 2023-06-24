using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ShopWindow : MonoBehaviour
{
    [SerializeField] Button _playerButton, _presidentButton;
    [SerializeField] Button _buyButton;
    [SerializeField] CosmeticShopItem _cosmeticShopItemPrefab;

    [Header("Player Settings")]
    [SerializeField] Transform _playerGridParent;
    [SerializeField] Image _playerHeadSprite, _playerTorsoSprite, _playerRightLegSprite, _playerLeftLegSprite, _playerRightHandSprite, _playerLeftHandSprite;

    [Header("President Settings")]
    [SerializeField] Transform _presidentGridParent;
    [SerializeField] Image _presidentHeadSprite, _presidentTorsoSprite, _presidentRightLegSprite, _presidentLeftLegSprite, _presidentRightHandSprite, _presidentLeftHandSprite;

    [SerializeField] Color _selectedColor;

    CosmeticData[] _playerCosmetics;
    CosmeticData[] _presidentCosmetics;

    [SerializeField] CosmeticData _cosmeticSelected;
    void Start()
    {
        _playerCosmetics = Resources.LoadAll<CosmeticData>("Cosmetics/Player");
        _presidentCosmetics = Resources.LoadAll<CosmeticData>("Cosmetics/President");

        List<Button> playerCosmeticsButton = new List<Button>();
        List<Button> presidentCosmeticsButton = new List<Button>();

        #region Instanceo de Cosmetics

        for (int i = 0; i < _playerCosmetics.Length; i++)
        {
            var cosmeticItem = Instantiate(_cosmeticShopItemPrefab);
            cosmeticItem.transform.SetParent(_playerGridParent, true);
            cosmeticItem.SetCosmeticData(_playerCosmetics[i], _playerCosmetics[i].headSprite);
            var button = cosmeticItem.GetComponent<Button>();
            playerCosmeticsButton.Add(button);
            button.onClick.AddListener(() =>
            {
                ShowSelectedPlayerCosmetic(cosmeticItem);
                _cosmeticSelected = cosmeticItem.CosmeticData;

                for (int i = 0; i < presidentCosmeticsButton.Count; i++)
                    playerCosmeticsButton[i].image.color = Color.clear;

                button.image.color = _selectedColor;

                _buyButton.GetComponentInChildren<TextMeshProUGUI>().text = _cosmeticSelected.cost.ToString();
            });
        }

        for (int i = 0; i < _presidentCosmetics.Length; i++)
        {
            var cosmeticItem = Instantiate(_cosmeticShopItemPrefab);
            cosmeticItem.transform.SetParent(_presidentGridParent, true);
            cosmeticItem.SetCosmeticData(_presidentCosmetics[i], _presidentCosmetics[i].headSprite);
            var button = cosmeticItem.GetComponent<Button>();
            presidentCosmeticsButton.Add(button);
            button.onClick.AddListener(() =>
            {
                ShowSelectedPresidentCosmetic(cosmeticItem);
                _cosmeticSelected = cosmeticItem.CosmeticData;

                for (int i = 0; i < presidentCosmeticsButton.Count; i++)
                    presidentCosmeticsButton[i].image.color = Color.white;

                button.image.color = _selectedColor;
                _buyButton.GetComponent<TextMeshProUGUI>().text = _cosmeticSelected.cost.ToString();
            });
        }

        #endregion

        #region botones

        _playerButton.onClick.AddListener(() =>
        {
            _playerButton.image.color = Color.green * 5;
            _presidentButton.image.color = Color.green * .5f;
        });

        _presidentButton.onClick.AddListener(() =>
        {
            _presidentButton.image.color = Color.green * 5;
            _playerButton.image.color = Color.green * .5f;
        });

        #endregion

        //_buyButton.onClick.AddListener(() =>;  GUARDAR EL SCRIPTABLE OBJECT SELECCIONADO

        _playerButton.onClick.Invoke();
    }

    void ShowSelectedPlayerCosmetic(CosmeticShopItem cosmeticItem)
    {
        cosmeticItem.SetCosmetics(ref _playerHeadSprite, ref _playerTorsoSprite, ref _playerRightLegSprite,
                ref _playerLeftLegSprite, ref _playerRightHandSprite, ref _playerLeftHandSprite);
    }
    void ShowSelectedPresidentCosmetic(CosmeticShopItem cosmeticItem)
    {
        cosmeticItem.SetCosmetics(ref _presidentHeadSprite, ref _presidentTorsoSprite, ref _presidentRightLegSprite,
                ref _presidentLeftLegSprite, ref _presidentRightHandSprite, ref _presidentLeftHandSprite);
    }
}
