﻿using UnityEngine;

namespace Utility
{
        public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
        {
            public static T Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();
                    }

                    return _instance;
                }
            }

            private static T _instance;
        }
}