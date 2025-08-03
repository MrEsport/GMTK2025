using TMPro;
using UnityEngine;

using Random = UnityEngine.Random;

public class ScoreUIHandler : MonoBehaviour
{
    [SerializeField] TMP_Text scoreTextUI;

    [SerializeField] TMP_Text scorePopupUI;
    [SerializeField] PopupUIHandler popupHandler;

    private void Start()
    {
        scorePopupUI.gameObject.SetActive(false);
    }

    public void SetScoreText(int score)
    {
        scoreTextUI.text = score.ToString();
    }

    public void SetScoreText(int score, int addedScore)
    {
        SetScoreText(score);
        HandleScorePopup(addedScore);
    }

    public void HidePopup() => popupHandler.DelayHide(1f);

    private void HandleScorePopup(int score)
    {
        scorePopupUI.gameObject.SetActive(true);

        scorePopupUI.text = score.ToString();
        Vector3 rot = Vector3.zero;
        rot.z = Random.Range(-10f, 10f);
        scorePopupUI.rectTransform.localRotation = Quaternion.Euler(rot);
    }
}
