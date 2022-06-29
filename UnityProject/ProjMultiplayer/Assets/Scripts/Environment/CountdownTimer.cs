using System.Collections;
using UnityEngine;
using UnityEngine.UI;

    public class CountdownTimer : MonoBehaviour
    {
        float currentTime = 0.0f;
        //float startingTime = 65.0f;

        [SerializeField] Text text;

        bool timerStarted = false;

        public void StartTimer(float time)
        {
            currentTime = time;
            timerStarted = true;
        }

        private void Update()
        {
            if (!timerStarted) return;


            if (currentTime <= 0.0f)
            {
                currentTime = 0.0f;
                return;
            }

            currentTime -= 1 * Time.deltaTime;

            string str = formatTimer(currentTime);

            if (text == null) return;

            text.text = str;
            if(currentTime < 10.0f)
            {
                text.color = Color.Lerp(text.color, Color.red, 0.1f);
            }
            else if(currentTime < 30.0f)
            {
                text.color = Color.Lerp(text.color, new Color(0.8f, 0.4f, 0), 0.1f);
            }
            else if(currentTime < 60.0f)
            {
                text.color = Color.Lerp(text.color, Color.yellow, 0.1f);
            }
        }

        private string formatTimer(float timeInSeconds)
        {
            if (timeInSeconds > 60 && timeInSeconds <= 3600)
            {
                int minutes = (int)timeInSeconds / 60;
                int seconds = (int)timeInSeconds % 60;

                return Format(minutes) + ":" + Format(seconds);
            }
            else if (timeInSeconds > 3600)
            {
                int hours = (int)timeInSeconds / 3600;
                int minutes = (int)(timeInSeconds - (hours * 3600)) / 60;
                int seconds = (int)(timeInSeconds - (hours * 3600)) % 60;

                return Format(hours) + ":" + Format(minutes) + ":" + Format(seconds);
            }
            else
            {
                return timeInSeconds.ToString("0.0");
            }
        }

        private string Format(int number)
        {
            return number < 10 ? "0" + number : number.ToString();
        }

    }
