﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZkData
{
	public static class GlobalConst
	{
		public const string AuthServiceUri = "net.tcp://localhost:8202";
		public const string MissionServiceUri = "http://zero-k.info/missions/MissionService.svc";
		public const string NightwatchName = "Nightwatch";
		public const string MissionScriptFileName = "_missionScript.txt";
		public const string MissionSlotsFileName = "_missionSlots.xml";
		public const string LoginCookieName = "zk_login";
		public const string PasswordHashCookieName = "zk_passwordHash";
	}
}
