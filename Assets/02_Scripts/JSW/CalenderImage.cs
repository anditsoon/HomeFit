using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CalenderImage : MonoBehaviour
{
    public Button dateButton;
    private TMP_Text dateText;
    private CalendarManager calendarManager;

    private void Awake()
    {
        dateButton = GetComponent<Button>();
        dateText = GetComponent<TMP_Text>();
        calendarManager = FindObjectOfType<CalendarManager>();

        if (calendarManager == null)
        {
            Debug.LogError("CalendarManager not found in the scene!");
        }

        dateButton.onClick.AddListener(OnDateClicked);
    }

    private void OnDateClicked()
    {
        calendarManager.OnDateSelected(this);
    }

    public string GetDateString()
    {
        return $"2024-09-{dateText.text.PadLeft(2, '0')}";
    }

    public void SetBold(bool isBold)
    {
        dateText.fontStyle = isBold ? FontStyles.Bold : FontStyles.Normal;
        dateText.fontSize = isBold ? 45 : 40; // Adjust font size as needed
    }
}