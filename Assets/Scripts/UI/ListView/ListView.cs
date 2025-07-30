using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[System.Serializable]
public class ItemSelectEvent : UnityEvent<ListView.ListViewItem>
{

}

public class ListView : MonoBehaviour
{
    public UnityAction<ListViewItem> onItemSelected;

    public class ListViewItem: MonoBehaviour,IPointerClickHandler
    {
        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                onSelected(selected);
            }
        }
        public virtual void onSelected(bool selected)
        {
        }

        public ListView owner;
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!this.selected)
            {
                this.Selected = true;
            }
            if (owner != null)
            {
                owner.SelectedItem = this;
            }
        }
    }

    List<ListViewItem> items = new List<ListViewItem>();

    private ListViewItem selectedItem = null;
    public ListViewItem SelectedItem
    {
        get { return selectedItem; }
       private set
        {
            if (selectedItem!=null && selectedItem!=value)
            {
                selectedItem.Selected = false;
            }
            selectedItem = value;
            if (onItemSelected != null)
                onItemSelected.Invoke(value);
        }
    }
    public T InstantiateItem<T>(GameObject gameObject, Transform form)
    {
        GameObject go = Instantiate(gameObject, form);
        var ui = go.GetComponent<T>();
        return ui;
    }
    public void AddItem(ListViewItem item)
    {
        item.owner = this;
        this.items.Add(item);
    }

    public void RemoveAll()
    {
        foreach (var it in items)
        {
            if (it != null && it.gameObject != null)
            {
                Destroy(it.gameObject);
            }
        }
        items.Clear();
    }
}
