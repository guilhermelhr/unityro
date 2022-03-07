using ROIO;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Canvas _displayControl;
    [SerializeField] private RectMask2D _mask;
    [SerializeField] private RawImage _mapBase;
    [SerializeField] private Button _btnPlus;
    [SerializeField] private Button _btnMinus;
    [SerializeField] private Button _btnMinimize;
    [SerializeField] private TextMeshProUGUI _coordinates;
    [SerializeField] private RawImage _playerIndicator;

    [SerializeField] private int m_nZoom = DEFAULT_ZOOM_INDEX;
    [SerializeField] private float m_fOffSetX = 0.0f;
    [SerializeField] private float m_fOffSetY = 0.0f;
    [SerializeField] private int m_nCenterX = 0;
    [SerializeField] private int m_nCenterY = 0;

    private Texture2D MapThumbTexture;
    private Texture2D PlayerIndicatorTexture;

    private const int DEFAULT_ZOOM_INDEX = 0;

    private string currentMap;

    private float m_fMapPicRealWidht = 0.0f;
    private float m_fMapPicRealHeight = 0.0f;

    private int m_nMapWidth;
    private int m_nMapHeight;

    private float m_f1MaskWidth = 0.0f;
    private float m_f1MaskHeight = 0.0f;

    private float[] m_fZoomValue =
    {
        0.25f,
        0.32f,
        0.425f,
        0.75f,
        1.0f
    };

    void Awake()
    {
        InitializeUI();

        //GameEventUI.EventUpdateCurrentMiniMap += OnEventUpdateMap;
        GameEventUI.EventUpdateCoordinateMiniMap += OnEventUpdateCoordinateMiniMap;
    }

    private void Start()
    {
        PlayerIndicatorTexture = FileManager.Load($"{DBManager.INTERFACE_PATH}map/map_arrow.bmp") as Texture2D;
        _playerIndicator.texture = PlayerIndicatorTexture;
        GameManager.OnMapLoaded += OnEventUpdateMap;
    }

    private void Update()
    {
        if(currentMap != null && _mapBase.texture == null)
        {
            OnEventUpdateMap();
        }
    }

    void LateUpdate()
    {
        if (Session.CurrentSession == null)
            return;

        UpdatePlayerArrow();
    }

    void OnDestroy()
    {
        //GameEventUI.EventUpdateCurrentMiniMap -= OnEventUpdateMap;
        GameEventUI.EventUpdateCoordinateMiniMap -= OnEventUpdateCoordinateMiniMap;
        GameManager.OnMapLoaded -= OnEventUpdateMap;
    }

    void InitializeUI()
    {
        _btnPlus.onClick.RemoveAllListeners();
        _btnPlus.onClick.AddListener(OnClickMiniMapPlus);
        _btnMinus.onClick.RemoveAllListeners();
        _btnMinus.onClick.AddListener(OnClickMiniMapMinus);

        /*
        Debug.Log("PASSEI AQUI");
        var arrow = FileManager.Load($"{DBManager.INTERFACE_PATH}map/map_arrow.bmp") as Texture2D;
        _playerIndicator.texture = arrow;
        Debug.Log("PLAYER INDICATOR SET");
        */
    }

    private void OnEventUpdateMap()
    {
        var player = Session.CurrentSession.Entity as Entity;
        currentMap = Session.CurrentSession.CurrentMap;

        if(player == null)
        {
            Debug.LogError("Player entity is null");
            return;
        }

        if(currentMap == null)
        {
            Debug.LogError("MapName is null");
            return;
        }

        var position = player.transform.position;

        MapThumbTexture = FileManager.Load($"{DBManager.INTERFACE_PATH}map/{currentMap}.bmp") as Texture2D;
        _mapBase.texture = MapThumbTexture;

        // Reset de Zoom
        m_nZoom = DEFAULT_ZOOM_INDEX;

        m_fMapPicRealWidht = _mapBase.rectTransform.rect.width;
        m_fMapPicRealHeight = _mapBase.rectTransform.rect.height;

        m_f1MaskWidth = _mask.rectTransform.sizeDelta.x;
        m_f1MaskHeight = _mask.rectTransform.sizeDelta.y;

        m_nMapWidth = (int)MapRenderer.width;
        m_nMapHeight = (int)MapRenderer.height;

        SetCoordinateMiniMap(position.x, position.z);
        UpdateMiniMapOffSet(true);
    }

    private void OnEventUpdateCoordinateMiniMap()
    {
        var player = Session.CurrentSession.Entity as Entity;
        SetCoordinateMiniMap(player.transform.position.x, player.transform.position.z);
    }

    private void SetCoordinateMiniMap(float x, float z)
    {
        _coordinates.text = $"{(int)x} {(int)z}";
    }

    private void UpdatePlayerArrow()
    {
        // Get the player
        var player = Session.CurrentSession.Entity as Entity;

        if(player == null)
        {
            return;
        }

        // Get the current position of the player
        var playerPosition = player.transform.position;

        if(playerPosition.x == 0 && playerPosition.z == 0)
        {
            return;
        }

        // MiniMap point
        int minimapPointX = 0;
        int minimapPointY = 0;
        ConvertMapXYToMiniMapXY((int)playerPosition.x, (int)playerPosition.z,
            ref minimapPointX, ref minimapPointY);

        // Update the player arrow in the minimap
        _playerIndicator.rectTransform.anchoredPosition3D = new Vector3(
            ConvertRealPixelPointX(minimapPointX), ConvertRealPixelPointY(minimapPointY), 0.0f);

        // Update de mark rotation
        _playerIndicator.rectTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, player.GetDirectionToDegrees());

        int vtW = 0;
        int vtH = 0;

        ConvertMapXYToMiniMapXY((int) playerPosition.x, (int) playerPosition.z,
            ref vtW, ref vtH);
        SetCenterPoint(vtW, vtH);
        UpdateMiniMapOffSet(false);
    }

    private void OnClickMiniMapPlus()
    {
        MiniMapZoom(true);
    }

    private void OnClickMiniMapMinus()
    {
        MiniMapZoom(false);
    }

    private void MiniMapZoom(bool zoomState)
    {
        var player = Session.CurrentSession.Entity as Entity;

        int w1 = 0;
        int h1 = 0;

        ConvertMapXYToMiniMapXY(
            (int)player.transform.position.x,
            (int)player.transform.position.z,
            ref w1, ref h1);

        if (zoomState)
        {
            ZoomOut(w1, h1);
        }
        else 
        {
            ZoomIn(w1, h1);
        }

        UpdateMiniMapOffSet(true);
    }

    private void UpdateMiniMapOffSet(bool isUpdateScale)
    {
        var scale = m_fZoomValue[m_nZoom];

        if (isUpdateScale)
        {
            _mapBase.rectTransform.localScale = new Vector2(scale, scale);
        }

        _mapBase.rectTransform.anchoredPosition3D = new Vector3(m_fOffSetX, m_fOffSetY, 0.0f);
    }

    private void ZoomOut(int f1X, int f1Y)
    {
        m_nZoom++;

        if(m_nZoom >= m_fZoomValue.Length)
        {
            m_nZoom = m_fZoomValue.Length - 1;
        }

        SetCenterPoint(f1X, f1Y);
    }

    private void ZoomIn(int f1X, int f1Y)
    {
        m_nZoom--;
        if(m_nZoom < 0)
        {
            m_nZoom = DEFAULT_ZOOM_INDEX;
            SetCenterPoint(f1X, f1Y);
            return;
        }

        if(m_fMapPicRealWidht * m_fZoomValue[m_nZoom] < m_f1MaskWidth ||
           m_fMapPicRealHeight * m_fZoomValue[m_nZoom] < m_f1MaskHeight)
        {
            ZoomOut(f1X, f1Y);
        }

        SetCenterPoint(f1X, f1Y);
    }

    private void SetCenterPoint(int nX, int nY)
    {
        m_nCenterX = nX;
        m_nCenterY = nY;
        m_fOffSetX = (m_f1MaskWidth / 2) - nX * m_fZoomValue[m_nZoom];
        m_fOffSetY = (m_f1MaskHeight / 2) - nY * m_fZoomValue[m_nZoom];

        m_fOffSetX = Math.Min(m_fOffSetX, 0);
        m_fOffSetY = Math.Min(m_fOffSetY, 0);
        m_fOffSetX = Math.Max(m_fOffSetX, -m_fMapPicRealWidht * m_fZoomValue[m_nZoom] + m_f1MaskWidth);
        m_fOffSetY = Math.Max(m_fOffSetY, -m_fMapPicRealHeight * m_fZoomValue[m_nZoom] + m_f1MaskHeight);
    }

    private void ConvertMapXYToMiniMapXY(int nMapX, int nMapY, ref int nMiniMapX, ref int nMiniMapY)
    {
        nMiniMapX = nMapX * (int) m_fMapPicRealWidht / m_nMapWidth;
        nMiniMapY = nMapY * (int) m_fMapPicRealHeight / m_nMapHeight;
    }

    private float ConvertRealPixelPointX(float f1X)
    {
        return f1X * m_fZoomValue[m_nZoom] + m_fOffSetX;
    }

    private float ConvertRealPixelPointY(float f1Y) {
        return f1Y * m_fZoomValue[m_nZoom] + m_fOffSetY;
    }
}
