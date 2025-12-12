using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MapPointComponent : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string _sceneName = "Battle";
    [SerializeField] private FieldObject _missionData;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(_sceneName);
        SceneManager.LoadScene(_sceneName);
    }
}
