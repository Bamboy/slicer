using UnityEngine;
using System.Collections;

public class AudioManager : Singleton<AudioManager> {
	AudioSource bgAudioSource;

	public override void Awake() {
		base.Awake ();
		bgAudioSource = GetComponent<AudioSource>();
	}

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

	public override void OnApplicationQuit() {
		StopBackgroundMusic ();
		base.OnApplicationQuit();
	}
}
