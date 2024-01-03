using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;
using System;
using Zenject;

namespace CannonFightBase
{
    public class JsonSaver:MonoBehaviour,IInitializable
    {

        private bool _isLoading = false;
        private bool _isSaving;
        private string _saveFileName = "CannonTraits";


        public void Start()
        {

        }


        public void Initialize()
        {
        }



        public void SaveData(PlayerSaveData data)
        {

            if (!Social.localUser.authenticated)
            {
                Debug.LogError("User is not authenticated to Google Play Services");
                return;
            }

            if (_isSaving)
            {
                Debug.LogError("Already saving data");
                return;
            }

            _isSaving = true;
            PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(
                _saveFileName,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseMostRecentlySaved,
                (status, metadata) =>
                {
                    if (status != SavedGameRequestStatus.Success)
                    {
                        Debug.LogError("Error opening saved game");
                        _isSaving = false;
                        return;
                    }

                    string jsonString = JsonUtility.ToJson(data);
                    byte[] savedData = Encoding.ASCII.GetBytes(jsonString);

                    SavedGameMetadataUpdate updatedMetadata = new SavedGameMetadataUpdate.Builder()
                        .WithUpdatedDescription("My Save File Description")
                        .Build();

                    PlayGamesPlatform.Instance.SavedGame.CommitUpdate(
                        metadata,
                        updatedMetadata,
                        savedData,
                        (commitStatus, _) =>
                        {
                            _isSaving = false;
                            string text = commitStatus == SavedGameRequestStatus.Success
                                ? "Data saved successfully"
                                : "Error saving data";

                            Debug.Log(text);
                        });
                });
        }

        public PlayerSaveData LoadData(Action<PlayerSaveData> callback)
        {
            if (_isLoading)
            {
                Debug.LogWarning("Load already in progress");
                return null;
            }
            PlayerSaveData data = null;

            _isLoading = true;
            PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(
                _saveFileName,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime,
                (status, metadata) =>
                {
                    Debug.Log("LOAD RESULT: " + SavedGameRequestStatus.Success);

                    if (status != SavedGameRequestStatus.Success)
                    {
                        Debug.LogError("Error opening saved game");
                        _isLoading = false;
                        return;
                    }

                    PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(
                        metadata,
                        (readStatus, savedData) =>
                        {
                            if (readStatus != SavedGameRequestStatus.Success)
                            {
                                Debug.LogError("Error reading saved game data");
                                _isLoading = false;
                                return;
                            }

                            string jsonString = Encoding.ASCII.GetString(savedData);
                            data = JsonUtility.FromJson<PlayerSaveData>(jsonString);
                            
                            callback(data);

                            //Debug.Log("Loaded data: "+data);
                            _isLoading = false;
                        });

                    //PlayGamesPlatform.Instance.SavedGame.Delete(metadata);

                });

            return data;
        }

    }
}
