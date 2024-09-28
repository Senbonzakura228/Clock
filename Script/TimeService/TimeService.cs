using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Script.Clock;
using UnityEngine;
using UnityEngine.Networking;

namespace Script
{
    public class TimeService : MonoBehaviour
    {
        readonly string getTimeRequestURL = "https://yandex.com/time/sync.json";
        private DateTime _currentTime;
        private DateTime lastUpdateTime;
        private float timeSinceLastTimeUpdate;
        private DateTime _currentLocalTime;
        private Coroutine _synhServerTimeRoutine;
        public DateTime CurrentTime => _currentLocalTime;

        void Start()
        {
            _synhServerTimeRoutine = StartCoroutine(SynhServerTime());
        }

        private void Update()
        {
            timeSinceLastTimeUpdate = (float) (DateTime.Now - lastUpdateTime).TotalMilliseconds;
            _currentLocalTime = _currentTime.AddMilliseconds(timeSinceLastTimeUpdate);
        }

        public void ChangeTime(TimeType type, int value, bool isLineClockFormat = true)
        {
            _currentTime = TimeChanger.GetNewTime(type, value, _currentLocalTime, isLineClockFormat);
            lastUpdateTime = DateTime.Now;
        }

        IEnumerator SynhServerTime()
        {
            while (true)
            {
                var requestStartTime = Time.realtimeSinceStartup;

                using UnityWebRequest request = UnityWebRequest.Get(getTimeRequestURL);
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var json = request.downloadHandler.text;
                    var timeResponse = JsonConvert.DeserializeObject<GetTimeDataResponse>(json);
                    if (timeResponse?.time == null)
                    {
                        Debug.LogError("Error: " + "wrong data format");
                        yield break;
                    }

                    var timeData = timeResponse.time;
                    var unixTimeInSeconds = timeData / 1000;
                    _currentTime = DateTimeOffset.FromUnixTimeSeconds(unixTimeInSeconds).DateTime;
                    var requestEndTime = Time.realtimeSinceStartup;
                    var requestDuration = requestEndTime - requestStartTime;
                    _currentTime = _currentTime.AddSeconds(requestDuration);
                    lastUpdateTime = DateTime.Now;
                    Debug.Log("Time - : " + _currentTime);
                }
                else
                {
                    Debug.LogError("Error: " + request.error);
                }

                yield return new WaitForSeconds(3600);
            }
        }

        private void OnDestroy()
        {
            StopCoroutine(_synhServerTimeRoutine);
        }
    }
}