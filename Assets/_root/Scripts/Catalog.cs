using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Catalog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _itemId;
    [SerializeField] TextMeshProUGUI _itemDescription;
    [SerializeField] GameObject _itemIdHolder;
    [SerializeField] GameObject _itemDescriptionHolder;

    private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();

    private void Start()
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnFailure);
    }

    private void OnFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }

    private void OnGetCatalogSuccess(GetCatalogItemsResult result)
    {
        HandleCatalog(result.Catalog);
        Debug.Log($"Catalog was loaded successfully!");
    }

    private void HandleCatalog(List<CatalogItem> catalog)
    {
        foreach (var item in catalog)
        {
            _catalog.Add(item.ItemId, item);
            Debug.Log($"Catalog item {item.ItemId} was added successfully!");
            TextMeshProUGUI tempId = Instantiate(_itemId, _itemIdHolder.transform);
            tempId.gameObject.SetActive(true);
            tempId.text = item.ItemId;

            TextMeshProUGUI tempDescription = Instantiate(_itemDescription, _itemDescriptionHolder.transform);
            tempDescription.gameObject.SetActive(true);
            tempDescription.text = item.Description;
        }
    }
}
