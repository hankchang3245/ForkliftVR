using System;
using edu.tnu.dgd.util;
using UnityEngine;

namespace edu.tnu.dgd.vrlearn
{
    [Serializable]
    public class Learner
    {
        [SerializeField]
        private string _id;
        [SerializeField]
        private string _school;
        [SerializeField]
        private string _schoolId;
        [SerializeField]
        private string _name;
        [SerializeField]
        private string _gameId;
        [SerializeField]
        private int _sessionId;

        public string school
        {
            get
            {
                return _school;
            }
        }

        public string schoolId
        {
            get
            {
                return _schoolId;
            }
        }
        public string id
        {
            get
            {
                return _id;
            }
        }

        public string name
        {
            get
            {
                return _name;
            }
        }

        public string gameId
        {
            get
            {
                return _gameId;
            }
        }

        public int sessionId
        {
            get
            {
                return _sessionId;
            }

            set
            {
                _sessionId = value;
            }
        }

        public Learner(string id, string school, string schoolId, string gameId, int sessionId, string name)
        {
            this._id = id;
            this._school = school;
            this._schoolId = schoolId;
            this._gameId = gameId;
            this._sessionId = sessionId;
            this._name = name;
        }

        public Learner(string id, string school, string schoolId, string gameId, int sessionId)
        {
            this._id = id;
            this._school = school;
            this._schoolId = schoolId;
            this._gameId = gameId;
            this._sessionId = sessionId;
        }

        
        public static Learner Load()
        {
            return Load(true);
        }
        

        public static Learner Load(bool ifNoLearnerCreateGuestAccount)
        {
            string learnerJson = PlayerPrefs.GetString("CurrentLearner");
            if (learnerJson != null && learnerJson.Length > 0)
            {
                Learner u = JsonHelper.fromJson<Learner>(learnerJson);
                if (u != null)
                {
                    return u;
                }
            }

            if (ifNoLearnerCreateGuestAccount)
            {
                return CreateGuestLearner();
            }
            return null;
            
        }


        public void Save()
        {
            string learnerJson = JsonHelper.toJson<Learner>(this);
            if (learnerJson != null && learnerJson.Length > 0)
            {
                PlayerPrefs.SetString("CurrentLearner", learnerJson);
                PlayerPrefs.Save();
            }
        }

        public static Learner CreateGuestLearner()
        {
            Learner u = new Learner("Guest", "", "000000", Guid.NewGuid().ToString(), 1);
            string ujson = JsonHelper.toJson<Learner>(u);
            PlayerPrefs.SetString("CurrentLearner", ujson);
            PlayerPrefs.Save();
            return u;
        }

        public override string ToString()
        {
            return "id:" + id + ", school:" + school + ", schoolId:" + schoolId + ", name:" + name;
        }
    }
}

