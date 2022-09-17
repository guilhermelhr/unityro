using NUnit.Framework;
using ROIO;
using ROIO.Loaders;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Tests {

    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class AsyncMapLoaderTests {

        [OneTimeSetUp]
        public void SetUp() {
            var config = ConfigurationLoader.Init();
            FileManager.LoadGRF(config.root, config.grf);
        }

        [Test]
        [Ignore("Use only to regerenate other methods")]
        public void AssertEveryMapLoadCorrectly() {
            var MapNames = new List<string>();

            var descriptors = FileManager.GetFileDescriptors();
            foreach (var key in descriptors.Keys) {
                if (Path.GetExtension(key.ToString()) == ".rsw") {
                    MapNames.Add(key.ToString().Replace("data/", "").Replace(".rsw", ""));
                }
            }

            MapNames.Sort();

            using (StreamWriter w = File.AppendText("D:/Ragnarok/log.txt")) {
                foreach (var mapName in MapNames) {
                    w.WriteLine("[Test]");
                    w.WriteLine($"public void Assert_{mapName.Replace("@", "_").Replace("-", "_")}_Loads() {{");
                    w.WriteLine("var stopWatch = new System.Diagnostics.Stopwatch();");
                    w.WriteLine("stopWatch.Restart();");
                    w.WriteLine($"Debug.Log(\"Starting to load {mapName}\");");
                    w.WriteLine($"var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load(\"{mapName}.rsw\"));");
                    w.WriteLine("stopWatch.Stop();");
                    w.WriteLine($"Assert.NotNull(world);");
                    w.WriteLine($"Debug.Log($\"{mapName} took {{stopWatch.Elapsed}} to load\");");
                    w.WriteLine("}");
                    w.WriteLine();
                }
            }
        }

        [Test]
        public void Assert_06guild_r_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 06guild_r");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("06guild_r.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"06guild_r took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_air1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@air1");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@air1.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@air1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_air2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@air2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@air2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@air2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_begi_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@begi");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@begi.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@begi took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_cash_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@cash");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@cash.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@cash took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_cata_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@cata");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@cata.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@cata took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_def01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@def01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@def01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@def01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_def02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@def02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@def02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@def02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_def03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@def03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@def03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@def03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_dth1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@dth1");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@dth1.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@dth1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_dth2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@dth2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@dth2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@dth2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_dth3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@dth3");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@dth3.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@dth3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_ecl_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@ecl");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@ecl.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@ecl took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_eom_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@eom");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@eom.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@eom took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_face_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@face");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@face.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@face took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_ge_st_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@ge_st");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@ge_st.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@ge_st took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_gef_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@gef");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@gef.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@gef took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_gef_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@gef_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@gef_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@gef_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_gl_k_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@gl_k");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@gl_k.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@gl_k took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_gl_kh_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@gl_kh");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@gl_kh.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@gl_kh took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_glast_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@glast");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@glast.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@glast took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_jtb_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@jtb");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@jtb.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@jtb took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_lab_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@lab");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@lab.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@lab took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_lhz_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@lhz");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@lhz.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@lhz took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_ma_b_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@ma_b");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@ma_b.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@ma_b took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_ma_c_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@ma_c");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@ma_c.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@ma_c took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_ma_h_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@ma_h");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@ma_h.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@ma_h took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_mcd_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@mcd");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@mcd.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@mcd took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_mir_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@mir");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@mir.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@mir took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_mist_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@mist");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@mist.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@mist took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_nyd_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@nyd");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@nyd.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@nyd took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_orcs_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@orcs");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@orcs.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@orcs took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_pump_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@pump");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@pump.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@pump took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_rev_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@rev");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@rev.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@rev took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_sara_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@sara");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@sara.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@sara took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_spa_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@spa");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@spa.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@spa took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_sthb_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@sthb");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@sthb.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@sthb took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_sthc_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@sthc");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@sthc.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@sthc took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_sthd_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@sthd");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@sthd.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@sthd took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_tnm1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@tnm1");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@tnm1.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@tnm1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_tnm2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@tnm2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@tnm2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@tnm2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_tnm3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@tnm3");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@tnm3.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@tnm3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_tower_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@tower");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@tower.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@tower took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_uns_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@uns");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@uns.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@uns took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_xm_d_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@xm_d");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("1@xm_d.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"1@xm_d took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_cata_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@cata");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("2@cata.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"2@cata took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_gl_k_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@gl_k");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("2@gl_k.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"2@gl_k took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_gl_kh_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@gl_kh");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("2@gl_kh.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"2@gl_kh took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_mir_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@mir");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("2@mir.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"2@mir took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_nyd_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@nyd");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("2@nyd.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"2@nyd took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_orcs_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@orcs");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("2@orcs.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"2@orcs took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_pump_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@pump");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("2@pump.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"2@pump took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_tower_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@tower");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("2@tower.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"2@tower took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_3_tower_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 3@tower");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("3@tower.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"3@tower took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_4_tower_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 4@tower");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("4@tower.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"4@tower took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_5_tower_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 5@tower");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("5@tower.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"5@tower took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_6_tower_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 6@tower");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("6@tower.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"6@tower took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_abbey01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load abbey01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("abbey01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"abbey01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_abbey02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load abbey02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("abbey02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"abbey02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_abbey03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load abbey03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("abbey03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"abbey03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_abyss_01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load abyss_01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("abyss_01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"abyss_01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_abyss_02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load abyss_02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("abyss_02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"abyss_02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_abyss_03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load abyss_03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("abyss_03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"abyss_03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_airplane_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load airplane");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("airplane.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"airplane took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_airport_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load airport");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("airport.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"airport took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alb_ship_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alb_ship");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("alb_ship.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"alb_ship took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alb2trea_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alb2trea");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("alb2trea.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"alb2trea took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alberta_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alberta");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("alberta.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"alberta took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alberta_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alberta_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("alberta_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"alberta_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alde_alche_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alde_alche");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("alde_alche.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"alde_alche took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alde_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alde_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("alde_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"alde_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alde_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alde_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("alde_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"alde_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alde_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alde_dun03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("alde_dun03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"alde_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alde_dun04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alde_dun04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("alde_dun04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"alde_dun04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alde_gld_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alde_gld");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("alde_gld.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"alde_gld took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alde_tt02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alde_tt02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("alde_tt02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"alde_tt02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alde_tt03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alde_tt03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("alde_tt03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"alde_tt03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_aldeba_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load aldeba_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("aldeba_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"aldeba_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_aldebaran_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load aldebaran");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("aldebaran.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"aldebaran took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_aldeg_cas01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load aldeg_cas01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("aldeg_cas01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"aldeg_cas01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_aldeg_cas02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load aldeg_cas02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("aldeg_cas02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"aldeg_cas02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_aldeg_cas03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load aldeg_cas03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("aldeg_cas03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"aldeg_cas03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_aldeg_cas04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load aldeg_cas04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("aldeg_cas04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"aldeg_cas04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_aldeg_cas05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load aldeg_cas05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("aldeg_cas05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"aldeg_cas05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ama_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ama_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ama_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ama_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ama_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ama_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ama_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ama_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ama_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ama_dun03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ama_dun03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ama_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ama_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ama_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ama_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ama_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ama_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ama_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ama_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ama_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ama_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ama_in02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ama_in02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ama_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ama_test_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ama_test");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ama_test.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ama_test took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_amatsu_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load amatsu");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("amatsu.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"amatsu took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_anthell01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load anthell01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("anthell01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"anthell01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_anthell02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load anthell02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("anthell02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"anthell02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_arena_room_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load arena_room");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("arena_room.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"arena_room took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_aru_gld_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load aru_gld");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("aru_gld.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"aru_gld took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_arug_cas01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load arug_cas01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("arug_cas01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"arug_cas01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_arug_cas02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load arug_cas02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("arug_cas02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"arug_cas02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_arug_cas03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load arug_cas03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("arug_cas03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"arug_cas03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_arug_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load arug_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("arug_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"arug_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_arug_que01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load arug_que01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("arug_que01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"arug_que01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_auction_01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load auction_01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("auction_01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"auction_01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_auction_02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load auction_02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("auction_02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"auction_02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ayo_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ayo_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ayo_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ayo_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ayo_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ayo_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ayo_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ayo_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ayo_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ayo_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ayo_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ayo_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ayo_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ayo_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ayo_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ayo_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ayo_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ayo_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ayo_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ayo_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ayo_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ayo_in02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ayo_in02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ayo_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ayothaya_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ayothaya");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ayothaya.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ayothaya took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bat_a01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bat_a01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("bat_a01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"bat_a01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bat_b01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bat_b01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("bat_b01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"bat_b01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bat_c01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bat_c01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("bat_c01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"bat_c01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bat_room_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bat_room");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("bat_room.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"bat_room took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_beach_dun_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load beach_dun");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("beach_dun.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"beach_dun took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_beach_dun2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load beach_dun2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("beach_dun2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"beach_dun2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_beach_dun3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load beach_dun3");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("beach_dun3.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"beach_dun3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bif_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bif_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("bif_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"bif_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bif_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bif_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("bif_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"bif_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bra_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bra_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("bra_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"bra_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bra_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bra_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("bra_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"bra_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bra_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bra_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("bra_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"bra_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bra_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bra_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("bra_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"bra_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_brasilis_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load brasilis");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("brasilis.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"brasilis took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_brz_n_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load brz_n");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("brz_n.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"brz_n took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_c_tower1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load c_tower1");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("c_tower1.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"c_tower1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_c_tower2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load c_tower2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("c_tower2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"c_tower2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_c_tower2__Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load c_tower2_");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("c_tower2_.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"c_tower2_ took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_c_tower3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load c_tower3");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("c_tower3.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"c_tower3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_c_tower3__Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load c_tower3_");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("c_tower3_.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"c_tower3_ took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_c_tower4_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load c_tower4");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("c_tower4.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"c_tower4 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cave_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cave");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("cave.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"cave took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("cmd_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"cmd_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("cmd_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"cmd_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("cmd_fild03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"cmd_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("cmd_fild04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"cmd_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("cmd_fild05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"cmd_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild06");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("cmd_fild06.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"cmd_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild07");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("cmd_fild07.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"cmd_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild08");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("cmd_fild08.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"cmd_fild08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild09");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("cmd_fild09.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"cmd_fild09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("cmd_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"cmd_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_in02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("cmd_in02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"cmd_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_comodo_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load comodo");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("comodo.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"comodo took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_conch_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load conch_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("conch_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"conch_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dali_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dali");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("dali.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"dali took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dali02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dali02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("dali02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"dali02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dew_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dew_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("dew_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"dew_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dew_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dew_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("dew_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"dew_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dew_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dew_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("dew_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"dew_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dew_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dew_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("dew_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"dew_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dewata_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dewata");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("dewata.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"dewata took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dic_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dic_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("dic_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"dic_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dic_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dic_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("dic_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"dic_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dic_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dic_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("dic_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"dic_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dic_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dic_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("dic_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"dic_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dic_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dic_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("dic_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"dic_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dicastes01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dicastes01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("dicastes01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"dicastes01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dicastes02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dicastes02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("dicastes02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"dicastes02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_e_hugel_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load e_hugel");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("e_hugel.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"e_hugel took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_e_tower_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load e_tower");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("e_tower.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"e_tower took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ecl_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ecl_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_hub01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_hub01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ecl_hub01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ecl_hub01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ecl_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ecl_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_in02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ecl_in02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ecl_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_in03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_in03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ecl_in03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ecl_in03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_in04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_in04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ecl_in04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ecl_in04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_tdun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_tdun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ecl_tdun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ecl_tdun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_tdun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_tdun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ecl_tdun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ecl_tdun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_tdun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_tdun03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ecl_tdun03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ecl_tdun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_tdun04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_tdun04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ecl_tdun04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ecl_tdun04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_eclage_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load eclage");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("eclage.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"eclage took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ein_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ein_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ein_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ein_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ein_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ein_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ein_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ein_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ein_fild03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ein_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ein_fild04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ein_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ein_fild05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ein_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild06");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ein_fild06.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ein_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild07");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ein_fild07.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ein_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild08");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ein_fild08.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ein_fild08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild09");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ein_fild09.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ein_fild09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild10");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ein_fild10.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ein_fild10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ein_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ein_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_einbech_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load einbech");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("einbech.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"einbech took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_einbroch_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load einbroch");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("einbroch.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"einbroch took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_evt_bomb_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load evt_bomb");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("evt_bomb.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"evt_bomb took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_evt_mobroom_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load evt_mobroom");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("evt_mobroom.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"evt_mobroom took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_force_map1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load force_map1");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("force_map1.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"force_map1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_force_map2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load force_map2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("force_map2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"force_map2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_force_map3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load force_map3");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("force_map3.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"force_map3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_dun00_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_dun00");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_dun00.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_dun00 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_dun03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_dun03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild00_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild00");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_fild00.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_fild00 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_fild03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_fild04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_fild05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild06");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_fild06.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild07");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_fild07.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild08");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_fild08.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_fild08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild09");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_fild09.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_fild09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild10");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_fild10.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_fild10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild11_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild11");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_fild11.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_fild11 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild12_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild12");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_fild12.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_fild12 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild13_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild13");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_fild13.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_fild13 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild14_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild14");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_fild14.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_fild14 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_tower_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_tower");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gef_tower.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gef_tower took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefenia01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefenia01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gefenia01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gefenia01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefenia02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefenia02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gefenia02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gefenia02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefenia03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefenia03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gefenia03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gefenia03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefenia04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefenia04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gefenia04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gefenia04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_geffen_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load geffen");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("geffen.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"geffen took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_geffen_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load geffen_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("geffen_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"geffen_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefg_cas01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefg_cas01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gefg_cas01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gefg_cas01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefg_cas02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefg_cas02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gefg_cas02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gefg_cas02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefg_cas03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefg_cas03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gefg_cas03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gefg_cas03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefg_cas04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefg_cas04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gefg_cas04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gefg_cas04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefg_cas05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefg_cas05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gefg_cas05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gefg_cas05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_cas01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_cas01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_cas01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_cas01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_cas02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_cas02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_cas02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_cas02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_cas02__Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_cas02_");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_cas02_.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_cas02_ took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_church_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_church");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_church.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_church took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_chyard_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_chyard");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_chyard.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_chyard took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_chyard__Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_chyard_");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_chyard_.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_chyard_ took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_knt01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_knt01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_knt01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_knt01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_knt02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_knt02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_knt02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_knt02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_prison_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_prison");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_prison.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_prison took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_prison1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_prison1");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_prison1.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_prison1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_sew01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_sew01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_sew01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_sew01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_sew02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_sew02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_sew02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_sew02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_sew03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_sew03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_sew03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_sew03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_sew04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_sew04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_sew04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_sew04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_step_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_step");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gl_step.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gl_step took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_glast_01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load glast_01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("glast_01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"glast_01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gld_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gld_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gld_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gld_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gld_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gld_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gld_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gld_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gld_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gld_dun03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gld_dun03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gld_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gld_dun04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gld_dun04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gld_dun04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gld_dun04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gld2_ald_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gld2_ald");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gld2_ald.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gld2_ald took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gld2_gef_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gld2_gef");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gld2_gef.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gld2_gef took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gld2_pay_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gld2_pay");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gld2_pay.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gld2_pay took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gld2_prt_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gld2_prt");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gld2_prt.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gld2_prt took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gon_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gon_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gon_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gon_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gon_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gon_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gon_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gon_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gon_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gon_dun03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gon_dun03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gon_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gon_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gon_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gon_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gon_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gon_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gon_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gon_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gon_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gon_test_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gon_test");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gon_test.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gon_test took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gonryun_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gonryun");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("gonryun.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"gonryun took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_guild_vs1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load guild_vs1");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("guild_vs1.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"guild_vs1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_guild_vs2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load guild_vs2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("guild_vs2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"guild_vs2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_guild_vs3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load guild_vs3");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("guild_vs3.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"guild_vs3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_guild_vs4_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load guild_vs4");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("guild_vs4.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"guild_vs4 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_guild_vs5_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load guild_vs5");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("guild_vs5.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"guild_vs5 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hu_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hu_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("hu_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"hu_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hu_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hu_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("hu_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"hu_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hu_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hu_fild03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("hu_fild03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"hu_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hu_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hu_fild04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("hu_fild04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"hu_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hu_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hu_fild05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("hu_fild05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"hu_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hu_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hu_fild06");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("hu_fild06.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"hu_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hu_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hu_fild07");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("hu_fild07.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"hu_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hu_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hu_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("hu_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"hu_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hugel_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hugel");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("hugel.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"hugel took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ice_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ice_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ice_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ice_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ice_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ice_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ice_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ice_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ice_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ice_dun03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ice_dun03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ice_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ice_dun04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ice_dun04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ice_dun04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ice_dun04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_hunter_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_hunter");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("in_hunter.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"in_hunter took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_moc_16_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_moc_16");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("in_moc_16.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"in_moc_16 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_orcs01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_orcs01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("in_orcs01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"in_orcs01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_rogue_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_rogue");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("in_rogue.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"in_rogue took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_sphinx1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_sphinx1");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("in_sphinx1.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"in_sphinx1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_sphinx2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_sphinx2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("in_sphinx2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"in_sphinx2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_sphinx3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_sphinx3");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("in_sphinx3.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"in_sphinx3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_sphinx4_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_sphinx4");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("in_sphinx4.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"in_sphinx4 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_sphinx5_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_sphinx5");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("in_sphinx5.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"in_sphinx5 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_int_land_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load int_land");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("int_land.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"int_land took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_int_land01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load int_land01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("int_land01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"int_land01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_int_land02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load int_land02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("int_land02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"int_land02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_int_land03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load int_land03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("int_land03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"int_land03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_int_land04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load int_land04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("int_land04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"int_land04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_ac01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_ac01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac01_a_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac01_a");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_ac01_a.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_ac01_a took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac01_b_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac01_b");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_ac01_b.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_ac01_b took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac01_c_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac01_c");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_ac01_c.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_ac01_c took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac01_d_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac01_d");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_ac01_d.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_ac01_d took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_ac02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_ac02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac02_a_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac02_a");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_ac02_a.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_ac02_a took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac02_b_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac02_b");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_ac02_b.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_ac02_b took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac02_c_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac02_c");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_ac02_c.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_ac02_c took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac02_d_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac02_d");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_ac02_d.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_ac02_d took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_dun00_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_dun00");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_dun00.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_dun00 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_dun03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_dun03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_dun04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_dun04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_dun04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_dun04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_dun05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_dun05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_dun05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_dun05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_int_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_int");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_int.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_int took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_int01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_int01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_int01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_int01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_int02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_int02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_int02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_int02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_int03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_int03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_int03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_int03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_int04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_int04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_int04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_int04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ng01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ng01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("iz_ng01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"iz_ng01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_izlu2dun_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load izlu2dun");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("izlu2dun.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"izlu2dun took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_izlude_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load izlude");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("izlude.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"izlude took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_izlude_a_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load izlude_a");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("izlude_a.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"izlude_a took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_izlude_b_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load izlude_b");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("izlude_b.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"izlude_b took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_izlude_c_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load izlude_c");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("izlude_c.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"izlude_c took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_izlude_d_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load izlude_d");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("izlude_d.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"izlude_d took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_izlude_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load izlude_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("izlude_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"izlude_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jawaii_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jawaii");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("jawaii.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"jawaii took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jawaii_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jawaii_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("jawaii_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"jawaii_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_cru_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_cru");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_cru.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_cru took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_duncer_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_duncer");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_duncer.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_duncer took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_gun_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_gun");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_gun.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_gun took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_hunte_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_hunte");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_hunte.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_hunte took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_hunter_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_hunter");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_hunter.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_hunter took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_knight_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_knight");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_knight.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_knight took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_knt_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_knt");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_knt.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_knt took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_ko_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_ko");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_ko.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_ko took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_monk_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_monk");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_monk.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_monk took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_priest_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_priest");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_priest.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_priest took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_prist_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_prist");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_prist.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_prist took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_sage_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_sage");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_sage.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_sage took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_soul_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_soul");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_soul.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_soul took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_star_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_star");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_star.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_star took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_sword1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_sword1");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_sword1.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_sword1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_thief1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_thief1");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_thief1.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_thief1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_wiz_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_wiz");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_wiz.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_wiz took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_wizard_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_wizard");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job_wizard.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job_wizard took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_arch01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_arch01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job3_arch01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job3_arch01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_arch02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_arch02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job3_arch02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job3_arch02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_gen01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_gen01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job3_gen01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job3_gen01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_guil01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_guil01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job3_guil01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job3_guil01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_guil02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_guil02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job3_guil02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job3_guil02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_guil03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_guil03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job3_guil03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job3_guil03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_rang01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_rang01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job3_rang01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job3_rang01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_rang02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_rang02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job3_rang02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job3_rang02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_rune01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_rune01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job3_rune01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job3_rune01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_rune02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_rune02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job3_rune02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job3_rune02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_sha01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_sha01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job3_sha01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job3_sha01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_war01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_war01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("job3_war01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"job3_war01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jupe_area1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jupe_area1");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("jupe_area1.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"jupe_area1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jupe_area2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jupe_area2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("jupe_area2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"jupe_area2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jupe_cave_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jupe_cave");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("jupe_cave.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"jupe_cave took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jupe_core_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jupe_core");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("jupe_core.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"jupe_core took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jupe_ele_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jupe_ele");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("jupe_ele.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"jupe_ele took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jupe_ele_r_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jupe_ele_r");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("jupe_ele_r.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"jupe_ele_r took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jupe_gate_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jupe_gate");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("jupe_gate.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"jupe_gate took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_juperos_01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load juperos_01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("juperos_01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"juperos_01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_juperos_02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load juperos_02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("juperos_02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"juperos_02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_kh_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load kh_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("kh_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"kh_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_kh_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load kh_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("kh_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"kh_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_kh_kiehl01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load kh_kiehl01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("kh_kiehl01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"kh_kiehl01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_kh_kiehl02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load kh_kiehl02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("kh_kiehl02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"kh_kiehl02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_kh_mansion_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load kh_mansion");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("kh_mansion.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"kh_mansion took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_kh_rossi_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load kh_rossi");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("kh_rossi.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"kh_rossi took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_kh_school_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load kh_school");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("kh_school.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"kh_school took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_kh_vila_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load kh_vila");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("kh_vila.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"kh_vila took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lasa_dun_q_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lasa_dun_q");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lasa_dun_q.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lasa_dun_q took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lasa_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lasa_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lasa_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lasa_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lasa_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lasa_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lasa_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lasa_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lasa_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lasa_dun03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lasa_dun03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lasa_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lasa_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lasa_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lasa_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lasa_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lasa_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lasa_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lasa_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lasa_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lasa_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lasa_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lasa_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lasa_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lasagna_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lasagna");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lasagna.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lasagna took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_cube_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_cube");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lhz_cube.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lhz_cube took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_d_n2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_d_n2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lhz_d_n2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lhz_d_n2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_dun_n_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_dun_n");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lhz_dun_n.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lhz_dun_n took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lhz_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lhz_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lhz_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lhz_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_dun03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lhz_dun03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lhz_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_dun04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_dun04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lhz_dun04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lhz_dun04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lhz_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lhz_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lhz_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lhz_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_fild03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lhz_fild03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lhz_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lhz_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lhz_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_in02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lhz_in02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lhz_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_in03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_in03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lhz_in03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lhz_in03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_que01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_que01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lhz_que01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lhz_que01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lighthalzen_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lighthalzen");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lighthalzen.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lighthalzen took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lou_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lou_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lou_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lou_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lou_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lou_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lou_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lou_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lou_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lou_dun03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lou_dun03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lou_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lou_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lou_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lou_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lou_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lou_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lou_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lou_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lou_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lou_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lou_in02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("lou_in02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"lou_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_louyang_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load louyang");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("louyang.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"louyang took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ma_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ma_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ma_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ma_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ma_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ma_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ma_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ma_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_scene01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_scene01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ma_scene01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ma_scene01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ma_zif01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ma_zif01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ma_zif02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ma_zif02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ma_zif03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ma_zif03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ma_zif04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ma_zif04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ma_zif05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ma_zif05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif06");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ma_zif06.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ma_zif06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif07");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ma_zif07.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ma_zif07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif08");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ma_zif08.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ma_zif08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif09");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ma_zif09.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ma_zif09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mag_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mag_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mag_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mag_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mag_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mag_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mag_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mag_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mal_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mal_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mal_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mal_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mal_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mal_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mal_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mal_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mal_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mal_in02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mal_in02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mal_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_malangdo_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load malangdo");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("malangdo.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"malangdo took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_malaya_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load malaya");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("malaya.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"malaya took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_man_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load man_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("man_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"man_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_man_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load man_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("man_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"man_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_man_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load man_fild03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("man_fild03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"man_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_man_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load man_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("man_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"man_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_manuk_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load manuk");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("manuk.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"manuk took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mid_camp_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mid_camp");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mid_camp.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mid_camp took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mid_campin_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mid_campin");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mid_campin.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mid_campin took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjo_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjo_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mjo_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mjo_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjo_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjo_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mjo_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mjo_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjo_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjo_dun03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mjo_dun03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mjo_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mjolnir_01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mjolnir_01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mjolnir_02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mjolnir_02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mjolnir_03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mjolnir_03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mjolnir_04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mjolnir_04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_04_1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_04_1");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mjolnir_04_1.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mjolnir_04_1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mjolnir_05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mjolnir_05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_06");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mjolnir_06.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mjolnir_06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_07");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mjolnir_07.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mjolnir_07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_08");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mjolnir_08.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mjolnir_08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_09");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mjolnir_09.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mjolnir_09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_10");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mjolnir_10.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mjolnir_10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_11_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_11");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mjolnir_11.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mjolnir_11 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_12_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_12");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mjolnir_12.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mjolnir_12 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_castle_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_castle");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_castle.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_castle took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild06");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild06.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild07");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild07.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild08");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild08.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild09");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild09.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild10");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild10.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild11_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild11");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild11.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild11 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild12_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild12");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild12.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild12 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild13_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild13");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild13.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild13 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild14_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild14");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild14.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild14 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild15_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild15");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild15.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild15 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild16_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild16");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild16.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild16 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild17_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild17");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild17.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild17 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild18_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild18");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild18.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild18 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild19_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild19");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild19.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild19 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild20_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild20");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild20.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild20 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild21_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild21");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild21.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild21 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild22_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild22");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild22.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild22 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild22b_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild22b");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_fild22b.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_fild22b took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_para01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_para01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_para01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_para01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_pryd01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_pryd01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_pryd01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_pryd01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_pryd02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_pryd02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_pryd02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_pryd02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_pryd03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_pryd03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_pryd03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_pryd03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_pryd04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_pryd04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_pryd04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_pryd04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_pryd05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_pryd05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_pryd05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_pryd05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_pryd06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_pryd06");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_pryd06.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_pryd06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_prydb1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_prydb1");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_prydb1.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_prydb1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_prydn1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_prydn1");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_prydn1.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_prydn1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_prydn2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_prydn2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_prydn2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_prydn2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_ruins_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_ruins");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moc_ruins.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moc_ruins took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_monk_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load monk_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("monk_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"monk_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_monk_test_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load monk_test");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("monk_test.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"monk_test took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mora_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mora");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mora.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mora took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moro_cav_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moro_cav");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moro_cav.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moro_cav took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moro_vol_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moro_vol");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moro_vol.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moro_vol took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_morocc_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load morocc");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("morocc.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"morocc took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_morocc_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load morocc_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("morocc_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"morocc_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moscovia_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moscovia");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("moscovia.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"moscovia took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mosk_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mosk_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mosk_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mosk_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mosk_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mosk_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mosk_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mosk_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mosk_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mosk_dun03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mosk_dun03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mosk_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mosk_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mosk_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mosk_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mosk_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mosk_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mosk_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mosk_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mosk_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mosk_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mosk_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mosk_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mosk_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mosk_que_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mosk_que");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mosk_que.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mosk_que took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mosk_ship_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mosk_ship");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("mosk_ship.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"mosk_ship took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_n_castle_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load n_castle");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("n_castle.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"n_castle took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_nameless_i_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load nameless_i");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("nameless_i.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"nameless_i took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_nameless_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load nameless_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("nameless_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"nameless_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_nameless_n_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load nameless_n");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("nameless_n.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"nameless_n took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_new_event_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load new_event");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("new_event.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"new_event took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_new_zone01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load new_zone01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("new_zone01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"new_zone01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_new_zone02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load new_zone02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("new_zone02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"new_zone02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_new_zone03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load new_zone03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("new_zone03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"new_zone03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_new_zone04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load new_zone04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("new_zone04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"new_zone04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_nif_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load nif_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("nif_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"nif_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_nif_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load nif_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("nif_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"nif_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_nif_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load nif_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("nif_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"nif_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_niflheim_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load niflheim");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("niflheim.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"niflheim took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_niflxmas_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load niflxmas");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("niflxmas.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"niflxmas took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_nyd_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load nyd_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("nyd_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"nyd_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_nyd_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load nyd_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("nyd_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"nyd_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_odin_tem01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load odin_tem01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("odin_tem01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"odin_tem01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_odin_tem02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load odin_tem02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("odin_tem02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"odin_tem02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_odin_tem03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load odin_tem03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("odin_tem03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"odin_tem03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_orcsdun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load orcsdun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("orcsdun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"orcsdun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_orcsdun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load orcsdun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("orcsdun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"orcsdun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ordeal_a00_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ordeal_a00");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ordeal_a00.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ordeal_a00 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ordeal_a02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ordeal_a02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ordeal_a02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ordeal_a02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_p_track01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load p_track01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("p_track01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"p_track01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_paramk_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load paramk");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("paramk.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"paramk took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_arche_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_arche");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_arche.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_arche took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_dun00_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_dun00");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_dun00.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_dun00 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_dun03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_dun03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_dun04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_dun04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_dun04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_dun04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_fild03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_fild04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_fild05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild06");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_fild06.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild07");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_fild07.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild08");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_fild08.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_fild08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild09");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_fild09.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_fild09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild10");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_fild10.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_fild10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild11_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild11");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_fild11.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_fild11 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_gld_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_gld");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pay_gld.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pay_gld took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payg_cas01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payg_cas01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("payg_cas01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"payg_cas01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payg_cas02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payg_cas02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("payg_cas02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"payg_cas02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payg_cas03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payg_cas03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("payg_cas03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"payg_cas03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payg_cas04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payg_cas04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("payg_cas04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"payg_cas04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payg_cas05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payg_cas05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("payg_cas05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"payg_cas05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payon_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payon");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("payon.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"payon took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payon_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payon_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("payon_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"payon_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payon_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payon_in02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("payon_in02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"payon_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payon_in03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payon_in03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("payon_in03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"payon_in03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_poring_c01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load poring_c01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("poring_c01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"poring_c01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_poring_c02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load poring_c02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("poring_c02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"poring_c02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_poring_w01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load poring_w01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("poring_w01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"poring_w01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_poring_w02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load poring_w02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("poring_w02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"poring_w02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prontera_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prontera");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prontera.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prontera took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_are_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_are_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_are_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_are_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_are01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_are01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_are01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_are01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_cas_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_cas");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_cas.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_cas took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_cas_q_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_cas_q");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_cas_q.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_cas_q took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_castle_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_castle");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_castle.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_castle took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_church_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_church");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_church.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_church took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild00_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild00");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_fild00.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_fild00 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_fild03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_fild04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_fild05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild06");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_fild06.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild07");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_fild07.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild08");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_fild08.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_fild08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild08a_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild08a");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_fild08a.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_fild08a took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild08b_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild08b");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_fild08b.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_fild08b took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild08c_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild08c");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_fild08c.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_fild08c took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild08d_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild08d");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_fild08d.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_fild08d took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild09");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_fild09.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_fild09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild10");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_fild10.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_fild10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild11_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild11");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_fild11.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_fild11 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_gld_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_gld");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_gld.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_gld took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_lib_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_lib");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_lib.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_lib took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_lib_q_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_lib_q");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_lib_q.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_lib_q took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_maze01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_maze01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_maze01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_maze01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_maze02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_maze02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_maze02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_maze02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_maze03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_maze03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_maze03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_maze03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_monk_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_monk");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_monk.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_monk took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_pri00_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_pri00");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_pri00.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_pri00 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_prison_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_prison");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_prison.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_prison took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_q_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_q");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_q.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_q took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_sewb1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_sewb1");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_sewb1.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_sewb1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_sewb2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_sewb2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_sewb2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_sewb2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_sewb3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_sewb3");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_sewb3.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_sewb3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_sewb4_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_sewb4");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prt_sewb4.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prt_sewb4 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prtg_cas01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prtg_cas01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prtg_cas01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prtg_cas01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prtg_cas02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prtg_cas02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prtg_cas02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prtg_cas02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prtg_cas03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prtg_cas03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prtg_cas03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prtg_cas03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prtg_cas04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prtg_cas04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prtg_cas04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prtg_cas04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prtg_cas05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prtg_cas05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("prtg_cas05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"prtg_cas05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_2vs2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_2vs2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pvp_2vs2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pvp_2vs2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_room_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_room");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pvp_room.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pvp_room took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_y_1_2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_y_1-2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pvp_y_1-2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pvp_y_1-2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_y_2_2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_y_2-2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pvp_y_2-2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pvp_y_2-2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_y_3_2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_y_3-2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pvp_y_3-2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pvp_y_3-2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_y_4_2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_y_4-2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pvp_y_4-2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pvp_y_4-2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_y_5_2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_y_5-2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pvp_y_5-2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pvp_y_5-2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_y_6_2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_y_6-2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pvp_y_6-2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pvp_y_6-2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_y_7_2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_y_7-2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pvp_y_7-2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pvp_y_7-2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_y_8_2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_y_8-2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("pvp_y_8-2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"pvp_y_8-2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_avan01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_avan01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("que_avan01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"que_avan01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_ba_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_ba");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("que_ba.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"que_ba took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_bingo_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_bingo");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("que_bingo.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"que_bingo took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_dan01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_dan01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("que_dan01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"que_dan01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_dan02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_dan02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("que_dan02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"que_dan02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_god01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_god01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("que_god01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"que_god01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_god02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_god02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("que_god02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"que_god02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_house_s_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_house_s");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("que_house_s.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"que_house_s took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_hugel_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_hugel");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("que_hugel.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"que_hugel took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_job01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_job01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("que_job01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"que_job01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_job02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_job02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("que_job02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"que_job02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_lhz_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_lhz");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("que_lhz.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"que_lhz took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_ng_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_ng");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("que_ng.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"que_ng took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_qsch01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_qsch01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("que_qsch01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"que_qsch01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_rachel_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_rachel");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("que_rachel.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"que_rachel took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_sign01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_sign01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("que_sign01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"que_sign01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_thor_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_thor");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("que_thor.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"que_thor took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_quiz_00_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load quiz_00");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("quiz_00.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"quiz_00 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_quiz_01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load quiz_01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("quiz_01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"quiz_01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_quiz_02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load quiz_02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("quiz_02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"quiz_02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_fild03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_fild04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_fild05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild06");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_fild06.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild07");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_fild07.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild08");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_fild08.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_fild08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild09");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_fild09.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_fild09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild10");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_fild10.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_fild10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild11_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild11");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_fild11.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_fild11 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild12_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild12");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_fild12.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_fild12 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild13_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild13");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_fild13.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_fild13 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_san01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_san01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_san01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_san01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_san02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_san02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_san02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_san02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_san03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_san03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_san03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_san03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_san04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_san04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_san04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_san04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_san05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_san05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_san05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_san05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_temin_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_temin");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_temin.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_temin took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_temple_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_temple");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_temple.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_temple took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_temsky_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_temsky");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ra_temsky.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ra_temsky took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_rachel_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load rachel");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("rachel.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"rachel took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_rwc01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load rwc01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("rwc01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"rwc01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_rwc03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load rwc03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("rwc03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"rwc03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_s_atelier_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load s_atelier");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("s_atelier.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"s_atelier took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_sch_gld_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load sch_gld");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("sch_gld.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"sch_gld took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_schg_cas01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load schg_cas01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("schg_cas01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"schg_cas01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_schg_cas02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load schg_cas02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("schg_cas02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"schg_cas02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_schg_cas03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load schg_cas03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("schg_cas03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"schg_cas03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_schg_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load schg_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("schg_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"schg_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_sec_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load sec_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("sec_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"sec_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_sec_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load sec_in02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("sec_in02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"sec_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_sec_pri_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load sec_pri");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("sec_pri.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"sec_pri took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_siege_test_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load siege_test");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("siege_test.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"siege_test took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_silk_lair_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load silk_lair");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("silk_lair.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"silk_lair took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_spl_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load spl_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("spl_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"spl_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_spl_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load spl_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("spl_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"spl_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_spl_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load spl_fild03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("spl_fild03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"spl_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_spl_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load spl_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("spl_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"spl_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_spl_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load spl_in02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("spl_in02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"spl_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_splendide_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load splendide");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("splendide.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"splendide took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_alde_gld_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_alde_gld");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("te_alde_gld.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"te_alde_gld took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_aldecas1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_aldecas1");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("te_aldecas1.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"te_aldecas1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_aldecas2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_aldecas2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("te_aldecas2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"te_aldecas2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_aldecas3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_aldecas3");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("te_aldecas3.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"te_aldecas3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_aldecas4_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_aldecas4");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("te_aldecas4.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"te_aldecas4 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_aldecas5_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_aldecas5");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("te_aldecas5.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"te_aldecas5 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_prt_gld_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_prt_gld");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("te_prt_gld.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"te_prt_gld took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_prtcas01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_prtcas01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("te_prtcas01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"te_prtcas01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_prtcas02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_prtcas02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("te_prtcas02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"te_prtcas02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_prtcas03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_prtcas03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("te_prtcas03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"te_prtcas03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_prtcas04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_prtcas04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("te_prtcas04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"te_prtcas04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_prtcas05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_prtcas05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("te_prtcas05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"te_prtcas05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_teg_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load teg_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("teg_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"teg_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_teg_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load teg_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("teg_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"teg_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_scene01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_scene01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tha_scene01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tha_scene01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tha_t01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tha_t01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tha_t02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tha_t02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tha_t03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tha_t03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tha_t04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tha_t04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tha_t05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tha_t05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t06");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tha_t06.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tha_t06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t07");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tha_t07.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tha_t07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t08");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tha_t08.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tha_t08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t09");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tha_t09.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tha_t09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t10");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tha_t10.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tha_t10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t11_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t11");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tha_t11.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tha_t11 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t12_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t12");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tha_t12.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tha_t12 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_thana_boss_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load thana_boss");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("thana_boss.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"thana_boss took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_thana_step_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load thana_step");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("thana_step.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"thana_step took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_thor_camp_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load thor_camp");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("thor_camp.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"thor_camp took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_thor_v01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load thor_v01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("thor_v01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"thor_v01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_thor_v02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load thor_v02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("thor_v02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"thor_v02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_thor_v03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load thor_v03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("thor_v03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"thor_v03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_treasure_n1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load treasure_n1");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("treasure_n1.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"treasure_n1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_treasure_n2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load treasure_n2");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("treasure_n2.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"treasure_n2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_treasure01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load treasure01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("treasure01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"treasure01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_treasure02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load treasure02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("treasure02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"treasure02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tur_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tur_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tur_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tur_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tur_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tur_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tur_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tur_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tur_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tur_dun03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tur_dun03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tur_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tur_dun04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tur_dun04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tur_dun04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tur_dun04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tur_dun05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tur_dun05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tur_dun05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tur_dun05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tur_dun06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tur_dun06");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("tur_dun06.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"tur_dun06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_turbo_room_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load turbo_room");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("turbo_room.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"turbo_room took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_um_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load um_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("um_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"um_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_um_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load um_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("um_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"um_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_um_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load um_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("um_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"um_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_um_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load um_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("um_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"um_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_um_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load um_fild03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("um_fild03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"um_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_um_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load um_fild04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("um_fild04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"um_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_um_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load um_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("um_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"um_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_umbala_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load umbala");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("umbala.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"umbala took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_un_bk_q_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load un_bk_q");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("un_bk_q.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"un_bk_q took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_un_bunker_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load un_bunker");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("un_bunker.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"un_bunker took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_un_myst_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load un_myst");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("un_myst.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"un_myst took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_valkyrie_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load valkyrie");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("valkyrie.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"valkyrie took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ve_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ve_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ve_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ve_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_fild03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ve_fild03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ve_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_fild04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ve_fild04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ve_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_fild05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ve_fild05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ve_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_fild06");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ve_fild06.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ve_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_fild07");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ve_fild07.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ve_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ve_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ve_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_in02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ve_in02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ve_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_veins_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load veins");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("veins.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"veins took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ver_eju_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ver_eju");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ver_eju.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ver_eju took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ver_tunn_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ver_tunn");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("ver_tunn.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"ver_tunn took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_verus01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load verus01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("verus01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"verus01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_verus02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load verus02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("verus02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"verus02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_verus03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load verus03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("verus03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"verus03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_verus04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load verus04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("verus04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"verus04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_xmas_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load xmas");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("xmas.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"xmas took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_xmas_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load xmas_dun01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("xmas_dun01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"xmas_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_xmas_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load xmas_dun02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("xmas_dun02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"xmas_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_xmas_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load xmas_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("xmas_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"xmas_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_xmas_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load xmas_in");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("xmas_in.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"xmas_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yggdrasil01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yggdrasil01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yggdrasil01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yggdrasil01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_fild01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_fild02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_fild03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_fild04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_fild05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild06");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_fild06.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild07");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_fild07.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild08");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_fild08.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_fild08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild09");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_fild09.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_fild09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild10");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_fild10.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_fild10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild11_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild11");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_fild11.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_fild11 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild12_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild12");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_fild12.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_fild12 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_in01");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_in01.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_in02");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_in02.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_in03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_in03");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_in03.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_in03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_in04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_in04");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_in04.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_in04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_in05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_in05");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_in05.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_in05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_pre_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_pre");
            var world = UnityTestUtils.RunAsyncMethodSync(() => new AsyncMapLoader().Load("yuno_pre.rsw"));
            stopWatch.Stop();
            Assert.NotNull(world);
            Debug.Log($"yuno_pre took {stopWatch.Elapsed} to load");
        }


    }
}
