using UnityEngine.UIElements;
using UnityEngine;

public class PlayerStatsListEntryController
{
    VisualElement rootElement;
    
    Label m_NameLabel;
    Label m_MoneyLabel;
    Label m_WorthLabel;

    public void SetVisualElement(VisualElement visualElement)
    {
        rootElement = visualElement;
    }

    public void SetName(string name)
    {
        if (m_NameLabel == null)
        {
            m_NameLabel = rootElement.Q<Label>("PlayerName");
        }

        m_NameLabel.text = name;
    }

    public void SetMoney(int amount)
    {
        if (m_MoneyLabel == null)
        {
            m_MoneyLabel = rootElement.Q<Label>("Money");
        }

        m_MoneyLabel.text = $"$ {amount}";
    }

    public void SetWorth(int amount)
    {
        if (m_WorthLabel == null)
        {
            m_WorthLabel = rootElement.Q<Label>("TotalWealth");
        }

        m_WorthLabel.text = amount.ToString();
    }

    public void SetBackground(Color color)
    {
        Color backgroundColor = new Color(color.r, color.g, color.b, .6f);
        rootElement.style.backgroundColor = backgroundColor;
    }

    public void SetSuit(string suit, bool isGained)
    {
        VisualElement suitElement = rootElement.Q<VisualElement>($"{suit}Suit");

        if (isGained)
        {
            suitElement.AddToClassList("is-visible");
        }
        else
        {
            suitElement.RemoveFromClassList("is-visible");
        }
    }
}
