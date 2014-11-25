using UnityEngine;

public class ProgressBar : MonoBehaviour
{

    public float Progress;
    public Vector2 Position;
    public Vector2 Size;
    public bool UseSimpleBackground;
    public bool UseSimplePartial;
    public bool UseEndColor;
    public Color BackgroundColor;
    public Color StartColor;
    public Color EndColor;
    public Texture2D ProgressBarEmpty;
    public Texture2D ProgressBarFull;

    public void OnGUI()
    {
        Rect fullSize = new Rect(Position.x * Screen.width, Position.y * Screen.height, Size.x * Screen.width, Size.y * Screen.height);
        Rect partialSize = new Rect(Position.x * Screen.width, Position.y * Screen.height, (Size.x * Mathf.Clamp01(Progress)) * Screen.width, Size.y * Screen.height);
        
        if (UseSimpleBackground)
        {
            Texture2D t = new Texture2D(1, 1);
            t.wrapMode = TextureWrapMode.Repeat;
            t.SetPixel(0, 0, BackgroundColor);
            t.Apply();
            GUI.DrawTexture(fullSize, t);
        } else if (ProgressBarEmpty != null)
        {
            GUI.DrawTexture(fullSize, ProgressBarEmpty);
        }
        
        if (UseSimplePartial)
        {
            Texture2D t = new Texture2D(1, 1);
            t.wrapMode = TextureWrapMode.Repeat;
            t.SetPixel(0, 0, FadeColor());
            t.Apply();
            GUI.DrawTexture(partialSize, t);
        } else if (ProgressBarFull != null)
        {
            GUI.DrawTexture(partialSize, ProgressBarFull, ScaleMode.ScaleAndCrop, false, Size.x / Size.y);
        }
    }
    
    private Color FadeColor()
    {
        if (UseEndColor)
        {
            float clampProgress = Mathf.Clamp01(Progress);
            float rDiff = (StartColor.r - EndColor.r) * clampProgress;
            float gDiff = (StartColor.g - EndColor.g) * clampProgress;
            float bDiff = (StartColor.b - EndColor.b) * clampProgress;
            
            Color retVal = new Color(EndColor.r + rDiff, EndColor.g + gDiff, EndColor.b + bDiff);
            return retVal;
        } else
        {
            return StartColor;
        }
    }
    
    private float? progressStepPerSecond = null;
    private float? finalProgress = null;
    
    public void Update()
    {
        if (progressStepPerSecond.HasValue && finalProgress.HasValue)
        {
            float step = Time.deltaTime * progressStepPerSecond.Value;
            Progress += step;
            bool done = false;
            if ((step < 0.0f && Progress <= finalProgress.Value) || (step > 0.0f && Progress >= finalProgress.Value))
            {
                done = true;
            }
            if (done)
            {
                Progress = finalProgress.Value;
                progressStepPerSecond = null;
                finalProgress = null;
            }
        }
    }
    
    public void AnimateProgress(float progress, float seconds)
    {
        finalProgress = progress;
        progressStepPerSecond = (progress - Progress) / seconds;
    }
}
