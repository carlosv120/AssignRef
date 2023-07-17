import React, { useEffect, useState } from "react";
import { getDashboardData } from "../../services/dashboardServices";
import "./admindashboard.css";
import AdminDbCard from "./AdminDashCards";
import GoogleAnalytics from "components/dashboard/analytics/GoogleAnalytics";

function AdminDashboard() {
  const [dashData, setDashData] = useState({
    conferenceId: 0,
    conferenceOptions: [],
    topData: {
      totalConferences: null,
      totalGames: null,
      totalOfficials: null,
      totalUsers: null,
    },
  });

  useEffect(() => {
    getDashboardData(dashData.conferenceId)
      .then(onGetDashDataSuccess)
      .catch(onError);
  }, [dashData.conferenceId]);

  const onGetDashDataSuccess = (response) => {
    setDashData((prev) => {
      const dt = { ...prev };
      dt.topData = response.item;
      dt.conferenceOptions = response.item?.conferences?.map(mapOptions);
      return dt;
    });
  };

  const selectConference = (e) => {
    let value = e.target.value;
    setDashData((prev) => {
      const dt = { ...prev };
      dt.conferenceId = value;
      return dt;
    });
  };

  const onError = () => {};

  const mapOptions = (item) => {
    return (
      <option key={item.id} value={item.id}>
        {item.name}
      </option>
    );
  };

  return (
    <React.Fragment>
      <div className="row dashboard-top-row">
        <div className="col-lg-6 col-md-4">
          <div className="dashboard-font">Dashboard</div>
        </div>

        <div className="col-lg-4 col-md-6">
          <div className="mb-1 input-group">
            <div className="col">
              <label
                className="px-2 form-label mb-1 text-secondary fw-bold"
                htmlFor="formGridState"
              >
                Conference
              </label>
              <select
                className="form-select form-select-sm"
                id="formGridState"
                onChange={selectConference}
              >
                <option value="0"> All</option>
                {dashData.conferenceOptions}
              </select>
            </div>
          </div>
        </div>
      </div>

      <div>
        <AdminDbCard data={dashData.topData} />

        <GoogleAnalytics />
      </div>
    </React.Fragment>
  );
}

export default AdminDashboard;
