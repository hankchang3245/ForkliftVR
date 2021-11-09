using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using edu.tnu.dgd.vrlearn;
using edu.tnu.dgd.assemble;
using TMPro;

namespace edu.tnu.dgd.assemble
{
    public class ScoringController : MonoBehaviour
    {
        [Tooltip("True為組裝流程；False為拆裝流程。")]
        public bool isAssemble = true;

        public int accumulatedScore = 0;

        public TMP_Text scoreText;
        public TMP_Text learnerText;
        private Learner learner;

        //public GameObject standButtonGroup;

        // 為了防止重複累加分數
        private Dictionary<string, int> dict_score = new Dictionary<string, int>();

        private static ScoringController _instance;
        public static ScoringController instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ScoringController>();
                }
                return _instance;
            }
        }

        void Start()
        {
            
            learner = Learner.Load();
            if (learnerText != null)
            {
                learnerText.text = learner.id;
            }
            
        }

        public void AddScore(string name, int score)
        {
            try
            {
                int tmp;
                if (!dict_score.TryGetValue(name, out tmp))
                {
                    accumulatedScore = accumulatedScore + score;
                    dict_score.Add(name, score);

                    if (accumulatedScore >= 100)
                    {
                        AudioController.instance.PlayEndingAudio();
                    }
                }
            }
            catch
            {

            }
        }

        private void OnDestroy()
        {
            _instance = null;
        }

    }
}

