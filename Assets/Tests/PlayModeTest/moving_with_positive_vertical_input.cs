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
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.transform.localScale = new Vector3(50, 0.1f, 50);
            floor.transform.position = Vector3.zero;

            var playerObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            playerObject.AddComponent<CharacterController>();
            playerObject.transform.position = new Vector3(0, 1.10f, 0);

            Player player = playerObject.AddComponent<Player>();

            float startingZPosition = playerObject.transform.position.z;

            var playerInput = Substitute.For<PlayerInput>();
            playerInput.Vertical.Returns(1f);

            player.PlayerInput = playerInput;

            yield return new WaitForSeconds(1f);

            float endingZPosition = playerObject.transform.position.z;

            //ACT
            Assert.Less(startingZPosition, endingZPosition);

        }

        [UnityTest]
        public IEnumerator moving_backwards()
        {
            //ARRANGE
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.transform.localScale = new Vector3(50, 0.1f, 50);
            floor.transform.position = Vector3.zero;

            var playerObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            playerObject.AddComponent<CharacterController>();
            playerObject.transform.position = new Vector3(0, 1.10f, 0);

            Player player = playerObject.AddComponent<Player>();

            float startingZPosition = playerObject.transform.position.z;

            var playerInput = Substitute.For<PlayerInput>();
            playerInput.Vertical.Returns(-1f);

            player.PlayerInput = playerInput;

            yield return new WaitForSeconds(1f);

            float endingZPosition = playerObject.transform.position.z;

            //ACT
            Assert.Greater(startingZPosition, endingZPosition);
        }

        [UnityTest]
        public IEnumerator standing_still()
        {
            //ARRANGE
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.transform.localScale = new Vector3(50, 0.1f, 50);
            floor.transform.position = Vector3.zero;

            var playerObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            playerObject.AddComponent<CharacterController>();
            playerObject.transform.position = new Vector3(0, 1.10f, 0);

            Player player = playerObject.AddComponent<Player>();

            float startingZPosition = playerObject.transform.position.z;

            var playerInput = Substitute.For<PlayerInput>();
            playerInput.Vertical.Returns(0);

            player.PlayerInput = playerInput;

            yield return new WaitForSeconds(0.5f);

            float endingZPosition = playerObject.transform.position.z;

            //ACT
            Assert.AreEqual(startingZPosition, endingZPosition);

        }
    }

}
