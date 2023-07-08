import React, { useState, useEffect } from "react";
import debug from "sabio-debug";
import conferencesService from "services/conferenceService";
import TrainingVideos from "../dashboard/assigner/TrainingVideos";
import Announcement from "../dashboard/assigner/Announcement";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faSheetPlastic,
  faTv,
  faUsers,
  faListCheck,
} from "@fortawesome/free-solid-svg-icons";
import toastr from "toastr";
import PropTypes from "prop-types";
import GetCurrentAssignment from "components/getcurrentassignments/GetCurrentAssignments";

const _logger = debug.extend("Dash");

function OfficialDashboard(props) {
  const [dashboard, setDashboard] = useState({
    officials: 0,
    reports: 0,
    teams: 0,
    games: 0,
    announcements: [],
    crews: "South",
    trainingVideos: [],
    code: "",
    logo: "",
    conference: 1,
    grades: "98.52",
    myGames: 54,
    require: "Tests ",
  });

  useEffect(() => {
    conferencesService
      .getByIdDetail(props.currentUser.conferenceId)
      .then(onGetConferenceSuccess)
      .catch(onGetConferenceError);
  }, [props.currentUser.conferenceId]);

  const onGetConferenceSuccess = (response) => {
    _logger("response", response);
    let trainingVideos = response.item.trainingVideos;
    let announcements = response.item.announcements;
    setDashboard((prev) => {
      let dd = { ...prev };
      dd.announcements = response.item.announcements;
      dd.trainingVideosArray = trainingVideos?.map(mapVideos);
      dd.announcementArray = announcements?.map(mapAnnouncement);
      dd.officials = response.item.officials;
      dd.reports = response.item.reports;
      dd.teams = response.item.teams;
      dd.games = response.item.games;
      dd.name = response.item.name;
      return dd;
    });
  };

  const onGetConferenceError = () => {
    toastr["error"]("There was a problem getting the result", "error");
  };

  const mapVideos = (obj) => {
    return <TrainingVideos video={obj} key={obj.id}></TrainingVideos>;
  };

  const mapAnnouncement = (obj) => {
    return <Announcement announcement={obj} key={obj.id}></Announcement>;
  };

  return (
    <React.Fragment>
      <div className="col-lg-12 col-md-12 col-12">
        <div className="border-bottom pb-3 mb-3 d-lg-flex justify-content-between align-items-center">
          <div className="row mb-3 mb-lg-0 align-items-center">
            <div className="col">
              <h1 className="ms-1 mb-0  fw-bold">Officials Dashboard</h1>
              <div className="col">
                <h3 className="ms-3 ">{dashboard.name}</h3>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div className="row">
        <div className="col-xl-3 col-lg-6 col-md-12 col-sm-12">
          <div className="mb-4 card border-light">
            <div className="card-body py-3 px-4">
              <a href="/games" className="fs-6 text-uppercase fw-semi-bold">
                Grade Average
              </a>
              <div className="mt-2 d-flex justify-content-between align-items-center">
                <div className="lh-1">
                  <h2 className="h2 fw-bold mb-1">{dashboard.grades}</h2>
                </div>
                <div>
                  <span className="bg-light-info icon-shape icon-lg rounded-3 text-dark-info">
                    <div>
                      <FontAwesomeIcon icon={faSheetPlastic} size="2xl" />
                    </div>
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-xl-3 col-lg-6 col-md-12 col-sm-12">
          <div className="mb-4 card border-light">
            <div className="card-body py-3 px-4">
              <a href="/teams" className="fs-6 text-uppercase fw-semi-bold">
                {" "}
                My Games
              </a>
              <div className="mt-2 d-flex justify-content-between align-items-center">
                <div className="lh-1">
                  <h2 className="h2 fw-bold mb-1">{dashboard.myGames}</h2>
                </div>
                <div>
                  <span className="bg-light-warning icon-shape icon-lg rounded-3 text-dark-warning">
                    <div>
                      <FontAwesomeIcon icon={faTv} size="2xl" />
                    </div>
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-xl-3 col-lg-6 col-md-12 col-sm-12">
          <div className="mb-4 card border-light">
            <div className="card-body py-3 px-4">
              <a
                href="/foulreport"
                className="fs-6 text-uppercase fw-semi-bold"
              >
                Crew
              </a>
              <div className="mt-2 d-flex justify-content-between align-items-center">
                <div className="lh-1">
                  <h2 className="h2 fw-bold mb-1">{dashboard.crews}</h2>
                </div>
                <div>
                  <span className="bg-light-success icon-shape icon-lg rounded-3 text-dark-success">
                    <div>
                      <FontAwesomeIcon icon={faUsers} size="2xl" />
                    </div>
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-xl-3 col-lg-6 col-md-12 col-sm-12">
          <div className="mb-4 card border-light">
            <div className="card-body py-3 px-4">
              <a
                href="/foulreport"
                className="fs-6 text-uppercase fw-semi-bold"
              >
                Requirements
              </a>
              <div className="mt-2 d-flex justify-content-between align-items-center">
                <div className="lh-1">
                  <h2 className="h2 fw-bold mb-1">{dashboard.require}</h2>
                </div>
                <div>
                  <span className="bg-light-danger icon-shape icon-lg rounded-3 text-dark-danger">
                    <div>
                      <FontAwesomeIcon icon={faListCheck} size="2xl" />
                    </div>
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div style={{ paddingBottom: "25px" }}>
        <GetCurrentAssignment currentUser={props.currentUser} />
      </div>
      <div className="row">
        <div className="col-xl-6 col-lg-12 col-md-12 col-12 mb-6">
          <div className="card h-100">
            <div
              className="card-header d-flex align-items-center
                              justify-content-between card-header-height"
            >
              <h4 className="mb-0">Training Videos</h4>
            </div>
            <div className="card-body">
              <ul className="list-group list-group-flush">
                {dashboard.trainingVideosArray}
              </ul>
            </div>
          </div>
        </div>
        <div className="col-xl-6 col-lg-12 col-md-12 col-12 mb-6">
          <div className="card h-100">
            <div className="card-header card-header-height d-flex align-items-center">
              <h4 className="mb-0">Announcements</h4>
            </div>
            <div className="card-body">
              <ul className="list-group list-group-flush list-timeline-activity">
                {dashboard.announcementArray}
              </ul>
            </div>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
}

OfficialDashboard.propTypes = {
  currentUser: PropTypes.shape({
    conferenceId: PropTypes.number.isRequired,
  }).isRequired,
};

export default OfficialDashboard;
