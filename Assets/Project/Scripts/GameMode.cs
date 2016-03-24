using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GameMode {
	public string name;
	public GameObject[] displayObjects;
	public AudioClip[] backgroundMusic;

	bool active = false;

	public GameMode(string name) {
		this.name = name;
	}

	public virtual void Initialize() { }
	public virtual void Update() { }

	public virtual void Activate() {
		active = true;

		ToggleDisplayObjects (true);
		AudioManager.Instance.PlayBackgroundMusic (backgroundMusic);
	}

	public virtual void Deactivate() {
		active = false;

		ToggleDisplayObjects (false);
	}

	public void ToggleDisplayObjects(bool isOn) {
		if (displayObjects.Length > 0) {
			foreach (GameObject obj in displayObjects) {
				obj.SetActive (isOn);
			}
		}
	}

	public void BindButton(Button button, System.Action callback) {
		button.onClick.AddListener (delegate {
			if (active) {
				callback();
			}
		});
	}
}