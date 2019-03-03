using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class BattleUIScript : MonoBehaviour
{
	public GameObject camera;
	public GameObject enemygroup;
	
	public Button interpret;
	public Text soultext;
	public GameObject cardbuttons;
	public Button back;
	public GameObject carddesc;
	public GameObject enemydesc;
	public GameObject attacktext;
	public Image fade;
	
	private Component[] buttons;
	
	private Dictionary<string, string> desc = new Dictionary<string, string>();
	
	private string currentcard = "";
	
	private string solution = "FREEDOM";
	
	private string enemytext = "A hostile spirit with the appearance of a lamp. \nSeems to be quick, energetic and curious. \nLight enough to be battered by even the slightest gust of wind.";
	
	private int soul = 3;
	
	
    // Start is called before the first frame update
    void Start()
    {
		desc.Add("FREEDOM", "The essense of unconstrained movement and action. \n\nWhile freedom is often thought of as a good thing, its essense can be described as difficult to predict and often rebellious.");
		desc.Add("CHAINS", "The essense of constraints and moral weights. \n\nThis includes the many things which keep us anchored to reality, such as social bonds, logic, and fear of consequence.");
		desc.Add("BELIEF", "The essense of faith and possibility. \n\nBelief encompasses the things we may not understand or comprehend which we assign meaning to. \n\nThis includes subjects which currently only exist in our imagination.");
		desc.Add("DENIAL", "The essense of close-mindedness and despair. \n\nThis is a disturbingly powerful essense, capable of directing society towards lies and disaster. \n\nDenial is the true manifestation of our inner demons.");
		desc.Add("POWER", "The essense of strength and domination. \n\nHealth, skill and strength are closely connected, although unchecked power will often result in a corruption of the mind and damage to other individuals.");
		desc.Add("MERCY", "The essense of sacrifice and forgiveness. \n\nMercy is the rejection of power in order to avoid signifianct harm or destruction. \n\nThis is a rare essense to find in the world naturally.");
		
        interpret.onClick.AddListener(delegate{Focus(enemygroup.transform.Find("Enemy1").gameObject);});
		back.onClick.AddListener(GoBack);
		
		buttons = cardbuttons.GetComponentsInChildren<Button>();
		
		foreach (Button child in buttons)
		{
			string name = child.gameObject.name;
			
			child.onClick.AddListener(delegate{Click(name);});
		}
		
		fade.gameObject.SetActive(true);
		var c = fade.color;
		fade.color = new Color(c.r, c.b, c.g, 1);
		fade.DOColor(new Color(c.r, c.b, c.g, 0), 0.5f).OnComplete(RemoveFade);
		
		soultext.text = "SOUL: " + soul.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void RemoveFade()
	{
		fade.gameObject.SetActive(false);
	}
	
	void EndGame()
	{
		fade.gameObject.SetActive(true);
		
		var c = fade.color;
		fade.DOColor(new Color(c.r, c.b, c.g, 1), 0.5f).OnComplete(NextScene);
	}
	
	void NextScene()
	{
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}
	
	
	public void OnMouseOver(GameObject button)
    {
		string name = button.name;
		Color newcolor = button.GetComponent<Image>().color;
		
		currentcard = name;
		
		carddesc.transform.Find("TitleText").gameObject.GetComponent<Text>().text = name;
		carddesc.transform.Find("BodyText").gameObject.GetComponent<Text>().text = desc[name];
		
		carddesc.GetComponent<Image>().color = newcolor;
		
		carddesc.SetActive(true);
    }
	
	void Click(string name)
	{
		cardbuttons.SetActive(false);
		back.gameObject.SetActive(false);
		carddesc.SetActive(false);
		enemydesc.SetActive(false);
		
		currentcard = "";
		
		
		if (name == solution){
			CriticalHit();
		} else {
			Miss();
		}
	}
	
	
	void CriticalHit()
	{
		ZoomOut();
		
		attacktext.transform.Find("Text").gameObject.GetComponent<Text>().text = "Illusion demistified.";
		attacktext.SetActive(true);
		
		GameObject enemybody = enemygroup.transform.Find("Enemy1").gameObject.transform.Find("Body").gameObject;
		Material m = enemybody.GetComponent<Renderer>().material;
		Color c = m.color;
		m.DOColor(new Color(c.r, c.b, c.g, 0), 1).SetEase(Ease.OutQuint);
		
		StartCoroutine(PauseEnd());
	}
	
	void Miss()
	{
		ZoomOut();
		
		attacktext.transform.Find("Text").gameObject.GetComponent<Text>().text = "There was no effect.";
		attacktext.SetActive(true);
		
		Transform enemybodyt = enemygroup.transform.Find("Enemy1").gameObject.transform.Find("Body").gameObject.transform;
		enemybodyt.DOShakePosition(0.5f, new Vector3(0.5f, 0, 0), 15, 30, false, true);
		
		soul--;
		
		if (soul == 0)
		{
			StartCoroutine(Lose());
		} else {
			StartCoroutine(PauseReturn());
		}
	}
	
	
	IEnumerator Lose()
    {
        yield return new WaitForSeconds(1);
		
		attacktext.transform.Find("Text").gameObject.GetComponent<Text>().text = "Your soul has been depleted.";
		
		yield return new WaitForSeconds(1);
		EndGame();
    }
	
	IEnumerator PauseEnd()
	{
		yield return new WaitForSeconds(1);
		attacktext.transform.Find("Text").gameObject.GetComponent<Text>().text = "The area is clear.";
		
		yield return new WaitForSeconds(0.5f);
		EndGame();
	}
	
	IEnumerator PauseReturn()
    {
        yield return new WaitForSeconds(1);
		
		attacktext.SetActive(false);
		
		soultext.text = "SOUL: " + soul.ToString();
		
		soultext.gameObject.SetActive(true);
		interpret.gameObject.SetActive(true);
    }
	
	
	
	
	void ZoomIn(GameObject enemy)
	{
		camera.transform.DOMove(enemy.transform.position + new Vector3(1.8f, 2.5f, -9), 0.5f).SetEase(Ease.OutQuint);
		camera.transform.DORotate(new Vector3(3, 0, -3), 0.5f).SetEase(Ease.OutQuint);
	}
	
	void ZoomOut()
	{
		camera.transform.DOMove(new Vector3(0, 2, -10), 0.5f).SetEase(Ease.OutQuint);
		camera.transform.DORotate(new Vector3(3, 1, -5), 0.5f).SetEase(Ease.OutQuint);
	}
	
	
	void Focus(GameObject enemy)
	{
		ZoomIn(enemy);
		
		soultext.gameObject.SetActive(false);
		interpret.gameObject.SetActive(false);
		
		enemydesc.transform.Find("Text").gameObject.GetComponent<Text>().text = enemytext;
		
		cardbuttons.SetActive(true);
		back.gameObject.SetActive(true);
		enemydesc.SetActive(true);
	}
	
	void GoBack()
	{
		ZoomOut();
		
		soultext.text = "SOUL: " + soul.ToString();
		
		soultext.gameObject.SetActive(true);
		interpret.gameObject.SetActive(true);
		
		cardbuttons.SetActive(false);
		back.gameObject.SetActive(false);
		carddesc.SetActive(false);
		enemydesc.SetActive(false);
		
		currentcard = "";
	}
	
}
