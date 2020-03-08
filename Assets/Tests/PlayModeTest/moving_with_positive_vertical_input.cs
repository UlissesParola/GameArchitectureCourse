using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using System.Collections;
using NSubstitute;

namespace a_player
{
    public class moving_with_vertical_input
    {
        [UnityTest]
        public IEnumerator moving_forward()
        {
            //ARRANGE
            yield return TestHelper.LoadMovementTestScene();
            var player = TestHelper.GetPlayer();
            player.PlayerInput.Vertical.Returns(1f);

            float startingZPosition = player.transform.position.z;

            //ACT
            yield return new WaitForSeconds(0.5f);

            float endingZPosition = player.transform.position.z;

            //ASSERT
            Assert.Less(startingZPosition, endingZPosition);

        }

        [UnityTest]
        public IEnumerator moving_backwards()
        {
            //ARRANGE
            yield return TestHelper.LoadMovementTestScene();
            var player = TestHelper.GetPlayer();
            player.PlayerInput.Vertical.Returns(-1f);
            float startingZPosition = player.transform.position.z;

            //ACT
            yield return new WaitForSeconds(0.5f);

            float endingZPosition = player.transform.position.z;

            //ASSERT
            Assert.Greater(startingZPosition, endingZPosition);
        }

        [UnityTest]
        public IEnumerator standing_still()
        {
            //ARRANGE
            yield return TestHelper.LoadMovementTestScene();
            var player = TestHelper.GetPlayer();
            player.PlayerInput.Vertical.Returns(0);
            float startingZPosition = player.transform.position.z;

            //ACT
            yield return new WaitForSeconds(0.5f);

            float endingZPosition = player.transform.position.z;

            //ASSERT
            Assert.AreEqual(startingZPosition, endingZPosition);

        }
    }

}
