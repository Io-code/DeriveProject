using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public UIData uidata;
	public AudioMixerGroup mixer;
	public Sound[] sounds;

	private void Awake()
	{
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.playOnAwake = false;
			s.source.outputAudioMixerGroup = mixer;
			s.source.clip = s.clip;

			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
	}

	private void Start()
	{
		StartCoroutine(AwakeSound());
	}

	private IEnumerator AwakeSound()
	{
		yield return new WaitForEndOfFrame();
		Debug.Log(sounds[2]);
		if (!uidata.inGame) sounds[2].Play(); //Jouer la musique du menu d'accueil
		else
		{
			sounds[0].Play();
			//Jouer la musique de readyGo
			yield return new WaitUntil(() => sounds[0].IsPlaying() == false);
			//Jouer la musique de fond
			sounds[1].Play();
		}
	}

	public void Play(string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if (s == null) return;

		s.source.Play();
	}
}

[System.Serializable]
public class Sound
{
	public string name;
	public AudioClip clip;

	[HideInInspector] public AudioSource source;

	public float volume;
	public float pitch;

	public bool loop;

	private bool isPlaying;

	public void Play()
	{
		source.Play();
	}

	public bool IsPlaying()
	{
		return source.isPlaying;
	}
}
