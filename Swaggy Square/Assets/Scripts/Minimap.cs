using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform mapOrigin;
    [SerializeField]
    private RectTransform miniMapPlayer;
    [SerializeField]
    private RectTransform miniMapArea;
    [SerializeField]
    private float mapScale = 6f;

    void Update()
    {
        Vector3 relativePosition = player.position - mapOrigin.position;

        miniMapPlayer.anchoredPosition = new Vector2(relativePosition.x * mapScale, relativePosition.y * mapScale);
    }
}