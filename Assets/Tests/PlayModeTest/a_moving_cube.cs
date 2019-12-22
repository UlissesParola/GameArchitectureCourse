using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace a_player
{
    public class a_moving_cube
    {
        [UnityTest]
        public IEnumerator moving_forward_changes_position()
        {
            //ARRANGE
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = Vector3.zero;

            for(int i = 0; i < 10; i++)
            {
                //ACT
                cube.transform.position += Vector3.forward;

                //ASSERT
                // Use the Assert class to test conditions.
                Assert.AreEqual( i+1 , cube.transform.position.z);
                // Use yield to skip a frame.
                yield return null;
            }

            GameObject.Destroy(cube);
        }
    }
}
