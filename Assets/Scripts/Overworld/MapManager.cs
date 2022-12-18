﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class MapManager : MonoBehaviour
{
    public OverworldGUI overworldGUI;
    public WorldViewManager worldView;

    //Pin Information//
	public OverworldLevelPin startPin;
	public OverworldLevelPin targetPin;
    public OverworldLevelPin previousPin;
    public List<OverworldLevelPin> levelPins;
    public List<OverworldGate> overworldGates;

    //Player Information//
    public OverworldPlayer player;
    public OverworldFollowCamera followCamera;
    public ScriptablePlayerSettings settings;
    public ResolutionOptions resolution;

    //UI Information//
    public Button backButton;
    public Button playButton;
    public Slider musicSlider;
    public Slider soundSlider;

    //Sound Information//
    public AudioClip backsound;
    public AudioClip playsound;
    public GameSoundManagement soundManagement;
    public MusicManagement musicManagement;
    public OverworldMusicSelector overworldMusic;

    private void Start ()
	{
        GameControl.control.AutoLoadCheck();

        soundManagement = FindObjectOfType<GameSoundManagement>();
        musicManagement = FindObjectOfType<MusicManagement>();

        soundManagement.slider = soundSlider;
        musicManagement.slider = musicSlider;

        SetSlidersFromSettings();
        SetResolutionFromSettings();

        if (GameControl.control.lastSceneWasLevel)
        {
            GameControl.control.savedOverworldPlayerPosition = levelPins[GameControl.control.savedPinID - 1].transform.position + new Vector3(0, -2f, 0);
            GameControl.control.lastSceneWasLevel = false;
        }

        if (GameControl.control.savedPin != null)
        {
            startPin = GameControl.control.savedPin;
        }

        musicManagement.onLevelStart.Invoke();

        InitializeLevelState();
        UpdateOverworldMusic(GameControl.control.overworldMusicProgress);
        UpdateWorldView(GameControl.control.currentWorldView);
        UpdatePlayerPosition();
    }

    public void UpdateOverworldMusic(int index)
    {
        overworldMusic.currentProgress = index;
        overworldMusic.overworldmusicCheck();
    }

    public void UpdateWorldView(int index)
    {
        worldView.UpdateDrawnObjects(index);
        followCamera.overworldCamera.backgroundColor = GameControl.control.backgroundColor;
    }

    private void SetResolutionFromSettings()
    {
        resolution.SetResolution(settings.resolutionWidth, settings.resolutionHeight);
    }

    private void SetSlidersFromSettings()
    {
        musicSlider.value = settings.bgmVolume;
        soundSlider.value = settings.sfxVolume;
    }

    public void InitializeLevelState()
    {
        for (int i = 1; i < GameControl.control.completedlevels.Count - 1; i++)
        {
            bool isComplete = GameControl.control.completedlevels[i];
            bool isGolden = GameControl.control.goldenpellets[i];
            bool isTimer = GameControl.control.timerchallenge[i];
        }
    }

    public void UpdatePlayerPosition()
    {
        player.gameObject.transform.position = GameControl.control.savedOverworldPlayerPosition + new Vector3(0, -2f, 0);
        followCamera.gameObject.transform.position = GameControl.control.savedCameraPosition + new Vector3(0, -2f, 0);
    }

    public void UpdateWorldGates()
    {
        foreach (OverworldGate gate in overworldGates)
        {
            gate.SetPlateState(false);
            gate.SetGateState();
        }
    }
    public void SetWorldGateData(List<bool> lockedgates, List<bool> destroyedgates)
    {
        //for (int i = 0; i < worldGates.Count; i++)
        //{
        //    worldGates[i].locked = lockedgates[i];
        //    worldGates[i].destroyed = destroyedgates[i];
        //    worldGates[i].OnLevelLoaded.Invoke();
        //}
    }

    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene($"Level{levelNumber + 1}");
    }

    public void LoadLevelFromSceneIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
    
    public void SetSavedOverworldPlayerPosition(OverworldPlayer character)
    {
        GameControl.control.savedOverworldPlayerPosition = character.currentPin.transform.position + Vector3.down * 2f;
        //GameControl.control.savedCameraPosition = overworldCamera.transform.position;
        //GameControl.control.savedOrtographicSize = overworldCamera.gameCamera.orthographicSize;
    }

    public void UpdateLevelPinProgress()
    {
        foreach (OverworldLevelPin pin in levelPins)
        {
            pin.view.ViewProgressCheck();
        }
    }
}
