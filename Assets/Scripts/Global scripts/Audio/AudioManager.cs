using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[Serializable]
public struct Clip
{
    public AudioClip playerTurnClip;
    public AudioClip IATurnClip;
}
public enum MeowType
{
    women,
    men
}
public class AudioManager : MonoBehaviour
{
    public float transitionWait = 1;
    public Clip[] musicClips;

    public static AudioManager audioManager;

    private AudioSource music;
    public AudioSource transition;
    private bool playerTurn = false;
    private float prevVolume;
    private bool transitioning;

    public AudioClip[] meowWomen;
    public AudioClip[] meowMen;

    private AudioClip prevMeow;
    public AudioClip GetAudioClip(MeowType m)
    {
        if (m == MeowType.women)
            return GetRandomClip(meowWomen);

        return GetRandomClip(meowMen);
    }

    private AudioClip GetRandomClip(AudioClip[] meowArray)
    {
        AudioClip currentMeow;
        do
            currentMeow = meowArray[Random.Range(0, meowArray.Length)];
        while (currentMeow == prevMeow);

        prevMeow = currentMeow;
        return currentMeow;
    }
    private void Awake()
    {
        if (audioManager != null)
        {
            Destroy(gameObject);
        }
        else
        {
            audioManager = this;
        }
    }
    private void Start()
    {
        music = GetComponent<AudioSource>();
        music.loop = true;
    }

    private void Update()
    {
        if ((!playerTurn && TurnManager.TeamTurn == Team.TeamPlayer) || (playerTurn && TurnManager.TeamTurn == Team.TeamAI))
        {
            transitioning = true;
            StartCoroutine(ChangeTurn());
        }
        if (music.volume == 0 && !transitioning)
        {
            StopAllCoroutines();
            StartCoroutine(FadeIn(transitionWait, prevVolume));
        }
    }

    private IEnumerator ChangeTurn()
    {
        playerTurn = !playerTurn;
        //transition.Play();
        StartCoroutine(FadeOut(transitionWait));
        prevVolume = music.volume;

        yield return new WaitForSeconds(transitionWait);

        if (playerTurn)
        {
            music.clip = musicClips[0].playerTurnClip;
        }
        else
        {
            music.clip = musicClips[0].IATurnClip;
        }
        music.Play();
        StartCoroutine(FadeIn(transitionWait, prevVolume));
        transitioning = false;
    }
    private IEnumerator FadeIn(float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            music.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);

            if (start != prevVolume) //someone change the settings transitioning
                prevVolume = start;
            yield return null;
        }
        yield break;
    }
    private IEnumerator FadeOut(float duration)
    {
        float start = music.volume;
        while (music.volume > 0)
        {
            music.volume -= start * Time.deltaTime / duration;
            if (start != prevVolume) //someone change the settings transitioning
                prevVolume = start;
            yield return null;
        }
        yield break;
    }
    private void ChageVolume(float value)
    {
        
    }
}
