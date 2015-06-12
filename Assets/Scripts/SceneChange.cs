using UnityEngine;
using System.Collections;

public class SceneChange : MonoBehaviour {

	// Update is called once per frame
	public void ChangeToScene (int sceneToChangeTo) {
		Application.LoadLevel (sceneToChangeTo);
	}
}
