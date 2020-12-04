using UnityEngine;

namespace ElJardin {
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        protected static T m_instance;

        public static T Instance {
            get {
                if (m_instance == null) {
                    m_instance = (T)FindObjectOfType(typeof(T));

                    if (!m_instance) {
                        var singletonObject = new GameObject();
                        m_instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";
                        if (Application.isEditor) {
                            Debug.LogError("Instance of " + typeof(T) + " not found");
                        }
                    }

                }

                return m_instance;
            }
        }
    }
}
