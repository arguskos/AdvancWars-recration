using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class TestINjection : MonoBehaviour, IHighlightable{
    Highlighter.HighlighterFactory highlightFactory;


    [Inject]
    public void Construct(Highlighter.HighlighterFactory highlighterFactory)
    {
        this.highlightFactory = highlighterFactory;
    }
    public void Highlight()
    {
        highlightFactory.Create();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
