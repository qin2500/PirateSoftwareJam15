using System.Xml.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;


    public void SetMasterVolume(float level){
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level)*20f );
    }
    public void SetSFXVolume(float level){
        audioMixer.SetFloat("SFXVolume",  Mathf.Log10(level)*20f);
    }
    public void SetMusicVolume(float level){
        audioMixer.SetFloat("musicVolume",  Mathf.Log10(level)*20f);
    }


}
