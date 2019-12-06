using System.Collections;
using System.Collections.Generic;
using UnityEngine.TestTools;
using UnityEngine;
using NUnit.Framework;
using NSubstitute;

public class moving_with_horizontal_input
{
    [UnityTest]
    public IEnumerator moving_left()
    {
        //Arrange
        var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.transform.localScale = new Vector3(50, 0.1f, 50);
        floor.transform.position = Vector3.zero;

        var playerObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        playerObject.transform.position = new Vector3(0, 1.10f, 0);
        playerObject.AddComponent<CharacterController>();

        Player player = playerObject.AddComponent<Player>();

        var PlayerObjectInput = Substitute.For<PlayerInput>();

        player.PlayerInput = PlayerObjectInput;

        PlayerObjectInput.Horizontal.Returns(-1f);

        float startXPosition = playerObject.transform.position.x;

        //Act
        yield return new WaitForSeconds(5f);

        float endingXPosition = playerObject.transform.position.x;

        //Assert
        Assert.Greater(startXPosition, endingXPosition);

    }

}
