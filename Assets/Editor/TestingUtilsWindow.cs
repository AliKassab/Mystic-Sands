using System.Reflection;
using DialogueEditor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Developer cheat panel for fast iteration: jump between scenes, set quest flags,
/// and drive the maze rounds without playing through. Editor-only (lives in an Editor
/// folder), so it is never included in a build.
/// Open via: Tools > Mystic Sands > Testing Utils.
/// </summary>
public class TestingUtilsWindow : EditorWindow
{
    private Vector2 _scroll;

    private const BindingFlags Flags =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    // Per-conversation expand state + cached deserialized graph (keyed by instance id).
    private readonly System.Collections.Generic.Dictionary<int, bool> _expanded =
        new System.Collections.Generic.Dictionary<int, bool>();
    private readonly System.Collections.Generic.Dictionary<int, Conversation> _graphs =
        new System.Collections.Generic.Dictionary<int, Conversation>();

    // Fully qualified: the game defines its own global `MenuItem` class that otherwise shadows the attribute.
    [UnityEditor.MenuItem("Tools/Mystic Sands/Testing Utils %#t")] // Ctrl/Cmd+Shift+T
    private static void Open()
    {
        var window = GetWindow<TestingUtilsWindow>("Testing Utils");
        window.minSize = new Vector2(280, 360);
        window.Show();
    }

    private void OnInspectorUpdate() => Repaint(); // keep runtime state readout live

    private void OnGUI()
    {
        _scroll = EditorGUILayout.BeginScrollView(_scroll);

        DrawScenes();
        EditorGUILayout.Space(6);
        DrawTime();
        EditorGUILayout.Space(6);
        DrawQuestFlags();
        EditorGUILayout.Space(6);
        DrawConversations();
        EditorGUILayout.Space(6);
        DrawMaze();

        EditorGUILayout.EndScrollView();
    }

    // ----------------------------------------------------------------- Scenes
    private void DrawScenes()
    {
        Header("Scenes");
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            var name = System.IO.Path.GetFileNameWithoutExtension(scene.path);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(name, GUILayout.MinWidth(90));

            if (GUILayout.Button("Open", GUILayout.Width(60)))
                LoadScene(scene.path, false);
            if (GUILayout.Button("Play", GUILayout.Width(60)))
                LoadScene(scene.path, true);

            EditorGUILayout.EndHorizontal();
        }

        if (Application.isPlaying)
            EditorGUILayout.HelpBox("Current: " + SceneManager.GetActiveScene().name, MessageType.None);
    }

    private void LoadScene(string path, bool play)
    {
        if (play)
        {
            if (!Application.isPlaying)
            {
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
                EditorSceneManager.OpenScene(path);
                EditorApplication.isPlaying = true;
            }
            else
            {
                SceneManager.LoadScene(path);
            }
        }
        else
        {
            if (Application.isPlaying)
            {
                SceneManager.LoadScene(path);
            }
            else if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(path);
            }
        }
    }

    // ------------------------------------------------------------------- Time
    private void DrawTime()
    {
        Header("Time");
        EditorGUILayout.BeginHorizontal();
        foreach (var s in new[] { 0f, 0.25f, 1f, 2f, 4f })
        {
            if (GUILayout.Button(s == 0f ? "Pause" : s + "x"))
                Time.timeScale = s;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("timeScale", Time.timeScale.ToString("0.00"));
    }

    // ------------------------------------------------------------ Quest flags
    private void DrawQuestFlags()
    {
        Header("Quest Flags (Flagger)");
        var flagger = FindByTypeName("Flagger");
        if (flagger == null)
        {
            EditorGUILayout.HelpBox("No Flagger in the loaded scene. Enter Play or open OpenWorld.",
                MessageType.Info);
            return;
        }

        FlagToggle(flagger, "MetPotTrader");
        FlagToggle(flagger, "MetCommander");
        FlagToggle(flagger, "InitiateEnding");

        EditorGUILayout.Space(2);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Good Ending"))
        {
            // Good ending = met pot trader, NOT commander (see Flagger).
            SetBool(flagger, "MetPotTrader", true);
            SetBool(flagger, "MetCommander", false);
            SetBool(flagger, "InitiateEnding", true);
        }
        if (GUILayout.Button("Secret Ending"))
        {
            SetBool(flagger, "MetPotTrader", true);
            SetBool(flagger, "MetCommander", true);
            SetBool(flagger, "InitiateEnding", true);
        }
        if (GUILayout.Button("Reset"))
        {
            SetBool(flagger, "MetPotTrader", false);
            SetBool(flagger, "MetCommander", false);
            SetBool(flagger, "InitiateEnding", false);
        }
        EditorGUILayout.EndHorizontal();
    }

    private void FlagToggle(MonoBehaviour target, string field)
    {
        var fi = target.GetType().GetField(field, Flags);
        if (fi == null) return;
        bool cur = (bool)fi.GetValue(target);
        bool next = EditorGUILayout.ToggleLeft(field, cur);
        if (next != cur) SetBool(target, field, next);
    }

    private void SetBool(MonoBehaviour target, string field, bool value)
    {
        var fi = target.GetType().GetField(field, Flags);
        if (fi == null) return;
        Undo.RecordObject(target, "Set " + field);
        fi.SetValue(target, value);
        EditorUtility.SetDirty(target);
    }

    // ---------------------------------------------------------- Conversations
    // Quest progression is driven by the UnityEvents on conversation nodes
    // (TriggerNextDialogue / BlockDialogue / Flagger sets / ...). This panel deserializes
    // each conversation's graph and lets you fire any individual node or option event, so
    // you can walk whichever branch/ending you want instead of forcing the whole thing.
    private void DrawConversations()
    {
        Header("Conversations (fire any node/option)");

        var convos = Object.FindObjectsByType<NPCConversation>(
            FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

        if (convos == null || convos.Length == 0)
        {
            EditorGUILayout.HelpBox("No NPCConversation in the loaded scene. Open OpenWorld or enter Play.",
                MessageType.Info);
            return;
        }

        EditorGUILayout.HelpBox("Expand a conversation, then Fire the node/option whose event " +
            "leads where you want (e.g. the bad-ending node). Best run in Play mode.", MessageType.None);

        foreach (var c in convos)
        {
            int id = c.GetInstanceID();
            _expanded.TryGetValue(id, out bool open);

            EditorGUILayout.BeginHorizontal();
            open = EditorGUILayout.Foldout(open, c.gameObject.name, true);
            _expanded[id] = open;
            if (GUILayout.Button("Fire all", GUILayout.Width(64)))
                FireConversationEvents(c);
            EditorGUILayout.EndHorizontal();

            if (open)
                DrawConversationTree(c, id);
        }

        EditorGUILayout.Space(2);
        if (GUILayout.Button("Fire ALL conversations (everything)"))
            foreach (var c in convos) FireConversationEvents(c);
    }

    private void DrawConversationTree(NPCConversation c, int id)
    {
        if (!_graphs.TryGetValue(id, out var convo) || convo == null)
        {
            try { convo = c.Deserialize(); }
            catch (System.Exception e)
            {
                EditorGUILayout.HelpBox("Could not read graph: " + e.Message, MessageType.Warning);
                return;
            }
            _graphs[id] = convo;
        }

        EditorGUI.indentLevel++;
        if (GUILayout.Button("Refresh graph", GUILayout.Width(110)))
            _graphs.Remove(id);

        if (convo == null || convo.Root == null)
        {
            EditorGUILayout.LabelField("(empty conversation)");
            EditorGUI.indentLevel--;
            return;
        }

        var ordered = new System.Collections.Generic.List<(ConversationNode node, int depth)>();
        Walk(convo.Root, 0, ordered, new System.Collections.Generic.HashSet<ConversationNode>());

        foreach (var (node, depth) in ordered)
        {
            var ev = GetEvent(node);
            bool option = node is OptionNode;
            string prefix = new string(' ', depth * 2) + (option ? "▸ " : "• ");
            string body = option ? Snip(node.Text) : (((SpeechNode)node).Name + ": " + Snip(node.Text));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(prefix + body, GUILayout.MinWidth(140));

            int count = ev != null ? ev.GetPersistentEventCount() : 0;
            using (new EditorGUI.DisabledScope(count == 0))
            {
                if (GUILayout.Button(count == 0 ? "-" : "Fire", GUILayout.Width(50)))
                    FireEvent(ev, c, body);
            }
            EditorGUILayout.EndHorizontal();

            string listeners = Listeners(ev);
            if (listeners != null)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField(listeners, EditorStyles.miniLabel);
                EditorGUI.indentLevel--;
            }
        }
        EditorGUI.indentLevel--;
    }

    private static void Walk(ConversationNode node, int depth,
        System.Collections.Generic.List<(ConversationNode, int)> outp,
        System.Collections.Generic.HashSet<ConversationNode> seen)
    {
        if (node == null || seen.Contains(node)) return;
        seen.Add(node);
        outp.Add((node, depth));
        foreach (var conn in node.Connections)
        {
            if (conn is SpeechConnection sc) Walk(sc.SpeechNode, depth + 1, outp, seen);
            else if (conn is OptionConnection oc) Walk(oc.OptionNode, depth + 1, outp, seen);
        }
    }

    private static UnityEngine.Events.UnityEvent GetEvent(ConversationNode n) =>
        (n as SpeechNode)?.Event ?? (n as OptionNode)?.Event;

    private static string Snip(string s)
    {
        if (string.IsNullOrEmpty(s)) return "(no text)";
        s = s.Replace("\n", " ").Trim();
        return s.Length > 38 ? s.Substring(0, 38) + "…" : s;
    }

    private static string Listeners(UnityEngine.Events.UnityEventBase ev)
    {
        if (ev == null) return null;
        int n = ev.GetPersistentEventCount();
        if (n == 0) return null;
        var parts = new System.Collections.Generic.List<string>();
        for (int i = 0; i < n; i++)
        {
            var t = ev.GetPersistentTarget(i);
            parts.Add(ev.GetPersistentMethodName(i) + (t != null ? " → " + t.name : ""));
        }
        return string.Join(",  ", parts);
    }

    private void FireEvent(UnityEngine.Events.UnityEvent ev, Object ctx, string label)
    {
        if (ev == null) return;
        try
        {
            ev.Invoke();
            Debug.Log($"TestingUtils: fired '{label}'.", ctx);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"TestingUtils: '{label}' event failed: {e.Message}", ctx);
        }
    }

    private void FireConversationEvents(NPCConversation c)
    {
        foreach (var holder in c.GetComponentsInChildren<NodeEventHolder>(true))
        {
            if (holder == null || holder.Event == null || holder.Event.GetPersistentEventCount() == 0)
                continue;
            try { holder.Event.Invoke(); }
            catch (System.Exception e)
            {
                Debug.LogError($"TestingUtils: event on '{c.gameObject.name}' node {holder.NodeID} " +
                    $"failed: {e.Message}", c);
            }
        }
        Debug.Log($"TestingUtils: fired all events for '{c.gameObject.name}'.", c);
    }

    // ------------------------------------------------------------------- Maze
    private void DrawMaze()
    {
        Header("Maze (GameManager)");

        var gm = FindByTypeName("GameManager");
        var maze = FindByTypeName("MazeRenderer");

        if (gm != null)
        {
            var round = gm.GetType().GetField("roundCounter", Flags);
            var timer = gm.GetType().GetField("timer", Flags);
            if (round != null) EditorGUILayout.LabelField("Round", round.GetValue(gm).ToString());
            if (timer != null) EditorGUILayout.LabelField("Time left", ((float)timer.GetValue(gm)).ToString("0.0"));
        }
        else
        {
            EditorGUILayout.HelpBox("No GameManager loaded.", MessageType.Info);
        }

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Maze actions need Play mode.", MessageType.None);
            return;
        }

        EditorGUILayout.BeginHorizontal();
        if (maze != null && GUILayout.Button("Rebuild Maze"))
            Invoke(maze, "buildNewMaze");
        if (gm != null && GUILayout.Button("Skip Round"))
            Invoke(gm, "ResetMap");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (gm != null && GUILayout.Button("Force Win"))
            Invoke(gm, "RunEndSequence", true);
        if (gm != null && GUILayout.Button("Force Lose"))
            Invoke(gm, "RunEndSequence", false);
        EditorGUILayout.EndHorizontal();
    }

    // Game scripts live in Assembly-CSharp, while this window is in the Editor assembly,
    // so the type must be resolved with its assembly qualifier.
    private static MonoBehaviour FindByTypeName(string typeName)
    {
        var type = System.Type.GetType(typeName + ", Assembly-CSharp");
        return type == null ? null : Object.FindAnyObjectByType(type) as MonoBehaviour;
    }

    private void Invoke(MonoBehaviour target, string method, params object[] args)
    {
        var mi = target.GetType().GetMethod(method, Flags);
        if (mi != null) mi.Invoke(target, args);
        else Debug.LogWarning($"TestingUtils: '{method}' not found on {target.GetType().Name}.");
    }

    private static void Header(string title)
    {
        EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
        var r = EditorGUILayout.GetControlRect(false, 1);
        EditorGUI.DrawRect(r, new Color(0.5f, 0.5f, 0.5f, 0.4f));
    }
}
