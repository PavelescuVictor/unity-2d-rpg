using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenu : GameMenu 
{
	public Button closeMenu;
	public Button backToMainMenu;

	void OnEnable()
	{
		LoadPlayer();
	}
	
	public void LoadPlayer()
	{
		PlayerData data = SaveManager.LoadPlayer();

		Vector3 position;
		position.x = data.position[0];
		position.y = data.position[1];
		position.z = data.position[2];
		GameObject player = GameObject.FindGameObjectWithTag("MainCharacter");
		player.transform.position = position;
	}
	
}
