using System;
using System.Collections.Generic;
using System.Linq;
using LorcanaLorebook.ScriptableObjects;
using LorcanaLorebook.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LorebookManager : MonoBehaviour
{
    public AssetLabelReference cardsLabel;

    private List<Card> _cards = new List<Card>();

    // Start is called before the first frame update
    void Start()
    {
        //Once addressables load let's get all cards.
        Addressables.InitializeAsync().Completed += OnAddressablesLoaded;
    }

    private void OnAddressablesLoaded(AsyncOperationHandle<IResourceLocator> resourceLocations)
    {
        Debug.Log("Addressables initialized. Trying to load cards using label.");
        Addressables.LoadAssetsAsync<Card>(cardsLabel, null).Completed += OnCardsLoaded;
    }

    private void OnCardsLoaded(AsyncOperationHandle<IList<Card>> handle)
    {
        _cards = handle.Result.ToList();
    }
}
