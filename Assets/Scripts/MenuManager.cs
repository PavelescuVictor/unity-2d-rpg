using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using yaSingleton;

[CreateAssetMenu(fileName = "MenuManager", menuName = "Systems/MenuManager")]
public class MenuManager : Singleton<MenuManager>
{
	public GameObject mainMenuPrefab;
	public GameObject pauseMenuPrefab;
	public GameObject saveMenuPrefab;
	public GameObject canvasPrefab;
	public GameObject loadMenuPrefab;
	public GameObject optionMenuPrefab;
	public string mainScene;
	private Canvas _canvas;
	private List<GameMenu> activeMenus;

	protected override void Initialize()
	{
		base.Initialize();
		activeMenus = new List<GameMenu>();
		ValidateAndCreateMenuPrefab(mainMenuPrefab);
	}

	public Canvas sceneCanvas
	{
		get
		{
			// Check if canvas variables is set
			if (_canvas == null)
			{
				// If not set, look in scene for canvas
				_canvas = FindObjectOfType<Canvas>();

				if (_canvas == null)
				{
					// If no canvas in scene, create one
					_canvas = Instantiate(canvasPrefab).GetComponent<Canvas>();
				}
			}

			return _canvas;
		}
	}

	public void CreateMenu(GameMenu menu)
	{
		// Checking to see if one exists
		// Creates a new game menu
		GameObject newMenu = Instantiate(menu.gameObject, sceneCanvas.transform);
		GameMenu menuComponent = newMenu.GetComponent<GameMenu>();
		System.Type type = menuComponent.GetType();

		if (type == typeof(MainMenu))
		{
			AddMainMenuFunctionality((MainMenu) menuComponent);
		}
		else if (type == typeof(PauseMenu))
		{
			AddPauseMenuFunctionality((PauseMenu) menuComponent);
		}
		else if (type == typeof(SaveMenu))
		{
			AddSaveMenuFunctionality((SaveMenu) menuComponent);
		}
		else if (type == typeof(LoadMenu))
		{
			AddLoadMenuFunctionality((LoadMenu) menuComponent);
		}
		else if (type == typeof(OptionMenu))
		{
			AddOptionMenuFunctionality((OptionMenu) menuComponent);
		}

		activeMenus.Add(menu);
	}

	public void AddPauseMenuFunctionality(PauseMenu pauseMenu)
	{
		pauseMenu.openSaveMenu.onClick.AddListener(
			delegate { ValidateAndCreateMenuPrefab(saveMenuPrefab); }
		);
		pauseMenu.openSaveMenu.onClick.AddListener(
			delegate { DestroyMenu(pauseMenu); }
		);
		pauseMenu.openLoadMenu.onClick.AddListener(
			delegate { ValidateAndCreateMenuPrefab(loadMenuPrefab); }
		);
		pauseMenu.openLoadMenu.onClick.AddListener(
			delegate { DestroyMenu(pauseMenu); }
		);
		pauseMenu.optionMenu.onClick.AddListener(
			delegate { ValidateAndCreateMenuPrefab(optionMenuPrefab); }
		);
		pauseMenu.optionMenu.onClick.AddListener(
			delegate { DestroyMenu(pauseMenu); }
		);
		pauseMenu.closeMenu.onClick.AddListener(
			delegate { DestroyMenu(pauseMenu); }
		);
		pauseMenu.exitToMainMenu.onClick.AddListener(
			delegate { ValidateAndCreateMenuPrefab(mainMenuPrefab); }
		);
		pauseMenu.exitToMainMenu.onClick.AddListener(
			delegate { DestroyMenu(pauseMenu); }
		);
	}

	public void AddSaveMenuFunctionality(SaveMenu saveMenu)
	{
		saveMenu.closeMenu.onClick.AddListener(
			delegate { DestroyMenu(saveMenu); }
		);
		saveMenu.backToMainMenu.onClick.AddListener(
			delegate { ValidateAndCreateMenuPrefab(pauseMenuPrefab); }
		);
		saveMenu.backToMainMenu.onClick.AddListener(
			delegate { DestroyMenu(saveMenu); }
		);
	}
	
	public void AddLoadMenuFunctionality(LoadMenu loadMenu)
	{
		loadMenu.closeMenu.onClick.AddListener(
			delegate { DestroyMenu(loadMenu); }
		);
		loadMenu.backToMainMenu.onClick.AddListener(
			delegate { ValidateAndCreateMenuPrefab(pauseMenuPrefab); }
		);
		loadMenu.backToMainMenu.onClick.AddListener(
			delegate { DestroyMenu(loadMenu); }
		);
	}

	public void AddMainMenuFunctionality(MainMenu mainMenu)
	{

		mainMenu.newGame.onClick.AddListener(
			delegate { LoadMainScene(); }
		);

		mainMenu.newGame.onClick.AddListener(
			delegate { DestroyMenu(mainMenu); }
		);

		mainMenu.loadGame.onClick.AddListener(
			delegate { ValidateAndCreateMenuPrefab(loadMenuPrefab); }
		);

		mainMenu.loadGame.onClick.AddListener(
			delegate { DestroyMenu(mainMenu); }
		);

		mainMenu.exitGame.onClick.AddListener(
			delegate { DestroyMenu(mainMenu); }
		);
		
		mainMenu.exitGame.onClick.AddListener(
			delegate { ExitApplication(); }
		);
	}

	public void AddOptionMenuFunctionality(OptionMenu optionMenu)
	{
		optionMenu.fullscreenToggle.onValueChanged.AddListener(
			delegate { optionMenu.FullScreenToggle(); }
		);
        
		optionMenu.resolutionDropdown.onValueChanged.AddListener(
			delegate { optionMenu.ResolutionChange(); }
		);
         
		optionMenu.textureQualityDropdown.onValueChanged.AddListener(
			delegate { optionMenu.TextureQualityChange(); }
		);
 
		optionMenu.antiAliasingDropdown.onValueChanged.AddListener(
			delegate { optionMenu.AntiAliasingChange(); }
		);
 
		optionMenu.vSyncDropdown.onValueChanged.AddListener(
			delegate { optionMenu.VSyncChange(); }
		);

		optionMenu.musicVolumeSlider.onValueChanged.AddListener(
			delegate { optionMenu.MusicVolumeChange(); }
		);
		optionMenu.closeMenu.onClick.AddListener(
			delegate { DestroyMenu(optionMenu); }
		);
		optionMenu.applySettings.onClick.AddListener(
			delegate { optionMenu.ApplySettings(); }
		);
		optionMenu.applySettings.onClick.AddListener(
			delegate { ValidateAndCreateMenuPrefab(pauseMenuPrefab); }
		);
		optionMenu.applySettings.onClick.AddListener(
			delegate { DestroyMenu(optionMenu); }
		);
	}

	/// <summary>
	/// Checking if the prefab has a GameMenu script.
	/// If so, instantiate the prefab
	/// </summary>
	/// <param name="prefab"></param>
	private void ValidateAndCreateMenuPrefab(GameObject prefab)
	{
		GameMenu menu = prefab.GetComponent<GameMenu>();

		if (menu != null)
		{
			CreateMenu(menu);
		}
		else
		{
			Debug.LogError("Menu Prefab has no GameMenu script attached to it.");
		}
	}

	public void DestroyMenu(GameMenu menu)
	{
		if (menu != null)
		{
			Destroy(menu.gameObject);
			activeMenus.RemoveAt(0);
		}
	}

	public void LoadMainScene()
	{
		SceneManager.LoadScene(mainScene);
		
	}

	public void ExitApplication()
	{
		Application.Quit();
	}

	public override void OnUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && activeMenus.Count == 0)
		{
			bool exists = false;
			foreach (GameMenu menu in activeMenus)
			{
				if (menu.GetType() == typeof(PauseMenu))
				{
					exists = true;
					break;
				}
			}
			if (!exists)
			{
				ValidateAndCreateMenuPrefab(pauseMenuPrefab);
			}
		}
	}
}
