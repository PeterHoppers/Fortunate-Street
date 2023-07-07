using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PropertySummaryView : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset m_PropertySummaryAsset;

    UIDocument m_Document;
    PropertySummaryController m_ControllerLogic;

    VisualElement m_VisualElement;
    VisualElement m_RootElement;

    public void CreatePropetySummary()
    {
        // The UXML is already instantiated by the UIDocument component
        m_Document = GetComponent<UIDocument>();
        m_ControllerLogic = new PropertySummaryController();

        //grabs where in the scene the UI should be rendered
        m_RootElement = m_Document.rootVisualElement.Query("PropertySummaryHolder");
        m_VisualElement = m_PropertySummaryAsset.Instantiate().Query("PropertySummary").First();

        m_RootElement.Add(m_VisualElement);
        m_ControllerLogic.SetVisualElement(m_VisualElement);

        m_ControllerLogic.ToggleVisiblity(false);
    }

    public void UpdatePropertySummary(Property property)
    {
        m_ControllerLogic.SetTitle(property.district.Name);
        m_ControllerLogic.SetName(property.spaceName);
        m_ControllerLogic.SetValue(property.shopValue);
        m_ControllerLogic.SetPrice(property.GetShopPrice());
        m_ControllerLogic.SetCapital(property.GetMaxInvestment());
        m_ControllerLogic.ToggleVisiblity(true);
    }

    public void HidePropertySummary() 
    {
        if (m_ControllerLogic != null) 
        {
            m_ControllerLogic.ToggleVisiblity(false);
        }
    }
}
