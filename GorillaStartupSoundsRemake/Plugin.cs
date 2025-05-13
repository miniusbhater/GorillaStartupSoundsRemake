using System;
using System.IO;
using BepInEx;
using UnityEngine;
using UnityEngine.UIElements;
using Utilla;

namespace GorillaStartupSoundsRemake
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;
        private AudioSource GSSAudio;
        

        void Start()
        {
            Logger.LogInfo("GorillaStartupSoundsRemake");
            Logger.LogInfo("Creating game object");
            GameObject obj = new GameObject("GorillaStartupSounds");
            GSSAudio = obj.AddComponent<AudioSource>();
            DontDestroyOnLoad(obj);

            string sound = Path.Combine(Paths.PluginPath, "GSS Sound", "StartSound.wav");

            if (File.Exists(sound))
            {
                StartCoroutine(LoadAndPlaySound(sound));
            }
            else
            {
                Logger.LogError($"Sound was not found in: {sound}");
            }
        }

        private System.Collections.IEnumerator LoadAndPlaySound(string path)
        {
            using (WWW www = new WWW("file://" + path))
            {
                Logger.LogInfo("Playing sound");
                yield return www;
                AudioClip aud = www.GetAudioClip(false, false);
                GSSAudio.clip = aud;
                GSSAudio.Play();
            }
        }



        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            /* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
        }

        void Update()
        {
            /* Code here runs every frame when the mod is enabled */
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = false;
        }
    }
}
