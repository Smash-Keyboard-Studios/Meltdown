using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteMenu : MonoBehaviour
{
	public static NoteMenu Current;

	[Serializable]
	public struct PageType
	{
		public NoteUtil.NoteType noteType;
		public GameObject Page;
		public GameObject TitleText;
		public GameObject Text;
	}

	public bool MenuOpen = false;

	public PageType[] UIElements;

	void Awake()
	{
		if (Current != null && Current != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Current = this;
		}
	}

	void Start()
	{
		CloseAll();

		MenuOpen = false;
	}

	void Update()
	{
		if (MenuOpen && Input.GetKeyDown(KeyCode.Escape))
		{
			CloseAll();

			MenuOpen = false;

			StartCoroutine(EnablePauseLater());

		}
	}

	public void CloseAll()
	{
		foreach (PageType page in UIElements)
		{
			page.Page.SetActive(false);
		}
	}

	public PageType OpenUI(NoteUtil.NoteType type)
	{
		PageType pageType = new();
		foreach (PageType page in UIElements)
		{
			if (page.noteType == type)
			{
				page.Page.SetActive(true);
				pageType = page;
			}
			else
			{
				page.Page.SetActive(false);
			}
		}
		return pageType;
	}

	public bool UIExist(NoteUtil.NoteType type)
	{
		bool exsist = false;

		foreach (PageType page in UIElements)
		{
			if (page.noteType == type)
			{
				exsist = true;
			}
		}

		return exsist;
	}

	public void OpenNote(NoteObject note)
	{
		if (!UIExist(note.noteType))
		{
			Debug.LogError("UI Element does not exsist");
			return;
		}

		PageType page = OpenUI(note.noteType);

		if (page.TitleText.GetComponent<TMP_Text>() != null) page.TitleText.GetComponent<TMP_Text>().text = note.Title;

		if (page.Text.GetComponent<TMP_Text>() != null) page.Text.GetComponent<TMP_Text>().text = note.Content;

		MenuOpen = true;


		PauseMenu.Overiding = true;
		PauseMenu.Paused = false;
	}

	IEnumerator EnablePauseLater()
	{
		yield return new WaitForSeconds(0.1f);

		PauseMenu.Overiding = false;
		PauseMenu.Paused = false;
	}
}
