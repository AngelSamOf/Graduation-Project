using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string _sceneName = "Main";

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(_sceneName);
    }
}
