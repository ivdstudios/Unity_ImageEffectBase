using UnityEngine;

/// <summary>
/// ImageEffect の基底クラスです。
/// </summary>
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class ImageEffectBase : MonoBehaviour
{
    #region Field

    // NOTE:
    // スクリプトからシェーダを参照してマテリアルを自動生成する手段もありますが止めました。
    // Shader.Find は、ビルトインシェーダの設定が必要になり、見通しが良くありません。
    // 他にシェーダを Inspector から設定する方法もありますが、2 つ良くない点がありました。
    // (1) Inspector からマテリアルの設定値を変更することができない。
    // (2) Material が生成済みかどうかをレンダリングの度に検証する必要がある。

    /// <summary>
    /// ImageEffect のマテリアル。
    /// </summary>
    public Material material;

    /// <summary>
    /// カメラへの参照。
    /// </summary>
    protected new Camera camera;

    /// <summary>
    /// カメラの DepthTextureMode 。
    /// </summary>
    public DepthTextureMode depthTextureMode;

    #endregion Field

    #region Method

    /// <summary>
    /// 初期化時に呼び出されます。
    /// </summary>
    protected virtual void Awake()
    {
        this.camera = base.GetComponent<Camera>();
        this.camera.depthTextureMode = this.depthTextureMode;
    }

    /// <summary>
    /// 初期化時に呼び出されます。
    /// </summary>
    protected virtual void Start()
    {
         // NOTE:
        // Awake 時でも良いように見えますが、問題があります。
        // Awake 時に呼び出すと、material への参照が設定されている場合でも、material が null になります。

        DisableWhenImageEffectsInvalid();
    }

    /// <summary>
    /// 描画時に呼び出されます。
    /// </summary>
    /// <param name="source">
    /// 描画元の RenderTexture 。
    /// </param>
    /// <param name="destination">
    /// 描画先の RenderTexture 。
    /// </param>
    protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, this.material);
    }

    /// <summary>
    /// Inspector の更新時に呼び出されます。
    /// </summary>
    protected virtual void OnValidate()
    {
        if (this.camera != null)
        {
            this.camera.depthTextureMode = this.depthTextureMode;
        }
    }

    /// <summary>
    /// ImageEffect が無効なとき Disable にします。
    /// </summary>
    protected void DisableWhenImageEffectsInvalid()
    {
        if (!SystemInfo.supportsImageEffects
         || !this.material
         || !this.material.shader.isSupported)
        {
            base.enabled = false;
        }
    }

    #endregion Method
}