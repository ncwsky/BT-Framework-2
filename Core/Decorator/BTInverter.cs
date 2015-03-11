﻿using UnityEngine;
using System.Collections;

namespace BT {

	public class BTInverter : BTDecorator {

		public BTInverter (BTNode child) : base (child) {}

		public override BTResult Tick () {
			switch (child.Tick()) {
			case BTResult.Running:
				return BTResult.Running;
			case BTResult.Success:
				return BTResult.Failed;
			case BTResult.Failed:
				return BTResult.Success;
			}
			return BTResult.Failed;
		}
	}

}