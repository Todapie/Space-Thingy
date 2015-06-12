using UnityEngine;
using System.Collections;

public class AudioToggle : MonoBehaviour {
	
	public void AudioToggleCheck(bool Toggle) {
		if (Toggle) {
			if (GetComponent<AudioSource> ().isPlaying) {
				GetComponent<AudioSource> ().Stop ();
			} else {
				GetComponent<AudioSource> ().Play ();
			}
		}
	}
}
