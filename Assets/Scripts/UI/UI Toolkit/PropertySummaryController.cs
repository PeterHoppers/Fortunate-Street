using UnityEngine.UIElements;
using UnityEngine;

public class PropertySummaryController
{
    VisualElement rootElement;

    Label m_TitleLabel;
    Label m_NameLabel;
    Label m_ValueLabel;
    Label m_PriceLabel;
    Label m_CaptialLabel;

    public void SetVisualElement(VisualElement visualElement)
    {
        rootElement = visualElement;
    }

    public void SetTitle(string name)
    {
        if (m_TitleLabel == null)
        {
            m_TitleLabel = rootElement.Q<Label>("Title");
        }

        m_TitleLabel.text = name;
    }

    public void SetName(string name)
    {
        if (m_NameLabel == null)
        {
            m_NameLabel = rootElement.Q<Label>("PropertyName");
        }

        m_NameLabel.text = name;
    }

    public void SetValue(int amount)
    {
        if (m_ValueLabel == null)
        {
            m_ValueLabel = rootElement.Q<Label>("Value");
        }

        m_ValueLabel.text = $"$ {amount}";
    }

    public void SetPrice(int amount)
    {
        if (m_PriceLabel == null)
        {
            m_PriceLabel = rootElement.Q<Label>("Price");
        }

        m_PriceLabel.text = amount.ToString();
    }

    public void SetCapital(int amount)
    {
        if (m_CaptialLabel == null)
        {
            m_CaptialLabel = rootElement.Q<Label>("Capital");
        }

        m_CaptialLabel.text = amount.ToString();
    }

    public void SetBackground(Color color)
    {
        Color backgroundColor = new Color(color.r, color.g, color.b, .6f);
        rootElement.style.backgroundColor = backgroundColor;
    }

    public void ToggleVisiblity(bool isVisible)
    {
        if (isVisible)
        {
            rootElement.style.display = StyleKeyword.Auto;            
        }
        else
        {
            rootElement.style.display = StyleKeyword.None;
        }
    }
}
