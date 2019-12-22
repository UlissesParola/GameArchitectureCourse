using UnityEngine;
using NSubstitute;
using System;

namespace a_player
{
    public class TestHelper
    {
        public static void CreateFloor()
        {
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.transform.localScale = new Vector3(50, 0.1f, 50);
            floor.transform.position = Vector3.zero;
        }

        public static Player CreatePlayer()
        {
            var playerObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            playerObject.transform.position = new Vector3(0, 1.30f, 0);
            playerObject.AddComponent<CharacterController>();

            Player player = playerObject.AddComponent<Player>();

            var playerObjectInput = Substitute.For<IPlayerInput>();
            player.PlayerInput = playerObjectInput;

            return player;
        }

        public static float CalculateRotation(Quaternion startingRotation, Quaternion endingRotation)
        {
            var crossProduct = Vector3.Cross(startingRotation * Vector3.forward, endingRotation * Vector3.forward);
            var dotProduct = Vector3.Dot(crossProduct, Vector3.up);

            return dotProduct;
        }
    }
}

