using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{

    protected override void Start()
    {
        base.Start();
    }
    
    public void SetupCraft(ItemData_Equipment _data)
    {

        if (_data == null)
            return;
        item.data = _data;
        itemImage.sprite = _data.icon; 
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.SetupCraftWindow(item.data as ItemData_Equipment);
    }
}
