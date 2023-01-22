using UnityEngine;

namespace _Scripts.Controllers
{
    public class NpcController : MonoBehaviour
    {
        private CharacterAnimationController _animationController;

        private void Start()
        {
            _animationController=GetComponent<CharacterAnimationController>();
            _animationController.PlayAnimation(AnimationType.Idle);
        }
    }
}