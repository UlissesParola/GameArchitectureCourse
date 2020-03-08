using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace a_player
{
    public class ui_test
    {
        [UnityTest]
        public IEnumerator picks_up_and_equips_item()
        {
            yield return TestHelper.LoadItemTestScene();
            var player = TestHelper.GetPlayer();

            Item item = GameObject.FindObjectOfType<Item>();
            
            Assert.AreNotSame(item, player.GetComponent<Inventory>().EquipedItem);
            
            item.transform.position = player.transform.position;

            yield return null;

            Assert.AreSame(item, player.GetComponent<Inventory>().EquipedItem);
        }
        
        [UnityTest]
        public IEnumerator change_crosshair()
        {
            yield return TestHelper.LoadItemTestScene();
            var player = TestHelper.GetPlayer();
            Crosshair crosshair = GameObject.FindObjectOfType<Crosshair>();
            
            Item item = GameObject.FindObjectOfType<Item>();
            
            
            Assert.AreNotSame(item.CrosshairDefinition.Sprite, crosshair.GetComponent<Image>().sprite);
            
            item.transform.position = player.transform.position;

            yield return null;

            Assert.AreSame(item.CrosshairDefinition.Sprite, crosshair.GetComponent<Image>().sprite);
        }
        
    }
}