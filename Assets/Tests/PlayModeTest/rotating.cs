using NSubstitute;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace a_player
{
    public class rotating
    {
        [UnityTest]
        public IEnumerator with_negative_mouse_X()
        {
            TestHelper.CreateFloor();
            var player = TestHelper.CreatePlayer();

            var startingRotation = player.transform.rotation;

            player.PlayerInput.MouseX.Returns(-1f);

            yield return new WaitForSeconds(0.5f);

            var endingRotation = player.transform.rotation;

            float turnAmount = TestHelper.CalculateRotation(startingRotation, endingRotation);

            Assert.Less(turnAmount, 0);
        }
    }
}

