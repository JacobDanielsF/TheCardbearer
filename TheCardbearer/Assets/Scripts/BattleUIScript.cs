using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class BattleUIScript : MonoBehaviour
{
	public Material BG1;
	public Material BG2;
	public Material BG3;
	
	public GameObject lamp_prefab;
	public GameObject ribbon_prefab;
	
	public GameObject camera;
	public GameObject enemygroup;
	public GameObject background;
	public GameObject player;
	public GameObject playerbody;
	
	public Button interpret;
	public Text soultext;
	public GameObject cardbuttons;
	public Button back;
	public GameObject carddesc;
	public GameObject enemydesc1;
	public GameObject enemydesc2;
	public GameObject enemydesc3;
	public GameObject attacktext;
	public Text multitext;
	public Image fade;
	public GameObject minicard1;
	public GameObject minicard2;
	public GameObject minicard3;
	public Text subtract;
	
	public Sprite sprite_blank;
	public Sprite sprite_freedom;
	public Sprite sprite_chains;
	public Sprite sprite_belief;
	public Sprite sprite_denial;
	public Sprite sprite_mercy;
	public Sprite sprite_power;
	
	private int enemies;
	private bool[] alive;
	private string[] cardstack;
	private int stacknum = 0;
	private int alivetotal;
	
	private Component[] buttons;
	
	private string[] enemyelements;
	
	private Dictionary<string, string> elementdesc = new Dictionary<string, string>();
	private Dictionary<string, string[]> elementadj = new Dictionary<string, string[]>();
	private Dictionary<int, string[]> enemydesc = new Dictionary<int, string[]>();
	
	private string currentcard = "";
	
	private int soul = 5;
	
	private string[] elements = {"FREEDOM", "CHAINS", "BELIEF", "DENIAL", "POWER", "MERCY"};
	
	
    // Start is called before the first frame update
    void Start()
    {
		int enemynum = StaticVariables.SceneEnemies;
		
		for (int i = 0; i < enemynum; i++)
		{
			float temp = Random.Range(0f, 1f);
			
			if (temp < 0.5f)
			{
				GameObject newmodel = Instantiate(lamp_prefab, enemygroup.transform);
				newmodel.name = "Enemy" + (i+1).ToString();
				
				float offset = 0;
				if (i == 1)
				{
					offset = 2;
				}
				
				newmodel.transform.localPosition = new Vector3((i*1.5f) - ((enemynum-1)*0.75f), 0, offset);
				
			} else if (temp >= 0.5f)
			{
				GameObject newmodel = Instantiate(ribbon_prefab, enemygroup.transform);
				newmodel.name = "Enemy" + (i+1).ToString();
				
				float offset = 0;
				if (i == 1)
				{
					offset = 3;
				}
				
				newmodel.transform.localPosition = new Vector3((i*1.5f) - ((enemynum-1)*0.75f), 0, offset);
				
			}
		}
		
		elementdesc.Add("FREEDOM", "The essense of unconstrained movement and action. \n\nWhile freedom is often thought of as a good thing, its essense can be described as difficult to predict and often rebellious.");
		elementdesc.Add("CHAINS", "The essense of constraints and moral weights. \n\nThis includes the many things which keep us anchored to reality, such as social bonds, logic, and fear of consequence.");
		elementdesc.Add("BELIEF", "The essense of faith and possibility. \n\nBelief encompasses the things we may not understand or comprehend which we assign meaning to. \n\nThis includes subjects which currently only exist in our imagination.");
		elementdesc.Add("DENIAL", "The essense of close-mindedness and despair. \n\nThis is a disturbingly powerful essense, capable of directing society towards lies and disaster. \n\nDenial is the true manifestation of our inner demons.");
		elementdesc.Add("POWER", "The essense of strength and domination. \n\nHealth, skill and strength are closely connected, although unchecked power will often result in a corruption of the mind and damage to other individuals.");
		elementdesc.Add("MERCY", "The essense of sacrifice and forgiveness. \n\nMercy is the rejection of power in order to avoid signifianct harm or destruction. \n\nThis is a rare essense to find in the world naturally.");
		
		elementadj.Add("FREEDOM", new string[] {"quick", "energetic", "curious", "agile", "fleeting", "puzzling"});
		elementadj.Add("CHAINS", new string[] {"restricted", "considerate", "cautious", "grounded", "wary", "insightful"});
		elementadj.Add("BELIEF", new string[] {"dogmatic", "creative", "inspired", "passionate", "pious", "spiritual"});
		elementadj.Add("DENIAL", new string[] {"apathetic", "antagonistic", "egotistical", "heartless", "harmful", "poisoned"});
		elementadj.Add("POWER", new string[] {"skilled", "elite", "mighty", "sturdy", "potent", "influential"});
		elementadj.Add("MERCY", new string[] {"apologetic", "graceful", "modest", "peaceful", "forgiven", "moral"});
		
        interpret.onClick.AddListener(delegate{Focus();});
		back.onClick.AddListener(GoBack);
		
		buttons = cardbuttons.GetComponentsInChildren<Button>();
		
		foreach (Button child in buttons)
		{
			child.onClick.AddListener(delegate{Click(child.gameObject);});
		}
		
		fade.gameObject.SetActive(true);
		var c = fade.color;
		fade.color = new Color(c.r, c.b, c.g, 1);
		fade.DOColor(new Color(c.r, c.b, c.g, 0), 0.5f).OnComplete(RemoveFade);
		
		soultext.text = "SOUL: " + soul.ToString();
		
		enemies = enemynum;
		alive = new bool[enemies];
		alivetotal = enemies;
		
		enemyelements = new string[enemies];
		cardstack = new string[enemies];
		
		int lasttemp = 0;
		
		for (int i = 0; i < enemies; i++)
		{
			string thiselement = elements[Random.Range(0, 5)];
			enemyelements[i] = thiselement;
			alive[i] = true;
			
			Debug.Log(enemyelements[i]);
			
			// get 2 random numbers between 0 and 5
			int a = Random.Range(0, 5);
			int b = a + Random.Range(1, 5);
			if (b > 5) b -= 6;
			
			Debug.Log(elementadj[enemyelements[i]][a] + ", " + elementadj[enemyelements[i]][b]);
			
			
			int temp;
			if (lasttemp == 0)
			{
				temp = Random.Range(1, 4);
				
			} else 
			{
				temp = lasttemp + Random.Range(1, 3);
				if (temp > 4) temp -= 4;
				
			}
			lasttemp = temp;
			
			string middletext = "";
			
			if (temp == 1)
			{
				middletext = "Seems to be ";
				
			} else if (temp == 2)
			{
				middletext = "Its essence suggests it is ";
				
			} else if (temp == 3)
			{
				middletext = "May perhaps be ";
			
			} else if (temp == 4)
			{
				middletext = "Possibly ";
				
			}
			
			
			if (enemygroup.transform.Find("Enemy" + (i+1).ToString()).Find("Lamp"))
			{
				Debug.Log("Found lamp");
				
				string enemytext = "A hostile spirit with the appearance of a lamppost. \n";
				string[] text = new string[] {enemytext, middletext, elementadj[enemyelements[i]][a], " and ", elementadj[enemyelements[i]][b], "."};
				enemydesc.Add(i, text);
			}
			
			if (enemygroup.transform.Find("Enemy" + (i+1).ToString()).Find("Ribbon"))
			{
				Debug.Log("Found ribbon");
				
				string enemytext = "A cursed artifact containing a devious spirit. \n";
				string[] text = new string[] {enemytext, middletext, elementadj[enemyelements[i]][a], " and ", elementadj[enemyelements[i]][b], "."};
				enemydesc.Add(i, text);
			}
		}
		
		
		float BGtemp = Random.Range(0, 3);
		
		if (BGtemp < 1)
		{
			Transform[] walls = background.GetComponentsInChildren<Transform>();
			
			foreach (Transform child in walls)
			{
				if (child.gameObject != background)
				{
					child.gameObject.GetComponent<MeshRenderer>().material = BG1;
				}
			}
			
		} else if (BGtemp < 2)
		{
			Transform[] walls = background.GetComponentsInChildren<Transform>();
			
			foreach (Transform child in walls)
			{
				if (child.gameObject != background)
				{
					child.gameObject.GetComponent<MeshRenderer>().material = BG2;
				}
			}
			
		} else
		{
			Transform[] walls = background.GetComponentsInChildren<Transform>();
			
			foreach (Transform child in walls)
			{
				if (child.gameObject != background)
				{
					child.gameObject.GetComponent<MeshRenderer>().material = BG3;
				}
			}
			
		}
    }
	
	
	
    // Update is called once per frame
    void Update()
    {
		Vector2 worldpos = camera.GetComponent<Camera>().WorldToScreenPoint(enemygroup.transform.position + new Vector3(0, -0.3f, 0));
		interpret.GetComponent<RectTransform>().anchoredPosition = new Vector2(worldpos.x, worldpos.y);
		
		worldpos = camera.GetComponent<Camera>().WorldToScreenPoint(enemygroup.transform.Find("Enemy1").position + new Vector3(0.2f, 1f, 0));
		minicard1.GetComponent<RectTransform>().anchoredPosition = new Vector2(worldpos.x, worldpos.y);
		
		if (enemies > 1)
		{
			worldpos = camera.GetComponent<Camera>().WorldToScreenPoint(enemygroup.transform.Find("Enemy2").position + new Vector3(0.2f, 1f, 0));
			minicard2.GetComponent<RectTransform>().anchoredPosition = new Vector2(worldpos.x, worldpos.y);
			
		}
		if (enemies > 2){
			worldpos = camera.GetComponent<Camera>().WorldToScreenPoint(enemygroup.transform.Find("Enemy3").position + new Vector3(0.2f, 1f, 0));
			minicard3.GetComponent<RectTransform>().anchoredPosition = new Vector2(worldpos.x, worldpos.y);
			
		}
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
		carddesc.transform.Find("BodyText").gameObject.GetComponent<Text>().text = elementdesc[name];
		
		//carddesc.GetComponent<Image>().color = newcolor;
		
		if (button.name == "FREEDOM"){
			carddesc.GetComponent<Image>().sprite = sprite_freedom;
			
		} else if (button.name == "CHAINS"){
			carddesc.GetComponent<Image>().sprite = sprite_chains;
			
		} else if (button.name == "BELIEF"){
			carddesc.GetComponent<Image>().sprite = sprite_belief;
			
		} else if (button.name == "DENIAL"){
			carddesc.GetComponent<Image>().sprite = sprite_denial;
			
		} else if (button.name == "MERCY"){
			carddesc.GetComponent<Image>().sprite = sprite_mercy;
			
		} else if (button.name == "POWER"){
			carddesc.GetComponent<Image>().sprite = sprite_power;
			
		}
		
		carddesc.SetActive(true);
    }
	
	
	void Click(GameObject button)
	{
		if (alivetotal == 1){
			int aliveindex = 0;
			
			while(alive[aliveindex] == false && aliveindex < enemies)
			{
				aliveindex++;
			}
			
			int KO = 0;
			int temp = aliveindex;
			int missed = -1;
			int damage = 0;
			
			if (currentcard == enemyelements[aliveindex]){
				alive[aliveindex] = false;
				alivetotal--;
				KO++;
				
				KOText(1, alivetotal);
				CriticalHit(aliveindex+1, true);
			} else
			{
				missed++;
				Miss(aliveindex+1);
				
				if (enemygroup.transform.Find("Enemy" + (aliveindex+1).ToString()).Find("Lamp"))
				{
					damage += 1;
					
				} else if (enemygroup.transform.Find("Enemy" + (aliveindex+1).ToString()).Find("Ribbon"))
				{
					damage += 2;
					
				}
			}
			
			currentcard = "";
			
			KOText(KO, temp);
				
			Debug.Log("missed: " + missed.ToString());
			
			if (missed != -1)
			{
				soultext.text = "SOUL: " + soul.ToString();
				soultext.gameObject.SetActive(true);
				soultext.GetComponent<RectTransform>().DOShakeAnchorPos(0.5f, new Vector2(40, 5), 15, 30, false, true);
				
				subtract.text = "-" + damage.ToString();
				subtract.color = new Color(1, 0, 0, 1);
				
				StartCoroutine(FadeColor());
			}
			
			
		} else if (alivetotal > 1) {
			
			cardstack[stacknum] = button.name;
			
			int aliveindex = stacknum;
			while(alive[aliveindex] == false)
			{
				aliveindex++;
			}
			
			
			//Color newcolor = button.GetComponent<Image>().color;
			
			Sprite newsprite = sprite_freedom;
			
			if (button.name == "CHAINS"){
				newsprite = sprite_chains;
				
			} else if (button.name == "BELIEF"){
				newsprite = sprite_belief;
				
			} else if (button.name == "DENIAL"){
				newsprite = sprite_denial;
				
			} else if (button.name == "MERCY"){
				newsprite = sprite_mercy;
				
			} else if (button.name == "POWER"){
				newsprite = sprite_power;
				
			}
			
			if (aliveindex == 0)
			{
				minicard1.transform.Find("Text").gameObject.GetComponent<Text>().text = "";
				minicard1.GetComponent<Image>().sprite = newsprite;
				
			} else if (aliveindex == 1){
				minicard2.transform.Find("Text").gameObject.GetComponent<Text>().text = "";
				minicard2.GetComponent<Image>().sprite = newsprite;
				
			} else if (aliveindex == 2){
				minicard3.transform.Find("Text").gameObject.GetComponent<Text>().text = "";
				minicard3.GetComponent<Image>().sprite = newsprite;
				
			}
			
			
			stacknum++;
			Debug.Log("stacknum: " + stacknum.ToString());
			Debug.Log("alivetotal: " + alivetotal.ToString());
			
			if (stacknum >= alivetotal)
			{
				currentcard = "";
				
				minicard1.SetActive(false);
				minicard2.SetActive(false);
				minicard3.SetActive(false);
				
				int stackindex = 0;
				int KO = 0;
				int temp = aliveindex;
				int missed = -1;
				int damage = 0;
				
				for (int i = 0; i < enemies; i++)
				{
					if (alive[i] == true)
					{
						Debug.Log(cardstack[stackindex] + " ? " + enemyelements[i]);
						
						if (cardstack[stackindex] == enemyelements[i])
						{
							alive[i] = false;
							alivetotal--;
							
							bool last = false;
							if (i == enemies-1) last = true;
							
							CriticalHit(i+1, last);
							KO++;
							
						} else {
							Miss(i+1);
							missed = i;
							
							if (enemygroup.transform.Find("Enemy" + (i+1).ToString()).Find("Lamp"))
							{
								damage += 1;
								
							} else if (enemygroup.transform.Find("Enemy" + (i+1).ToString()).Find("Ribbon"))
							{
								damage += 2;
								
							}
						}
						
						stackindex++;
					}
				}
				
				KOText(KO, temp);
				
				Debug.Log("missed: " + missed.ToString());
				
				if (missed != -1)
				{
					soultext.text = "SOUL: " + soul.ToString();
					soultext.gameObject.SetActive(true);
					soultext.GetComponent<RectTransform>().DOShakeAnchorPos(0.5f, new Vector2(40, 5), 15, 30, false, true);
					
					subtract.text = "-" + damage.ToString();
					subtract.color = new Color(1, 0, 0, 1);
					
					StartCoroutine(FadeColor());
					
					if (alivetotal == 1)
					{
						StartCoroutine(MoveToCenter(missed+1));
					}
				}
			}
			
		}
	}
	
	
	IEnumerator FadeColor()
	{
		playerbody.transform.DOShakePosition(0.5f, new Vector3(-0.5f, 0, 0), 15, 30, false, true);
		
		yield return new WaitForSeconds(1);
		subtract.DOColor(new Color(1, 0, 0, 0), 1.5f).SetEase(Ease.OutQuint);
		
	}
	
	IEnumerator MoveToCenter(int missed)
    {
		yield return new WaitForSeconds(0.5f);
		
		enemygroup.transform.Find("Enemy" + missed.ToString()).DOLocalMove(new Vector3(0,0,0), 1f).SetEase(Ease.OutQuint);
	}
	
	
	void KOText(int KO, int cap)
	{
		if (KO == 0)
		{
			attacktext.transform.Find("Text").gameObject.GetComponent<Text>().text = "There was no effect.";
			
		} else if (KO == 1) {
			attacktext.transform.Find("Text").gameObject.GetComponent<Text>().text = "Illusion demistified.";
			
		} else if (KO == cap-1) {
			attacktext.transform.Find("Text").gameObject.GetComponent<Text>().text = "All specters dissipated.";
				
		} else {
			attacktext.transform.Find("Text").gameObject.GetComponent<Text>().text = "Illusions demistified.";
		}
		
		attacktext.SetActive(true);
	}
	
	
	void HideZoomedUI()
	{
		cardbuttons.SetActive(false);
		back.gameObject.SetActive(false);
		carddesc.SetActive(false);
		enemydesc1.SetActive(false);
		enemydesc2.SetActive(false);
		enemydesc3.SetActive(false);
		multitext.gameObject.SetActive(false);
	}
	
	void CriticalHit(int i, bool last)
	{
		Debug.Log("Critical Hit");
		
		HideZoomedUI();
		ZoomOut();
		
		GameObject enemybody = enemygroup.transform.Find("Enemy" + i.ToString()).Find("Body").gameObject;
		Material m = enemybody.GetComponent<Renderer>().material;
		Color c = m.color;
		m.DOColor(new Color(c.r, c.b, c.g, 0), 1).SetEase(Ease.OutQuint);
		
		enemybody.transform.DOShakePosition(0.5f, new Vector3(0.5f, 0, 0), 15, 30, false, true);
		
		if (last == true && alivetotal == 0)
		{
			StartCoroutine(PauseEnd());
		} else if (last == true) {
			StartCoroutine(PauseReturn());
		}
	}
	
	void Miss(int i)
	{
		Debug.Log("Miss");
		
		HideZoomedUI();
		ZoomOut();
		
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
		
		yield return new WaitForSeconds(1);
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
	
	
	
	void ZoomIn()
	{
		camera.transform.DOMove(enemygroup.transform.position + new Vector3(2.2f, 2.5f, -11), 0.5f).SetEase(Ease.OutQuint);
		camera.transform.DORotate(new Vector3(3, 0, -3), 0.5f).SetEase(Ease.OutQuint);
	}
	
	void ZoomOut()
	{
		camera.transform.DOMove(new Vector3(0, 2, -10), 0.5f).SetEase(Ease.OutQuint);
		camera.transform.DORotate(new Vector3(3, 1, -5), 0.5f).SetEase(Ease.OutQuint);
	}
	
	
	void Focus()
	{
		ZoomIn();
		
		soultext.gameObject.SetActive(false);
		interpret.gameObject.SetActive(false);
		
		if (alivetotal > 1)
		{
			multitext.gameObject.SetActive(true);
			multitext.GetComponent<Text>().text = "SELECT " + alivetotal.ToString() + " CARDS";
		}
		
		alivetotal = 0;
		for (int i = 0; i < enemies; i++)
		{
			cardstack[i] = null;
			if (alive[i] == true) alivetotal++;
		}
		
		stacknum = 0;
		
		if (alivetotal == 1)
		{
			for (int i = 0; i < enemies; i++){
				if (alive[i] == true)
				{
					string newtext = "";
					string[] array = enemydesc[i];
					
					for (int x = 0; x < array.Length; x++)
					{
						newtext += array[x];
					}
					
					enemydesc1.transform.Find("Text").gameObject.GetComponent<Text>().text = newtext;
				}
			}
			
			enemydesc1.SetActive(true);
			
		} else if (alivetotal == 2)
		{
			int index = 1;
			
			for (int i = 0; i < enemies; i++){
				if (alive[i] == true)
				{
					string newtext = "";
					string[] array = enemydesc[i];
					
					for (int x = 0; x < array.Length; x++)
					{
						newtext += array[x];
					}
					
					enemydesc2.transform.Find("Desc" + index.ToString()).Find("Text").gameObject.GetComponent<Text>().text = newtext;
					index++;
				}
			}
			
			enemydesc2.SetActive(true);
			
		} else if (alivetotal == 3)
		{
			int index = 1;
			
			for (int i = 0; i < enemies; i++){
				if (alive[i] == true)
				{
					string newtext = "";
					string[] array = enemydesc[i];
					
					for (int x = 0; x < array.Length; x++)
					{
						newtext += array[x];
					}
					
					enemydesc3.transform.Find("Desc" + index.ToString()).Find("Text").gameObject.GetComponent<Text>().text = newtext;
					index++;
				}
			}
			
			enemydesc3.SetActive(true);
			
		}
		
		minicard1.GetComponent<Image>().sprite = sprite_blank;
		minicard2.GetComponent<Image>().sprite = sprite_blank;
		minicard3.GetComponent<Image>().sprite = sprite_blank;
		
		if (alivetotal > 1){
			int aliveindex = 1;
			
			if (alive[0] == true)
			{
				minicard1.transform.Find("Text").gameObject.GetComponent<Text>().text = aliveindex.ToString();
				minicard1.SetActive(true);
				aliveindex++;
			}
			
			if (enemies > 1 && alive[1] == true)
			{
				minicard2.transform.Find("Text").gameObject.GetComponent<Text>().text = aliveindex.ToString();
				minicard2.SetActive(true);
				aliveindex++;
			}
			
			if (enemies > 2 && alive[2] == true)
			{
				minicard3.transform.Find("Text").gameObject.GetComponent<Text>().text = aliveindex.ToString();
				minicard3.SetActive(true);
				aliveindex++; // in case more enemies are added
			}
		}
		
		
		cardbuttons.SetActive(true);
		back.gameObject.SetActive(true);
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
		enemydesc1.SetActive(false);
		enemydesc2.SetActive(false);
		enemydesc3.SetActive(false);
		multitext.gameObject.SetActive(false);
		minicard1.SetActive(false);
		minicard2.SetActive(false);
		minicard3.SetActive(false);
		
		currentcard = "";
	}
	
}
