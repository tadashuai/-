//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;
[ExecuteAlways]
[CustomEditor( typeof( UITweener ), true )]
public class UITweenerEditor : Editor
{
    public UITweener tweener;

    protected void Awake()
    {
        tweener = target as UITweener;
        tweener.m_PreTime = ( float )EditorApplication.timeSinceStartup;
    }

    public void CustomUpdate()
    {
        if ( tweener == null )
            return;

        if ( tweener.m_Duration != tweener.duration )
        {
            tweener.m_Duration = tweener.duration;
            tweener.m_AmountPerDelta = Mathf.Abs( ( tweener.duration > 0f ) ? 1f / tweener.duration : 1000f ) * Mathf.Sign( tweener.m_AmountPerDelta );
        }
        tweener.m_amountPerDelta = tweener.m_AmountPerDelta;

        float delta = ( float )EditorApplication.timeSinceStartup - tweener.m_PreTime;
        tweener.m_PreTime = ( float )EditorApplication.timeSinceStartup;

        if ( tweener.isPlay && !tweener.isStart )
        {
            tweener.isStart = true;
            tweener.m_StartTime = tweener.m_PreTime + tweener.delay;
        }

        if ( tweener.m_StartTime > tweener.m_PreTime )
            return;
        else if ( !tweener.isPlay )
            tweener.m_StartTime += delta;

        if ( !tweener.isPlay )
            return;

        // Advance the sampling factor
        tweener.m_Factor += tweener.m_amountPerDelta * delta;

        // Loop style simply resets the play factor after it exceeds 1.
        if ( tweener.style == UITweener.Style.Loop )
        {
            if ( tweener.m_Factor > 1f )
            {
                tweener.m_Factor -= Mathf.Floor( tweener.m_Factor );
            }
        }
        else if ( tweener.style == UITweener.Style.PingPong )
        {
            // Ping-pong style reverses the direction
            if ( tweener.m_Factor > 1f )
            {
                tweener.m_Factor = 1f - ( tweener.m_Factor - Mathf.Floor( tweener.m_Factor ) );
                tweener.m_AmountPerDelta = -tweener.m_AmountPerDelta;
            }
            else if ( tweener.m_Factor < 0f )
            {
                tweener.m_Factor = -tweener.m_Factor;
                tweener.m_Factor -= Mathf.Floor( tweener.m_Factor );
                tweener.m_AmountPerDelta = -tweener.m_AmountPerDelta;
            }
        }
        else if ( tweener.style == UITweener.Style.PingPongOnce )
        {
            // Ping-pong style reverses the direction
            if ( tweener.m_Factor > 1f )
            {
                tweener.m_Factor = 1f - ( tweener.m_Factor - Mathf.Floor( tweener.m_Factor ) );
                tweener.m_AmountPerDelta = -tweener.m_AmountPerDelta;
                if ( tweener.m_PingPongOneCount == 2 )
                {
                    tweener.ResetToBeginning();
                    tweener.enabled = false;
                }
                tweener.m_PingPongOneCount++;
            }
            else if ( tweener.m_Factor < 0f )
            {
                tweener.m_Factor = -tweener.m_Factor;
                tweener.m_Factor -= Mathf.Floor( tweener.m_Factor );
                tweener.m_AmountPerDelta = -tweener.m_AmountPerDelta;
                if ( tweener.m_PingPongOneCount == 2 )
                {
                    tweener.ResetToBeginning();
                    tweener.enabled = false;
                }
                tweener.m_PingPongOneCount++;
            }
        }

        if ( ( tweener.style == UITweener.Style.Once ) && ( tweener.duration == 0f || tweener.m_Factor > 1f || tweener.m_Factor < 0f ) )
        {
            tweener.m_Factor = 1f;
            tweener.Sample( 1f, false );
        }
        else tweener.Sample( tweener.m_Factor, false );

        NGUITools.SetDirty( tweener );
    }

    protected void OnEnable()
    {
        EditorApplication.update += CustomUpdate;
    }

    protected void OnDisable()
    {
        EditorApplication.update -= CustomUpdate;
        tweener.m_PingPongOneCount = 0;
        tweener.m_PreTime = 0f;
        tweener.isPlay = false;
        tweener.isStart = false;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space( 6f );
        NGUIEditorTools.SetLabelWidth( 110f );
        base.OnInspectorGUI();
        DrawCommonProperties();
    }

    protected void DrawCommonProperties()
    {
        UITweener tw = target as UITweener;

        if ( NGUIEditorTools.DrawHeader( "Tweener" ) )
        {
            NGUIEditorTools.BeginContents();
            NGUIEditorTools.SetLabelWidth( 110f );

            GUI.changed = false;

            UITweener.Style style = ( UITweener.Style )EditorGUILayout.EnumPopup( "Play Style", tw.style );
            AnimationCurve curve = EditorGUILayout.CurveField( "Animation Curve", tw.animationCurve, GUILayout.Width( 170f ), GUILayout.Height( 62f ) );
            //UITweener.Method method = (UITweener.Method)EditorGUILayout.EnumPopup("Play Method", tw.method);

            GUILayout.BeginHorizontal();
            float dur = EditorGUILayout.FloatField( "Duration", tw.duration, GUILayout.Width( 170f ) );
            GUILayout.Label( "seconds" );
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            float del = EditorGUILayout.FloatField( "Start Delay", tw.delay, GUILayout.Width( 170f ) );
            GUILayout.Label( "seconds" );
            GUILayout.EndHorizontal();

            int tg = EditorGUILayout.IntField( "Tween Group", tw.tweenGroup, GUILayout.Width( 170f ) );
            bool ts = EditorGUILayout.Toggle( "Ignore TimeScale", tw.ignoreTimeScale );

            if ( GUI.changed )
            {
                NGUIEditorTools.RegisterUndo( "Tween Change", tw );
                tw.animationCurve = curve;
                //tw.method = method;
                tw.style = style;
                tw.ignoreTimeScale = ts;
                tw.tweenGroup = tg;
                tw.duration = dur;
                tw.delay = del;
                NGUITools.SetDirty( tw );
            }

            NGUIEditorTools.EndContents();
        }

        NGUIEditorTools.SetLabelWidth( 80f );
        NGUIEditorTools.DrawEvents( "On Finished", tw, tw.onFinished );
    }

    protected void OnSceneGUI()
    {
        tweener = target as UITweener;
        Selection.activeGameObject = tweener.gameObject;

        Handles.BeginGUI();

        Rect windowRect = new Rect( Screen.width - 210, 25, 200, 65 );
        GUI.Window( 1, windowRect, DrawNodeWindow, new GUIContent( "Tween Preview" ) );

        Handles.EndGUI();
    }

    protected void DrawNodeWindow( int id )
    {
        GUILayout.BeginHorizontal();

        if ( GUILayout.Button( !tweener.isPlay ? "Play" : "Pause", "ButtonLeft" ) )
        {
            tweener.OnPlayOrPause( !tweener.isPlay );
        }
        if ( GUILayout.Button( "Restart", "ButtonMid" ) )
        {
            tweener.OnRestart();
        }
        if ( GUILayout.Button( "Stop", "ButtonRIght" ) )
        {
            tweener.OnStop();
        }

        GUILayout.EndHorizontal();

        GUILayout.Label( "预览无视“Ignore TimeScale”选项" );
    }
}
