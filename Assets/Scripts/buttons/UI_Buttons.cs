using UnityEngine;
using UnityEngine.UI;

public class UI_Buttons : MonoBehaviour
{
	public Button Button_GO;

	void Start()
	{
		Button btn = Button_GO.GetComponent<Button>();
		btn.onClick.AddListener(CharacterStartSearchPath);
	}

	void CharacterStartSearchPath()
	{
		GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		gameController.startSearchPath();
	}
}
