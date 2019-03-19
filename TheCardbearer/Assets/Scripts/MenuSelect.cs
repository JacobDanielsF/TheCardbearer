using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuSelect : MonoBehaviour
{
	public GameObject camera;
	
	public GameObject MainText;
	public Button HowToPlay;
	public Button Button1;
	public Button Button2;
	public Button Button3;
	public Image fade;
	
	public AudioClip hit;
	public AudioClip shuffle;
	public AudioClip snap;
	public AudioClip thwip;
	
	private AudioSource source;
	
	public Image img;
	public GameObject TutorialText;
	public Button Next;
	public Button Back;
	
	public Sprite image1;
	public Sprite image2;
	
	private string text1 = "You will encounter hostile spirits which can only be dispelled with magic cards. Interpret spirits to analyze their essense.";
	private string text2 = "Read the qualities of each spirit and choose a card that best matches their description. Multiple cards must be chosen when encountering more than one spirit.";
	
	private int panel = 1;
	
	public GameObject card;
	public GameObject cardgroup;
	private Component[] cards;
	
	
    // Start is called before the first frame update
    void Start()
    {
		source = camera.GetComponent<AudioSource>();
		source.volume = 0;
		source.DOFade(1, 0.8f);
		
		fade.gameObject.SetActive(true);
		var c = fade.color;
		fade.color = new Color(c.r, c.b, c.g, 1);
		fade.DOColor(new Color(c.r, c.b, c.g, 0), 0.8f).OnComplete(RemoveFade);
		
		HowToPlay.onClick.AddListener(ViewTutorial);
		Button1.onClick.AddListener(delegate{Start(1);});
		Button2.onClick.AddListener(delegate{Start(2);});
		Button3.onClick.AddListener(delegate{Start(3);});
		Next.onClick.AddListener(NextPanel);
		Back.onClick.AddListener(BackPanel);
		
		for (int i = 0; i < 80; i++)
		{
			Vector3 pos;
			Vector3 rot;
			
			pos = new Vector3(Random.Range(-6f, 6f), Random.Range(0.4f, 4.5f), Random.Range(-4f, 8f));
			rot = new Vector3(Random.Range(-360f, 360f), Random.Range(-360f, 360f), Random.Range(-360f, 360f));
			
			GameObject newcard = Instantiate(card, pos, Quaternion.identity, cardgroup.transform);
			newcard.transform.eulerAngles = rot;
		}
		
		cards = cardgroup.GetComponentsInChildren<Transform>();
		
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		foreach (Transform child in cards)
		{
			child.Translate(new Vector3(0, Mathf.Cos((Time.frameCount * 0.05f) + ((child.transform.position.x+8))) * 0.005f, 0), Space.World);
		}
    }
	
	void ViewButtons(bool b)
	{
		MainText.SetActive(b);
		HowToPlay.gameObject.SetActive(b);
		Button1.gameObject.SetActive(b);
		Button2.gameObject.SetActive(b);
		Button3.gameObject.SetActive(b);
	}
	
	void ViewTutorial()
	{
		source.PlayOneShot(thwip, 1);
		
		ViewButtons(false);
		
		img.sprite = image1;
		TutorialText.transform.Find("Text").gameObject.GetComponent<Text>().text = text1;
		
		Next.transform.Find("Text").gameObject.GetComponent<Text>().text = "NEXT";
		
		img.gameObject.SetActive(true);
		TutorialText.SetActive(true);
		Next.gameObject.SetActive(true);
		
		
		camera.transform.DOMove(new Vector3(0, 0.7f, -5), 1).SetEase(Ease.OutQuint);
		camera.transform.DORotate(new Vector3(0, 0, 0), 1).SetEase(Ease.OutQuint);
		
	}
	
	void NextPanel()
	{
		if (panel == 1)
		{
			source.PlayOneShot(hit, 1);
			
			panel++;
			
			Back.gameObject.SetActive(true);
			
			img.sprite = image2;
			TutorialText.transform.Find("Text").gameObject.GetComponent<Text>().text = text2;
			
			Next.transform.Find("Text").gameObject.GetComponent<Text>().text = "CLOSE";
			
		}
		else if (panel == 2)
		{
			source.PlayOneShot(snap, 1);
			
			panel = 1;
			
			ViewButtons(true);
			
			img.gameObject.SetActive(false);
			TutorialText.SetActive(false);
			Next.gameObject.SetActive(false);
			Back.gameObject.SetActive(false);
			
			
			camera.transform.DOMove(new Vector3(0.946f, 0.7f, -10), 1).SetEase(Ease.OutQuint);
			camera.transform.DORotate(new Vector3(0, 0, 10), 1).SetEase(Ease.OutQuint);
			
		}	
	}
	
	void BackPanel()
	{
		source.PlayOneShot(hit, 1);
		
		panel--;
		
		Back.gameObject.SetActive(false);
		
		img.sprite = image1;
		TutorialText.transform.Find("Text").gameObject.GetComponent<Text>().text = text1;
		
		Next.transform.Find("Text").gameObject.GetComponent<Text>().text = "NEXT";
	}
	
	
	
	void RemoveFade()
	{
		fade.gameObject.SetActive(false);
	}
	
	void Start(int enemies)
	{
		source.PlayOneShot(shuffle, 1);
		
		StaticVariables.SceneEnemies = enemies;
		
		fade.gameObject.SetActive(true);
		
		source.DOFade(0, 0.8f);
		
		var c = fade.color;
		fade.DOColor(new Color(c.r, c.b, c.g, 1), 0.8f).OnComplete(NextScene);
	}
	
	void NextScene()
	{
		SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
	}
}
