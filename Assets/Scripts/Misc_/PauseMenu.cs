using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using IngameDebugConsole;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;

public class PauseMenu : MonoBehaviour
{

    public static PauseMenu Instance;

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject bindingMenuUI;
    public GameObject player;
    public GameObject ShopKeeperObj;

    public CambiarEscena _cambiarEscena;
    public ShopKeeper shopKeeper;
    public Cama bed;
    public GameObject UI;
    public GameObject Options;
    public GameObject PhysicsGun;
    public Button resumeButton;

    public AudioSource Music;

    public FPSLimit FPSLimit;
    public TextMeshProUGUI FPSText;

    public DebugLogManager debugLogManager;
    //public GameObject Hotbar;

    private void Awake() {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    private void Start() {
        player = GameObject.FindWithTag("Player");
        ShopKeeperObj = GameObject.FindGameObjectWithTag("Shop");
        shopKeeper = ShopKeeperObj.GetComponent<ShopKeeper>();
        
        if (FPSLimit.target == 0)
        {
            FPSLimit.target = 60;
            PlayerPrefs.SetInt("TargetaFPS", 60);
        }
        FPSLimit.target = PlayerPrefs.GetInt("TargetaFPS");
        FPSText.text = "FPS: " + FPSLimit.target;
    }

    private void Update() {
        if (GameInput.Instance.playerInputActions.Player.Pause.WasPressedThisFrame() && player.GetComponent<PlayerInventoryHolder>().isInventoryOpen == false && shopKeeper.IsBuying == false && bed._isSleeping == false) {
            if (GameIsPaused) {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        
        /*if (!GameIsPaused) {
            if(Input.GetKeyDown(KeyCode.BackQuote))
            {
                if( debugLogManager.IsLogWindowVisible )
                {
                    PauseConsole();
                }
                else
                {
                    ResumeConsole();
                }
            }
        }*/
    }

    public void Resume()
    {
        UI.SetActive(true);
        //Hotbar.SetActive(true);
        if (Music.clip != null)
        {
            Music.UnPause();
        }
        GameInput.Instance.playerInputActions.Player.Inventory.Enable();
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        PhysicsGun.SetActive(true);
        GameIsPaused = false;
        player.GetComponent<FirstPersonController>().enabled = true;
        player.GetComponent<Interactor>().enabled = true;
        player.GetComponent<CubePlacer>().enabled = true;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        UI.SetActive(false);
        //Hotbar.SetActive(true);
        if (Music.clip != null)
        {
            Music.Pause();
        }
        GameInput.Instance.playerInputActions.Player.Inventory.Disable();
        pauseMenuUI.SetActive(true);
        PhysicsGun.SetActive(false);
        GameIsPaused = true;
        player.GetComponent<FirstPersonController>().enabled = false;
        player.GetComponent<Interactor>().enabled = false;
        player.GetComponent<CubePlacer>().enabled = false;

        resumeButton.Select();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void AshudaMabel()
    {
        _cambiarEscena.LoadScene(0);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        //SceneManager.LoadScene("Menu");
        Debug.Log("Aún no hay menú xd");
    }

    public void OptionsMenu()
    {
        pauseMenuUI.SetActive(false);
        this.GetComponent<OptionsMenu>().isOptionsMenuOpen = true;
        Options.SetActive(true);
    }

    public void OptionsMenuBack()
    {
        pauseMenuUI.SetActive(true);
        this.GetComponent<OptionsMenu>().isOptionsMenuOpen = false;
        Options.SetActive(false);
    }

    public void BindingsMenu()
    {
        pauseMenuUI.SetActive(false);
        Options.SetActive(false);
        bindingMenuUI.SetActive(true);
    }

    public void BindingsMenuBack()
    {
        pauseMenuUI.SetActive(false);
        bindingMenuUI.SetActive(false);
        Options.SetActive(true);
    }



    public void QuitGame()
    {
        Application.Quit();
    }
}
