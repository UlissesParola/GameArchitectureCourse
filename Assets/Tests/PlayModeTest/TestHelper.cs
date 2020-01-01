using UnityEngine;
using NSubstitute;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace a_player
{
    public class TestHelper
    {
        public static IEnumerator LoadTestScene()
        {
            var operation = SceneManager.LoadSceneAsync("TestScene");
            while(operation.isDone == false)
            {
                yield return null;
            }
        }

        public static Player GetPlayer()
        {
            Player player = GameObject.FindObjectOfType<Player>();

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

