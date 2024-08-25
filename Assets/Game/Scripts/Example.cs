using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Core
{
    public class Example : MonoBehaviour
    {
        public void Run(string url, Action<string> success, Action<string> error)
        {
            StartCoroutine(RunCoroutine(url, success, error));
        }

        private IEnumerator RunCoroutine(string url, Action<string> success, Action<string> error)
        {
            using var request = new UnityWebRequest(url);
            
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                error?.Invoke(request.error);
            }
            else
            {
                success?.Invoke(request.downloadHandler.text);
            }
        }
    }
}