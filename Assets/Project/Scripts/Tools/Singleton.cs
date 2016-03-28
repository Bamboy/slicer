using UnityEngine;
using System.Collections;

/// 
/// MONOBEHAVIOR PSEUDO SINGLETON ABSTRACT CLASS
/// usage : best is to be attached to a gameobject but if not that is ok, this will create one on first access
/// example : '''public sealed class MyClass : Singleton {'''
/// references : http://tinyurl.com/d498g8c
///            : http://tinyurl.com/cc73a9h
///            : http://unifycommunity.com/wiki/index.php?title=Singleton
/// 

public abstract class Singleton<T> : MonoBehaviour
	where T : MonoBehaviour {

	private static T _instance = null;

	/// 
	/// gets the instance of this Singleton
	/// use this for all instance calls:
	/// MyClass.Instance.MyMethod();
	/// or make your public methods static
	/// and have them use Instance
	/// 

	public static T Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType <T> ();
				if (_instance == null) {
					GameObject obj = new GameObject ();
					obj.name = typeof(T).ToString ();
					_instance = obj.AddComponent<T> ();
				}
			}
			return _instance;
		}
	}

	public virtual void OnApplicationQuit () {
		// release reference on exit
		_instance = null;
	}

	public virtual void Awake () {
		if (this.gameObject.transform.parent) {
			DontDestroyOnLoad (this.gameObject.transform.parent);
		} else {
			DontDestroyOnLoad (this.gameObject);
		}

		if (_instance == null) {
			_instance = this as T;
		} else {
			Destroy (gameObject);
		}
	}
}