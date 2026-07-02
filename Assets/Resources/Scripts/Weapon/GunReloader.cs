using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class GunReloader 
{
    public event Action OnReload;
    public event Action OnReloadEnd;

    public int CurrentAmmo => _currentAmmo;
    public int ReserveAmmo => _reserveAmmo;
    public bool IsReloading => _isReloading;

    private int _currentAmmo;
    private int _magazineSize;
    private int _reserveAmmo;
    private float _reloadDuration;

    private bool _isReloading;

    private CancellationTokenSource _reloadCts;

    public void Initialize(IAmmoData ammoData)
    {
        _currentAmmo = ammoData.StartAmmo;
        _magazineSize = ammoData.MagazineSize;
        _reserveAmmo = ammoData.ReserveAmmo;
        _reloadDuration = ammoData.ReloadDuration;

        _isReloading = false;

        _reloadCts = new CancellationTokenSource();
    }

    public async UniTaskVoid ReloadTask()
    {
        _isReloading = true;
        OnReload?.Invoke();

        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_reloadDuration), cancellationToken: _reloadCts.Token);

            int neededAmmo = _magazineSize - _currentAmmo;
            int amountToReload = Math.Min(_reserveAmmo, neededAmmo);

            _currentAmmo += amountToReload;
            _reserveAmmo -= amountToReload;

            OnReloadEnd?.Invoke();
        }
        finally
        {
            _isReloading = false;
        }
    }

    public void Deinitialize()
    {
        if (_isReloading)
        {
            _reloadCts.Cancel();
            _reloadCts = new CancellationTokenSource();
        }
    }

    public void Reload()
    {
        if (_isReloading) return;
        if (_reserveAmmo < 0) return;
        if (_currentAmmo >= _magazineSize) return;
        ReloadTask().Forget();
    }

    public void UseBullet()
    {
        _currentAmmo--;
    }

    public bool CanShoot()
    {
        return _currentAmmo > 0;
    }
}