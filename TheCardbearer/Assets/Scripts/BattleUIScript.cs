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
	
	public Material lamp_fade;
	public Material ribbon_fade;
	public Material cloak_fade;
	
	public GameObject lamp_prefab;
	public GameObject ribbon_prefab;
	
	public GameObject camera;
	public GameObject enemygroup;
	public GameObject background;
	public GameObject player;
	public GameObject playerbody;
	public GameObject armpivotL;
	public GameObject armpivotR;
	public GameObject armL;
	public GameObject armR;
	public GameObject cardpack;
	
	public GameObject freedomP;
	public GameObject chainsP;
	public GameObject beliefP;
	public GameObject denialP;
	public GameObject mercyP;
	public GameObject powerP;
	public GameObject starP;
	public GameObject shadowP;
	
	private AudioSource source;
	public AudioClip hit;
	public AudioClip shuffle;
	public AudioClip snap;
	public AudioClip thwip;
	
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
	public Sprite sprite_star;
	public Sprite sprite_shadow;
	
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
	
	private string[] elements = {"FREEDOM", "CHAINS", "BELIEF", "DENIAL", "POWER", "MERCY", "STAR", "SHADOW"};
	
	
    // Start is called before the first frame update
    void Start()
    {
		RaiseHand();
		
		source = camera.GetComponent<AudioSource>();
		source.volume = 0;
		source.DOFade(1, 0.8f);
		
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
		elementdesc.Add("STAR", "The essense of guidance and illumination. \n\nThe light of the stars brightens our world and has traditionally been useful for navigation. \n\nWhen in need of guidance, look to the heavens.");
		elementdesc.Add("SHADOW", "The essence of obscurity and mystery. \n\nThis essence manifests itself in things we overlook, cannot see or do not understand. \n\nThe things which we lack understanding of are often the most important.");
		
		elementadj.Add("FREEDOM", new string[] {"quick", "energetic", "curious", "agile", "fleeting", "puzzling"});
		elementadj.Add("CHAINS", new string[] {"restricted", "considerate", "cautious", "grounded", "wary", "steady"});
		elementadj.Add("BELIEF", new string[] {"dogmatic", "creative", "inspired", "passionate", "pious", "spiritual"});
		elementadj.Add("DENIAL", new string[] {"apathetic", "antagonistic", "egotistical", "heartless", "harmful", "poisoned"});
		elementadj.Add("POWER", new string[] {"skilled", "elite", "mighty", "sturdy", "potent", "influential"});
		elementadj.Add("MERCY", new string[] {"apologetic", "graceful", "modest", "peaceful", "forgiven", "moral"});
		elementadj.Add("STAR", new string[] {"insightful", "bright", "resourceful", "shining", "visible", "radiant"});
		elementadj.Add("SHADOW", new string[] {"hidden", "unseen", "unidentified", "dark", "concealed", "secretive"});
		
        interpret.onClick.AddListener(Focus);
		back.onClick.AddListener(GoBack);
		
		buttons = cardbuttons.GetComponentsInChildren<Button>();
		
		foreach (Button child in buttons)
		{
			child.onClick.AddListener(delegate{Click(child.gameObject);});
		}
		
		fade.gameObject.SetActive(true);
		var c = fade.color;
		fade.color = new Color(c.r, c.b, c.g, 1);
		fade.DOColor(new Color(c.r, c.b, c.g, 0), 0.8f).OnComplete(RemoveFade);
		
		soultext.text = "SOUL: " + soul.ToString();
		
		enemies = enemynum;
		alive = new bool[enemies];
		alivetotal = enemies;
		
		enemyelements = new string[enemies];
		cardstack = new string[enemies];
		
		int lasttemp = 0;
		
		for (int i = 0; i < enemies; i++)
		{
			string thiselement = elements[Random.Range(0, 7)];
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
    void FixedUpdate()
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
		
		for (int i = 1; i <= enemies; i++)
		{
			if (alive[i-1] == true)
			{
				GameObject enemybody = enemygroup.transform.Find("Enemy" + i.ToString()).Find("Body").gameObject;
				
				enemybody.transform.Translate(new Vector3(0, Mathf.Cos((Time.frameCount * 0.05f) + ((enemybody.transform.position.x+8)*3)) * 0.005f, 0), Space.World);
			}
		}
    }
	
	
	
	void RemoveFade()
	{
		fade.gameObject.SetActive(false);
	}
	
	void EndGame()
	{
		LowerHands();
		
		source.DOFade(0, 0.8f);
		
		fade.gameObject.SetActive(true);
		
		var c = fade.color;
		fade.DOColor(new Color(c.r, c.b, c.g, 1), 0.8f).OnComplete(NextScene);
	}
	
	void NextScene()
	{
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}
	
	
	public void OnMouseOver(GameObject button)
    {
		source.PlayOneShot(hit, 1);
		
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
			
		} else if (button.name == "STAR"){
			carddesc.GetComponent<Image>().sprite = sprite_star;
			
		} else if (button.name == "SHADOW"){
			carddesc.GetComponent<Image>().sprite = sprite_shadow;
			
		}
		
		carddesc.SetActive(true);
    }
	
	
	void Click(GameObject button)
	{
		source.PlayOneShot(snap, 1);
		
		
		if (alivetotal == 1){
			HideZoomedUI();
			ZoomOut();
			Strike();
			
			int aliveindex = 0;
			
			while(alive[aliveindex] == false && aliveindex < enemies)
			{
				aliveindex++;
			}
			
			int KO = 0;
			int temp = aliveindex;
			int missed = -1;
			int damage = 0;
			
			if (currentcard == enemyelements[aliveindex])
			{
				alive[aliveindex] = false;
				alivetotal--;
				KO++;
				
				KOText(1, alivetotal);
				CriticalHit(aliveindex+1, true, currentcard);
			}
			else
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
				soul -= damage;
				
				soul = Mathf.Clamp(soul, 0, 10);
				
				soultext.text = "SOUL: " + soul.ToString();
				soultext.gameObject.SetActive(true);
				soultext.GetComponent<RectTransform>().DOShakeAnchorPos(0.5f, new Vector2(40, 5), 15, 30, false, true);
				
				subtract.text = "-" + damage.ToString();
				subtract.color = new Color(1, 0, 0, 1);
				
				StartCoroutine(FadeColor());
				
				if (soul <= 0)
				{
					StartCoroutine(Lose());
				}
				else
				{
					StartCoroutine(PauseReturn());
				}
				
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
				
			} else if (button.name == "STAR"){
				newsprite = sprite_star;
				
			} else if (button.name == "STAR"){
				newsprite = sprite_star;
				
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
				
				HideZoomedUI();
				ZoomOut();
				Strike();
				
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
							
							CriticalHit(i+1, last, cardstack[stackindex]);
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
					soul -= damage;
					
					soul = Mathf.Clamp(soul, 0, 10);
					
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
					
					if (soul <= 0)
					{
						StartCoroutine(Lose());
					} else {
						StartCoroutine(PauseReturn());
					}
				}
			}
			
		}
	}
	
	
	IEnumerator FadeColor()
	{
		player.transform.DOShakePosition(0.5f, new Vector3(-0.5f, 0, 0), 15, 30, false, true);
		
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
	
	void CriticalHit(int i, bool last, string cardtype)
	{
		Debug.Log("Critical Hit");
		
		GameObject enemybody = enemygroup.transform.Find("Enemy" + i.ToString()).Find("Body").gameObject;
		
		if (enemygroup.transform.Find("Enemy" + i.ToString()).Find("Lamp"))
		{
			enemybody.GetComponent<MeshRenderer>().material = lamp_fade;
			
		} else if (enemygroup.transform.Find("Enemy" + i.ToString()).Find("Ribbon"))
		{
			enemybody.GetComponent<MeshRenderer>().material = ribbon_fade;
			
		}
		
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
		
		
		GameObject temp;
		if (cardtype == "FREEDOM")
		{
			temp = Instantiate(freedomP, enemybody.transform.position, enemybody.transform.rotation);
		}
		else if (cardtype == "CHAINS")
		{
			temp = Instantiate(chainsP, enemybody.transform.position, enemybody.transform.rotation);
		}
		else if (cardtype == "BELIEF")
		{
			temp = Instantiate(beliefP, enemybody.transform.position, enemybody.transform.rotation);
		}
		else if (cardtype == "DENIAL")
		{
			temp = Instantiate(denialP, enemybody.transform.position, enemybody.transform.rotation);
		}
		else if (cardtype == "MERCY")
		{
			temp = Instantiate(mercyP, enemybody.transform.position, enemybody.transform.rotation);
		}
		else if (cardtype == "POWER")
		{
			temp = Instantiate(powerP, enemybody.transform.position, enemybody.transform.rotation);
		}
		else if (cardtype == "STAR")
		{
			temp = Instantiate(starP, enemybody.transform.position, enemybody.transform.rotation);
		}
		else if (cardtype == "SHADOW")
		{
			temp = Instantiate(shadowP, enemybody.transform.position, enemybody.transform.rotation);
		}
	}
	
	void Miss(int i)
	{
		Debug.Log("Miss");
	}
	
	
	IEnumerator Lose()
    {
		LowerHands();
		
		yield return new WaitForSeconds(0.5f);
		
		
		cardpack.SetActive(false);
		
		playerbody.GetComponent<MeshRenderer>().material = cloak_fade;
		armL.GetComponent<MeshRenderer>().material = cloak_fade;
		armR.GetComponent<MeshRenderer>().material = cloak_fade;
		
		Material m1 = playerbody.GetComponent<Renderer>().material;
		Color c1 = m1.color;
		m1.DOColor(new Color(c1.r, c1.b, c1.g, 0), 1).SetEase(Ease.OutQuint);
		
		Material m2 = armL.GetComponent<Renderer>().material;
		Color c2 = m2.color;
		m2.DOColor(new Color(c2.r, c2.b, c2.g, 0), 1).SetEase(Ease.OutQuint);
		
		Material m3 = armR.GetComponent<Renderer>().material;
		Color c3 = m3.color;
		m3.DOColor(new Color(c3.r, c3.b, c3.g, 0), 1).SetEase(Ease.OutQuint);
		
		
		yield return new WaitForSeconds(0.5f);
		
		attacktext.transform.Find("Text").gameObject.GetComponent<Text>().text = "Your soul has been depleted.";
		
		yield return new WaitForSeconds(1);
		EndGame();
    }
	
	IEnumerator PauseEnd()
	{
		yield return new WaitForSeconds(1);
		
		LowerHands();
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
		
		RaiseHand();
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
		source.PlayOneShot(thwip, 1);
		
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
		source.PlayOneShot(shuffle, 1);
		
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
	
	
	void RaiseHand()
	{
		float tweentime = 0.5f;
		
		player.transform.DORotate(new Vector3(0, 10, 0), tweentime);
		
		armpivotR.transform.DOLocalRotate(new Vector3(0, 0, 10), tweentime);
		
		armpivotL.transform.DOLocalRotate(new Vector3(0, 0, -45), tweentime);
		
		cardpack.transform.DOScale(new Vector3(0.03f, 0.03f, 0.03f), tweentime);
	}
	
	void Strike()
	{
		float tweentime = 0.5f;
		
		cardpack.transform.localScale = new Vector3(0,0,0);
		
		armpivotR.transform.eulerAngles = new Vector3(0, 0, -60);
		
		armpivotL.transform.eulerAngles = new Vector3(0, 0, 10);
		
		player.transform.DORotate(new Vector3(0, 30, 0), tweentime).SetEase(Ease.OutQuint);
		
	}
	
	void LowerHands()
	{
		float tweentime = 0.5f;
		
		armpivotR.transform.DOLocalRotate(new Vector3(0, 0, 0), tweentime);
		
		armpivotL.transform.DOLocalRotate(new Vector3(0, 0, 0), tweentime);
	}
	
}
