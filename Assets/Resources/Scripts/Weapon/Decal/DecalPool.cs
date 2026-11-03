using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DecalPool : MonoBehaviour
{
    [SerializeField] private GameObject _decalPrefab;
    [SerializeField] private int _initializePoolSize;
    [SerializeField] private int _maxPoolSize;
    [SerializeField] private float _liveTime;
    [SerializeField] private float _surfaceOffset;

    private Queue<GameObject> _availableDecals;

    private List<GameObject> _allDecals;

    private CancellationTokenSource _destroyCts;

    private void Awake()
    {
        _availableDecals = new Queue<GameObject>(_initializePoolSize);
        _allDecals = new List<GameObject>(_initializePoolSize);
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
        _allDecals.Add(decal);
        _availableDecals.Enqueue(decal);
        return decal;
    }

    public void Get(HitData hitData)
    {
        GameObject decalToUse = null;

        while (_availableDecals.Count > 0)
        {
            var candidate = _availableDecals.Dequeue();
            if (candidate != null)
            {
                decalToUse = candidate;
                break;
            }
        }

        if (decalToUse == null)
        {
            if (_allDecals.Count < _maxPoolSize)
            {
                decalToUse = CreateNewDecalInstance();
            }
            else
            {
                decalToUse = _allDecals[0];
                _allDecals.RemoveAt(0);
                _allDecals.Add(decalToUse);
            }
        }

        if (hitData.Normal != Vector3.zero)
        {
            decalToUse.transform.position = hitData.HitPoint + (hitData.Normal * _surfaceOffset);
            decalToUse.transform.rotation = Quaternion.LookRotation(-hitData.Normal);
        }

        decalToUse.SetActive(true);

        DisableDecalAfterTimeAsync(decalToUse, _liveTime, _destroyCts.Token).Forget();
    }

    private async UniTaskVoid DisableDecalAfterTimeAsync(GameObject decal, float delaySeconds, CancellationToken destroyToken)
    {
        try
        {
            var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(destroyToken, decal.GetCancellationTokenOnDestroy());

            await UniTask.Delay(System.TimeSpan.FromSeconds(delaySeconds), delayType: DelayType.DeltaTime, cancellationToken: linkedToken.Token);

            linkedToken.Dispose();

            if (decal != null && decal.activeSelf)
            {
                decal.SetActive(false);
                _availableDecals.Enqueue(decal);
            }
        }
        catch (System.OperationCanceledException)
        {
        }
    }
}