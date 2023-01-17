using UnityEngine;

namespace _Scripts.Managers
{
  public class SettingManager : MonoBehaviour
  {
    private bool _isSoundOn;
    
    public void ToggleMusic() => SoundManager.Instance.ToggleMusic();
    
    public void ToggleEffects() => SoundManager.Instance.ToggleEffects();
  }
}
