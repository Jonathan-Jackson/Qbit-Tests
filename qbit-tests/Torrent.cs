namespace qbit_tests {

    public class Torrent {
        public ulong added_on { get; set; }
        public ulong amount_left { get; set; }
        public bool auto_tmm { get; set; }
        public float availability { get; set; }
        public string category { get; set; }
        public ulong completed { get; set; }
        public ulong completion_on { get; set; }
        public ulong dl_limit { get; set; }
        public ulong dlspeed { get; set; }
        public ulong downloaded { get; set; }
        public ulong downloaded_session { get; set; }
        public ulong eta { get; set; }
        public bool f_l_piece_prio { get; set; }
        public bool force_start { get; set; }
        public string hash { get; set; }
        public ulong last_activity { get; set; }
        public string magnet_uri { get; set; }
        public long max_ratio { get; set; }
        public long max_seeding_time { get; set; }
        public string name { get; set; }
        public ulong num_complete { get; set; }
        public ulong num_incomplete { get; set; }
        public ulong num_leechs { get; set; }
        public ulong num_seeds { get; set; }
        public ulong priority { get; set; }
        public float progress { get; set; }
        public decimal ratio { get; set; }
        public decimal ratio_limit { get; set; }
        public string save_path { get; set; }
        public long seeding_time_limit { get; set; }
        public ulong seen_complete { get; set; }
        public bool seq_dl { get; set; }
        public ulong size { get; set; }
        public string state { get; set; }
        public bool super_seeding { get; set; }
        public string tags { get; set; }
        public ulong time_active { get; set; }
        public ulong total_size { get; set; }
        public string tracker { get; set; }
        public long up_limit { get; set; }
        public ulong uploaded { get; set; }
        public ulong uploaded_session { get; set; }
        public ulong upspeed { get; set; }
    }
}