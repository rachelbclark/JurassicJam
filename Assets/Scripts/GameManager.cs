using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Paraphernalia.Components;


public class GameManager : MonoBehaviour 
{
	public static GameManager instance;

	public enum SceneType
	{
        Menu, Game
    }
	public SceneType sceneType;

	public AudioClip backgroundMusic;
	public AudioClip hiddenMusic;
	public AudioClip bossMusic;
	public AudioClip explodingBlock;
	public AudioClip lockQuack;
	public AudioClip unlockQuack;
	public AudioClip enemy;

	public Image key;
	public Image[] healthImages;
	public HealthController playerHealth;

	public Button startButton;

	public Material effectMaterial;

	public string nextLevel;

	public int keyCount = 0;
	private int oldKeyCount = 0;

	public bool inBossRoom = false;
	public bool finalDoor = false;
	
	void Awake() 
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else 
		{
			GameManager.instance.sceneType = sceneType;
			GameManager.instance.nextLevel = nextLevel;
			GameManager.instance.effectMaterial = effectMaterial;
			Destroy(gameObject);
		}
	}
	
	public void Start()
	{
		if (!Application.isPlaying) return;
		AudioManager.PlayMusic(backgroundMusic);
		//Cursor.visible = false;

		switch (sceneType)
		{
			case SceneType.Game:
				GameStart();
				break;
		
			case SceneType.Menu:
				MenuStart();
				break;

			default:
				break;
		}
	}

	void MenuStart()
	{
		effectMaterial.SetFloat("_Magnitude", 0);
		effectMaterial.SetFloat("_Balckout", 1);
		if (!startButton.isActiveAndEnabled)
		{
			gameObject.SetActive(true);
		}
	}

	void GameStart()
	{
		key.gameObject.SetActive(false);
		StartCoroutine(BlurIn());
	}

	/* public static void Sanitize () 
	{
		DoorController[] doors = Object.FindObjectsOfType<DoorController>();
		foreach (DoorController door in doors) 
		{
			Collider[] colliders = Physics.OverlapBox
			(
				door.transform.position,
				Vector3.one * 2,
				Quaternion.identity				
			);
			foreach (Collider collider in colliders) 
			{
				DoorController otherDoor = collider.GetComponent<DoorController>();
				if (otherDoor != null && otherDoor != door) 
				{
					door.doorMate = otherDoor;
					if (otherDoor == null && door.access == DoorController.Access.AlwaysLocked) return;
					if (otherDoor == null && door.access == DoorController.Access.FinalDoor) return;
				}
			}
		}
	}*/

	void Update(){
		if (sceneType == SceneType.Game)
		{
			//GameUpdate();
		}
		if (sceneType == SceneType.Menu)
		{
			MenuUpdate();
		}
	}

	/* void GameUpdate(){
		if (PlayerController.instance.hidden)
		{
			AudioManager.CrossfadeMusic(hiddenMusic, 1f);
		}
		if (inBossRoom)
		{
			AudioManager.CrossfadeMusic(bossMusic, .5f);
		}
		if (keyCount != oldKeyCount)
		{
			key.gameObject.SetActive(true);
			oldKeyCount = keyCount;
		}
		if (keyCount == 0)
		{
			key.gameObject.SetActive(false);
		}
		if (finalDoor)
		{
			finalDoor = false;
			Time.timeScale = 0;
			StartCoroutine(BlurOut(5));
		}

		HealthUIUpdate();
	}*/

	public IEnumerator NextLevel()
	{
		yield return SceneManager.LoadSceneAsync (nextLevel);
		yield return StartCoroutine(BlurIn());
		
		Time.timeScale = 1;
		yield return new WaitForEndOfFrame();
	}

	void MenuUpdate()
	{
		
	}

	public void ClickToStart () 
	{
		Time.timeScale = 1;
		StartCoroutine(BlurOut(5));
	}
	public IEnumerator BlurOut(float amount)
	{
		for (float i = 0; i < amount; i += Time.unscaledDeltaTime*5)
		{
			effectMaterial.SetFloat("_Magnitude", i);
			if (amount >= 1) effectMaterial.SetFloat("_BlackOut", i/amount);
			yield return new WaitForEndOfFrame();
		}
		effectMaterial.SetFloat("_Magnitude", amount);
		yield return StartCoroutine(NextLevel());
	}

	public IEnumerator BlurIn()
	{
		float blackout = effectMaterial.GetFloat("_BlackOut");
		if (blackout <= 1) effectMaterial.SetFloat("_BlackOut", 1);
		for (float i = effectMaterial.GetFloat("_Magnitude"); i >=0 ; i -= Time.unscaledDeltaTime*5)
		{
			effectMaterial.SetFloat("_Magnitude", i);
			effectMaterial.SetFloat("_BlackOut", i*.5f + .5f);
			yield return new WaitForEndOfFrame();
		}
		effectMaterial.SetFloat("_Magnitude", 0);
	}

	public void Reset()
	{
		keyCount = 0;
		SceneManager.LoadScene("StartScreen");
	}

	bool deathSoundPlayed = false;
	/* public void HealthUIUpdate () 
	{
		if (playerHealth == null) playerHealth = PlayerController.instance.GetComponent<HealthController>();
		if (playerHealth == null) return;
		
		for (int i = 0; i < healthImages.Length; i++)
		{
			Image img = healthImages[i];
			img.enabled = i < playerHealth.health;
		}

		if (playerHealth.health == 0 && !deathSoundPlayed)
		{
			deathSoundPlayed = true;
			AudioManager.PlayEffect("DuckDeath", null, 1, 1);
		}

		if (playerHealth.health > 0) deathSoundPlayed = false;
	}*/
}
