using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MapPointComponent : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string _sceneName = "Battle";
    [SerializeField] private FieldObject _missionData;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameStorage.SetFieldData(_missionData);
        SceneManager.LoadScene(_sceneName);
    }
}
