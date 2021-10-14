using UnityEngine;
using UnityEngine.UI;

public class UI_Buttons : MonoBehaviour
{
	private GameController gameController;

	public Button Button_GO;
	public Button Button_SwitchTurn;
	public Button Skill_1;

	void Start()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

		Button btn = Button_GO.GetComponent<Button>();
		btn.onClick.AddListener(CharacterStartSearchPath);

		Button btn_switch = Button_SwitchTurn.GetComponent<Button>();
		btn_switch.onClick.AddListener(SwitchTurn);

		Button btn_skill_1 = Skill_1.GetComponent<Button>();
		btn_skill_1.onClick.AddListener(InitSkill1);
	}

	void CharacterStartSearchPath()
	{
		gameController.startSearchPath();
	}

	void SwitchTurn()
	{
		gameController.SwitchTurn();
	}

	void InitSkill1()
    {
		Character character = gameController.GetActiveCharacter();

		if (character != null)
		{
			character.skills[0].Init(character);
			character.active_skill = 0;

			gameController.status = GameStatuses.use_skill;
		}
    }
}
