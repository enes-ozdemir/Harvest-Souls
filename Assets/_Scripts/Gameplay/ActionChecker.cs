using _Scripts.Managers;
using _Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Gameplay
{
    public class ActionChecker : MonoBehaviour
    {
        [SerializeField] private bool isContinue;
        [SerializeField] private TextMeshProUGUI text;

        private void Start()
        {
            text.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        public void Update()
        {
            if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= 5f)
            {
                text.gameObject.SetActive(true);
                CheckIfPressed();
            }
            else
            {
                text.gameObject.SetActive(false);
            }
        }

        private void CheckIfPressed()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isContinue)
                {
                    //todo spawn next wave
                }
                else
                {
                    print("Go townscene");
                    SceneManager.LoadScene("TownScene");
                }
            }
        }
    }
}