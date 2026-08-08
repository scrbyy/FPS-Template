using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DecalPool : MonoBehaviour
{
    [SerializeField] private GameObject _decalPrefab;
    [SerializeField] private int _initializePoolSize;
    [SerializeField] private float _fadeTime;
    [SerializeField] private float _surfaceOffset;

    private List<GameObject> _pool;

    private CancellationTokenSource _destroyCts;

    private void Awake()
    {
        _pool = new List<GameObject>();
        _destroyCts = new CancellationTokenSource();

        for (int i = 0; i < _initializePoolSize; i++)
        {
            CreateNewDecalInstance();
        }
    }

    private void OnDestroy()
    {
        _destroyCts?.Cancel();
        _destroyCts?.Dispose();
    }

    private GameObject CreateNewDecalInstance()
    {
        GameObject decal = Instantiate(_decalPrefab, transform);
        decal.SetActive(false);
        _pool.Add(decal);
        return decal;
    }

    public void Get(HitData hitData)
    {
        GameObject decalToUse = null;

        foreach (var decal in _pool)
        {
            if (decal != null && !decal.activeSelf)
            {
                decalToUse = decal;
                break;
            }
        }

        if (decalToUse == null)
        {
            decalToUse = CreateNewDecalInstance();
        }


        if (hitData.normal != Vector3.zero)
        {
            decalToUse.transform.position = hitData.hit + (hitData.normal * _surfaceOffset);
            decalToUse.transform.rotation = Quaternion.LookRotation(-hitData.normal);
        }

        decalToUse.SetActive(true);

        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_destroyCts.Token, decalToUse.GetCancellationTokenOnDestroy());
        DisableDecalAfterTimeAsync(decalToUse, _fadeTime, linkedCts).Forget();
    }

    private async UniTaskVoid DisableDecalAfterTimeAsync(GameObject decal, float delaySeconds, CancellationTokenSource cts)
    {
        try
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(delaySeconds), delayType: DelayType.DeltaTime, cancellationToken: cts.Token);

            if (decal != null)
            {
                decal.SetActive(false);
            }
        }
        catch (System.OperationCanceledException)
        {
        }
        finally
        {
            cts.Dispose();
        }
    }
}