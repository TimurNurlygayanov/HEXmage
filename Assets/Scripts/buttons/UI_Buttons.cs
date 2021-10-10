using UnityEngine;
using UnityEngine.UI;

public class UI_Buttons : MonoBehaviour
{
	public Button Button_GO;
	public Button Button_SwitchTurn;

	void Start()
	{
		Button btn = Button_GO.GetComponent<Button>();
		btn.onClick.AddListener(CharacterStartSearchPath);

		Button btn_switch = Button_SwitchTurn.GetComponent<Button>();
		btn_switch.onClick.AddListener(SwitchTurn);
	}

	void CharacterStartSearchPath()
	{
		GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		gameController.startSearchPath();
	}

	void SwitchTurn()
	{
		GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		gameController.SwitchTurn();

		Debug.Log("SwithTurn");
	}
}
