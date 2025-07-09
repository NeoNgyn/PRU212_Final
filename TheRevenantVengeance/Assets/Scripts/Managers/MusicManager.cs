using UnityEngine;
using UnityEngine.SceneManagement; // ?? bi?t khi n�o Scene thay ??i

public class MusicManager : MonoBehaviour
{
	public static MusicManager instance; // Singleton pattern ?? ??m b?o ch? c� 1 MusicManager

	[SerializeField] private AudioClip backgroundMusic;
	private AudioSource audioSource;

	void Awake()
	{
		// Th?c hi?n Singleton pattern
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject); // R?t quan tr?ng: Gi? GameObject n�y t?n t?i qua c�c Scene
			audioSource = GetComponent<AudioSource>();

			if (audioSource == null)
			{
				audioSource = gameObject.AddComponent<AudioSource>();
			}

			// Thi?t l?p AudioSource cho nh?c n?n
			audioSource.clip = backgroundMusic;
			audioSource.loop = true; // L?p l?i li�n t?c
			audioSource.playOnAwake = false; // Ch�ng ta s? t? ?i?u khi?n vi?c ph�t

			// ??ng k� s? ki?n khi Scene ???c load
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
		else
		{
			// N?u ?� c� m?t MusicManager kh�c, h?y b? c�i m?i n�y
			Destroy(gameObject);
		}
	}

	void OnDestroy()
	{
		// H?y ??ng k� s? ki?n khi GameObject n�y b? h?y
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// Khi m?t Scene m?i ???c load
		// B?n c� th? th�m logic ? ?�y ?? ki?m tra Scene n�o n�n ph�t nh?c
		// ho?c d?ng nh?c n?u ?� l� Scene menu, v.v.

		// V� d?: Ph�t nh?c n?u ch?a ph�t
		if (!audioSource.isPlaying)
		{
			audioSource.Play();
		}

		// V� d?: D?ng nh?c n?u v�o Scene "MainMenu"
		// if (scene.name == "MainMenu")
		// {
		//     audioSource.Stop();
		// }
		// else if (!audioSource.isPlaying) // Ph�t n?u kh�ng ph?i MainMenu v� ch?a ph�t
		// {
		//     audioSource.Play();
		// }
	}

	// C�c h�m c�ng c?ng ?? ?i?u khi?n nh?c t? c�c script kh�c
	public void PlayMusic()
	{
		if (audioSource != null && !audioSource.isPlaying)
		{
			audioSource.Play();
		}
	}

	public void StopMusic()
	{
		if (audioSource != null && audioSource.isPlaying)
		{
			audioSource.Stop();
		}
	}

	public void SetVolume(float volume)
	{
		if (audioSource != null)
		{
			audioSource.volume = volume;
		}
	}
}