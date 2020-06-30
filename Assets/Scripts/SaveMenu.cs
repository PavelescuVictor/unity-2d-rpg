using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveMenu : GameMenu
{
    public Button closeMenu;
    public Button backToMainMenu;
    private Player player;

    void OnEnable()
    {
        SavePlayer();
    }
    
    public void SavePlayer()
    {
        player = GameObject.FindGameObjectWithTag("MainCharacter").GetComponent<Player>();
        SaveManager.SavePlayer(player);
    }
}
