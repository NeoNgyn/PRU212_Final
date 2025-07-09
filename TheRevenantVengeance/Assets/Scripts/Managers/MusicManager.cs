using UnityEngine;
using UnityEngine.SceneManagement; // ?? bi?t khi nào Scene thay ??i

public class MusicManager : MonoBehaviour
{
	public static MusicManager instance; // Singleton pattern ?? ??m b?o ch? có 1 MusicManager

	[SerializeField] private AudioClip backgroundMusic;
	private AudioSource audioSource;

	void Awake()
	{
		// Th?c hi?n Singleton pattern
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject); // R?t quan tr?ng: Gi? GameObject này t?n t?i qua các Scene
			audioSource = GetComponent<AudioSource>();

			if (audioSource == null)
			{
				audioSource = gameObject.AddComponent<AudioSource>();
			}

			// Thi?t l?p AudioSource cho nh?c n?n
			audioSource.clip = backgroundMusic;
			audioSource.loop = true; // L?p l?i liên t?c
			audioSource.playOnAwake = false; // Chúng ta s? t? ?i?u khi?n vi?c phát

			// ??ng ký s? ki?n khi Scene ???c load
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
		else
		{
			// N?u ?ã có m?t MusicManager khác, h?y b? cái m?i này
			Destroy(gameObject);
		}
	}

	void OnDestroy()
	{
		// H?y ??ng ký s? ki?n khi GameObject này b? h?y
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// Khi m?t Scene m?i ???c load
		// B?n có th? thêm logic ? ?ây ?? ki?m tra Scene nào nên phát nh?c
		// ho?c d?ng nh?c n?u ?ó là Scene menu, v.v.

		// Ví d?: Phát nh?c n?u ch?a phát
		if (!audioSource.isPlaying)
		{
			audioSource.Play();
		}

		// Ví d?: D?ng nh?c n?u vào Scene "MainMenu"
		// if (scene.name == "MainMenu")
		// {
		//     audioSource.Stop();
		// }
		// else if (!audioSource.isPlaying) // Phát n?u không ph?i MainMenu và ch?a phát
		// {
		//     audioSource.Play();
		// }
	}

	// Các hàm công c?ng ?? ?i?u khi?n nh?c t? các script khác
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