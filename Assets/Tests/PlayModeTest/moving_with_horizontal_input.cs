using System.Collections;
using System.Collections.Generic;
using UnityEngine.TestTools;
using UnityEngine;
using NUnit.Framework;
using NSubstitute;

namespace a_player
{
    public class moving_with_horizontal_input
    {
        [UnityTest]
        public IEnumerator moving_left()
        {
            //Arrange
            yield return TestHelper.LoadMovementTestScene();

            var player = TestHelper.GetPlayer();


            player.PlayerInput.Horizontal.Returns(-1f);

            float startXPosition = player.transform.position.x;

            //Act
            yield return new WaitForSeconds(0.5f);

            float endingXPosition = player.transform.position.x;

            //Assert
            Assert.Greater(startXPosition, endingXPosition);
        }

        [UnityTest]
        public IEnumerator moving_right()
        {
            //Arrange
            yield return TestHelper.LoadMovementTestScene();

            var player = TestHelper.GetPlayer();


            player.PlayerInput.Horizontal.Returns(1f);

            float startXPosition = player.transform.position.x;

            //Act
            yield return new WaitForSeconds(0.5f);

            float endingXPosition = player.transform.position.x;

            //Assert
            Assert.Less(startXPosition, endingXPosition);
        }

    }
}

