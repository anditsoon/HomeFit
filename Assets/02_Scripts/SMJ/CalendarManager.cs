using UnityEngine;
using TMPro;
using System.Collections;

public class CalendarManager : MonoBehaviour
{
    public TMP_Text CalenderDay;
    public TMP_Text ExerciseCount;
    public TMP_Text KcalCount;
    public TMP_Text MinutesCount;
    private ProfileGetManager profileGetManager;
    private CalenderImage lastSelectedDate;

    private float ExerciseCountNum = 0;
    private float KcalCountNum = 0;
    private float MinutesCountNum = 0;
    private float NowExerciseCountNum = 0;
    private float NowKcalCountNum = 0;
    private float NowMinutesCountNum = 0;

    private void Start()
    {
        profileGetManager = FindObjectOfType<ProfileGetManager>();
        if (profileGetManager == null)
        {
            Debug.LogError("ProfileGetManager not found in the scene!");
        }
    }

    public void OnDateSelected(CalenderImage selectedDate)
    {
        if (lastSelectedDate != null)
        {
            lastSelectedDate.SetBold(false);
        }

        selectedDate.SetBold(true);
        lastSelectedDate = selectedDate;

        JSWSoundManager.Get().PlayEftSound(JSWSoundManager.ESoundType.EFT_PROFILESCENE);

        string dateString = selectedDate.GetDateString();
        CalenderDay.text = dateString;

        profileGetManager.SendDataToServer(dateString, false);
        StartCoroutine(WaitForDataAndUpdateUI());

        // Trigger profile UI animation
        GameObject profile = GameObject.Find("Profile");
        if (profile != null)
        {
            Animator animator = profile.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = true;
                animator.CrossFade("ProfileUIUI", 0);
            }
        }
    }

    private IEnumerator WaitForDataAndUpdateUI()
    {
        yield return new WaitForSeconds(0.5f);

        // Reset current values to start new interpolation
        NowExerciseCountNum = 0;
        NowKcalCountNum = 0;
        NowMinutesCountNum = 0;

        // Update target values from AvatarInfo
        ExerciseCountNum = AvatarInfo.instance.totalExerciseCount;
        KcalCountNum = (float)AvatarInfo.instance.totalCaloriesBurned;
        MinutesCountNum = ExerciseCountNum / 2;

        // Start coroutine for smooth value transition
        StartCoroutine(SmoothValueTransition());
    }

    private IEnumerator SmoothValueTransition()
    {
        float elapsedTime = 0;
        float transitionDuration = 1f; // Adjust this value to change the speed of the transition

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            NowExerciseCountNum = Mathf.Lerp(0, ExerciseCountNum, t);
            NowKcalCountNum = Mathf.Lerp(0, KcalCountNum, t);
            NowMinutesCountNum = Mathf.Lerp(0, MinutesCountNum, t);

            UpdateUIText();

            yield return null;
        }

        // Ensure final values are set
        NowExerciseCountNum = ExerciseCountNum;
        NowKcalCountNum = KcalCountNum;
        NowMinutesCountNum = MinutesCountNum;
        UpdateUIText();
    }

    private void UpdateUIText()
    {
        ExerciseCount.text = ((int)NowExerciseCountNum).ToString();
        KcalCount.text = ((int)NowKcalCountNum).ToString();
        MinutesCount.text = ((int)NowMinutesCountNum).ToString();
    }
}
