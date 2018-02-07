using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent : MonoBehaviour {

	public static GameObject parent;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (transform.gameObject);
		parent = transform.gameObject;
	}
	
}
