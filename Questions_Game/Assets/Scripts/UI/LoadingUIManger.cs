using PizzaPie.QuestionsGame.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.QuestionsGame.UI
{
    public class LoadingUIManger : MonoBehaviour , IState 
    {
        [SerializeField]
        private GameObject parentUI;
        private StateMachine stateMachine;

        public StateType GetStateType => StateType.LOADING;

        public void Handler(object sender, LoadEventArgs e)
        {
            e.SequenceLoader.AddOnFinishAction(() => parentUI.SetActive(false));
        }

        public void Init(StateMachine stateMachine)
        {
            parentUI.SetActive(false);
            this.stateMachine = stateMachine;
        }

        public void Enter()
        {
            parentUI.SetActive(true);
            var sequence = new Unity.Utils.SequenceLoader();
            sequence.AddOnFinishAction(()=>stateMachine.ChangeState(StateType.WHEEL));
            Services.Instance.EventAggregator.Invoke
                (this, new LoadEventArgs(sequence, stateMachine.GetBlackBoardValue<DifficultyDefinition>().GameDifficulty));
            sequence.Start();
        }

        public void Exit()
        {
            parentUI.SetActive(false);
        }

        public void _Reset()
        {
        }

        public void Interupt()
        {
        }
    }
}
