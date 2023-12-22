using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }
    private void Awake()
    {
        if (Instance!=null && Instance!= this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    /*[SerializeField] private Button hostBtn;
    [SerializeField] private Button joinBtn;*/
    [SerializeField] private GameObject interacteImage;

    void Start()
    {
        /*hostBtn.onClick.AddListener(() => NetworkManagerHUD.StartHost());    
        joinBtn.onClick.AddListener(() => NetworkManager.Singleton.StartClient());*/
        interacteImage.SetActive(false);
    }
    public void DisplayInteraction()
    {
        interacteImage.SetActive(true);
    }
    public void HideInteraction()
    {
        interacteImage.SetActive(false);
    }
}
