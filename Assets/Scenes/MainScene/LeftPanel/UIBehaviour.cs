using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIBehaviourBase : MonoBehaviour
{
    public static Action<GameObject, Func<string>> AssocTooltip;
}

public abstract class UIBehaviour<T> : UIBehaviourBase
    where T:class
{

    private T _dataSource;

    private Dictionary<Text, Func<T, object>> dictText;
    private Dictionary<NumberText, Func<T, object>> dictNumberText;
    private Dictionary<Toggle, Func<T, bool>> dictToggles;
    private Dictionary<IncFlag, Func<T, bool>> dictIncFlag;

    public T dataSource
    {
        get
        {
            return _dataSource;
        }
        set
        {
            if(_dataSource == value)
            {
                return;
            }

            dictText = new Dictionary<Text, Func<T, object>>();
            dictNumberText = new Dictionary<NumberText, Func<T, object>>();
            dictToggles = new Dictionary<Toggle, Func<T, bool>>();
            dictIncFlag = new Dictionary<IncFlag, Func<T, bool>>();

            _dataSource = value;

            AssocDataSource();
        }
    }

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }


    protected virtual void FixedUpdate()
    {
        foreach(var pair in dictText)
        {
            pair.Key.text = pair.Value(dataSource).ToString();
        }

        foreach(var pair in dictNumberText)
        {
            pair.Key.Value = Convert.ToDouble(pair.Value(dataSource));
        }

        foreach(var pair in dictToggles)
        {
            pair.Key.isOn = pair.Value(dataSource);
        }

        foreach(var pair in dictIncFlag)
        {
            pair.Key.isInc = pair.Value(dataSource);
        }
    }

    protected virtual void OnDestroy()
    {

    }

    protected void Bind(Func<T, object> func, Text text)
    {
        dictText.Add(text, func);
    }

    protected void Bind(Func<T, object> func, NumberText text)
    {
        dictNumberText.Add(text, func);
    }

    protected void Bind(Func<T, bool> func, IncFlag flag)
    {
        dictIncFlag.Add(flag, func);
    }

    protected void Bind<TItem, TUIItem>(Func<T, IEnumerable<TItem>> func, UICollectionBehaviour<TItem, TUIItem> uiContainer)
        where TItem : class
        where TUIItem : UIBehaviour<TItem>
    {
        Func<object, IEnumerable<TItem>> adpt = (object obj) => func((T)obj);
        uiContainer.SetDataSource(dataSource, adpt);
    }

    protected void SetPropertyValue<TValue>(Expression<Func<T, TValue>> memberLamda, TValue value)
    {
        var memberSelectorExpression = memberLamda.Body as MemberExpression;
        if (memberSelectorExpression != null)
        {
            var property = memberSelectorExpression.Member as PropertyInfo;
            if (property != null)
            {
                property.SetValue(dataSource, value, null);
            }
        }
    }

    protected void BindTwoWay<TValue>(Expression<Func<T, TValue>> memberLamda, ToggleGroupEx toggles)
        where TValue: Enum
    {
        toggles.Clear();

        var memberSelectorExpression = memberLamda.Body as MemberExpression;
        if (memberSelectorExpression == null)
        {
            throw new Exception();
        }

        var property = memberSelectorExpression.Member as PropertyInfo;
        if (property == null)
        {
            throw new Exception();
        }

        foreach (TValue item in Enum.GetValues(typeof(TValue)))
        {
            var toggle = toggles.Add(item);

            toggle.onValueChanged.AddListener((isOn) =>
            {
                if(isOn)
                {
                    property.SetValue(dataSource, toggles.GetEnum<TValue>(toggle), null);
                }
            });

            dictToggles.Add(toggle, (d) =>
            {
                return toggles.GetEnum<TValue>(toggle).Equals((TValue)property.GetValue(dataSource));
            });
        }
    }

    protected void AddToolTip(Text text, Func<T, string> func)
    {
        AssocTooltip(text.gameObject, () => func(dataSource));
    }

    protected void AddTooltip<TValue>(ToggleGroupEx toggleGroup, Func<T, TValue, string> tooltip)
        where TValue : Enum
    {
        foreach(var toggle in toggleGroup.toggles)
        {
            toggle.GetComponent<DynamicToolTip>().GenerateBody = () => tooltip(dataSource, toggleGroup.GetEnum<TValue>(toggle));
        }
    }


    protected abstract void AssocDataSource();
}