﻿using UnityEngine;
using System.Collections;

namespace BT {

	public class BTSelector : BTComposite {

		private int _activeChildIndex = -1;
		private int _previousSuccessChildIndex = -1;


		public int activeChildIndex {get {return _activeChildIndex;}}


		public override BTResult Tick () {
			for (int i=0; i<children.Count; i++) {
				BTNode child = children[i];

				switch (child.Tick()) {
				case BTResult.Running:
					if (_activeChildIndex != i && _activeChildIndex != -1) {
						children[_activeChildIndex].Clear();
					}
					_activeChildIndex = i;
					_previousSuccessChildIndex = -1;
					return BTResult.Running;

				case BTResult.Success:
					if (_activeChildIndex != i && _activeChildIndex != -1) {
						children[_activeChildIndex].Clear();
					}
					child.Clear();
					_activeChildIndex = -1;
					_previousSuccessChildIndex = i;
					return BTResult.Success;

				case BTResult.Failed:	
					child.Clear();
					continue;
				}
			}

			_activeChildIndex = -1;
			_previousSuccessChildIndex = -1;
			return BTResult.Failed;
		}

		public override void Clear () {
			switch (clearOpt) {
			case BTClearOpt.Default:
				if (_activeChildIndex != -1) {
					children[_activeChildIndex].Clear();
				}
				break;

			case BTClearOpt.Selected:
				foreach (BTNode child in selectedChildrenForClear) {
					int index = children.IndexOf(child);
					if (index > _previousSuccessChildIndex) {
						child.Clear();
					}
				}
				break;

			case BTClearOpt.DefaultAndSelected:
				if (_activeChildIndex != -1) {
					BTNode activeChild = children[_activeChildIndex];
					if (!selectedChildrenForClear.Contains(activeChild)) {
						activeChild.Clear();
					}
				}
				int split = Mathf.Max(_activeChildIndex, _previousSuccessChildIndex);
				foreach (BTNode child in selectedChildrenForClear) {
					int index = children.IndexOf(child);
					if (index > split) {
						child.Clear();
					}
				}
				break;

			case BTClearOpt.All:
				split = Mathf.Max(_activeChildIndex-1, _previousSuccessChildIndex);
				foreach (BTNode child in children) {
					int index = children.IndexOf(child);
					if (index > split) {
						child.Clear();
					}
				}
				break;
			}
			
			_activeChildIndex = -1;
			_previousSuccessChildIndex = -1;
		}
	}
}