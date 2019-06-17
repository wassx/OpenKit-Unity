using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Dynatrace.OpenKit;
using Dynatrace.OpenKit.API;
using Dynatrace.OpenKit.Unity.Logger;
using UnityEngine;
using XRTK.Definitions;
using XRTK.Services;

public class OpenKitService : BaseExtensionService {
    private static readonly string applicationName = "OpenKit Unity";
    private static readonly string applicationID = "<enterAppIdHere>";
    private static readonly string endpointURL = "<enterEndpointUrlHere>";
    private ISession _session;

    private readonly Dictionary<string, IRootAction> _rootActionDictionary = new Dictionary<string, IRootAction>();

    public IOpenKit OpenKit { get; private set; }


    public OpenKitService(string name, uint priority, BaseMixedRealityExtensionServiceProfile profile) : base(name,
        priority, profile) { }

    public override void Initialize() {
        if (!Application.isPlaying) {
            return;
        }

        if (ImportCerts()) {
            string operatingSystem = SystemInfo.operatingSystem;
            OpenKit = new DynatraceOpenKitBuilder(endpointURL, applicationID, "MyDeviceId 1")
                .WithApplicationName(applicationName)
                .WithOperatingSystem(operatingSystem)
                .WithLogger(new UnityLogger())
                .WithBeaconCacheMaxRecordAge(3600 * 1000)
                .WithBeaconCacheLowerMemoryBoundary(100 * 10000)
                .WithBeaconCacheUpperMemoryBoundary(150 * 10000)
                .Build();
        }

        Application.logMessageReceivedThreaded += MonitorExceptions;
    }


    public override void Destroy() {
        EndSession();
        OpenKit?.Shutdown();
    }

    public ISession GetSession() {
        if (_session == null) {
            _session = OpenKit?.CreateSession("");
        }

        return _session;
    }


    private bool ImportCerts() {
#if !UNITY_WSA || UNITY_EDITOR
        TextAsset[] certs = Resources.LoadAll("Certs", typeof(TextAsset)).Cast<TextAsset>().ToArray();
        Debug.Log(certs.Length + " certs found.");
        if (certs.Length > 0) {
            Debug.Log("Loading certs...");
            using (X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser)) {
                store.Open(OpenFlags.ReadWrite);
                if (store.Certificates.Count == 0) {
                    foreach (var cert in certs) {
                        store.Add(new X509Certificate2(cert.bytes));
                    }
                }
            }

            Debug.Log("Certs loaded");
            return true;
        } else {
            return false;
        }
#else
            return true;
#endif
    }

    public bool TryGetRootAction(string actionName, out IRootAction action) {
        if (_rootActionDictionary.ContainsKey(actionName)) {
            action = _rootActionDictionary[actionName];
            return true;
        }

        action = null;
        return false;
    }

    public IRootAction GetRootAction(string actionName) {
        IRootAction rootAction;
        TryGetRootAction(actionName, out rootAction);
        return rootAction;
    }

    /// <summary>
    /// Creates or gets an <see cref="IRootAction"/>.
    /// </summary>
    /// <param name="actionName"></param>
    /// <returns>An existing or newly created Action</returns>
    public IRootAction EnsureRootAction(string actionName) {
        IRootAction rootAction;
        if (TryGetRootAction(actionName, out rootAction)) {
            return rootAction;
        }

        rootAction = GetSession()?.EnterAction(actionName);
        _rootActionDictionary.Add(actionName, rootAction);
        return rootAction;
    }

    public void LeaveRootAction(string actionName) {
        IRootAction rootAction;
        if (TryGetRootAction(actionName, out rootAction)) {
            rootAction?.LeaveAction();
            _rootActionDictionary.Remove(actionName);
        }
    }

    public void LeaveRootAction(IRootAction action) {
        if (action != null && _rootActionDictionary.ContainsValue(action)) {
            LeaveRootAction(_rootActionDictionary.Where(v => v.Value == action).First().Key);
        } else {
            action?.LeaveAction();
        }
    }

    public void EndSession() {
        foreach (KeyValuePair<string, IRootAction> keyValuePair in _rootActionDictionary) {
            keyValuePair.Value?.LeaveAction();
        }

        _rootActionDictionary.Clear();

        GetSession()?.End();
        _session = null;
    }

    private void MonitorExceptions(string condition, string stackTrace, LogType type) {
        if (OpenKit == null) {
            return;
        }

        if (type == LogType.Exception) {
            GetSession()?.ReportCrash(condition, condition, stackTrace);
        }
    }

    protected override void OnDispose(bool finalizing) {
        _session?.Dispose();
        OpenKit?.Dispose();
    }
}