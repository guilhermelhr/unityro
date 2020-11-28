using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;
using UnityEngine;

namespace Tests {

    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class MapLoaderTests {

        MapSelector mapSelector;

        [OneTimeSetUp]
        public void SetUp() {
            FileManager.loadGrf("D:/Ragnarok/kro_data.grf");
            mapSelector = new MapSelector(FileManager.Grf);
        }
        // A Test behaves as an ordinary method

        [Test]
        [Ignore("Use only to regerenate other methods")]
        public void AssertEveryMapLoadCorrectly() {

            using (StreamWriter w = File.AppendText("D:/Ragnarok/log.txt")) {
                foreach (var filename in mapSelector.GetMapList()) {
                    var mapName = MapSelector.GetMapName(filename);
                    w.WriteLine("[Test]");
                    w.WriteLine($"public void Assert_{mapName.Replace("@", "_").Replace("-", "_")}_Loads() {{");
                    w.WriteLine("var stopWatch = new System.Diagnostics.Stopwatch();");
                    w.WriteLine("stopWatch.Restart();");
                    w.WriteLine($"Debug.Log(\"Starting to load {mapName}\");");
                    w.WriteLine($"new MapLoader().Load(\"{mapName}.rsw\", Substitute.For<Action<string, string, object>>());");
                    w.WriteLine("stopWatch.Stop();");
                    w.WriteLine($"Debug.Log($\"{mapName} took {{stopWatch.Elapsed}} to load\");");
                    w.WriteLine("}");
                    w.WriteLine();
                }
            }
        }

        [Test]
        public void Assert_ein_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild02");
            new MapLoader().Load("ein_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ein_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_bingo_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_bingo");
            new MapLoader().Load("que_bingo.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_bingo took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild03");
            new MapLoader().Load("ra_fild03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_hunte_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_hunte");
            new MapLoader().Load("job_hunte.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_hunte took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_kh_kiehl01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load kh_kiehl01");
            new MapLoader().Load("kh_kiehl01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"kh_kiehl01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_eclage_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load eclage");
            new MapLoader().Load("eclage.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"eclage took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_jtb_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@jtb");
            new MapLoader().Load("1@jtb.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@jtb took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif01");
            new MapLoader().Load("ma_zif01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ma_zif01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_nif_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load nif_fild01");
            new MapLoader().Load("nif_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"nif_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_c_tower3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load c_tower3");
            new MapLoader().Load("c_tower3.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"c_tower3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_sp_os_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load sp_os");
            new MapLoader().Load("sp_os.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"sp_os took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_geffen_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load geffen");
            new MapLoader().Load("geffen.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"geffen took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lou_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lou_dun02");
            new MapLoader().Load("lou_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lou_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_beach_dun2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load beach_dun2");
            new MapLoader().Load("beach_dun2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"beach_dun2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_orcsdun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load orcsdun01");
            new MapLoader().Load("orcsdun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"orcsdun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_05");
            new MapLoader().Load("mjolnir_05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mjolnir_05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_gol1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@gol1");
            new MapLoader().Load("1@gol1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@gol1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_guil03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_guil03");
            new MapLoader().Load("job3_guil03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job3_guil03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_tdun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_tdun02");
            new MapLoader().Load("ecl_tdun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ecl_tdun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_soul_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_soul");
            new MapLoader().Load("job_soul.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_soul took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_face_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@face");
            new MapLoader().Load("1@face.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@face took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_in02");
            new MapLoader().Load("yuno_in02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_dun01");
            new MapLoader().Load("ein_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ein_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_gl_prq_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@gl_prq");
            new MapLoader().Load("1@gl_prq.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@gl_prq took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_air1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@air1");
            new MapLoader().Load("1@air1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@air1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_dth3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@dth3");
            new MapLoader().Load("1@dth3.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@dth3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_kh_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load kh_dun02");
            new MapLoader().Load("kh_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"kh_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_kh_mansion_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load kh_mansion");
            new MapLoader().Load("kh_mansion.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"kh_mansion took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bra_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bra_in01");
            new MapLoader().Load("bra_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"bra_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_izlude_b_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load izlude_b");
            new MapLoader().Load("izlude_b.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"izlude_b took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_maze01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_maze01");
            new MapLoader().Load("prt_maze01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_maze01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_abbey02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load abbey02");
            new MapLoader().Load("abbey02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"abbey02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_gl_k2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@gl_k2");
            new MapLoader().Load("2@gl_k2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"2@gl_k2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac01_b_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac01_b");
            new MapLoader().Load("iz_ac01_b.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_ac01_b took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_dun04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_dun04");
            new MapLoader().Load("iz_dun04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_dun04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_star_frst_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load star_frst");
            new MapLoader().Load("star_frst.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"star_frst took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_6_tower_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 6@tower");
            new MapLoader().Load("6@tower.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"6@tower took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_dun00_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_dun00");
            new MapLoader().Load("gef_dun00.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_dun00 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mosk_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mosk_fild02");
            new MapLoader().Load("mosk_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mosk_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild17_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild17");
            new MapLoader().Load("moc_fild17.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild17 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild22b_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild22b");
            new MapLoader().Load("moc_fild22b.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild22b took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lasa_dun_q_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lasa_dun_q");
            new MapLoader().Load("lasa_dun_q.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lasa_dun_q took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_fild02");
            new MapLoader().Load("ve_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ve_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gon_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gon_dun03");
            new MapLoader().Load("gon_dun03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gon_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild03");
            new MapLoader().Load("gef_fild03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ba_go_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ba_go");
            new MapLoader().Load("ba_go.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ba_go took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_geffen_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load geffen_in");
            new MapLoader().Load("geffen_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"geffen_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild07");
            new MapLoader().Load("moc_fild07.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_kh_school_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load kh_school");
            new MapLoader().Load("kh_school.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"kh_school took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tur_dun04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tur_dun04");
            new MapLoader().Load("tur_dun04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tur_dun04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prontera_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prontera");
            new MapLoader().Load("prontera.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prontera took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_san04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_san04");
            new MapLoader().Load("ra_san04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_san04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_pryd05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_pryd05");
            new MapLoader().Load("moc_pryd05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_pryd05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_poring_w01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load poring_w01");
            new MapLoader().Load("poring_w01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"poring_w01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_fild01");
            new MapLoader().Load("ecl_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ecl_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_prtcas05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_prtcas05");
            new MapLoader().Load("te_prtcas05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"te_prtcas05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild08");
            new MapLoader().Load("pay_fild08.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_fild08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_y_2_2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_y_2-2");
            new MapLoader().Load("pvp_y_2-2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pvp_y_2-2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alde_alche_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alde_alche");
            new MapLoader().Load("alde_alche.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"alde_alche took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tur_dun06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tur_dun06");
            new MapLoader().Load("tur_dun06.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tur_dun06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_hub01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_hub01");
            new MapLoader().Load("ecl_hub01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ecl_hub01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ba_lost_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ba_lost");
            new MapLoader().Load("ba_lost.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ba_lost took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_y_1_2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_y_1-2");
            new MapLoader().Load("pvp_y_1-2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pvp_y_1-2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_int03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_int03");
            new MapLoader().Load("iz_int03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_int03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_ko_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_ko");
            new MapLoader().Load("job_ko.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_ko took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jupe_area1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jupe_area1");
            new MapLoader().Load("jupe_area1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"jupe_area1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hu_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hu_fild06");
            new MapLoader().Load("hu_fild06.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"hu_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild04");
            new MapLoader().Load("prt_fild04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_thana_step_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load thana_step");
            new MapLoader().Load("thana_step.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"thana_step took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yggdrasil01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yggdrasil01");
            new MapLoader().Load("yggdrasil01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yggdrasil01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ama_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ama_dun03");
            new MapLoader().Load("ama_dun03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ama_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild05");
            new MapLoader().Load("ein_fild05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ein_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_guild_vs1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load guild_vs1");
            new MapLoader().Load("guild_vs1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"guild_vs1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moscovia_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moscovia");
            new MapLoader().Load("moscovia.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moscovia took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild11_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild11");
            new MapLoader().Load("yuno_fild11.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_fild11 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild04");
            new MapLoader().Load("yuno_fild04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif03");
            new MapLoader().Load("ma_zif03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ma_zif03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_os_b_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@os_b");
            new MapLoader().Load("1@os_b.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@os_b took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_xmas_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load xmas");
            new MapLoader().Load("xmas.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"xmas took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_sp_cor_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load sp_cor");
            new MapLoader().Load("sp_cor.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"sp_cor took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild01");
            new MapLoader().Load("yuno_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_evt_mobroom_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load evt_mobroom");
            new MapLoader().Load("evt_mobroom.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"evt_mobroom took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_mcd_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@mcd");
            new MapLoader().Load("1@mcd.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@mcd took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t11_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t11");
            new MapLoader().Load("tha_t11.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tha_t11 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cave_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cave");
            new MapLoader().Load("cave.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"cave took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dew_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dew_dun02");
            new MapLoader().Load("dew_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"dew_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild03");
            new MapLoader().Load("ein_fild03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ein_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alde_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alde_dun01");
            new MapLoader().Load("alde_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"alde_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_kh_kiehl02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load kh_kiehl02");
            new MapLoader().Load("kh_kiehl02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"kh_kiehl02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_hunter_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_hunter");
            new MapLoader().Load("in_hunter.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"in_hunter took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif04");
            new MapLoader().Load("ma_zif04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ma_zif04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_c_tower3__Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load c_tower3_");
            new MapLoader().Load("c_tower3_.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"c_tower3_ took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_sewb2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_sewb2");
            new MapLoader().Load("prt_sewb2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_sewb2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_nif_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load nif_fild02");
            new MapLoader().Load("nif_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"nif_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_c_tower4_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load c_tower4");
            new MapLoader().Load("c_tower4.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"c_tower4 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_auction_01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load auction_01");
            new MapLoader().Load("auction_01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"auction_01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lou_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lou_dun03");
            new MapLoader().Load("lou_dun03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lou_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_cru_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_cru");
            new MapLoader().Load("job_cru.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_cru took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_nameless_i_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load nameless_i");
            new MapLoader().Load("nameless_i.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"nameless_i took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_orcsdun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load orcsdun02");
            new MapLoader().Load("orcsdun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"orcsdun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_pre_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_pre");
            new MapLoader().Load("yuno_pre.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_pre took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_06");
            new MapLoader().Load("mjolnir_06.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mjolnir_06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_gol2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@gol2");
            new MapLoader().Load("1@gol2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@gol2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_tdun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_tdun03");
            new MapLoader().Load("ecl_tdun03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ecl_tdun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_c_tower2__Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load c_tower2_");
            new MapLoader().Load("c_tower2_.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"c_tower2_ took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjo_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjo_dun01");
            new MapLoader().Load("mjo_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mjo_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_in01");
            new MapLoader().Load("ein_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ein_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_def01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@def01");
            new MapLoader().Load("1@def01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@def01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_thts_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@thts");
            new MapLoader().Load("1@thts.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@thts took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild13_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild13");
            new MapLoader().Load("ra_fild13.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_fild13 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_in03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_in03");
            new MapLoader().Load("yuno_in03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_in03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_anthell01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load anthell01");
            new MapLoader().Load("anthell01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"anthell01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_nif_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load nif_in");
            new MapLoader().Load("nif_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"nif_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_wizard_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_wizard");
            new MapLoader().Load("job_wizard.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_wizard took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payon_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payon_in01");
            new MapLoader().Load("payon_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"payon_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_air2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@air2");
            new MapLoader().Load("1@air2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@air2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mosk_que_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mosk_que");
            new MapLoader().Load("mosk_que.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mosk_que took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_rev_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@rev");
            new MapLoader().Load("1@rev.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@rev took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_sphinx1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_sphinx1");
            new MapLoader().Load("in_sphinx1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"in_sphinx1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_um_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load um_fild02");
            new MapLoader().Load("um_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"um_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_guil01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_guil01");
            new MapLoader().Load("job3_guil01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job3_guil01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_d04_i_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_d04_i");
            new MapLoader().Load("iz_d04_i.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_d04_i took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_fild03");
            new MapLoader().Load("lhz_fild03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lhz_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_conch_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load conch_in");
            new MapLoader().Load("conch_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"conch_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_ruins_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_ruins");
            new MapLoader().Load("moc_ruins.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_ruins took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_nameless_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load nameless_in");
            new MapLoader().Load("nameless_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"nameless_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_quiz_01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load quiz_01");
            new MapLoader().Load("quiz_01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"quiz_01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_airplane_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load airplane");
            new MapLoader().Load("airplane.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"airplane took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_izlude_c_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load izlude_c");
            new MapLoader().Load("izlude_c.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"izlude_c took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_maze02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_maze02");
            new MapLoader().Load("prt_maze02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_maze02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_cas02__Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_cas02_");
            new MapLoader().Load("gl_cas02_.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_cas02_ took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_slabw01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load slabw01");
            new MapLoader().Load("slabw01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"slabw01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac01_c_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac01_c");
            new MapLoader().Load("iz_ac01_c.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_ac01_c took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_xm_d_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@xm_d");
            new MapLoader().Load("1@xm_d.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@xm_d took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefenia04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefenia04");
            new MapLoader().Load("gefenia04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gefenia04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alde_tt02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alde_tt02");
            new MapLoader().Load("alde_tt02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"alde_tt02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_rebel_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load rebel_in");
            new MapLoader().Load("rebel_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"rebel_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_dun01");
            new MapLoader().Load("gef_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild18_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild18");
            new MapLoader().Load("moc_fild18.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild18 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_fild03");
            new MapLoader().Load("ve_fild03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ve_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mosk_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mosk_dun03");
            new MapLoader().Load("mosk_dun03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mosk_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dic_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dic_dun01");
            new MapLoader().Load("dic_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"dic_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lou_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lou_fild01");
            new MapLoader().Load("lou_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lou_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_int_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_int");
            new MapLoader().Load("iz_int.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_int took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild04");
            new MapLoader().Load("gef_fild04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_swat_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_swat");
            new MapLoader().Load("que_swat.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_swat took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_einbech_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load einbech");
            new MapLoader().Load("einbech.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"einbech took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild04");
            new MapLoader().Load("cmd_fild04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"cmd_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_lab_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@lab");
            new MapLoader().Load("1@lab.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@lab took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild09");
            new MapLoader().Load("ra_fild09.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_fild09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alberta_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alberta");
            new MapLoader().Load("alberta.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"alberta took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_gl_he_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@gl_he");
            new MapLoader().Load("1@gl_he.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@gl_he took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_pryd06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_pryd06");
            new MapLoader().Load("moc_pryd06.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_pryd06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_poring_w02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load poring_w02");
            new MapLoader().Load("poring_w02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"poring_w02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_fild01");
            new MapLoader().Load("lhz_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lhz_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild09");
            new MapLoader().Load("pay_fild09.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_fild09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_c_tower2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load c_tower2");
            new MapLoader().Load("c_tower2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"c_tower2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lasa_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lasa_fild02");
            new MapLoader().Load("lasa_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lasa_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_lhz_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_lhz");
            new MapLoader().Load("que_lhz.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_lhz took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild07");
            new MapLoader().Load("cmd_fild07.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"cmd_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_04");
            new MapLoader().Load("mjolnir_04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mjolnir_04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_lib_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_lib");
            new MapLoader().Load("prt_lib.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_lib took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payg_cas01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payg_cas01");
            new MapLoader().Load("payg_cas01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"payg_cas01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_step_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_step");
            new MapLoader().Load("gl_step.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_step took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_dun02");
            new MapLoader().Load("gl_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjo_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjo_dun03");
            new MapLoader().Load("mjo_dun03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mjo_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_int04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_int04");
            new MapLoader().Load("iz_int04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_int04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jupe_area2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jupe_area2");
            new MapLoader().Load("jupe_area2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"jupe_area2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_fild01");
            new MapLoader().Load("ma_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ma_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dali_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dali");
            new MapLoader().Load("dali.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"dali took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hu_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hu_fild07");
            new MapLoader().Load("hu_fild07.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"hu_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild05");
            new MapLoader().Load("prt_fild05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_knt_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_knt");
            new MapLoader().Load("job_knt.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_knt took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_morocc_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load morocc");
            new MapLoader().Load("morocc.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"morocc took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild12_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild12");
            new MapLoader().Load("yuno_fild12.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_fild12 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_kh_vila_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load kh_vila");
            new MapLoader().Load("kh_vila.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"kh_vila took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dicastes01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dicastes01");
            new MapLoader().Load("dicastes01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"dicastes01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_qsch01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_qsch01");
            new MapLoader().Load("que_qsch01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_qsch01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_d02_i_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_d02_i");
            new MapLoader().Load("ein_d02_i.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ein_d02_i took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild02");
            new MapLoader().Load("yuno_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_cube_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_cube");
            new MapLoader().Load("lhz_cube.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lhz_cube took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_cata_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@cata");
            new MapLoader().Load("1@cata.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@cata took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moro_vol_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moro_vol");
            new MapLoader().Load("moro_vol.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moro_vol took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_un_bunker_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load un_bunker");
            new MapLoader().Load("un_bunker.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"un_bunker took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_cas02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_cas02");
            new MapLoader().Load("gl_cas02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_cas02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_abbey01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load abbey01");
            new MapLoader().Load("abbey01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"abbey01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ordeal_a00_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ordeal_a00");
            new MapLoader().Load("ordeal_a00.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ordeal_a00 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild08");
            new MapLoader().Load("prt_fild08.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_fild08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild04");
            new MapLoader().Load("ein_fild04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ein_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alde_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alde_dun02");
            new MapLoader().Load("alde_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"alde_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gld2_ald_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gld2_ald");
            new MapLoader().Load("gld2_ald.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gld2_ald took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_int_land02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load int_land02");
            new MapLoader().Load("int_land02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"int_land02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_4_thts_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 4@thts");
            new MapLoader().Load("4@thts.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"4@thts took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild08a_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild08a");
            new MapLoader().Load("prt_fild08a.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_fild08a took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_are_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_are_in");
            new MapLoader().Load("prt_are_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_are_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ama_test_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ama_test");
            new MapLoader().Load("ama_test.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ama_test took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_halo_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@halo");
            new MapLoader().Load("1@halo.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@halo took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mosk_ship_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mosk_ship");
            new MapLoader().Load("mosk_ship.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mosk_ship took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mid_campin_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mid_campin");
            new MapLoader().Load("mid_campin.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mid_campin took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_knt01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_knt01");
            new MapLoader().Load("gl_knt01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_knt01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_07");
            new MapLoader().Load("mjolnir_07.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mjolnir_07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_rune01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_rune01");
            new MapLoader().Load("job3_rune01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job3_rune01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_tdun04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_tdun04");
            new MapLoader().Load("ecl_tdun04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ecl_tdun04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_3_tower_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 3@tower");
            new MapLoader().Load("3@tower.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"3@tower took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_odin_tem01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load odin_tem01");
            new MapLoader().Load("odin_tem01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"odin_tem01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ice_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ice_dun01");
            new MapLoader().Load("ice_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ice_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lighthalzen_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lighthalzen");
            new MapLoader().Load("lighthalzen.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lighthalzen took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_in04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_in04");
            new MapLoader().Load("yuno_in04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_in04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_anthell02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load anthell02");
            new MapLoader().Load("anthell02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"anthell02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_izlu2dun_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load izlu2dun");
            new MapLoader().Load("izlu2dun.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"izlu2dun took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ba_lib_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ba_lib");
            new MapLoader().Load("ba_lib.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ba_lib took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_treasure01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load treasure01");
            new MapLoader().Load("treasure01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"treasure01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dali02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dali02");
            new MapLoader().Load("dali02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"dali02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_malangdo_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load malangdo");
            new MapLoader().Load("malangdo.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"malangdo took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_alde_gld_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_alde_gld");
            new MapLoader().Load("te_alde_gld.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"te_alde_gld took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_maze03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_maze03");
            new MapLoader().Load("prt_maze03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_maze03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_sara_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@sara");
            new MapLoader().Load("1@sara.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@sara took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac01_d_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac01_d");
            new MapLoader().Load("iz_ac01_d.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_ac01_d took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jupe_gate_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jupe_gate");
            new MapLoader().Load("jupe_gate.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"jupe_gate took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_dun02");
            new MapLoader().Load("gef_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild19_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild19");
            new MapLoader().Load("moc_fild19.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild19 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_vis_h01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load vis_h01");
            new MapLoader().Load("vis_h01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"vis_h01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_fild04");
            new MapLoader().Load("ve_fild04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ve_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dic_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dic_dun02");
            new MapLoader().Load("dic_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"dic_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_louyang_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load louyang");
            new MapLoader().Load("louyang.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"louyang took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_schg_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load schg_dun01");
            new MapLoader().Load("schg_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"schg_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild05");
            new MapLoader().Load("gef_fild05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_gl_k_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@gl_k");
            new MapLoader().Load("1@gl_k.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@gl_k took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bif_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bif_fild02");
            new MapLoader().Load("bif_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"bif_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_izlude_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load izlude");
            new MapLoader().Load("izlude.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"izlude took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild07");
            new MapLoader().Load("pay_fild07.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild09");
            new MapLoader().Load("moc_fild09.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_e_hugel_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load e_hugel");
            new MapLoader().Load("e_hugel.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"e_hugel took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_begi_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@begi");
            new MapLoader().Load("1@begi.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@begi took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_monk_test_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load monk_test");
            new MapLoader().Load("monk_test.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"monk_test took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_temple_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_temple");
            new MapLoader().Load("ra_temple.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_temple took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hu_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hu_fild05");
            new MapLoader().Load("hu_fild05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"hu_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dew_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dew_fild01");
            new MapLoader().Load("dew_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"dew_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_dun04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_dun04");
            new MapLoader().Load("lhz_dun04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lhz_dun04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_verus04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load verus04");
            new MapLoader().Load("verus04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"verus04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_fild02");
            new MapLoader().Load("lhz_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lhz_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_odin_tem03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load odin_tem03");
            new MapLoader().Load("odin_tem03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"odin_tem03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alb2trea_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alb2trea");
            new MapLoader().Load("alb2trea.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"alb2trea took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ayo_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ayo_dun01");
            new MapLoader().Load("ayo_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ayo_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild08");
            new MapLoader().Load("cmd_fild08.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"cmd_fild08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_dun01");
            new MapLoader().Load("ma_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ma_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild14_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild14");
            new MapLoader().Load("gef_fild14.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_fild14 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_nyd_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@nyd");
            new MapLoader().Load("1@nyd.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@nyd took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_fild02");
            new MapLoader().Load("ma_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ma_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_quiz_02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load quiz_02");
            new MapLoader().Load("quiz_02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"quiz_02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ba_2whs01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ba_2whs01");
            new MapLoader().Load("ba_2whs01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ba_2whs01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild06");
            new MapLoader().Load("prt_fild06.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_para01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_para01");
            new MapLoader().Load("moc_para01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_para01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_prt_gld_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_prt_gld");
            new MapLoader().Load("te_prt_gld.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"te_prt_gld took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hu_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hu_fild04");
            new MapLoader().Load("hu_fild04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"hu_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefenia01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefenia01");
            new MapLoader().Load("gefenia01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gefenia01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild10");
            new MapLoader().Load("ra_fild10.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_fild10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payg_cas05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payg_cas05");
            new MapLoader().Load("payg_cas05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"payg_cas05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_manuk_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load manuk");
            new MapLoader().Load("manuk.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"manuk took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_treasure_n1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load treasure_n1");
            new MapLoader().Load("treasure_n1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"treasure_n1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_church_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_church");
            new MapLoader().Load("prt_church.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_church took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dicastes02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dicastes02");
            new MapLoader().Load("dicastes02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"dicastes02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif05");
            new MapLoader().Load("ma_zif05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ma_zif05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_sthb_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@sthb");
            new MapLoader().Load("1@sthb.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@sthb took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_mir_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@mir");
            new MapLoader().Load("2@mir.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"2@mir took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_house_s_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_house_s");
            new MapLoader().Load("que_house_s.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_house_s took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_izlude_d_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load izlude_d");
            new MapLoader().Load("izlude_d.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"izlude_d took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild10");
            new MapLoader().Load("pay_fild10.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_fild10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild03");
            new MapLoader().Load("yuno_fild03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_verus02_b_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load verus02_b");
            new MapLoader().Load("verus02_b.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"verus02_b took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_silk_lair_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load silk_lair");
            new MapLoader().Load("silk_lair.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"silk_lair took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_man_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load man_fild01");
            new MapLoader().Load("man_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"man_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prtg_cas01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prtg_cas01");
            new MapLoader().Load("prtg_cas01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prtg_cas01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_in02");
            new MapLoader().Load("lhz_in02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lhz_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ayo_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ayo_fild01");
            new MapLoader().Load("ayo_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ayo_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_x_lhz_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load x_lhz");
            new MapLoader().Load("x_lhz.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"x_lhz took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t04");
            new MapLoader().Load("tha_t04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tha_t04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dic_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dic_fild01");
            new MapLoader().Load("dic_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"dic_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t07");
            new MapLoader().Load("tha_t07.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tha_t07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alde_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alde_dun03");
            new MapLoader().Load("alde_dun03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"alde_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_int_land03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load int_land03");
            new MapLoader().Load("int_land03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"int_land03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild08b_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild08b");
            new MapLoader().Load("prt_fild08b.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_fild08b took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_gl_k2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@gl_k2");
            new MapLoader().Load("1@gl_k2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@gl_k2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_rachel_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_rachel");
            new MapLoader().Load("que_rachel.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_rachel took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_thts_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@thts");
            new MapLoader().Load("2@thts.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"2@thts took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_um_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load um_fild04");
            new MapLoader().Load("um_fild04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"um_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ayothaya_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ayothaya");
            new MapLoader().Load("ayothaya.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ayothaya took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_knt02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_knt02");
            new MapLoader().Load("gl_knt02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_knt02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_wiz_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_wiz");
            new MapLoader().Load("job_wiz.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_wiz took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_s_atelier_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load s_atelier");
            new MapLoader().Load("s_atelier.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"s_atelier took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_rune02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_rune02");
            new MapLoader().Load("job3_rune02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job3_rune02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_2vs2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_2vs2");
            new MapLoader().Load("pvp_2vs2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pvp_2vs2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lasa_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lasa_dun01");
            new MapLoader().Load("lasa_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lasa_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_def03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@def03");
            new MapLoader().Load("1@def03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@def03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_odin_tem02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load odin_tem02");
            new MapLoader().Load("odin_tem02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"odin_tem02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ice_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ice_dun02");
            new MapLoader().Load("ice_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ice_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jupe_ele_r_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jupe_ele_r");
            new MapLoader().Load("jupe_ele_r.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"jupe_ele_r took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_in05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_in05");
            new MapLoader().Load("yuno_in05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_in05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gld_dun04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gld_dun04");
            new MapLoader().Load("gld_dun04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gld_dun04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_prison1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_prison1");
            new MapLoader().Load("gl_prison1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_prison1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_man_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load man_in01");
            new MapLoader().Load("man_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"man_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_int01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_int01");
            new MapLoader().Load("iz_int01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_int01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_cas_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_cas");
            new MapLoader().Load("prt_cas.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_cas took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_q_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_q");
            new MapLoader().Load("prt_q.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_q took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild07");
            new MapLoader().Load("ein_fild07.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ein_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_evt_bomb_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load evt_bomb");
            new MapLoader().Load("evt_bomb.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"evt_bomb took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild05");
            new MapLoader().Load("moc_fild05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_treasure02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load treasure02");
            new MapLoader().Load("treasure02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"treasure02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_Prt_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load Prt_fild01");
            new MapLoader().Load("Prt_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"Prt_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_odin_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@odin");
            new MapLoader().Load("1@odin.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@odin took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_moc_16_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_moc_16");
            new MapLoader().Load("in_moc_16.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"in_moc_16 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_kh_rossi_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load kh_rossi");
            new MapLoader().Load("kh_rossi.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"kh_rossi took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_dun03");
            new MapLoader().Load("gef_dun03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alde_dun04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alde_dun04");
            new MapLoader().Load("alde_dun04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"alde_dun04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild10");
            new MapLoader().Load("moc_fild10.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_vis_h02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load vis_h02");
            new MapLoader().Load("vis_h02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"vis_h02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_fild05");
            new MapLoader().Load("ve_fild05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ve_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_rockmi1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load rockmi1");
            new MapLoader().Load("rockmi1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"rockmi1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bat_a01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bat_a01");
            new MapLoader().Load("bat_a01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"bat_a01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild06");
            new MapLoader().Load("gef_fild06.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_ecl_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@ecl");
            new MapLoader().Load("1@ecl.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@ecl took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_in02");
            new MapLoader().Load("ve_in02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ve_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_teg_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load teg_dun01");
            new MapLoader().Load("teg_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"teg_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dew_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dew_in01");
            new MapLoader().Load("dew_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"dew_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_thor_v03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load thor_v03");
            new MapLoader().Load("thor_v03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"thor_v03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_eom_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@eom");
            new MapLoader().Load("1@eom.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@eom took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_p_track01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load p_track01");
            new MapLoader().Load("p_track01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"p_track01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_izlude_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load izlude_in");
            new MapLoader().Load("izlude_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"izlude_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_star_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load star_in");
            new MapLoader().Load("star_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"star_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefg_cas01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefg_cas01");
            new MapLoader().Load("gefg_cas01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gefg_cas01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_rachel_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load rachel");
            new MapLoader().Load("rachel.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"rachel took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pub_cat_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pub_cat");
            new MapLoader().Load("pub_cat.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pub_cat took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_chyard_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_chyard");
            new MapLoader().Load("gl_chyard.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_chyard took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_are01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_are01");
            new MapLoader().Load("prt_are01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_are01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_gl_kh_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@gl_kh");
            new MapLoader().Load("2@gl_kh.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"2@gl_kh took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_ba_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_ba");
            new MapLoader().Load("que_ba.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_ba took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ba_2whs02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ba_2whs02");
            new MapLoader().Load("ba_2whs02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ba_2whs02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild07");
            new MapLoader().Load("prt_fild07.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_force_map1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load force_map1");
            new MapLoader().Load("force_map1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"force_map1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefenia02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefenia02");
            new MapLoader().Load("gefenia02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gefenia02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild09");
            new MapLoader().Load("cmd_fild09.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"cmd_fild09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_thor_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_thor");
            new MapLoader().Load("que_thor.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_thor took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_orcs_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@orcs");
            new MapLoader().Load("1@orcs.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@orcs took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payg_cas02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payg_cas02");
            new MapLoader().Load("payg_cas02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"payg_cas02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tur_d03_i_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tur_d03_i");
            new MapLoader().Load("tur_d03_i.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tur_d03_i took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild11_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild11");
            new MapLoader().Load("ra_fild11.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_fild11 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_sew01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_sew01");
            new MapLoader().Load("gl_sew01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_sew01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_treasure_n2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load treasure_n2");
            new MapLoader().Load("treasure_n2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"treasure_n2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_monk_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_monk");
            new MapLoader().Load("prt_monk.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_monk took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_int_land_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load int_land");
            new MapLoader().Load("int_land.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"int_land took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_aldecas1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_aldecas1");
            new MapLoader().Load("te_aldecas1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"te_aldecas1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gld2_pay_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gld2_pay");
            new MapLoader().Load("gld2_pay.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gld2_pay took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_pop1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@pop1");
            new MapLoader().Load("1@pop1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@pop1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif06");
            new MapLoader().Load("ma_zif06.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ma_zif06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_09");
            new MapLoader().Load("mjolnir_09.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mjolnir_09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_sthc_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@sthc");
            new MapLoader().Load("1@sthc.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@sthc took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild07");
            new MapLoader().Load("ra_fild07.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_harboro2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load harboro2");
            new MapLoader().Load("harboro2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"harboro2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild01");
            new MapLoader().Load("ra_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild12_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild12");
            new MapLoader().Load("gef_fild12.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_fild12 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hugel_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hugel");
            new MapLoader().Load("hugel.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"hugel took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prtg_cas02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prtg_cas02");
            new MapLoader().Load("prtg_cas02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prtg_cas02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_soul_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@soul");
            new MapLoader().Load("1@soul.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@soul took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_temsky_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_temsky");
            new MapLoader().Load("ra_temsky.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_temsky took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_duncer_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_duncer");
            new MapLoader().Load("job_duncer.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_duncer took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ayo_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ayo_in01");
            new MapLoader().Load("ayo_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ayo_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_crd_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@crd");
            new MapLoader().Load("1@crd.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@crd took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_evt_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_evt_in");
            new MapLoader().Load("prt_evt_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_evt_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dic_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dic_fild02");
            new MapLoader().Load("dic_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"dic_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_aldeg_cas01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load aldeg_cas01");
            new MapLoader().Load("aldeg_cas01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"aldeg_cas01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_int_land04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load int_land04");
            new MapLoader().Load("int_land04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"int_land04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild08c_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild08c");
            new MapLoader().Load("prt_fild08c.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_fild08c took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_arug_cas01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load arug_cas01");
            new MapLoader().Load("arug_cas01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"arug_cas01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_mir_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@mir");
            new MapLoader().Load("1@mir.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@mir took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_rockrdg2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load rockrdg2");
            new MapLoader().Load("rockrdg2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"rockrdg2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mosk_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mosk_fild01");
            new MapLoader().Load("mosk_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mosk_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_orcs01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_orcs01");
            new MapLoader().Load("in_orcs01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"in_orcs01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_arch01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_arch01");
            new MapLoader().Load("job3_arch01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job3_arch01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_ma_b_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@ma_b");
            new MapLoader().Load("1@ma_b.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@ma_b took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ice_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ice_dun03");
            new MapLoader().Load("ice_dun03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ice_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno");
            new MapLoader().Load("yuno.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_lhz_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@lhz");
            new MapLoader().Load("1@lhz.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@lhz took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_sha01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_sha01");
            new MapLoader().Load("job3_sha01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job3_sha01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mal_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mal_in01");
            new MapLoader().Load("mal_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mal_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_xmas_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load xmas_dun02");
            new MapLoader().Load("xmas_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"xmas_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_tnm1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@tnm1");
            new MapLoader().Load("1@tnm1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@tnm1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pprontera_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pprontera");
            new MapLoader().Load("pprontera.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pprontera took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_gl_kh_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@gl_kh");
            new MapLoader().Load("1@gl_kh.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@gl_kh took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_abyss_01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load abyss_01");
            new MapLoader().Load("abyss_01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"abyss_01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_gun_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_gun");
            new MapLoader().Load("job_gun.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_gun took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mosk_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mosk_in");
            new MapLoader().Load("mosk_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mosk_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_juperos_01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load juperos_01");
            new MapLoader().Load("juperos_01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"juperos_01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_gl_he2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@gl_he2");
            new MapLoader().Load("1@gl_he2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@gl_he2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_prydn2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_prydn2");
            new MapLoader().Load("moc_prydn2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_prydn2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_vis_h03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load vis_h03");
            new MapLoader().Load("vis_h03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"vis_h03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_fild06");
            new MapLoader().Load("ve_fild06.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ve_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_um_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load um_in");
            new MapLoader().Load("um_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"um_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jupe_cave_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jupe_cave");
            new MapLoader().Load("jupe_cave.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"jupe_cave took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild07");
            new MapLoader().Load("gef_fild07.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_pop3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@pop3");
            new MapLoader().Load("1@pop3.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@pop3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ama_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ama_in02");
            new MapLoader().Load("ama_in02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ama_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_cas_q_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_cas_q");
            new MapLoader().Load("prt_cas_q.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_cas_q took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_md_gef_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@md_gef");
            new MapLoader().Load("1@md_gef.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@md_gef took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_san05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_san05");
            new MapLoader().Load("ra_san05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_san05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_in04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_in04");
            new MapLoader().Load("ecl_in04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ecl_in04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_turbo_room_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load turbo_room");
            new MapLoader().Load("turbo_room.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"turbo_room took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_arug_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load arug_dun01");
            new MapLoader().Load("arug_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"arug_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefg_cas02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefg_cas02");
            new MapLoader().Load("gefg_cas02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gefg_cas02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac01");
            new MapLoader().Load("iz_ac01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_ac01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_einbroch_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load einbroch");
            new MapLoader().Load("einbroch.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"einbroch took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild02");
            new MapLoader().Load("prt_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_uns_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@uns");
            new MapLoader().Load("1@uns.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@uns took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_in01");
            new MapLoader().Load("gl_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild06");
            new MapLoader().Load("ein_fild06.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ein_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild21_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild21");
            new MapLoader().Load("moc_fild21.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild21 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_com_d02_i_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load com_d02_i");
            new MapLoader().Load("com_d02_i.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"com_d02_i took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_man_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load man_fild03");
            new MapLoader().Load("man_fild03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"man_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_force_map2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load force_map2");
            new MapLoader().Load("force_map2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"force_map2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefenia03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefenia03");
            new MapLoader().Load("gefenia03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gefenia03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_aldebaran_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load aldebaran");
            new MapLoader().Load("aldebaran.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"aldebaran took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild12_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild12");
            new MapLoader().Load("ra_fild12.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_fild12 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_sew02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_sew02");
            new MapLoader().Load("gl_sew02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_sew02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_gef_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@gef_in");
            new MapLoader().Load("1@gef_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@gef_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_7_thts_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 7@thts");
            new MapLoader().Load("7@thts.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"7@thts took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payon_in03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payon_in03");
            new MapLoader().Load("payon_in03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"payon_in03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_new_zone01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load new_zone01");
            new MapLoader().Load("new_zone01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"new_zone01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_d01_i_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_d01_i");
            new MapLoader().Load("gef_d01_i.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_d01_i took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_aldecas2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_aldecas2");
            new MapLoader().Load("te_aldecas2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"te_aldecas2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild04");
            new MapLoader().Load("pay_fild04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild01");
            new MapLoader().Load("moc_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif07");
            new MapLoader().Load("ma_zif07.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ma_zif07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_sthd_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@sthd");
            new MapLoader().Load("1@sthd.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@sthd took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild02");
            new MapLoader().Load("ra_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payg_cas03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payg_cas03");
            new MapLoader().Load("payg_cas03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"payg_cas03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_dun00_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_dun00");
            new MapLoader().Load("iz_dun00.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_dun00 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild10");
            new MapLoader().Load("gef_fild10.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_fild10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_arug_que01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load arug_que01");
            new MapLoader().Load("arug_que01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"arug_que01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_knight_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_knight");
            new MapLoader().Load("job_knight.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_knight took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_dun03");
            new MapLoader().Load("lhz_dun03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lhz_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_new_event_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load new_event");
            new MapLoader().Load("new_event.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"new_event took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild04");
            new MapLoader().Load("moc_fild04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_war01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_war01");
            new MapLoader().Load("job3_war01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job3_war01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_siege_test_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load siege_test");
            new MapLoader().Load("siege_test.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"siege_test took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild00_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild00");
            new MapLoader().Load("prt_fild00.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_fild00 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_sec_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load sec_in01");
            new MapLoader().Load("sec_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"sec_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_3_thts_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 3@thts");
            new MapLoader().Load("3@thts.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"3@thts took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild02");
            new MapLoader().Load("pay_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_odin_past_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load odin_past");
            new MapLoader().Load("odin_past.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"odin_past took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hu_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hu_fild02");
            new MapLoader().Load("hu_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"hu_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_in01");
            new MapLoader().Load("ra_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_aldeg_cas02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load aldeg_cas02");
            new MapLoader().Load("aldeg_cas02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"aldeg_cas02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payon_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payon");
            new MapLoader().Load("payon.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"payon took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild08d_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild08d");
            new MapLoader().Load("prt_fild08d.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_fild08d took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_4_tower_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 4@tower");
            new MapLoader().Load("4@tower.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"4@tower took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_verus02_a_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load verus02_a");
            new MapLoader().Load("verus02_a.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"verus02_a took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_beach_dun3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load beach_dun3");
            new MapLoader().Load("beach_dun3.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"beach_dun3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_arug_cas02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load arug_cas02");
            new MapLoader().Load("arug_cas02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"arug_cas02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_un_bk_q_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load un_bk_q");
            new MapLoader().Load("un_bk_q.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"un_bk_q took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lou_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lou_dun01");
            new MapLoader().Load("lou_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lou_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_sphinx5_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_sphinx5");
            new MapLoader().Load("in_sphinx5.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"in_sphinx5 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_thor_v02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load thor_v02");
            new MapLoader().Load("thor_v02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"thor_v02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_ma_c_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@ma_c");
            new MapLoader().Load("1@ma_c.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@ma_c took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lasa_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lasa_dun03");
            new MapLoader().Load("lasa_dun03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lasa_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jawaii_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jawaii");
            new MapLoader().Load("jawaii.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"jawaii took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ice_dun04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ice_dun04");
            new MapLoader().Load("ice_dun04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ice_dun04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lasagna_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lasagna");
            new MapLoader().Load("lasagna.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lasagna took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_spl_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load spl_fild01");
            new MapLoader().Load("spl_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"spl_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_prist_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_prist");
            new MapLoader().Load("job_prist.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_prist took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bra_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bra_fild01");
            new MapLoader().Load("bra_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"bra_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mal_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mal_in02");
            new MapLoader().Load("mal_in02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mal_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_tnm2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@tnm2");
            new MapLoader().Load("1@tnm2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@tnm2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_abyss_02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load abyss_02");
            new MapLoader().Load("abyss_02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"abyss_02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_juperos_02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load juperos_02");
            new MapLoader().Load("juperos_02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"juperos_02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_rang01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_rang01");
            new MapLoader().Load("job3_rang01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job3_rang01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_lib_q_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_lib_q");
            new MapLoader().Load("prt_lib_q.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_lib_q took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jupe_core_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jupe_core");
            new MapLoader().Load("jupe_core.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"jupe_core took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_sewb1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_sewb1");
            new MapLoader().Load("prt_sewb1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_sewb1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_vis_h04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load vis_h04");
            new MapLoader().Load("vis_h04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"vis_h04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_10");
            new MapLoader().Load("mjolnir_10.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mjolnir_10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_fild07");
            new MapLoader().Load("ve_fild07.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ve_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild08");
            new MapLoader().Load("gef_fild08.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_fild08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_sp_rudus3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load sp_rudus3");
            new MapLoader().Load("sp_rudus3.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"sp_rudus3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_arena_room_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load arena_room");
            new MapLoader().Load("arena_room.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"arena_room took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_prison_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_prison");
            new MapLoader().Load("gl_prison.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_prison took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_force_map3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load force_map3");
            new MapLoader().Load("force_map3.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"force_map3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ice_d03_i_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ice_d03_i");
            new MapLoader().Load("ice_d03_i.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ice_d03_i took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bat_c01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bat_c01");
            new MapLoader().Load("bat_c01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"bat_c01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_sign01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_sign01");
            new MapLoader().Load("que_sign01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_sign01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alberta_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alberta_in");
            new MapLoader().Load("alberta_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"alberta_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_thana_boss_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load thana_boss");
            new MapLoader().Load("thana_boss.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"thana_boss took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_dan01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_dan01");
            new MapLoader().Load("que_dan01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_dan01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_rockmi2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load rockmi2");
            new MapLoader().Load("rockmi2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"rockmi2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_um_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load um_fild01");
            new MapLoader().Load("um_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"um_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_tower_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_tower");
            new MapLoader().Load("gef_tower.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_tower took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_rgsr_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load rgsr_in");
            new MapLoader().Load("rgsr_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"rgsr_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefg_cas03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefg_cas03");
            new MapLoader().Load("gefg_cas03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gefg_cas03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild08");
            new MapLoader().Load("ra_fild08.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_fild08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tur_d04_i_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tur_d04_i");
            new MapLoader().Load("tur_d04_i.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tur_d04_i took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_scene01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_scene01");
            new MapLoader().Load("tha_scene01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tha_scene01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payon_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payon_in02");
            new MapLoader().Load("payon_in02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"payon_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild20_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild20");
            new MapLoader().Load("moc_fild20.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild20 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild22_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild22");
            new MapLoader().Load("moc_fild22.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild22 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_brasilis_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load brasilis");
            new MapLoader().Load("brasilis.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"brasilis took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_nameless_n_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load nameless_n");
            new MapLoader().Load("nameless_n.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"nameless_n took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t05");
            new MapLoader().Load("tha_t05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tha_t05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_schg_cas01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load schg_cas01");
            new MapLoader().Load("schg_cas01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"schg_cas01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild09");
            new MapLoader().Load("prt_fild09.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_fild09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_veins_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load veins");
            new MapLoader().Load("veins.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"veins took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_cata_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@cata");
            new MapLoader().Load("2@cata.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"2@cata took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild12_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild12");
            new MapLoader().Load("moc_fild12.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild12 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t03");
            new MapLoader().Load("tha_t03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tha_t03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_sew03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_sew03");
            new MapLoader().Load("gl_sew03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_sew03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_monk_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_monk");
            new MapLoader().Load("job_monk.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_monk took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mal_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mal_dun01");
            new MapLoader().Load("mal_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mal_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_new_zone02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load new_zone02");
            new MapLoader().Load("new_zone02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"new_zone02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_aldecas3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_aldecas3");
            new MapLoader().Load("te_aldecas3.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"te_aldecas3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_6_thts_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 6@thts");
            new MapLoader().Load("6@thts.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"6@thts took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild02");
            new MapLoader().Load("moc_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_dun02");
            new MapLoader().Load("lhz_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lhz_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bra_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bra_dun01");
            new MapLoader().Load("bra_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"bra_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_dun01");
            new MapLoader().Load("iz_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gld2_gef_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gld2_gef");
            new MapLoader().Load("gld2_gef.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gld2_gef took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_gld_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_gld");
            new MapLoader().Load("prt_gld.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_gld took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_nyd_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@nyd");
            new MapLoader().Load("2@nyd.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"2@nyd took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prtg_cas04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prtg_cas04");
            new MapLoader().Load("prtg_cas04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prtg_cas04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_xmas_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load xmas_fild01");
            new MapLoader().Load("xmas_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"xmas_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_dun00_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_dun00");
            new MapLoader().Load("pay_dun00.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_dun00 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_harboro1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load harboro1");
            new MapLoader().Load("harboro1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"harboro1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ordeal_a02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ordeal_a02");
            new MapLoader().Load("ordeal_a02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ordeal_a02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_sec_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load sec_in02");
            new MapLoader().Load("sec_in02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"sec_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ayo_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ayo_dun02");
            new MapLoader().Load("ayo_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ayo_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild03");
            new MapLoader().Load("pay_fild03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_cas01__Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_cas01_");
            new MapLoader().Load("gl_cas01_.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_cas01_ took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild01");
            new MapLoader().Load("cmd_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"cmd_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tur_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tur_dun01");
            new MapLoader().Load("tur_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tur_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_ng_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_ng");
            new MapLoader().Load("que_ng.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_ng took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_aldeg_cas03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load aldeg_cas03");
            new MapLoader().Load("aldeg_cas03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"aldeg_cas03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_nyd_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load nyd_dun02");
            new MapLoader().Load("nyd_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"nyd_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_arug_cas03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load arug_cas03");
            new MapLoader().Load("arug_cas03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"arug_cas03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_pop2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@pop2");
            new MapLoader().Load("1@pop2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@pop2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hu_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hu_fild01");
            new MapLoader().Load("hu_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"hu_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_y_8_2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_y_8-2");
            new MapLoader().Load("pvp_y_8-2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pvp_y_8-2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hu_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hu_in01");
            new MapLoader().Load("hu_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"hu_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prtg_cas03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prtg_cas03");
            new MapLoader().Load("prtg_cas03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prtg_cas03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_y_6_2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_y_6-2");
            new MapLoader().Load("pvp_y_6-2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pvp_y_6-2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_dun03");
            new MapLoader().Load("ein_dun03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ein_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gld_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gld_dun01");
            new MapLoader().Load("gld_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gld_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ama_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ama_fild01");
            new MapLoader().Load("ama_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ama_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_y_4_2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_y_4-2");
            new MapLoader().Load("pvp_y_4-2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pvp_y_4-2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bat_room_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bat_room");
            new MapLoader().Load("bat_room.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"bat_room took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_md_pay_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@md_pay");
            new MapLoader().Load("1@md_pay.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@md_pay took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_sphinx2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_sphinx2");
            new MapLoader().Load("in_sphinx2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"in_sphinx2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif08");
            new MapLoader().Load("ma_zif08.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ma_zif08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gon_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gon_fild01");
            new MapLoader().Load("gon_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gon_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_c_tower1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load c_tower1");
            new MapLoader().Load("c_tower1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"c_tower1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_in01");
            new MapLoader().Load("cmd_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"cmd_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_d_n2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_d_n2");
            new MapLoader().Load("lhz_d_n2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lhz_d_n2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ver_eju_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ver_eju");
            new MapLoader().Load("ver_eju.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ver_eju took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_spl_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load spl_fild02");
            new MapLoader().Load("spl_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"spl_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_glast_01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load glast_01");
            new MapLoader().Load("glast_01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"glast_01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dew_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dew_dun01");
            new MapLoader().Load("dew_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"dew_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_xm_d2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@xm_d2");
            new MapLoader().Load("1@xm_d2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@xm_d2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_tnm3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@tnm3");
            new MapLoader().Load("1@tnm3.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@tnm3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_thor_camp_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load thor_camp");
            new MapLoader().Load("thor_camp.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"thor_camp took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lou_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lou_in01");
            new MapLoader().Load("lou_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lou_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_abyss_03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load abyss_03");
            new MapLoader().Load("abyss_03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"abyss_03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_sp_rudus_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load sp_rudus");
            new MapLoader().Load("sp_rudus.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"sp_rudus took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_rang02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_rang02");
            new MapLoader().Load("job3_rang02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job3_rang02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_dun01");
            new MapLoader().Load("lhz_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lhz_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_spl_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load spl_in02");
            new MapLoader().Load("spl_in02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"spl_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t12_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t12");
            new MapLoader().Load("tha_t12.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tha_t12 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ng01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ng01");
            new MapLoader().Load("iz_ng01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_ng01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ayo_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ayo_fild02");
            new MapLoader().Load("ayo_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ayo_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_temin_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_temin");
            new MapLoader().Load("ra_temin.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_temin took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild08");
            new MapLoader().Load("ein_fild08.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ein_fild08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_in01");
            new MapLoader().Load("ecl_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ecl_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t10");
            new MapLoader().Load("tha_t10.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tha_t10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_dan02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_dan02");
            new MapLoader().Load("que_dan02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_dan02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_rgsr_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@rgsr");
            new MapLoader().Load("1@rgsr.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@rgsr took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_arch02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_arch02");
            new MapLoader().Load("job3_arch02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job3_arch02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_01");
            new MapLoader().Load("mjolnir_01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mjolnir_01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_malaya_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load malaya");
            new MapLoader().Load("malaya.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"malaya took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_guild_vs2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load guild_vs2");
            new MapLoader().Load("guild_vs2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"guild_vs2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefg_cas04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefg_cas04");
            new MapLoader().Load("gefg_cas04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gefg_cas04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_sage_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_sage");
            new MapLoader().Load("job_sage.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_sage took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ba_bath_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ba_bath");
            new MapLoader().Load("ba_bath.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ba_bath took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_x_prt_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load x_prt");
            new MapLoader().Load("x_prt.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"x_prt took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lasa_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lasa_fild01");
            new MapLoader().Load("lasa_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lasa_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t06");
            new MapLoader().Load("tha_t06.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tha_t06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_sphinx4_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_sphinx4");
            new MapLoader().Load("in_sphinx4.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"in_sphinx4 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_schg_cas02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load schg_cas02");
            new MapLoader().Load("schg_cas02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"schg_cas02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild13_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild13");
            new MapLoader().Load("moc_fild13.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild13 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_dun_n_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_dun_n");
            new MapLoader().Load("lhz_dun_n.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lhz_dun_n took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_orcs_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@orcs");
            new MapLoader().Load("2@orcs.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"2@orcs took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_sew04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_sew04");
            new MapLoader().Load("gl_sew04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_sew04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild06");
            new MapLoader().Load("cmd_fild06.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"cmd_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_amatsu_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load amatsu");
            new MapLoader().Load("amatsu.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"amatsu took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_new_zone03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load new_zone03");
            new MapLoader().Load("new_zone03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"new_zone03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_aldecas4_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_aldecas4");
            new MapLoader().Load("te_aldecas4.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"te_aldecas4 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild03");
            new MapLoader().Load("moc_fild03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif09");
            new MapLoader().Load("ma_zif09.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ma_zif09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ba_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ba_in01");
            new MapLoader().Load("ba_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ba_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_tower_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@tower");
            new MapLoader().Load("1@tower.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@tower took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild04");
            new MapLoader().Load("ra_fild04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_fild04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_dun02");
            new MapLoader().Load("iz_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild07_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild07");
            new MapLoader().Load("yuno_fild07.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_fild07 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prtg_cas05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prtg_cas05");
            new MapLoader().Load("prtg_cas05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prtg_cas05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mora_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mora");
            new MapLoader().Load("mora.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mora took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_pryd01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_pryd01");
            new MapLoader().Load("moc_pryd01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_pryd01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dewata_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dewata");
            new MapLoader().Load("dewata.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"dewata took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_dun01");
            new MapLoader().Load("pay_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_sch_gld_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load sch_gld");
            new MapLoader().Load("sch_gld.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"sch_gld took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_chyard__Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_chyard_");
            new MapLoader().Load("gl_chyard_.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_chyard_ took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild02");
            new MapLoader().Load("cmd_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"cmd_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tur_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tur_dun02");
            new MapLoader().Load("tur_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tur_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_aldeg_cas04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load aldeg_cas04");
            new MapLoader().Load("aldeg_cas04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"aldeg_cas04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_beach_dun_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load beach_dun");
            new MapLoader().Load("beach_dun.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"beach_dun took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ver_tunn_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ver_tunn");
            new MapLoader().Load("ver_tunn.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ver_tunn took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_prtcas01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_prtcas01");
            new MapLoader().Load("te_prtcas01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"te_prtcas01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_dic_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load dic_in01");
            new MapLoader().Load("dic_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"dic_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_xmas_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load xmas_dun01");
            new MapLoader().Load("xmas_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"xmas_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gld_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gld_dun02");
            new MapLoader().Load("gld_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gld_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_cas01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_cas01");
            new MapLoader().Load("gl_cas01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_cas01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_sphinx3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_sphinx3");
            new MapLoader().Load("in_sphinx3.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"in_sphinx3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_prison_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_prison");
            new MapLoader().Load("prt_prison.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_prison took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_sewb3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_sewb3");
            new MapLoader().Load("prt_sewb3.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_sewb3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_aldeba_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load aldeba_in");
            new MapLoader().Load("aldeba_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"aldeba_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_paramk_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load paramk");
            new MapLoader().Load("paramk.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"paramk took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_in02");
            new MapLoader().Load("cmd_in02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"cmd_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t08");
            new MapLoader().Load("tha_t08.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tha_t08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_spl_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load spl_fild03");
            new MapLoader().Load("spl_fild03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"spl_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_spa_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@spa");
            new MapLoader().Load("1@spa.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@spa took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_sec_pri_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load sec_pri");
            new MapLoader().Load("sec_pri.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"sec_pri took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_sp_rudus2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load sp_rudus2");
            new MapLoader().Load("sp_rudus2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"sp_rudus2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild10");
            new MapLoader().Load("prt_fild10.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_fild10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_gen01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_gen01");
            new MapLoader().Load("job3_gen01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job3_gen01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_04_1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_04_1");
            new MapLoader().Load("mjolnir_04_1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mjolnir_04_1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_in01");
            new MapLoader().Load("ma_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ma_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac02_a_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac02_a");
            new MapLoader().Load("iz_ac02_a.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_ac02_a took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_church_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_church");
            new MapLoader().Load("gl_church.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_church took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_abyss_04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load abyss_04");
            new MapLoader().Load("abyss_04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"abyss_04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild01");
            new MapLoader().Load("ein_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ein_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t02");
            new MapLoader().Load("tha_t02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tha_t02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alb_ship_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alb_ship");
            new MapLoader().Load("alb_ship.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"alb_ship took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_12_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_12");
            new MapLoader().Load("mjolnir_12.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mjolnir_12 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_nyd_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load nyd_dun01");
            new MapLoader().Load("nyd_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"nyd_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mag_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mag_dun01");
            new MapLoader().Load("mag_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mag_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac02");
            new MapLoader().Load("iz_ac02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_ac02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lasa_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lasa_dun02");
            new MapLoader().Load("lasa_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lasa_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_xmas_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load xmas_in");
            new MapLoader().Load("xmas_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"xmas_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_job02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_job02");
            new MapLoader().Load("que_job02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_job02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lou_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lou_in02");
            new MapLoader().Load("lou_in02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lou_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_02");
            new MapLoader().Load("mjolnir_02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mjolnir_02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_in02");
            new MapLoader().Load("ecl_in02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ecl_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_airport_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load airport");
            new MapLoader().Load("airport.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"airport took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_dun02");
            new MapLoader().Load("ein_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ein_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_room_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_room");
            new MapLoader().Load("pvp_room.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pvp_room took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ba_pw01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ba_pw01");
            new MapLoader().Load("ba_pw01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ba_pw01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_06guild_r_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 06guild_r");
            new MapLoader().Load("06guild_r.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"06guild_r took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gefg_cas05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gefg_cas05");
            new MapLoader().Load("gefg_cas05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gefg_cas05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_gef_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@gef");
            new MapLoader().Load("1@gef.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@gef took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_thief1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_thief1");
            new MapLoader().Load("job_thief1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_thief1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_x_ra_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load x_ra");
            new MapLoader().Load("x_ra.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"x_ra took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_pump_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@pump");
            new MapLoader().Load("2@pump.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"2@pump took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_verus02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load verus02");
            new MapLoader().Load("verus02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"verus02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_mz03_i_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_mz03_i");
            new MapLoader().Load("prt_mz03_i.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_mz03_i took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_hugel_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_hugel");
            new MapLoader().Load("que_hugel.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_hugel took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_schg_cas03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load schg_cas03");
            new MapLoader().Load("schg_cas03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"schg_cas03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild14_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild14");
            new MapLoader().Load("moc_fild14.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild14 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_har_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load har_in01");
            new MapLoader().Load("har_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"har_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild00_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild00");
            new MapLoader().Load("gef_fild00.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_fild00 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bat_b01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bat_b01");
            new MapLoader().Load("bat_b01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"bat_b01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_new_zone04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load new_zone04");
            new MapLoader().Load("new_zone04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"new_zone04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_ge_st_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@ge_st");
            new MapLoader().Load("1@ge_st.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@ge_st took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_aldecas5_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_aldecas5");
            new MapLoader().Load("te_aldecas5.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"te_aldecas5 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_08");
            new MapLoader().Load("mjolnir_08.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mjolnir_08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_mist_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@mist");
            new MapLoader().Load("1@mist.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@mist took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild05");
            new MapLoader().Load("ra_fild05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ayo_in02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ayo_in02");
            new MapLoader().Load("ayo_in02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ayo_in02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_dun02");
            new MapLoader().Load("pay_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_dun03");
            new MapLoader().Load("iz_dun03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_5_tower_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 5@tower");
            new MapLoader().Load("5@tower.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"5@tower took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild08");
            new MapLoader().Load("yuno_fild08.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_fild08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_san01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_san01");
            new MapLoader().Load("ra_san01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_san01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_pryd02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_pryd02");
            new MapLoader().Load("moc_pryd02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_pryd02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild05");
            new MapLoader().Load("pay_fild05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_infi_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@infi");
            new MapLoader().Load("1@infi.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@infi took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_5_thts_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 5@thts");
            new MapLoader().Load("5@thts.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"5@thts took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_in01");
            new MapLoader().Load("lhz_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lhz_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild03");
            new MapLoader().Load("cmd_fild03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"cmd_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tur_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tur_dun03");
            new MapLoader().Load("tur_dun03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tur_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_aldeg_cas05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load aldeg_cas05");
            new MapLoader().Load("aldeg_cas05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"aldeg_cas05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild11_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild11");
            new MapLoader().Load("prt_fild11.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_fild11 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lasa_sea_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lasa_sea");
            new MapLoader().Load("lasa_sea.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lasa_sea took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gon_test_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gon_test");
            new MapLoader().Load("gon_test.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gon_test took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_herbs_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@herbs");
            new MapLoader().Load("1@herbs.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@herbs took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_prtcas02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_prtcas02");
            new MapLoader().Load("te_prtcas02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"te_prtcas02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_hu_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load hu_fild03");
            new MapLoader().Load("hu_fild03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"hu_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mosk_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mosk_dun01");
            new MapLoader().Load("mosk_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mosk_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gld_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gld_dun03");
            new MapLoader().Load("gld_dun03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gld_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gon_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gon_dun02");
            new MapLoader().Load("gon_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gon_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_rockrdg1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load rockrdg1");
            new MapLoader().Load("rockrdg1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"rockrdg1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_int_land01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load int_land01");
            new MapLoader().Load("int_land01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"int_land01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_valkyrie_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load valkyrie");
            new MapLoader().Load("valkyrie.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"valkyrie took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_arche_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_arche");
            new MapLoader().Load("pay_arche.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_arche took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_gl_k_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@gl_k");
            new MapLoader().Load("2@gl_k.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"2@gl_k took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_pump_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@pump");
            new MapLoader().Load("1@pump.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@pump took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_verus01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load verus01");
            new MapLoader().Load("verus01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"verus01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild10");
            new MapLoader().Load("ein_fild10.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ein_fild10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_in_rogue_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load in_rogue");
            new MapLoader().Load("in_rogue.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"in_rogue took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac02_b_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac02_b");
            new MapLoader().Load("iz_ac02_b.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_ac02_b took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_sewb4_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_sewb4");
            new MapLoader().Load("prt_sewb4.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_sewb4 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bra_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bra_dun02");
            new MapLoader().Load("bra_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"bra_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_niflheim_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load niflheim");
            new MapLoader().Load("niflheim.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"niflheim took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_def02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@def02");
            new MapLoader().Load("1@def02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@def02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jupe_ele_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jupe_ele");
            new MapLoader().Load("jupe_ele.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"jupe_ele took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_ghg_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@ghg");
            new MapLoader().Load("1@ghg.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@ghg took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mag_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mag_dun02");
            new MapLoader().Load("mag_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mag_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_n_castle_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load n_castle");
            new MapLoader().Load("n_castle.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"n_castle took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_03");
            new MapLoader().Load("mjolnir_03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mjolnir_03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_um_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load um_fild03");
            new MapLoader().Load("um_fild03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"um_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_y_7_2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_y_7-2");
            new MapLoader().Load("pvp_y_7-2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pvp_y_7-2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_um_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load um_dun01");
            new MapLoader().Load("um_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"um_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_in03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_in03");
            new MapLoader().Load("ecl_in03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ecl_in03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_cor_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@cor");
            new MapLoader().Load("1@cor.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@cor took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_y_3_2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_y_3-2");
            new MapLoader().Load("pvp_y_3-2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pvp_y_3-2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_scene01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_scene01");
            new MapLoader().Load("ma_scene01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ma_scene01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ba_pw02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ba_pw02");
            new MapLoader().Load("ba_pw02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ba_pw02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_swat_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@swat");
            new MapLoader().Load("1@swat.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@swat took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gon_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gon_in");
            new MapLoader().Load("gon_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gon_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjo_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjo_dun02");
            new MapLoader().Load("mjo_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mjo_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_guild_vs4_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load guild_vs4");
            new MapLoader().Load("guild_vs4.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"guild_vs4 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild09");
            new MapLoader().Load("gef_fild09.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_fild09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_dth1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@dth1");
            new MapLoader().Load("1@dth1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@dth1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_e_tower_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load e_tower");
            new MapLoader().Load("e_tower.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"e_tower took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_verus03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load verus03");
            new MapLoader().Load("verus03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"verus03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_spl_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load spl_in01");
            new MapLoader().Load("spl_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"spl_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_avan01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_avan01");
            new MapLoader().Load("que_avan01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_avan01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_in03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_in03");
            new MapLoader().Load("lhz_in03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lhz_in03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild11_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild11");
            new MapLoader().Load("gef_fild11.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_fild11 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lhz_que01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lhz_que01");
            new MapLoader().Load("lhz_que01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lhz_que01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_drdo_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@drdo");
            new MapLoader().Load("1@drdo.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@drdo took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild15_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild15");
            new MapLoader().Load("moc_fild15.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild15 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild08_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild08");
            new MapLoader().Load("moc_fild08.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild08 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gon_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gon_dun01");
            new MapLoader().Load("gon_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gon_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ma_zif02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ma_zif02");
            new MapLoader().Load("ma_zif02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ma_zif02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild01");
            new MapLoader().Load("gef_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_gld_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_gld");
            new MapLoader().Load("pay_gld.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_gld took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_slw_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@slw");
            new MapLoader().Load("1@slw.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@slw took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alde_gld_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alde_gld");
            new MapLoader().Load("alde_gld.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"alde_gld took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mjolnir_11_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mjolnir_11");
            new MapLoader().Load("mjolnir_11.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mjolnir_11 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_fild06");
            new MapLoader().Load("ra_fild06.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild09");
            new MapLoader().Load("yuno_fild09.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_fild09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_morocc_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load morocc_in");
            new MapLoader().Load("morocc_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"morocc_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_san02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_san02");
            new MapLoader().Load("ra_san02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_san02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_pryd03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_pryd03");
            new MapLoader().Load("moc_pryd03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_pryd03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_castle_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_castle");
            new MapLoader().Load("moc_castle.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_castle took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ba_chess_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ba_chess");
            new MapLoader().Load("ba_chess.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ba_chess took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_dun03");
            new MapLoader().Load("pay_dun03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild06");
            new MapLoader().Load("pay_fild06.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_cash_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@cash");
            new MapLoader().Load("1@cash.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@cash took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_guild_vs3_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load guild_vs3");
            new MapLoader().Load("guild_vs3.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"guild_vs3 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_lasa_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load lasa_in01");
            new MapLoader().Load("lasa_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"lasa_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t01");
            new MapLoader().Load("tha_t01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tha_t01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gl_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gl_dun01");
            new MapLoader().Load("gl_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gl_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_prtcas03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_prtcas03");
            new MapLoader().Load("te_prtcas03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"te_prtcas03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_abbey03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load abbey03");
            new MapLoader().Load("abbey03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"abbey03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_d05_i_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_d05_i");
            new MapLoader().Load("iz_d05_i.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_d05_i took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mosk_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mosk_dun02");
            new MapLoader().Load("mosk_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mosk_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_monk_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load monk_in");
            new MapLoader().Load("monk_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"monk_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild11_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild11");
            new MapLoader().Load("moc_fild11.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild11 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ama_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ama_dun01");
            new MapLoader().Load("ama_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ama_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_priest_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_priest");
            new MapLoader().Load("job_priest.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_priest took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_aru_gld_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load aru_gld");
            new MapLoader().Load("aru_gld.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"aru_gld took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_thor_v01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load thor_v01");
            new MapLoader().Load("thor_v01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"thor_v01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_niflxmas_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load niflxmas");
            new MapLoader().Load("niflxmas.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"niflxmas took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_comodo_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load comodo");
            new MapLoader().Load("comodo.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"comodo took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac02_c_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac02_c");
            new MapLoader().Load("iz_ac02_c.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_ac02_c took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_jawaii_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load jawaii_in");
            new MapLoader().Load("jawaii_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"jawaii_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gonryun_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gonryun");
            new MapLoader().Load("gonryun.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gonryun took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_nakhyang_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load nakhyang");
            new MapLoader().Load("nakhyang.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"nakhyang took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_quiz_00_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load quiz_00");
            new MapLoader().Load("quiz_00.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"quiz_00 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pvp_y_5_2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pvp_y_5-2");
            new MapLoader().Load("pvp_y_5-2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pvp_y_5-2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_prydn1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_prydn1");
            new MapLoader().Load("moc_prydn1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_prydn1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mag_dun03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mag_dun03");
            new MapLoader().Load("mag_dun03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mag_dun03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_bif_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load bif_fild01");
            new MapLoader().Load("bif_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"bif_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_man_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load man_fild02");
            new MapLoader().Load("man_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"man_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job3_guil02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job3_guil02");
            new MapLoader().Load("job3_guil02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job3_guil02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ecl_tdun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ecl_tdun01");
            new MapLoader().Load("ecl_tdun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ecl_tdun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_castle_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_castle");
            new MapLoader().Load("prt_castle.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_castle took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_tre_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@tre");
            new MapLoader().Load("1@tre.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@tre took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_um_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load um_dun02");
            new MapLoader().Load("um_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"um_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild05");
            new MapLoader().Load("yuno_fild05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_in01");
            new MapLoader().Load("yuno_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_alde_tt03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load alde_tt03");
            new MapLoader().Load("alde_tt03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"alde_tt03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_in");
            new MapLoader().Load("prt_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ba_pw03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ba_pw03");
            new MapLoader().Load("ba_pw03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ba_pw03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_payg_cas04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load payg_cas04");
            new MapLoader().Load("payg_cas04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"payg_cas04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_guild_vs5_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load guild_vs5");
            new MapLoader().Load("guild_vs5.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"guild_vs5 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_8_thts_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 8@thts");
            new MapLoader().Load("8@thts.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"8@thts took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_ffp_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@ffp");
            new MapLoader().Load("1@ffp.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@ffp took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_dth2_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@dth2");
            new MapLoader().Load("1@dth2.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@dth2 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_kh_dun01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load kh_dun01");
            new MapLoader().Load("kh_dun01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"kh_dun01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_glast_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@glast");
            new MapLoader().Load("1@glast.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@glast took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tha_t09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tha_t09");
            new MapLoader().Load("tha_t09.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tha_t09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_2_tower_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 2@tower");
            new MapLoader().Load("2@tower.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"2@tower took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_hunter_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_hunter");
            new MapLoader().Load("job_hunter.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_hunter took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_izlude_a_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load izlude_a");
            new MapLoader().Load("izlude_a.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"izlude_a took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild01");
            new MapLoader().Load("pay_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac01_a_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac01_a");
            new MapLoader().Load("iz_ac01_a.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_ac01_a took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_auction_02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load auction_02");
            new MapLoader().Load("auction_02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"auction_02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_lost_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@lost");
            new MapLoader().Load("1@lost.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@lost took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_bamq_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@bamq");
            new MapLoader().Load("1@bamq.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@bamq took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild16_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild16");
            new MapLoader().Load("moc_fild16.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild16 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_fild11_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_fild11");
            new MapLoader().Load("pay_fild11.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_fild11 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_un_myst_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load un_myst");
            new MapLoader().Load("un_myst.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"un_myst took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_fild01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_fild01");
            new MapLoader().Load("ve_fild01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ve_fild01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild02");
            new MapLoader().Load("gef_fild02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_fild02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_splendide_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load splendide");
            new MapLoader().Load("splendide.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"splendide took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_sword1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_sword1");
            new MapLoader().Load("job_sword1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_sword1 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_job01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_job01");
            new MapLoader().Load("que_job01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_job01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_fild06");
            new MapLoader().Load("moc_fild06.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_dun05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_dun05");
            new MapLoader().Load("iz_dun05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_dun05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_pri00_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_pri00");
            new MapLoader().Load("prt_pri00.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_pri00 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ra_san03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ra_san03");
            new MapLoader().Load("ra_san03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ra_san03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_pryd04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_pryd04");
            new MapLoader().Load("moc_pryd04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_pryd04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild06_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild06");
            new MapLoader().Load("yuno_fild06.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_fild06 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_dun04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_dun04");
            new MapLoader().Load("pay_dun04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_dun04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_umbala_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load umbala");
            new MapLoader().Load("umbala.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"umbala took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_job_star_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load job_star");
            new MapLoader().Load("job_star.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"job_star took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_cmd_fild05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load cmd_fild05");
            new MapLoader().Load("cmd_fild05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"cmd_fild05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_tur_dun05_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load tur_dun05");
            new MapLoader().Load("tur_dun05.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"tur_dun05 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_god01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_god01");
            new MapLoader().Load("que_god01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_god01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moro_cav_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moro_cav");
            new MapLoader().Load("moro_cav.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moro_cav took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_int02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_int02");
            new MapLoader().Load("iz_int02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_int02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_te_prtcas04_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load te_prtcas04");
            new MapLoader().Load("te_prtcas04.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"te_prtcas04 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_prt_fild03_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load prt_fild03");
            new MapLoader().Load("prt_fild03.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"prt_fild03 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ama_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ama_dun02");
            new MapLoader().Load("ama_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ama_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_que_god02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load que_god02");
            new MapLoader().Load("que_god02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"que_god02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_ma_h_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@ma_h");
            new MapLoader().Load("1@ma_h.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@ma_h took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_yuno_fild10_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load yuno_fild10");
            new MapLoader().Load("yuno_fild10.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"yuno_fild10 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ve_in_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ve_in");
            new MapLoader().Load("ve_in.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ve_in took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gld2_prt_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gld2_prt");
            new MapLoader().Load("gld2_prt.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gld2_prt took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_os_a_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@os_a");
            new MapLoader().Load("1@os_a.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@os_a took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ein_fild09_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ein_fild09");
            new MapLoader().Load("ein_fild09.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ein_fild09 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ama_in01_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ama_in01");
            new MapLoader().Load("ama_in01.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ama_in01 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_mid_camp_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load mid_camp");
            new MapLoader().Load("mid_camp.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"mid_camp took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_ba_maison_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load ba_maison");
            new MapLoader().Load("ba_maison.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"ba_maison took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_teg_dun02_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load teg_dun02");
            new MapLoader().Load("teg_dun02.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"teg_dun02 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_pay_d03_i_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load pay_d03_i");
            new MapLoader().Load("pay_d03_i.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"pay_d03_i took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_1_bamn_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load 1@bamn");
            new MapLoader().Load("1@bamn.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"1@bamn took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_iz_ac02_d_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load iz_ac02_d");
            new MapLoader().Load("iz_ac02_d.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"iz_ac02_d took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_gef_fild13_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load gef_fild13");
            new MapLoader().Load("gef_fild13.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"gef_fild13 took {stopWatch.Elapsed} to load");
        }

        [Test]
        public void Assert_moc_prydb1_Loads() {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Restart();
            Debug.Log("Starting to load moc_prydb1");
            new MapLoader().Load("moc_prydb1.rsw", Substitute.For<Action<string, string, object>>());
            stopWatch.Stop();
            Debug.Log($"moc_prydb1 took {stopWatch.Elapsed} to load");
        }
    }
}
