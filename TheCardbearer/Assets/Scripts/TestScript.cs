using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestScript : MonoBehaviour
{
	Tween myTween;
	Material m;
	
    // Start is called before the first frame update
    void Start()
    {
		m = GetComponent<Renderer>().material;
		
		myTween = transform.DOMove(new Vector3(2,2,2), 0.5f)
		.SetEase(Ease.OutQuint)
		.SetLoops(4)
		.OnComplete(myFunction);
		
		m.DOColor(Color.red, 0.5f).SetAs(myTween);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void myFunction()
	{
		Debug.Log("A");
		myTween.Rewind();
	}
	
}
