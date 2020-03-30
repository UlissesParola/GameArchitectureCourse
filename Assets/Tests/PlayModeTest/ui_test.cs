using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace a_player
{
    public class ui_test
    {
        private Item _item;
        private Player _player;

        [UnitySetUp]
        public IEnumerator init()
        {
            yield return TestHelper.LoadItemTestScene();
            _player = TestHelper.GetPlayer();
            _item = GameObject.FindObjectOfType<Item>();
        }
        
        [UnityTest]
        public IEnumerator picks_up_and_equips_item()
        {
            Assert.AreNotSame(_item, _player.GetComponent<Inventory>().EquipedItem);
            
            _item.transform.position = _player.transform.position;

            yield return new WaitForFixedUpdate();

            Assert.AreSame(_item, _player.GetComponent<Inventory>().EquipedItem);
        }
        
        [UnityTest]
        public IEnumerator change_crosshair()
        {
            Crosshair crosshair = GameObject.FindObjectOfType<Crosshair>();
            
            Assert.AreNotSame(_item.CrosshairDefinition.Sprite, crosshair.GetComponent<Image>().sprite);
            
            _item.transform.position = _player.transform.position;

            yield return new WaitForFixedUpdate();

            Assert.AreSame(_item.CrosshairDefinition.Sprite, crosshair.GetComponent<Image>().sprite);
        }
        
        [UnityTest]
        public IEnumerator change_slot_icon()
        {
            var hotbar = GameObject.FindObjectOfType<Hotbar>();
            var slot = hotbar.GetComponentInChildren<Slot>();
            
            Assert.AreNotSame(_item.Icon, slot.IconImage.sprite);
            
            _item.transform.position = _player.transform.position;

            yield return new WaitForFixedUpdate();

            Assert.AreSame(_item.Icon, slot.IconImage.sprite);
        }
        
    }
}