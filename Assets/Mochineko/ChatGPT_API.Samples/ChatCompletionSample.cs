#nullable enable
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Mochineko.ChatGPT_API.Samples
{
    public class ChatCompletionSample : MonoBehaviour
    {
        [SerializeField] private string apiKey = string.Empty;
        [SerializeField, TextArea] private string systemMessage = string.Empty;
        [SerializeField, TextArea] private string chatContent = string.Empty;

        private ChatGPTConnection? connection;
        
        private void Start()
        {
            Assert.IsNotNull(apiKey);
            
            connection = new ChatGPTConnection(apiKey, Model.Turbo);
            
            connection.AddSystemMessage(systemMessage);
        }

        [ContextMenu(nameof(SendChat))]
        public async void SendChat()
        {
            if (connection == null)
            {
                Debug.LogError($"[ChatGPT_API.Samples] Connection is null.");
                return;
            }
            
            string result;
            try
            {
                result = await connection
                    .SendMessage(chatContent, this.GetCancellationTokenOnDestroy());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return;
            }
            
            Debug.Log($"[ChatGPT_API.Samples] Result:\n{result}");
        }
    }
}
