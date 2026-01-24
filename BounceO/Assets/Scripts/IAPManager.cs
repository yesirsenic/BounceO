using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    public static IAPManager Instance { get; private set; }

    public const string PRODUCT_NO_ADS = "no_ads";

    private StoreController _store;

    private const string PREF_NO_ADS = "NO_ADS";

    private bool noAdsCached;

    public bool NoAdsOwned => noAdsCached;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        noAdsCached = PlayerPrefs.GetInt(PREF_NO_ADS, 0) == 1;

    }

    private async void Start()
    {
        await InitializeIAP();
    }

    public async Task InitializeIAP()
    {
        _store = UnityIAPServices.StoreController();

        _store.OnPurchasePending += OnPurchasePending;
        _store.OnPurchaseFailed += failed => Debug.LogWarning($"IAP Purchase Failed: {failed}");
        _store.OnProductsFetched += OnProductsFetched;
        _store.OnProductsFetchFailed += failed => Debug.LogWarning($"IAP Products Fetch Failed: {failed}");
        _store.OnPurchasesFetched += OnPurchasesFetched;
        _store.OnPurchasesFetchFailed += failed => Debug.LogWarning($"IAP Purchases Fetch Failed: {failed}");
        _store.OnStoreDisconnected += desc => Debug.LogWarning($"IAP Store Disconnected: {desc}");


        await _store.Connect();

        var productsToFetch = new List<ProductDefinition>
        {
            new ProductDefinition(PRODUCT_NO_ADS, ProductType.NonConsumable),
        };
        _store.FetchProducts(productsToFetch);
    }

    private void OnProductsFetched(List<Product> products)
    {
        
        _store.FetchPurchases(); 
    }

    private void OnPurchasesFetched(Orders orders)
    {
        
        foreach (var confirmed in orders.ConfirmedOrders) 
        {
            if (OrderContainsProductId(confirmed, PRODUCT_NO_ADS))
            {
                GrantNoAds();
                return;
            }
        }
    }

    private void OnPurchasePending(PendingOrder pendingOrder) 
    {
        if (OrderContainsProductId(pendingOrder, PRODUCT_NO_ADS))
        {
            Debug.Log("No Ads purchase pending...");
            // ❌ GrantNoAds() 절대 금지
        }
        _store.ConfirmPurchase(pendingOrder); // :contentReference[oaicite:7]{index=7}
    }

    private bool OrderContainsProductId(Order order, string productId)
    {
        // order.Info.PurchasedProductInfo[*].productId 로 확인
        var list = order.Info?.PurchasedProductInfo; // :contentReference[oaicite:8]{index=8}
        if (list == null) return false;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].productId == productId)
                return true;
        }
        return false;
    }

    private void GrantNoAds()
    {
        PlayerPrefs.SetInt(PREF_NO_ADS, 1);
        PlayerPrefs.Save();

        noAdsCached = true;
        AdManager.Instance.SetNoAds(true);
        Debug.Log("NoAds unlocked!");
        BannerAd.Instance.Hide();
    }

    // “광고 제거 구매” 버튼 OnClick에 이걸 연결하면 됨
    public void BuyNoAds()
    {
        if (_store == null)
        {
            Debug.LogWarning("IAP not initialized yet.");
            return;
        }

        _store.PurchaseProduct(PRODUCT_NO_ADS); // v5에서 productId로 바로 구매 가능 :contentReference[oaicite:9]{index=9}
    }

    public void RestorePurchases()
    {
        if (_store == null) return;
        _store.RestoreTransactions((ok, msg) => Debug.Log($"Restore result: {ok}, {msg}"));
    }


    //에디터 테스트용
    public void DebugUnlockNoAds()
    {
        PlayerPrefs.SetInt("NO_ADS", 1);
        PlayerPrefs.Save();

        noAdsCached = true;
        AdManager.Instance.SetNoAds(true);
        Debug.Log("NoAds DEBUG unlocked");
    }
}

