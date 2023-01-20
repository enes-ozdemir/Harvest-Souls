using _Scripts.SO;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Behaviour
{
    public class SkillDrop : MonoBehaviour
    {
        public float duration = 1f;
        private SkillData _skillData;
        [SerializeField] private SpriteRenderer image;

        public void SetupDrop(SkillData skillData)
        {
            _skillData = skillData;
            image.sprite = skillData.skillSprite;
            AnimateScale();
        }

        public SkillData GetSkill() => _skillData;

        private void AnimateScale()
        {
            transform.DOScale(0.8f, duration).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                transform.DOScale(0.5f, duration).SetEase(Ease.InCubic).OnComplete(AnimateScale);
            });
        }
    }
}