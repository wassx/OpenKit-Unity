using System.Collections.Generic;
using System.Threading.Tasks;
using Dynatrace.OpenKit.API;
using UnityEngine;
using UnityEngine.UI;
using XRTK.Services;

public class OpenKitTest : MonoBehaviour {


    [SerializeField] private Button _startActionBtn;
    [SerializeField] private Button _startTaskBtn;
    private IRootAction _rootAction;

    // Start is called before the first frame update
    void Start() {
        _rootAction = MixedRealityToolkit.GetService<OpenKitService>().EnsureRootAction("Test Root Action");
        _startActionBtn.onClick.AddListener(OnActionButtonClicked);
        _startTaskBtn.onClick.AddListener(OnStartTaskButtonClicked);
        _rootAction.ReportEvent("Added button listeners");
    }

    private void OnDestroy() {
        _rootAction.ReportEvent("OnDestroy").LeaveAction();
    }

    private async void OnStartTaskButtonClicked() {
        Task task = new Task(() => {
            IAction taskAction = _rootAction.EnterAction("Task action start.");
            Task.Delay(3000);
            taskAction.ReportEvent("Task delayed 3s.").LeaveAction();
        });
        await task;
    }

    private void OnActionButtonClicked() {
        List<string> list = new List<string>();
        string test = list[3]; //throw exception
    }

}