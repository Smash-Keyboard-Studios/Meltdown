using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
	public static DebugConsole Instance;

	public GameObject ConsoleWindow;

	public GameObject Content;

	public GameObject TextPrefab;

	public TMP_InputField InputField;

	public Scrollbar VertScrollBar;

	bool consoleOpen = false;


	public List<object> commands;

	public static Command Test;
	public static Command Help;
	public static Command<string[]> TestMessage;
	public static Command<int> LoadLevel;
	public static Command ToggleMouse;
	public static Command<float, float, float> TP;
	public static Command DestroyObjectCommand;
	public static Command<float> SetSprintSpeed;
	public static Command InfAmmo;


	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
		}

		InitCommands();
	}

	// Start is called before the first frame update
	void Start()
	{
		ConsoleWindow.SetActive(consoleOpen);
		Cursor.lockState = consoleOpen ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = consoleOpen ? true : false;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.BackQuote) || (consoleOpen && Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.UI))))
		{
			consoleOpen = !consoleOpen;

			ConsoleWindow.SetActive(consoleOpen);
			if (MouseLockManager.Instance == null)
			{
				Cursor.lockState = consoleOpen ? CursorLockMode.None : CursorLockMode.Locked;
				Cursor.visible = consoleOpen;
			}
			else
			{
				MouseLockManager.Instance.MouseVisable = consoleOpen;
			}
			PauseMenu.Overiding = consoleOpen;
			PauseMenu.Paused = false;
			Time.timeScale = consoleOpen ? 0 : 1;

			if (consoleOpen)
			{
				InputField.ActivateInputField();
			}
		}


		if (consoleOpen)
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				EndEdit();
			}
		}
	}

	public void TextToConsole(string text)
	{
		GameObject textObj = Instantiate(TextPrefab, Content.transform, false);

		textObj.GetComponent<TMP_Text>().text = text;

		VertScrollBar.value = 0;
	}

	public void InitCommands()
	{
		Test = new Command("test", "test the debug console", "test", () =>
		{
			TextToConsole($"Test + {System.DateTime.Now}");
		});

		TestMessage = new Command<string[]>("send", "sends the message to the debug console", "send <string>", (message) =>
		{
			string finalMessage = "";

			foreach (var str in message)
			{
				finalMessage += str + " ";
			}

			TextToConsole(finalMessage);
		});

		Help = new Command("help", "generates help message", "help", () =>
		{
			for (int i = 0; i < commands.Count; i++)
			{
				TextToConsole((commands[i] as CommandBase).CommandHelp + " - " + (commands[i] as CommandBase).CommandDescription);
			}
		});

		LoadLevel = new Command<int>("loadlevel", "Loads the desired scene with that build index", "loadlevel <int>", (index) =>
		{
			try
			{

				if (index >= SceneManager.sceneCountInBuildSettings)
				{
					TextToConsole("Does not exsist");
					throw new NullReferenceException();
				}

				TextToConsole($"Loading scene {index}");
				if (LevelLoading.Instance == null)
				{
					SceneManager.LoadScene(index);
					return;
				}

				if (LevelLoading.Instance.loading) return;
				LevelLoading.Instance.LoadScene(index);
			}
			catch (Exception e)
			{
				TextToConsole("I have failed to load that scene \n" + e.Message);
			}
		});

		ToggleMouse = new Command("togglemouse", "closes debug console and makes the mouse visible", "togglemouse", () =>
		{
			if (MouseLockManager.Instance == null)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = true;
			}
			else
			{
				MouseLockManager.Instance.MouseVisable = true;
			}

			consoleOpen = !consoleOpen;

			ConsoleWindow.SetActive(consoleOpen);

			PauseMenu.Overiding = consoleOpen;
			PauseMenu.Paused = false;
			Time.timeScale = consoleOpen ? 0 : 1;
		});

		TP = new Command<float, float, float>("tp", "teleports the player in that direction", "tp <float> <float> <float>", (x, y, z) =>
		{
#nullable enable
			GameObject? go = GameObject.FindGameObjectWithTag("Player");
#nullable restore
			if (go != null && go.transform.name == "Player")
			{
				go.transform.GetComponent<CharacterController>().enabled = false;
				go.transform.position += new Vector3(x, y, z);
				go.transform.GetComponent<CharacterController>().enabled = true;
				TextToConsole("Moved the player");
			}
			else
			{
				TextToConsole("Cannot find the player");
				return;
			}
		});

		DestroyObjectCommand = new Command("obliterate", "Deletes the game object 50m infront of the camera", "obliterate", () =>
		{
			try
			{
				RaycastHit hit;
				Physics.Raycast(Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f)), out hit, 50);
				Destroy(hit.transform.gameObject);
				TextToConsole("Gone!");
			}
			catch
			{
				TextToConsole("Failed");
			}
		});

		SetSprintSpeed = new Command<float>("sprintspeed", "set the sprint speed of the player", "sprintspeed <float>", (newSpeed) =>
		{
#nullable enable
			GameObject? go = GameObject.FindGameObjectWithTag("Player");
#nullable restore
			if (go != null && go.transform.name == "Player")
			{
				go.transform.GetComponent<PlayerMovementController>().SprintSpeed = newSpeed;
				TextToConsole("Set sprint speed of the player to " + newSpeed);
			}
			else
			{
				TextToConsole("Cannot find the player");
				return;
			}
		});

		InfAmmo = new Command("infammo", "Infinite ammo for the gun", "infammo", () =>
		{
#nullable enable
			GameObject? go = GameObject.FindGameObjectWithTag("Player");
#nullable restore
			if (go != null && go.transform.name == "Player")
			{
				go.transform.GetComponent<AmmoController>().InfAmmo = true;
				TextToConsole("Infinite ammo granted");
			}
			else
			{
				TextToConsole("Cannot find the player");
				return;
			}
		});

		// insert commands here

		commands = new List<object>
		{
			Test,
			TestMessage,
			Help,
			LoadLevel,
			ToggleMouse,
			TP,
			DestroyObjectCommand,
			SetSprintSpeed,
			InfAmmo
		};
	}

	public void EndEdit()
	{
		string command = InputField.text;

		if (string.IsNullOrEmpty(command))
		{
			TextToConsole("<color=red>Enter somthing valid!</color>");
			return;
		}

		int result = ParseCommand(command);

		if (result == 0) TextToConsole("Sucess");
		if (result == 1) TextToConsole("<color=red>Unkown Command!</color>");
		if (result == 2) TextToConsole("<color=red>INTERNAL ERROR</color>");

		InputField.text = "";

		InputField.ActivateInputField();

	}

	public int ParseCommand(string input)
	{
		string[] original = input.Split(' ');
		string command = original[0];
		string[] args = new string[original.Length];
		for (int i = 1; i < original.Length; i++)
		{
			args[i - 1] = original[i];
		}

		for (int i = 0; i < commands.Count; i++)
		{
			CommandBase commandBase = commands[i] as CommandBase;

			if (command.Contains(commandBase.Command, StringComparison.InvariantCultureIgnoreCase))
			{
				if (commands[i] as Command != null)
				{
					(commands[i] as Command).Invoke();
					return 0;
				}

				if (args.Length <= 0 || args[0] == null) return 1;

				else if (commands[i] as Command<string> != null)
				{
					(commands[i] as Command<string>).Invoke(args[0]);
					return 0;
				}

				else if (commands[i] as Command<string[]> != null)
				{
					(commands[i] as Command<string[]>).Invoke(args);
					return 0;
				}

				else if (commands[i] as Command<string, string> != null)
				{
					try
					{
						(commands[i] as Command<string, string>).Invoke(args[0], args[1]);
						return 0;
					}
					catch (Exception e)
					{
						print(e.Message);
						return 2;
					}
				}

				else if (commands[i] as Command<string, string, string> != null)
				{
					try
					{
						(commands[i] as Command<string, string, string>).Invoke(args[0], args[1], args[2]);
						return 0;
					}
					catch (Exception e)
					{
						print(e.Message);
						return 2;
					}
				}

				else if (commands[i] as Command<float> != null)
				{
					(commands[i] as Command<float>).Invoke(float.Parse(args[0]));
					return 0;
				}

				else if (commands[i] as Command<float, float> != null)
				{
					try
					{
						(commands[i] as Command<float, float>).Invoke(float.Parse(args[0]), float.Parse(args[1]));
						return 0;
					}
					catch (Exception e)
					{
						print(e.Message);
						return 2;
					}
				}

				else if (commands[i] as Command<float, float, float> != null)
				{
					try
					{
						(commands[i] as Command<float, float, float>).Invoke(float.Parse(args[0]), float.Parse(args[1]), float.Parse(args[2]));
						return 0;
					}
					catch (Exception e)
					{
						print(e.Message);
						return 2;
					}
				}

				else if (commands[i] as Command<bool> != null)
				{
					(commands[i] as Command<bool>).Invoke(bool.Parse(args[0]));
					return 0;
				}

				else if (commands[i] as Command<bool, bool> != null)
				{
					try
					{
						(commands[i] as Command<bool, bool>).Invoke(bool.Parse(args[0]), bool.Parse(args[1]));
						return 0;
					}
					catch (Exception e)
					{
						print(e.Message);
						return 2;
					}
				}

				else if (commands[i] as Command<bool, bool, bool> != null)
				{
					try
					{
						(commands[i] as Command<bool, bool, bool>).Invoke(bool.Parse(args[0]), bool.Parse(args[1]), bool.Parse(args[2]));
						return 0;
					}
					catch (Exception e)
					{
						print(e.Message);
						return 2;
					}
				}

				else if (commands[i] as Command<int> != null)
				{
					try
					{
						(commands[i] as Command<int>).Invoke(int.Parse(args[0]));
						return 0;
					}
					catch (Exception e)
					{
						print(e.Message);
						return 2;
					}
				}

				else if (commands[i] as Command<int, int> != null)
				{
					try
					{
						(commands[i] as Command<int, int>).Invoke(int.Parse(args[0]), int.Parse(args[1]));
						return 0;
					}
					catch (Exception e)
					{
						print(e.Message);
						return 2;
					}
				}

				else if (commands[i] as Command<int, int, int> != null)
				{
					try
					{
						(commands[i] as Command<int, int, int>).Invoke(int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]));
						return 0;
					}
					catch (Exception e)
					{
						print(e.Message);
						return 2;
					}
				}
			}
		}

		return 1;
	}
}
