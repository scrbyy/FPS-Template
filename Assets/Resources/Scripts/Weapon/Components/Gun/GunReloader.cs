using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class GunReloader 
{
    public event Action OnReloadStart;
    public event Action OnReloadEnd;

    public int CurrentAmmo => _currentAmmo;
    public int ReserveAmmo => _reserveAmmo;
    public bool IsReloading => _isReloading;

    private int _currentAmmo;
    private int _reserveAmmo;
    private bool _isReloading;

    private CancellationTokenSource _reloadCts;
    private IAmmoData _ammoData;

    public GunReloader(IAmmoData ammoData)
    {
        _ammoData = ammoData;
        _currentAmmo = ammoData.StartAmmo;
        _reserveAmmo = ammoData.ReserveAmmo;
    }

    public void Initialize()
    {
        _isReloading = false;

        _reloadCts = new CancellationTokenSource();
    }

    public void Deinitialize()
    {
        if (_isReloading)
        {
            _isReloading = false;
            _reloadCts?.Cancel();
        }
    }

    public void Reload()
    {
        if (_isReloading) return;
        if (_reserveAmmo <= 0) return;
        if (_currentAmmo >= _ammoData.MagazineSize) return;
        ReloadTask().Forget();
    }

    public void UseBullet()
    {
        _currentAmmo--;
    }

    public bool CanShoot()
    {
        return _currentAmmo > 0 && _isReloading == false;
    }

    private async UniTaskVoid ReloadTask()
    {
        _isReloading = true;

        ResetCts();

        OnReloadStart?.Invoke();

        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_ammoData.ReloadDuration), cancellationToken: _reloadCts.Token);

            int neededAmmo = _ammoData.MagazineSize - _currentAmmo;
            int amountToReload = Math.Min(_reserveAmmo, neededAmmo);

            _currentAmmo += amountToReload;
            _reserveAmmo -= amountToReload;

            OnReloadEnd?.Invoke();
        }
        catch (OperationCanceledException) { }
        finally
        {
            _isReloading = false;
        }
    }

    private void ResetCts()
    {
        _reloadCts?.Cancel();
        _reloadCts?.Dispose();
        _reloadCts = new CancellationTokenSource();
    }
}