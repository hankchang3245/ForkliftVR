using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using edu.tnu.dgd.value;
using UnityEngine.SceneManagement;


namespace edu.tnu.dgd.assemble
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioController : MonoBehaviour
    {
        private AudioSource audioSource;
        private Dictionary<string, AudioClip> dict_audio = new Dictionary<string, AudioClip>();
        private bool audioIsPlaying = false;

        private static AudioController _instance;
        public static AudioController instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AudioController>();
                }

                return _instance;
            }
        }

        public bool playVoiceAtStartupAndEnd = false;
        public bool playVoice = false;

        public string[] startupAudioClipNames;
        public string[] endingAudioClipNames;

        private List<string[]> waitingQueue;

        void Awake()
        {
            waitingQueue = new List<string[]>();
            audioSource = GetComponent<AudioSource>();
        }


        void Start()
        {
            LoadAudio();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //Debug.Log("OnSceneLoaded: " + scene.name);
            //Debug.Log(mode);
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                PlayStartupAudio();
            }
            
        }

        public void PlayStartupAudio()
        {
            if (this != null && playVoiceAtStartupAndEnd && startupAudioClipNames != null && startupAudioClipNames.Length > 0)
            {
                StartCoroutine(PlayVoicesAsync(startupAudioClipNames));
            }
        }

        public void PlayEndingAudio()
        {
            if (this != null && playVoiceAtStartupAndEnd && endingAudioClipNames != null && endingAudioClipNames.Length > 0)
            {
                StartCoroutine(PlayVoicesAsync(endingAudioClipNames));
            }
        }

        public void Stop()
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        public void PlayVoice(string[] names)
        {
            if (playVoice)
            {
                StartCoroutine(PlayVoicesAsync(names));
            }
        }

        private void LoadAudio()
        {
            List<string> allres = GetResourcesFiles();
            foreach (string file in allres)
            {
                int idx = file.LastIndexOf("/");
                string res = "";
                if (idx >= 0)
                {
                    res = file.Substring(idx + 1, file.Length - idx - 1 - 4);
                }
                else
                {
                    res = file;
                }

                try
                {
                    AudioClip ac = Resources.Load<AudioClip>("ProcessAudio/" + res);
                    dict_audio.Add(res, ac);
                }
                catch (Exception ex)
                {
                    Debug.Log("[AudioController] " + ex.StackTrace);
                }
            }
        }

        private void PlayAudioFromQueue()
        {
            if (audioIsPlaying || waitingQueue.Count == 0)
            {
                return;
            }
            if (playVoice)
            {
                audioIsPlaying = true;
                string[] item = waitingQueue[0];
                waitingQueue.RemoveAt(0);

                StartCoroutine(PlayVoicesAsync(item));
            }
        }

        private IEnumerator PlayVoicesAsync(string[] names)
        {
            if (this == null)
            {
                yield return null;
            }

            if (audioIsPlaying)
            {
                //audioSource.Stop();
                waitingQueue.Add(names);
                yield return null;
            }

            audioIsPlaying = true;
            foreach (string name in names)
            {
                float len = PlayVoice(name);
                yield return new WaitForSeconds(len);
            }
            audioIsPlaying = false;
        }

        private float PlayVoice(string name)
        {
            AudioClip ac;
            if (dict_audio.TryGetValue(name, out ac))
            {
                audioSource.PlayOneShot(ac);
                return ac.length;
            }

            return 0;
        }

        public AudioClip GetAudioClipByName(string name)
        {
            AudioClip ac;
            if (dict_audio.TryGetValue(name, out ac))
            {
                return ac;
            }

            return null;
        }

        public List<string> GetResourcesFiles()
        {
            List<string> result = AssembleProcessStore.instance.GetAllAudioClipNames();
            result.Add("主選單1");
            result.Add("主選單2");
            result.Add("主選單3");
            result.Add("主選單4");

            result.Add("單元1-1開頭1");
            result.Add("單元1-1開頭2");
            result.Add("單元1-1開頭3");
            result.Add("單元1-1開頭4");
            result.Add("單元1-1開頭5");
            result.Add("單元1-16結尾");

            result.Add("單元2-1開頭1");
            result.Add("單元2-1開頭2");
            result.Add("單元2-1開頭3");
            result.Add("單元2-1開頭4");
            result.Add("單元2-2結尾");

            result.Add("單元3-1開頭1");
            result.Add("單元3-1開頭2");
            result.Add("單元3-1開頭3");
            result.Add("單元3-1開頭4");
            result.Add("單元3-1開頭5");
            result.Add("單元3-1開頭6");
            result.Add("單元3-1開頭7");

            result.Add("單元3-14結尾");


            //Sound Effect
            result.Add("SFX_塗抹散熱膏");
            result.Add("SFX_零組件固定_短高");
            result.Add("SFX_零組件固定_輕聲");
            result.Add("SFX_螺絲轉動");
            
            return result;
        }


        private void OnDestroy()
        {
            _instance = null;
        }

    }
}

