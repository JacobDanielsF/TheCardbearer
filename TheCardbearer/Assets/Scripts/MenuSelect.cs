using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuSelect : MonoBehaviour
{
	public Button StartButton;
	public Image fade;
	
    // Start is called before the first frame update
    void Start()
    {
		fade.gameObject.SetActive(true);
		var c = fade.color;
		fade.color = new Color(c.r, c.b, c.g, 1);
		fade.DOColor(new Color(c.r, c.b, c.g, 0), 0.5f).OnComplete(RemoveFade);
		
		StartButton.onClick.AddListener(StartGame);
    }

    // Update is called once per frame
    void Update()
    {
		
    }
	
	void RemoveFade()
	{
		fade.gameObject.SetActive(false);
	}
	
	void StartGame()
	{
		fade.gameObject.SetActive(true);
		
		var c = fade.color;
		fade.DOColor(new Color(c.r, c.b, c.g, 1), 0.5f).OnComplete(NextScene);
	}
	
	void NextScene()
	{
		SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
	}
}
