using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patterns.Singleton
{
    public abstract class Singleton_DontDestroyOnLoad<T> : RyoMonoBehaviour where T : RyoMonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (Singleton_DontDestroyOnLoad<T>.instance == null)
                {
                    Singleton_DontDestroyOnLoad<T>.instance = GameObject.FindObjectOfType<T>();

                    if (Singleton_DontDestroyOnLoad<T>.instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        Singleton_DontDestroyOnLoad<T>.instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString();
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return instance;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (Singleton_DontDestroyOnLoad<T>.instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }


    }

}
