using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour {

	public KeyCode continueKey;

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (continueKey)) {
			// TODO: Go to next scen
		}
	}
}
