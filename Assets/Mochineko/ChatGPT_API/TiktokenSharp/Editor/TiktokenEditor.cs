#nullable enable
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TiktokenSharp.Editor
{
    internal sealed class TiktokenEditor : EditorWindow
    {
        [MenuItem("Mochineko/TiktokenEditor")]
        public static void Open()
        {
            GetWindow<TiktokenEditor>("Tiktoken");
        }

        private string model = "gpt-3.5-turbo";
        private string text = string.Empty;
        private List<int>? tokens = null;
        private List<string>? tokenizedText = null;
        private Vector2 scrollPosition = Vector2.zero;

        private void OnGUI()
        {
            EditorGUILayout.Space();

            model = EditorGUILayout.TextField("Model", model);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Text:");
            text = EditorGUILayout.TextArea(text);

            EditorGUILayout.Space();

            if (GUILayout.Button("Tokenize"))
            {
                var tikToken = TikToken.EncodingForModel(model);
                tokens = tikToken.Encode(text);
                tokenizedText = Decode(tikToken, tokens);
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField(
                "Token Length:",
                tokens != null ? tokens.Count.ToString() : "0");

            EditorGUILayout.Space();

            if (tokens != null && tokenizedText != null)
            {
                EditorGUILayout.LabelField("Tokens:");
                using (var scroll = new EditorGUILayout.ScrollViewScope(scrollPosition))
                using (_ = new EditorGUI.IndentLevelScope())
                {
                    scrollPosition = scroll.scrollPosition;
                    for (var i = 0; i < tokens.Count; i++)
                    {
                        EditorGUILayout.LabelField(
                            $"{i}: {tokens[i]}",
                            $"< {tokenizedText[i]}");
                    }
                }
            }
        }

        private static List<string> Decode(TikToken tikToken, List<int> tokens)
        {
            var result = new List<string>();
            for (var i = 0; i < tokens.Count; i++)
            {
                var tokenized = tikToken.Decode(tokens[i]);
                Debug.Log($"{i}:{tokenized}");
                result.Add(tokenized);
            }

            return result;
        }
    }
}