using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	#region Singleton Initialize
	private static AudioManager instance;
	public static AudioManager Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<AudioManager> ();
				if (instance == null) {
					GameObject obj = new GameObject ();
					obj.name = "Audio Manager";
					instance = obj.AddComponent<AudioManager> ();
				}
			}
			return instance;
		}
	}

	public virtual void Awake ()
	{
		DontDestroyOnLoad (this.gameObject);
		if (instance == null) {
			instance = this as AudioManager;
		} else {
			Destroy (gameObject);
		}

		#region Audio Initialize
		bgAudioSource = GetComponent<AudioSource>();
		#endregion
	}
	#endregion

	AudioSource bgAudioSource;

	public void PlayBackgroundMusic(AudioClip[] musicArray) {
		if (musicArray.Length > 0) {
			AudioClip randomMusic = musicArray [Random.Range (0, musicArray.Length)];
			PlayBackgroundMusic (randomMusic);
		}
	}

	public void PlayBackgroundMusic(AudioClip music) {
		StartCoroutine (_PlayBackgroundMusic (music));
	}

	IEnumerator _PlayBackgroundMusic(AudioClip music) {
		bgAudioSource.clip = music;
		bgAudioSource.Play ();

		while (!bgAudioSource.isPlaying) {
			bgAudioSource.clip = music;
			bgAudioSource.Play ();

			yield return new WaitForSeconds (0.5f);
		}
	}

	public void StopBackgroundMusic() {
		bgAudioSource.Stop ();
	}

	void OnApplicationQuit() {
		StopBackgroundMusic ();
	}
}
