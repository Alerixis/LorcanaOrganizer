using System.Collections.Generic;
using System.Linq;
using LorcanaLorebook.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LorcanaLorebook.UI
{
    /// <summary>
    /// Page that loads cards and allows user to select begin.
    /// </summary>
    public class LorebookStartupPage : LorebookPage 
    {
        public AssetLabelReference CardsLabel;

        public TextMeshProUGUI LoadingText;

        public override void Initialize(LorebookManager manager)
        {
            base.Initialize(manager);
            
            //Once addressables load let's get all cards.
            Addressables.InitializeAsync().Completed += OnAddressablesLoaded;
        }

        private void OnAddressablesLoaded(AsyncOperationHandle<IResourceLocator> resourceLocations)
        {
            Debug.Log("Addressables initialized. Trying to load cards using label.");
            Addressables.LoadAssetsAsync<Card>(CardsLabel, null).Completed += OnCardsLoaded;
        }

        private void OnCardsLoaded(AsyncOperationHandle<IList<Card>> handle)
        {
            _manager.OnCardsLoaded(handle.Result.ToList());
        }
    }
}