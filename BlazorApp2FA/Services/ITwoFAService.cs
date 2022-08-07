using BlazorApp2FA.Models;

namespace BlazorApp2FA.Services;
public interface ITwoFAService
{
    public Setup2FactorAuthemticationInfo setup2FactorAuthemticationInfo { get; }
    public Task<Setup2FactorAuthemticationInfo> Setup2FA();
    public Task<bool> Validate2FA(Validate2FAPin dto);
}
