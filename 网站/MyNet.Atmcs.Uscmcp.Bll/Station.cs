using Ext.Net;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class Station
    {
        public Station(string stationname, string stationid)
        {
            this.Stationname = stationname;
            this.Stationid = stationid;
        }

        public Station()
        {
        }

        public string Stationname { get; set; }

        public string Stationid { get; set; }

        public static Paging<Station> StationsPaging(int start, int limit, string sort, string dir, string filter)
        {
            List<Station> stations = Station.StationAllData;

            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                stations.RemoveAll(station => !station.Stationname.ToLower().StartsWith(filter.ToLower()));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                stations.Sort(delegate(Station x, Station y)
                {
                    object a;
                    object b;

                    int direction = dir == "DESC" ? -1 : 1;

                    a = x.GetType().GetProperty(sort).GetValue(x, null);
                    b = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }

            if ((start + limit) > stations.Count)
            {
                limit = stations.Count - start;
            }

            List<Station> rangePlants = (start < 0 || limit < 0) ? stations : stations.GetRange(start, limit);

            return new Paging<Station>(rangePlants, stations.Count);
        }

        public static List<Station> StationAllData
        {
            get
            {
                DataTable dtStation = new TgsPproperty().GetStationInfo("a.station_type_id in (01,02,03,06,07,08)");
                List<Station> data = new List<Station>();

                foreach (DataRow dr in dtStation.Rows)
                {
                    Station station = new Station();

                    station.Stationname = dr["col2"].ToString();
                    station.Stationid = dr["col1"].ToString();
                    data.Add(station);
                }

                return data;
            }
        }
    }
}