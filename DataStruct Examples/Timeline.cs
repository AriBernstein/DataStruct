using System;
using System.Collections.Generic;
using System.Text;
using DataStruct;

namespace DataStruct_Examples
{
    class Timeline
    {
        public static void Demo()
        {
            string versionA = "This is the original data that was stored.";
            string versionB = "This is the original data that was stored with a minor change.";
            string versionC = "This is the original data that was stored with two minor changes.";
            string versionB2 = "This is the first branch made on the data.";
            string versionB3 = "This is the first branch with some small modification.";
            string versionC2 = "This is a second branch made from node C.";
            Timeline<string> t = new Timeline<string>(versionA);
            int vb = t.AddVersion(0, versionB);
            int vc = t.AddVersion(vb, versionB);
            int vb2 = t.AddVersion(vb, versionB);
            int vb3 = t.AddVersion(vb2, versionB);
            int vc2 = t.AddVersion(vc, versionB);
        }
    }
}
