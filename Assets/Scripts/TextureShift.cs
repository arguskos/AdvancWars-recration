using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureShift : MonoBehaviour {
    Renderer r;
    Material m;
    float offset = 0;
	// Use this for initialization
	void Start () {
        r = GetComponent<Renderer>();
        m = r.material;
	}
	
	// Update is called once per frame
	void Update () {
        offset += Time.deltaTime;
        m.SetTextureOffset("_MainTex", new Vector2(offset, offset));
    }
}
